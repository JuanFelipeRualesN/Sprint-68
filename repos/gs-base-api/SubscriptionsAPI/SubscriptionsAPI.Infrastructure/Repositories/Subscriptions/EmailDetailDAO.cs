using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class EmailDetailDAO
    {
        private MySqlConnection _connection;
        public List<EmailDetailModel> Read(string connectionString, EmailDetailModel emailDetail, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `code`, `recipients`, `title`, `message`, `modified_by`, `created_at`, `updated_at`  FROM subst_email_details ";
                {
                    if (emailDetail != null)
                    {
                        if (!string.IsNullOrEmpty(emailDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailDetail.Id);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", emailDetail.Code);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.Recipients))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recipients` = @recipients ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recipients` = @recipients ");
                            }
                            cmd.Parameters.AddWithValue("@recipients", emailDetail.Recipients);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.Title))
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
                            cmd.Parameters.AddWithValue("@title", emailDetail.Title);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.Message))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message` = @message ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message` = @message ");
                            }
                            cmd.Parameters.AddWithValue("@message", emailDetail.Message);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailDetail.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailDetail.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailDetail.UpdatedAt);
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
                                    emailDetail.Id = reader.GetValue(0).ToString();
                                else
                                    emailDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailDetail.Code = reader.GetValue(1).ToString();
                                else
                                    emailDetail.Code = null;
                                if (!reader.IsDBNull(2))
                                    emailDetail.Recipients = reader.GetValue(2).ToString();
                                else
                                    emailDetail.Recipients = null;
                                if (!reader.IsDBNull(3))
                                    emailDetail.Title = reader.GetValue(3).ToString();
                                else
                                    emailDetail.Title = null;
                                if (!reader.IsDBNull(4))
                                    emailDetail.Message = reader.GetValue(4).ToString();
                                else
                                    emailDetail.Message = null;
                                if (!reader.IsDBNull(5))
                                    emailDetail.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    emailDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    emailDetail.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    emailDetail.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    emailDetail.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    emailDetail.UpdatedAt = null;
                                response.Add(emailDetail);
                                emailDetail = new EmailDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailDetailModel>();
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
                response = new List<EmailDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailDetailModel> Read(string connectionString, List<EmailDetailModel> emailDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `code`, `recipients`, `title`, `message`, `modified_by`, `created_at`, `updated_at`  FROM subst_email_details ";
                if (emailDetailWhere != null && emailDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailDetailWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, emailDetailWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].Recipients))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recipients` = @recipients" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recipients" + i, emailDetailWhere[i].Recipients);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, emailDetailWhere[i].Title);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].Message))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message` = @message" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@message" + i, emailDetailWhere[i].Message);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailDetailWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailDetailWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailDetailWhere.Count - 1)
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
                            EmailDetailModel emailDetail = new EmailDetailModel(); if (!reader.IsDBNull(0))
                                emailDetail.Id = reader.GetValue(0).ToString();
                            else
                                emailDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                emailDetail.Code = reader.GetValue(1).ToString();
                            else
                                emailDetail.Code = null;
                            if (!reader.IsDBNull(2))
                                emailDetail.Recipients = reader.GetValue(2).ToString();
                            else
                                emailDetail.Recipients = null;
                            if (!reader.IsDBNull(3))
                                emailDetail.Title = reader.GetValue(3).ToString();
                            else
                                emailDetail.Title = null;
                            if (!reader.IsDBNull(4))
                                emailDetail.Message = reader.GetValue(4).ToString();
                            else
                                emailDetail.Message = null;
                            if (!reader.IsDBNull(5))
                                emailDetail.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                emailDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                emailDetail.CreatedAt = reader.GetValue(6).ToString();
                            else
                                emailDetail.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                emailDetail.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                emailDetail.UpdatedAt = null;
                            response.Add(emailDetail);
                            emailDetail = new EmailDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailDetailModel> Create(string connectionString, EmailDetailModel emailDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_email_details (";
                string values = "(";
                if (!string.IsNullOrEmpty(emailDetail.Id))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `id` ");
                        values = string.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `id` ");
                        values = string.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailDetail.Id);
                }
                if (!string.IsNullOrEmpty(emailDetail.Code))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `code` ");
                        values = string.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `code` ");
                        values = string.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", emailDetail.Code);
                }
                if (!string.IsNullOrEmpty(emailDetail.Recipients))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `recipients` ");
                        values = string.Concat(values, ", @recipients");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `recipients` ");
                        values = string.Concat(values, " @recipients");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@recipients", emailDetail.Recipients);
                }
                if (!string.IsNullOrEmpty(emailDetail.Title))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `title` ");
                        values = string.Concat(values, ", @title");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `title` ");
                        values = string.Concat(values, " @title");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", emailDetail.Title);
                }
                if (!string.IsNullOrEmpty(emailDetail.Message))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `message` ");
                        values = string.Concat(values, ", @message");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `message` ");
                        values = string.Concat(values, " @message");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@message", emailDetail.Message);
                }
                if (!string.IsNullOrEmpty(emailDetail.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `modified_by` ");
                        values = string.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `modified_by` ");
                        values = string.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailDetail.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailDetail.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `created_at` ");
                        values = string.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `created_at` ");
                        values = string.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailDetail.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailDetail.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `updated_at` ");
                        values = string.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `updated_at` ");
                        values = string.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailDetail.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailDetail.Id = cmd.LastInsertedId.ToString();
                response.Add(emailDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
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
        public List<EmailDetailModel> Create(string connectionString, List<EmailDetailModel> emailDetailList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailDetailList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_email_details (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(emailDetailList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `id` ");
                                        values = string.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `id` ");
                                        values = string.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailDetailList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `code` ");
                                        values = string.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `code` ");
                                        values = string.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, emailDetailList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Recipients))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `recipients` ");
                                        values = string.Concat(values, ", @recipients" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `recipients` ");
                                        values = string.Concat(values, "@recipients" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@recipients" + i, emailDetailList[i].Recipients);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Title))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `title` ");
                                        values = string.Concat(values, ", @title" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `title` ");
                                        values = string.Concat(values, "@title" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, emailDetailList[i].Title);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Message))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `message` ");
                                        values = string.Concat(values, ", @message" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `message` ");
                                        values = string.Concat(values, "@message" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@message" + i, emailDetailList[i].Message);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailDetailList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `created_at` ");
                                        values = string.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `created_at` ");
                                        values = string.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailDetailList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `updated_at` ");
                                        values = string.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `updated_at` ");
                                        values = string.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailDetailList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailDetailList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
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

        public List<EmailDetailModel> Update(string connectionString, EmailDetailModel emailDetail, EmailDetailModel emailDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_email_details SET ";
                if (!string.IsNullOrEmpty(emailDetail.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailDetail.Id);
                }
                if (!string.IsNullOrEmpty(emailDetail.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", emailDetail.Code);
                }
                if (!string.IsNullOrEmpty(emailDetail.Recipients))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `recipients` = @recipients");
                    else
                        sql = string.Concat(sql, " `recipients` = @recipients");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@recipients", emailDetail.Recipients);
                }
                if (!string.IsNullOrEmpty(emailDetail.Title))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `title` = @title");
                    else
                        sql = string.Concat(sql, " `title` = @title");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", emailDetail.Title);
                }
                if (!string.IsNullOrEmpty(emailDetail.Message))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `message` = @message");
                    else
                        sql = string.Concat(sql, " `message` = @message");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@message", emailDetail.Message);
                }
                if (!string.IsNullOrEmpty(emailDetail.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailDetail.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailDetail.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailDetail.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailDetail.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailDetail.UpdatedAt);
                }
                if (emailDetailWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(emailDetailWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailDetailWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", emailDetailWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.Recipients))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `recipients` = @recipientsWhere");
                        else
                            sql = string.Concat(sql, " `recipients` = @recipientsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@recipientsWhere", emailDetailWhere.Recipients);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.Title))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `title` = @titleWhere");
                        else
                            sql = string.Concat(sql, " `title` = @titleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleWhere", emailDetailWhere.Title);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.Message))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `message` = @messageWhere");
                        else
                            sql = string.Concat(sql, " `message` = @messageWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@messageWhere", emailDetailWhere.Message);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailDetailWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailDetailWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailDetailWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailDetailWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
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

        public List<EmailDetailModel> Update(string connectionString, List<EmailDetailModel> emailDetailList, List<EmailDetailModel> emailDetailWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailDetailList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_email_details SET ";
                                if (!string.IsNullOrEmpty(emailDetailList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailDetailList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, emailDetailList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Recipients))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `recipients` = @recipients" + i);
                                    else
                                        sql = string.Concat(sql, " `recipients` = @recipients" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@recipients" + i, emailDetailList[i].Recipients);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Title))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `title` = @title" + i);
                                    else
                                        sql = string.Concat(sql, " `title` = @title" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, emailDetailList[i].Title);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].Message))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `message` = @message" + i);
                                    else
                                        sql = string.Concat(sql, " `message` = @message" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@message" + i, emailDetailList[i].Message);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailDetailList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailDetailList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailDetailList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailDetailList[i].UpdatedAt);
                                }
                                if (emailDetailWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailDetailWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, emailDetailWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].Recipients))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `recipients` = @recipientsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `recipients` = @recipientsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@recipientsWhere" + i, emailDetailWhereList[i].Recipients);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].Title))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `title` = @titleWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `title` = @titleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleWhere" + i, emailDetailWhereList[i].Title);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].Message))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `message` = @messageWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `message` = @messageWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@messageWhere" + i, emailDetailWhereList[i].Message);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailDetailWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailDetailWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetailWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailDetailWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailDetailList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
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

        public List<EmailDetailModel> Delete(string connectionString, EmailDetailModel emailDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_email_details  ";
                if (emailDetail != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(emailDetail.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailDetail.Id);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", emailDetail.Code);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.Recipients))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `recipients` = @recipients");
                        else
                            sql = string.Concat(sql, " `recipients` = @recipients");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@recipients", emailDetail.Recipients);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.Title))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `title` = @title");
                        else
                            sql = string.Concat(sql, " `title` = @title");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@title", emailDetail.Title);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.Message))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `message` = @message");
                        else
                            sql = string.Concat(sql, " `message` = @message");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@message", emailDetail.Message);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailDetail.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailDetail.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailDetail.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailDetail.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
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

        public List<EmailDetailModel> Delete(string connectionString, List<EmailDetailModel> emailDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailDetailModel> response = new List<EmailDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailDetail.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_email_details  ";
                                if (emailDetail[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(emailDetail[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailDetail[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, emailDetail[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].Recipients))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `recipients` = @recipients" + i);
                                        else
                                            sql = string.Concat(sql, " `recipients` = @recipients" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@recipients" + i, emailDetail[i].Recipients);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].Title))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `title` = @title" + i);
                                        else
                                            sql = string.Concat(sql, " `title` = @title" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@title" + i, emailDetail[i].Title);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].Message))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `message` = @message" + i);
                                        else
                                            sql = string.Concat(sql, " `message` = @message" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@message" + i, emailDetail[i].Message);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailDetail[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailDetail[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailDetail[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailDetail[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailDetail[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailDetailModel>();
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
