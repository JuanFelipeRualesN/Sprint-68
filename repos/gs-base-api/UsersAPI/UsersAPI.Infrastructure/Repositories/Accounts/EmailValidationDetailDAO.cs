using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
    class EmailValidationDetailDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailValidationDetailModel> ReadD(string connectionString, EmailValidationDetailModel emailValidationDetail, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailValidationDetailModel> response = new Dictionary<string, EmailValidationDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `email_token_type_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_details ";
                {
                    if (emailValidationDetail != null)
                    {
                        if (!String.IsNullOrEmpty(emailValidationDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailValidationDetail.Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", emailValidationDetail.Subject);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Header))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `header` = @header ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `header` = @header ");
                            }
                            cmd.Parameters.AddWithValue("@header", emailValidationDetail.Header);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Body))
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
                            cmd.Parameters.AddWithValue("@body", emailValidationDetail.Body);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Url))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `url` = @url ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `url` = @url ");
                            }
                            cmd.Parameters.AddWithValue("@url", emailValidationDetail.Url);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Footer))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `footer` = @footer ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `footer` = @footer ");
                            }
                            cmd.Parameters.AddWithValue("@footer", emailValidationDetail.Footer);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.LangCode))
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
                            cmd.Parameters.AddWithValue("@langCode", emailValidationDetail.LangCode);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.EmailTokenTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_token_type_id` = @emailTokenTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_token_type_id` = @emailTokenTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@emailTokenTypeId", emailValidationDetail.EmailTokenTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailValidationDetail.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailValidationDetail.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailValidationDetail.UpdatedAt);
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
                                    emailValidationDetail.Id = reader.GetValue(0).ToString();
                                else
                                    emailValidationDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailValidationDetail.Subject = reader.GetValue(1).ToString();
                                else
                                    emailValidationDetail.Subject = null;
                                if (!reader.IsDBNull(2))
                                    emailValidationDetail.Header = reader.GetValue(2).ToString();
                                else
                                    emailValidationDetail.Header = null;
                                if (!reader.IsDBNull(3))
                                    emailValidationDetail.Body = reader.GetValue(3).ToString();
                                else
                                    emailValidationDetail.Body = null;
                                if (!reader.IsDBNull(4))
                                    emailValidationDetail.Url = reader.GetValue(4).ToString();
                                else
                                    emailValidationDetail.Url = null;
                                if (!reader.IsDBNull(5))
                                    emailValidationDetail.Footer = reader.GetValue(5).ToString();
                                else
                                    emailValidationDetail.Footer = null;
                                if (!reader.IsDBNull(6))
                                    emailValidationDetail.LangCode = reader.GetValue(6).ToString();
                                else
                                    emailValidationDetail.LangCode = null;
                                if (!reader.IsDBNull(7))
                                    emailValidationDetail.EmailTokenTypeId = reader.GetValue(7).ToString();
                                else
                                    emailValidationDetail.EmailTokenTypeId = null;
                                if (!reader.IsDBNull(8))
                                    emailValidationDetail.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    emailValidationDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    emailValidationDetail.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    emailValidationDetail.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    emailValidationDetail.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    emailValidationDetail.UpdatedAt = null;
                                response.Add(emailValidationDetail.Id, emailValidationDetail);
                                emailValidationDetail = new EmailValidationDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailValidationDetailModel>();
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
                response = new Dictionary<string, EmailValidationDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailValidationDetailModel> ReadD(string connectionString, List<EmailValidationDetailModel> emailValidationDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailValidationDetailModel> response = new Dictionary<string, EmailValidationDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `email_token_type_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_details ";
                if (emailValidationDetailWhere != null && emailValidationDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailValidationDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailValidationDetailWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, emailValidationDetailWhere[i].Subject);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Header))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `header` = @header" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@header" + i, emailValidationDetailWhere[i].Header);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, emailValidationDetailWhere[i].Body);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Url))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `url` = @url" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@url" + i, emailValidationDetailWhere[i].Url);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Footer))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `footer` = @footer" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@footer" + i, emailValidationDetailWhere[i].Footer);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, emailValidationDetailWhere[i].LangCode);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].EmailTokenTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_token_type_id` = @emailTokenTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailTokenTypeId" + i, emailValidationDetailWhere[i].EmailTokenTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationDetailWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationDetailWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailValidationDetailWhere.Count - 1)
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
                            EmailValidationDetailModel emailValidationDetail = new EmailValidationDetailModel(); if (!reader.IsDBNull(0))
                                emailValidationDetail.Id = reader.GetValue(0).ToString();
                            else
                                emailValidationDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                emailValidationDetail.Subject = reader.GetValue(1).ToString();
                            else
                                emailValidationDetail.Subject = null;
                            if (!reader.IsDBNull(2))
                                emailValidationDetail.Header = reader.GetValue(2).ToString();
                            else
                                emailValidationDetail.Header = null;
                            if (!reader.IsDBNull(3))
                                emailValidationDetail.Body = reader.GetValue(3).ToString();
                            else
                                emailValidationDetail.Body = null;
                            if (!reader.IsDBNull(4))
                                emailValidationDetail.Url = reader.GetValue(4).ToString();
                            else
                                emailValidationDetail.Url = null;
                            if (!reader.IsDBNull(5))
                                emailValidationDetail.Footer = reader.GetValue(5).ToString();
                            else
                                emailValidationDetail.Footer = null;
                            if (!reader.IsDBNull(6))
                                emailValidationDetail.LangCode = reader.GetValue(6).ToString();
                            else
                                emailValidationDetail.LangCode = null;
                            if (!reader.IsDBNull(7))
                                emailValidationDetail.EmailTokenTypeId = reader.GetValue(7).ToString();
                            else
                                emailValidationDetail.EmailTokenTypeId = null;
                            if (!reader.IsDBNull(8))
                                emailValidationDetail.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                emailValidationDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                emailValidationDetail.CreatedAt = reader.GetValue(9).ToString();
                            else
                                emailValidationDetail.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                emailValidationDetail.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                emailValidationDetail.UpdatedAt = null;
                            response.Add(emailValidationDetail.Id, emailValidationDetail);
                            emailValidationDetail = new EmailValidationDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailValidationDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailValidationDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailValidationDetailModel> Read(string connectionString, EmailValidationDetailModel emailValidationDetail, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `email_token_type_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_details ";
                {
                    if (emailValidationDetail != null)
                    {
                        if (!String.IsNullOrEmpty(emailValidationDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailValidationDetail.Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Subject))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject` = @subject ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject` = @subject ");
                            }
                            cmd.Parameters.AddWithValue("@subject", emailValidationDetail.Subject);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Header))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `header` = @header ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `header` = @header ");
                            }
                            cmd.Parameters.AddWithValue("@header", emailValidationDetail.Header);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Body))
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
                            cmd.Parameters.AddWithValue("@body", emailValidationDetail.Body);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Url))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `url` = @url ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `url` = @url ");
                            }
                            cmd.Parameters.AddWithValue("@url", emailValidationDetail.Url);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.Footer))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `footer` = @footer ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `footer` = @footer ");
                            }
                            cmd.Parameters.AddWithValue("@footer", emailValidationDetail.Footer);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.LangCode))
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
                            cmd.Parameters.AddWithValue("@langCode", emailValidationDetail.LangCode);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.EmailTokenTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `email_token_type_id` = @emailTokenTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `email_token_type_id` = @emailTokenTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@emailTokenTypeId", emailValidationDetail.EmailTokenTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailValidationDetail.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailValidationDetail.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailValidationDetail.UpdatedAt);
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
                                    emailValidationDetail.Id = reader.GetValue(0).ToString();
                                else
                                    emailValidationDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailValidationDetail.Subject = reader.GetValue(1).ToString();
                                else
                                    emailValidationDetail.Subject = null;
                                if (!reader.IsDBNull(2))
                                    emailValidationDetail.Header = reader.GetValue(2).ToString();
                                else
                                    emailValidationDetail.Header = null;
                                if (!reader.IsDBNull(3))
                                    emailValidationDetail.Body = reader.GetValue(3).ToString();
                                else
                                    emailValidationDetail.Body = null;
                                if (!reader.IsDBNull(4))
                                    emailValidationDetail.Url = reader.GetValue(4).ToString();
                                else
                                    emailValidationDetail.Url = null;
                                if (!reader.IsDBNull(5))
                                    emailValidationDetail.Footer = reader.GetValue(5).ToString();
                                else
                                    emailValidationDetail.Footer = null;
                                if (!reader.IsDBNull(6))
                                    emailValidationDetail.LangCode = reader.GetValue(6).ToString();
                                else
                                    emailValidationDetail.LangCode = null;
                                if (!reader.IsDBNull(7))
                                    emailValidationDetail.EmailTokenTypeId = reader.GetValue(7).ToString();
                                else
                                    emailValidationDetail.EmailTokenTypeId = null;
                                if (!reader.IsDBNull(8))
                                    emailValidationDetail.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    emailValidationDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    emailValidationDetail.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    emailValidationDetail.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    emailValidationDetail.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    emailValidationDetail.UpdatedAt = null;
                                response.Add(emailValidationDetail);
                                emailValidationDetail = new EmailValidationDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailValidationDetailModel>();
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
                response = new List<EmailValidationDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailValidationDetailModel> Read(string connectionString, List<EmailValidationDetailModel> emailValidationDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `email_token_type_id`, `modified_by`, `created_at`, `updated_at`  FROM ul_email_validation_details ";
                if (emailValidationDetailWhere != null && emailValidationDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailValidationDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailValidationDetailWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Subject))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject` = @subject" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subject" + i, emailValidationDetailWhere[i].Subject);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Header))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `header` = @header" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@header" + i, emailValidationDetailWhere[i].Header);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Body))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `body` = @body" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@body" + i, emailValidationDetailWhere[i].Body);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Url))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `url` = @url" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@url" + i, emailValidationDetailWhere[i].Url);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].Footer))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `footer` = @footer" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@footer" + i, emailValidationDetailWhere[i].Footer);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, emailValidationDetailWhere[i].LangCode);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].EmailTokenTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `email_token_type_id` = @emailTokenTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@emailTokenTypeId" + i, emailValidationDetailWhere[i].EmailTokenTypeId);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationDetailWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationDetailWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(emailValidationDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailValidationDetailWhere.Count - 1)
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
                            EmailValidationDetailModel emailValidationDetail = new EmailValidationDetailModel(); if (!reader.IsDBNull(0))
                                emailValidationDetail.Id = reader.GetValue(0).ToString();
                            else
                                emailValidationDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                emailValidationDetail.Subject = reader.GetValue(1).ToString();
                            else
                                emailValidationDetail.Subject = null;
                            if (!reader.IsDBNull(2))
                                emailValidationDetail.Header = reader.GetValue(2).ToString();
                            else
                                emailValidationDetail.Header = null;
                            if (!reader.IsDBNull(3))
                                emailValidationDetail.Body = reader.GetValue(3).ToString();
                            else
                                emailValidationDetail.Body = null;
                            if (!reader.IsDBNull(4))
                                emailValidationDetail.Url = reader.GetValue(4).ToString();
                            else
                                emailValidationDetail.Url = null;
                            if (!reader.IsDBNull(5))
                                emailValidationDetail.Footer = reader.GetValue(5).ToString();
                            else
                                emailValidationDetail.Footer = null;
                            if (!reader.IsDBNull(6))
                                emailValidationDetail.LangCode = reader.GetValue(6).ToString();
                            else
                                emailValidationDetail.LangCode = null;
                            if (!reader.IsDBNull(7))
                                emailValidationDetail.EmailTokenTypeId = reader.GetValue(7).ToString();
                            else
                                emailValidationDetail.EmailTokenTypeId = null;
                            if (!reader.IsDBNull(8))
                                emailValidationDetail.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                emailValidationDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                emailValidationDetail.CreatedAt = reader.GetValue(9).ToString();
                            else
                                emailValidationDetail.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                emailValidationDetail.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                emailValidationDetail.UpdatedAt = null;
                            response.Add(emailValidationDetail);
                            emailValidationDetail = new EmailValidationDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailValidationDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailValidationDetailModel> Create(string connectionString, EmailValidationDetailModel emailValidationDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO ul_email_validation_details (";
                string values = "(";
                if (!String.IsNullOrEmpty(emailValidationDetail.Id))
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
                    cmd.Parameters.AddWithValue("@id", emailValidationDetail.Id);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Subject))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `subject` ");
                        values = String.Concat(values, ", @subject");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `subject` ");
                        values = String.Concat(values, " @subject");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", emailValidationDetail.Subject);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Header))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `header` ");
                        values = String.Concat(values, ", @header");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `header` ");
                        values = String.Concat(values, " @header");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@header", emailValidationDetail.Header);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Body))
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
                    cmd.Parameters.AddWithValue("@body", emailValidationDetail.Body);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Url))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `url` ");
                        values = String.Concat(values, ", @url");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `url` ");
                        values = String.Concat(values, " @url");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@url", emailValidationDetail.Url);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Footer))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `footer` ");
                        values = String.Concat(values, ", @footer");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `footer` ");
                        values = String.Concat(values, " @footer");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@footer", emailValidationDetail.Footer);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.LangCode))
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
                    cmd.Parameters.AddWithValue("@langCode", emailValidationDetail.LangCode);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.EmailTokenTypeId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `email_token_type_id` ");
                        values = String.Concat(values, ", @emailTokenTypeId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `email_token_type_id` ");
                        values = String.Concat(values, " @emailTokenTypeId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailTokenTypeId", emailValidationDetail.EmailTokenTypeId);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", emailValidationDetail.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", emailValidationDetail.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", emailValidationDetail.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailValidationDetail.Id = cmd.LastInsertedId.ToString();
                response.Add(emailValidationDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
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
        public List<EmailValidationDetailModel> Create(string connectionString, List<EmailValidationDetailModel> emailValidationDetailList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailValidationDetailList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO ul_email_validation_details (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, emailValidationDetailList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Subject))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `subject` ");
                                        values = String.Concat(values, ", @subject" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `subject` ");
                                        values = String.Concat(values, "@subject" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, emailValidationDetailList[i].Subject);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Header))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `header` ");
                                        values = String.Concat(values, ", @header" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `header` ");
                                        values = String.Concat(values, "@header" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@header" + i, emailValidationDetailList[i].Header);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Body))
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
                                    cmd.Parameters.AddWithValue("@body" + i, emailValidationDetailList[i].Body);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Url))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `url` ");
                                        values = String.Concat(values, ", @url" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `url` ");
                                        values = String.Concat(values, "@url" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@url" + i, emailValidationDetailList[i].Url);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Footer))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `footer` ");
                                        values = String.Concat(values, ", @footer" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `footer` ");
                                        values = String.Concat(values, "@footer" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@footer" + i, emailValidationDetailList[i].Footer);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].LangCode))
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
                                    cmd.Parameters.AddWithValue("@langCode" + i, emailValidationDetailList[i].LangCode);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].EmailTokenTypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `email_token_type_id` ");
                                        values = String.Concat(values, ", @emailTokenTypeId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `email_token_type_id` ");
                                        values = String.Concat(values, "@emailTokenTypeId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailTokenTypeId" + i, emailValidationDetailList[i].EmailTokenTypeId);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationDetailList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationDetailList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationDetailList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailValidationDetailList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailValidationDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailValidationDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
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

        public List<EmailValidationDetailModel> Update(string connectionString, EmailValidationDetailModel emailValidationDetail, EmailValidationDetailModel emailValidationDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE ul_email_validation_details SET ";
                if (!String.IsNullOrEmpty(emailValidationDetail.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailValidationDetail.Id);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Subject))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject` = @subject");
                    else
                        sql = String.Concat(sql, " `subject` = @subject");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subject", emailValidationDetail.Subject);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Header))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `header` = @header");
                    else
                        sql = String.Concat(sql, " `header` = @header");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@header", emailValidationDetail.Header);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Body))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `body` = @body");
                    else
                        sql = String.Concat(sql, " `body` = @body");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@body", emailValidationDetail.Body);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Url))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `url` = @url");
                    else
                        sql = String.Concat(sql, " `url` = @url");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@url", emailValidationDetail.Url);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.Footer))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `footer` = @footer");
                    else
                        sql = String.Concat(sql, " `footer` = @footer");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@footer", emailValidationDetail.Footer);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.LangCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = String.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", emailValidationDetail.LangCode);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.EmailTokenTypeId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `email_token_type_id` = @emailTokenTypeId");
                    else
                        sql = String.Concat(sql, " `email_token_type_id` = @emailTokenTypeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@emailTokenTypeId", emailValidationDetail.EmailTokenTypeId);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailValidationDetail.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailValidationDetail.CreatedAt);
                }
                if (!String.IsNullOrEmpty(emailValidationDetail.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailValidationDetail.UpdatedAt);
                }
                if (emailValidationDetailWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailValidationDetailWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.Subject))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject` = @subjectWhere");
                        else
                            sql = String.Concat(sql, " `subject` = @subjectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectWhere", emailValidationDetailWhere.Subject);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.Header))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `header` = @headerWhere");
                        else
                            sql = String.Concat(sql, " `header` = @headerWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@headerWhere", emailValidationDetailWhere.Header);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.Body))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `body` = @bodyWhere");
                        else
                            sql = String.Concat(sql, " `body` = @bodyWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@bodyWhere", emailValidationDetailWhere.Body);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.Url))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `url` = @urlWhere");
                        else
                            sql = String.Concat(sql, " `url` = @urlWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@urlWhere", emailValidationDetailWhere.Url);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.Footer))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `footer` = @footerWhere");
                        else
                            sql = String.Concat(sql, " `footer` = @footerWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@footerWhere", emailValidationDetailWhere.Footer);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.LangCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = String.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", emailValidationDetailWhere.LangCode);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.EmailTokenTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_token_type_id` = @emailTokenTypeIdWhere");
                        else
                            sql = String.Concat(sql, " `email_token_type_id` = @emailTokenTypeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailTokenTypeIdWhere", emailValidationDetailWhere.EmailTokenTypeId);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailValidationDetailWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailValidationDetailWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetailWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailValidationDetailWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailValidationDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
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

        public List<EmailValidationDetailModel> Update(string connectionString, List<EmailValidationDetailModel> emailValidationDetailList, List<EmailValidationDetailModel> emailValidationDetailWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailValidationDetailList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE ul_email_validation_details SET ";
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailValidationDetailList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Subject))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject` = @subject" + i);
                                    else
                                        sql = String.Concat(sql, " `subject` = @subject" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subject" + i, emailValidationDetailList[i].Subject);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Header))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `header` = @header" + i);
                                    else
                                        sql = String.Concat(sql, " `header` = @header" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@header" + i, emailValidationDetailList[i].Header);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Body))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `body` = @body" + i);
                                    else
                                        sql = String.Concat(sql, " `body` = @body" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@body" + i, emailValidationDetailList[i].Body);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Url))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `url` = @url" + i);
                                    else
                                        sql = String.Concat(sql, " `url` = @url" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@url" + i, emailValidationDetailList[i].Url);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].Footer))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `footer` = @footer" + i);
                                    else
                                        sql = String.Concat(sql, " `footer` = @footer" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@footer" + i, emailValidationDetailList[i].Footer);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = String.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, emailValidationDetailList[i].LangCode);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].EmailTokenTypeId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `email_token_type_id` = @emailTokenTypeId" + i);
                                    else
                                        sql = String.Concat(sql, " `email_token_type_id` = @emailTokenTypeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@emailTokenTypeId" + i, emailValidationDetailList[i].EmailTokenTypeId);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationDetailList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationDetailList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(emailValidationDetailList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationDetailList[i].UpdatedAt);
                                }
                                if (emailValidationDetailWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailValidationDetailWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject` = @subjectWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject` = @subjectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectWhere" + i, emailValidationDetailWhereList[i].Subject);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].Header))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `header` = @headerWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `header` = @headerWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@headerWhere" + i, emailValidationDetailWhereList[i].Header);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].Body))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `body` = @bodyWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `body` = @bodyWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@bodyWhere" + i, emailValidationDetailWhereList[i].Body);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].Url))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `url` = @urlWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `url` = @urlWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@urlWhere" + i, emailValidationDetailWhereList[i].Url);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].Footer))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `footer` = @footerWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `footer` = @footerWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@footerWhere" + i, emailValidationDetailWhereList[i].Footer);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, emailValidationDetailWhereList[i].LangCode);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].EmailTokenTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_token_type_id` = @emailTokenTypeIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `email_token_type_id` = @emailTokenTypeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailTokenTypeIdWhere" + i, emailValidationDetailWhereList[i].EmailTokenTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailValidationDetailWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailValidationDetailWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetailWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailValidationDetailWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailValidationDetailList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailValidationDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailValidationDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
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

        public List<EmailValidationDetailModel> Delete(string connectionString, EmailValidationDetailModel emailValidationDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM ul_email_validation_details  ";
                if (emailValidationDetail != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(emailValidationDetail.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailValidationDetail.Id);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.Subject))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject` = @subject");
                        else
                            sql = String.Concat(sql, " `subject` = @subject");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subject", emailValidationDetail.Subject);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.Header))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `header` = @header");
                        else
                            sql = String.Concat(sql, " `header` = @header");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@header", emailValidationDetail.Header);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.Body))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `body` = @body");
                        else
                            sql = String.Concat(sql, " `body` = @body");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@body", emailValidationDetail.Body);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.Url))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `url` = @url");
                        else
                            sql = String.Concat(sql, " `url` = @url");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@url", emailValidationDetail.Url);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.Footer))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `footer` = @footer");
                        else
                            sql = String.Concat(sql, " `footer` = @footer");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@footer", emailValidationDetail.Footer);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.LangCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = String.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", emailValidationDetail.LangCode);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.EmailTokenTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `email_token_type_id` = @emailTokenTypeId");
                        else
                            sql = String.Concat(sql, " `email_token_type_id` = @emailTokenTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@emailTokenTypeId", emailValidationDetail.EmailTokenTypeId);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailValidationDetail.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailValidationDetail.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(emailValidationDetail.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailValidationDetail.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailValidationDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
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

        public List<EmailValidationDetailModel> Delete(string connectionString, List<EmailValidationDetailModel> emailValidationDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailValidationDetailModel> response = new List<EmailValidationDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < emailValidationDetail.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM ul_email_validation_details  ";
                                if (emailValidationDetail[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailValidationDetail[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].Subject))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject` = @subject" + i);
                                        else
                                            sql = String.Concat(sql, " `subject` = @subject" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subject" + i, emailValidationDetail[i].Subject);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].Header))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `header` = @header" + i);
                                        else
                                            sql = String.Concat(sql, " `header` = @header" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@header" + i, emailValidationDetail[i].Header);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].Body))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `body` = @body" + i);
                                        else
                                            sql = String.Concat(sql, " `body` = @body" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@body" + i, emailValidationDetail[i].Body);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].Url))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `url` = @url" + i);
                                        else
                                            sql = String.Concat(sql, " `url` = @url" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@url" + i, emailValidationDetail[i].Url);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].Footer))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `footer` = @footer" + i);
                                        else
                                            sql = String.Concat(sql, " `footer` = @footer" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@footer" + i, emailValidationDetail[i].Footer);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = String.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, emailValidationDetail[i].LangCode);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].EmailTokenTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `email_token_type_id` = @emailTokenTypeId" + i);
                                        else
                                            sql = String.Concat(sql, " `email_token_type_id` = @emailTokenTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@emailTokenTypeId" + i, emailValidationDetail[i].EmailTokenTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailValidationDetail[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailValidationDetail[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(emailValidationDetail[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailValidationDetail[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailValidationDetail[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailValidationDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailValidationDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailValidationDetailModel>();
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
