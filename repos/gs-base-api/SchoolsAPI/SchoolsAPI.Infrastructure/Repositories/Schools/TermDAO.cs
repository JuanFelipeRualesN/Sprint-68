using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class TermDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TermModel> ReadD(string connectionString, TermModel term, bool firstOne, bool orderByLastDate, TermModel termWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TermModel> response = new Dictionary<string, TermModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                {
                    if (term != null)
                    {
                        if (!string.IsNullOrEmpty(term.Id))
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
                            cmd.Parameters.AddWithValue("@id", term.Id);
                        }
                        if (!string.IsNullOrEmpty(term.SchTerm))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sch_term` = @schTerm ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sch_term` = @schTerm ");
                            }
                            cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                        }
                        if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recommended_periods` = @recommendedPeriods ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recommended_periods` = @recommendedPeriods ");
                            }
                            cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(term.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(term.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(term.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                        }
                    }
                    if (termWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(termWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + termWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.SchTerm))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `sch_term` LIKE @schTermWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `sch_term` LIKE @schTermWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@schTermWhereLike", "%" + termWhereLike.SchTerm + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.RecommendedPeriods))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriodsWhereLike", "%" + termWhereLike.RecommendedPeriods + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + termWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + termWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + termWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                term = new TermModel();
                                if (!reader.IsDBNull(0))
                                    term.Id = reader.GetValue(0).ToString();
                                else
                                    term.Id = null;
                                if (!reader.IsDBNull(1))
                                    term.SchTerm = reader.GetValue(1).ToString();
                                else
                                    term.SchTerm = null;
                                if (!reader.IsDBNull(2))
                                    term.RecommendedPeriods = reader.GetValue(2).ToString();
                                else
                                    term.RecommendedPeriods = null;
                                if (!reader.IsDBNull(3))
                                    term.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    term.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    term.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    term.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    term.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    term.UpdatedAt = null;
                                response.Add(term.Id, term);
                                term = new TermModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TermModel>();
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
                response = new Dictionary<string, TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TermModel> ReadD(string connectionString, TermModel term, bool orderByLastDate, int fromRow, int limit, TermModel termWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TermModel> response = new Dictionary<string, TermModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                {
                    if (term != null)
                    {
                        if (!string.IsNullOrEmpty(term.Id))
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
                            cmd.Parameters.AddWithValue("@id", term.Id);
                        }
                        if (!string.IsNullOrEmpty(term.SchTerm))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sch_term` = @schTerm ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sch_term` = @schTerm ");
                            }
                            cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                        }
                        if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recommended_periods` = @recommendedPeriods ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recommended_periods` = @recommendedPeriods ");
                            }
                            cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(term.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(term.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(term.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                        }
                    }
                    if (termWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(termWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + termWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.SchTerm))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `sch_term` LIKE @schTermWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `sch_term` LIKE @schTermWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@schTermWhereLike", "%" + termWhereLike.SchTerm + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.RecommendedPeriods))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriodsWhereLike", "%" + termWhereLike.RecommendedPeriods + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + termWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + termWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + termWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                term = new TermModel();
                                if (!reader.IsDBNull(0))
                                    term.Id = reader.GetValue(0).ToString();
                                else
                                    term.Id = null;
                                if (!reader.IsDBNull(1))
                                    term.SchTerm = reader.GetValue(1).ToString();
                                else
                                    term.SchTerm = null;
                                if (!reader.IsDBNull(2))
                                    term.RecommendedPeriods = reader.GetValue(2).ToString();
                                else
                                    term.RecommendedPeriods = null;
                                if (!reader.IsDBNull(3))
                                    term.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    term.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    term.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    term.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    term.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    term.UpdatedAt = null;
                                response.Add(term.Id, term);
                                term = new TermModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TermModel>();
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
                response = new Dictionary<string, TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TermModel> ReadD(string connectionString, List<TermModel> termWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TermModel> response = new Dictionary<string, TermModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                if (termWhere != null && termWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < termWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(termWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, termWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].SchTerm))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sch_term` = @schTerm" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@schTerm" + i, termWhere[i].SchTerm);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].RecommendedPeriods))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recommended_periods` = @recommendedPeriods" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriods" + i, termWhere[i].RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, termWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, termWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, termWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != termWhere.Count - 1)
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
                            TermModel term = new TermModel(); if (!reader.IsDBNull(0))
                                term.Id = reader.GetValue(0).ToString();
                            else
                                term.Id = null;
                            if (!reader.IsDBNull(1))
                                term.SchTerm = reader.GetValue(1).ToString();
                            else
                                term.SchTerm = null;
                            if (!reader.IsDBNull(2))
                                term.RecommendedPeriods = reader.GetValue(2).ToString();
                            else
                                term.RecommendedPeriods = null;
                            if (!reader.IsDBNull(3))
                                term.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                term.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                term.CreatedAt = reader.GetValue(4).ToString();
                            else
                                term.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                term.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                term.UpdatedAt = null;
                            response.Add(term.Id, term);
                            term = new TermModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TermModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TermModel> ReadD(string connectionString, List<TermModel> termWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TermModel> response = new Dictionary<string, TermModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                if (termWhere != null && termWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < termWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(termWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, termWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].SchTerm))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sch_term` = @schTerm" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@schTerm" + i, termWhere[i].SchTerm);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].RecommendedPeriods))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recommended_periods` = @recommendedPeriods" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriods" + i, termWhere[i].RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, termWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, termWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, termWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != termWhere.Count - 1)
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
                            TermModel term = new TermModel(); if (!reader.IsDBNull(0))
                                term.Id = reader.GetValue(0).ToString();
                            else
                                term.Id = null;
                            if (!reader.IsDBNull(1))
                                term.SchTerm = reader.GetValue(1).ToString();
                            else
                                term.SchTerm = null;
                            if (!reader.IsDBNull(2))
                                term.RecommendedPeriods = reader.GetValue(2).ToString();
                            else
                                term.RecommendedPeriods = null;
                            if (!reader.IsDBNull(3))
                                term.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                term.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                term.CreatedAt = reader.GetValue(4).ToString();
                            else
                                term.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                term.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                term.UpdatedAt = null;
                            response.Add(term.Id, term);
                            term = new TermModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TermModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TermModel> Read(string connectionString, TermModel term, bool firstOne, bool orderByLastDate, TermModel termWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                {
                    if (term != null)
                    {
                        if (!string.IsNullOrEmpty(term.Id))
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
                            cmd.Parameters.AddWithValue("@id", term.Id);
                        }
                        if (!string.IsNullOrEmpty(term.SchTerm))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sch_term` = @schTerm ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sch_term` = @schTerm ");
                            }
                            cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                        }
                        if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recommended_periods` = @recommendedPeriods ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recommended_periods` = @recommendedPeriods ");
                            }
                            cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(term.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(term.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(term.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                        }
                    }
                    if (termWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(termWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + termWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.SchTerm))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `sch_term` LIKE @schTermWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `sch_term` LIKE @schTermWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@schTermWhereLike", "%" + termWhereLike.SchTerm + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.RecommendedPeriods))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriodsWhereLike", "%" + termWhereLike.RecommendedPeriods + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + termWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + termWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + termWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                term = new TermModel();
                                if (!reader.IsDBNull(0))
                                    term.Id = reader.GetValue(0).ToString();
                                else
                                    term.Id = null;
                                if (!reader.IsDBNull(1))
                                    term.SchTerm = reader.GetValue(1).ToString();
                                else
                                    term.SchTerm = null;
                                if (!reader.IsDBNull(2))
                                    term.RecommendedPeriods = reader.GetValue(2).ToString();
                                else
                                    term.RecommendedPeriods = null;
                                if (!reader.IsDBNull(3))
                                    term.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    term.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    term.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    term.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    term.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    term.UpdatedAt = null;
                                response.Add(term);
                                term = new TermModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TermModel>();
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
                response = new List<TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TermModel> Read(string connectionString, TermModel term, bool orderByLastDate, int fromRow, int limit, TermModel termWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                {
                    if (term != null)
                    {
                        if (!string.IsNullOrEmpty(term.Id))
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
                            cmd.Parameters.AddWithValue("@id", term.Id);
                        }
                        if (!string.IsNullOrEmpty(term.SchTerm))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sch_term` = @schTerm ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sch_term` = @schTerm ");
                            }
                            cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                        }
                        if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `recommended_periods` = @recommendedPeriods ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `recommended_periods` = @recommendedPeriods ");
                            }
                            cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(term.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(term.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(term.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                        }
                    }
                    if (termWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(termWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + termWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.SchTerm))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `sch_term` LIKE @schTermWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `sch_term` LIKE @schTermWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@schTermWhereLike", "%" + termWhereLike.SchTerm + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.RecommendedPeriods))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `recommended_periods` LIKE @recommendedPeriodsWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriodsWhereLike", "%" + termWhereLike.RecommendedPeriods + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + termWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + termWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(termWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + termWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                term = new TermModel();
                                if (!reader.IsDBNull(0))
                                    term.Id = reader.GetValue(0).ToString();
                                else
                                    term.Id = null;
                                if (!reader.IsDBNull(1))
                                    term.SchTerm = reader.GetValue(1).ToString();
                                else
                                    term.SchTerm = null;
                                if (!reader.IsDBNull(2))
                                    term.RecommendedPeriods = reader.GetValue(2).ToString();
                                else
                                    term.RecommendedPeriods = null;
                                if (!reader.IsDBNull(3))
                                    term.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    term.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    term.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    term.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    term.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    term.UpdatedAt = null;
                                response.Add(term);
                                term = new TermModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TermModel>();
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
                response = new List<TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TermModel> Read(string connectionString, List<TermModel> termWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                if (termWhere != null && termWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < termWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(termWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, termWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].SchTerm))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sch_term` = @schTerm" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@schTerm" + i, termWhere[i].SchTerm);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].RecommendedPeriods))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recommended_periods` = @recommendedPeriods" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriods" + i, termWhere[i].RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, termWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, termWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, termWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != termWhere.Count - 1)
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
                            TermModel term = new TermModel(); if (!reader.IsDBNull(0))
                                term.Id = reader.GetValue(0).ToString();
                            else
                                term.Id = null;
                            if (!reader.IsDBNull(1))
                                term.SchTerm = reader.GetValue(1).ToString();
                            else
                                term.SchTerm = null;
                            if (!reader.IsDBNull(2))
                                term.RecommendedPeriods = reader.GetValue(2).ToString();
                            else
                                term.RecommendedPeriods = null;
                            if (!reader.IsDBNull(3))
                                term.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                term.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                term.CreatedAt = reader.GetValue(4).ToString();
                            else
                                term.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                term.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                term.UpdatedAt = null;
                            response.Add(term);
                            term = new TermModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TermModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TermModel> Read(string connectionString, List<TermModel> termWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `sch_term`, `recommended_periods`, `modified_by`, `created_at`, `updated_at`  FROM sch_terms ";
                if (termWhere != null && termWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < termWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(termWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, termWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].SchTerm))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sch_term` = @schTerm" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@schTerm" + i, termWhere[i].SchTerm);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].RecommendedPeriods))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `recommended_periods` = @recommendedPeriods" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@recommendedPeriods" + i, termWhere[i].RecommendedPeriods);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, termWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, termWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(termWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, termWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != termWhere.Count - 1)
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
                            TermModel term = new TermModel(); if (!reader.IsDBNull(0))
                                term.Id = reader.GetValue(0).ToString();
                            else
                                term.Id = null;
                            if (!reader.IsDBNull(1))
                                term.SchTerm = reader.GetValue(1).ToString();
                            else
                                term.SchTerm = null;
                            if (!reader.IsDBNull(2))
                                term.RecommendedPeriods = reader.GetValue(2).ToString();
                            else
                                term.RecommendedPeriods = null;
                            if (!reader.IsDBNull(3))
                                term.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                term.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                term.CreatedAt = reader.GetValue(4).ToString();
                            else
                                term.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                term.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                term.UpdatedAt = null;
                            response.Add(term);
                            term = new TermModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TermModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TermModel> Create(string connectionString, TermModel term)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_terms (";
                string values = "(";
                if (!string.IsNullOrEmpty(term.Id))
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
                    cmd.Parameters.AddWithValue("@id", term.Id);
                }
                if (!string.IsNullOrEmpty(term.SchTerm))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `sch_term` ");
                        values = string.Concat(values, ", @schTerm");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `sch_term` ");
                        values = string.Concat(values, " @schTerm");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                }
                if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `recommended_periods` ");
                        values = string.Concat(values, ", @recommendedPeriods");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `recommended_periods` ");
                        values = string.Concat(values, " @recommendedPeriods");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                }
                if (!string.IsNullOrEmpty(term.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(term.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                }
                if (!string.IsNullOrEmpty(term.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                term.Id = cmd.LastInsertedId.ToString();
                response.Add(term);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
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
        public List<TermModel> Create(string connectionString, List<TermModel> termList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
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
                        for (int i = 0; i < termList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_terms (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(termList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, termList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(termList[i].SchTerm))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `sch_term` ");
                                        values = string.Concat(values, ", @schTerm" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `sch_term` ");
                                        values = string.Concat(values, "@schTerm" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@schTerm" + i, termList[i].SchTerm);
                                }
                                if (!string.IsNullOrEmpty(termList[i].RecommendedPeriods))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `recommended_periods` ");
                                        values = string.Concat(values, ", @recommendedPeriods" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `recommended_periods` ");
                                        values = string.Concat(values, "@recommendedPeriods" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@recommendedPeriods" + i, termList[i].RecommendedPeriods);
                                }
                                if (!string.IsNullOrEmpty(termList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, termList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(termList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, termList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(termList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, termList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(termList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TermModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TermModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
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

        public List<TermModel> Update(string connectionString, TermModel term, TermModel termWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_terms SET ";
                if (!string.IsNullOrEmpty(term.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", term.Id);
                }
                if (!string.IsNullOrEmpty(term.SchTerm))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `sch_term` = @schTerm");
                    else
                        sql = string.Concat(sql, " `sch_term` = @schTerm");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                }
                if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `recommended_periods` = @recommendedPeriods");
                    else
                        sql = string.Concat(sql, " `recommended_periods` = @recommendedPeriods");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                }
                if (!string.IsNullOrEmpty(term.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(term.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                }
                if (!string.IsNullOrEmpty(term.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                }
                if (termWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(termWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", termWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(termWhere.SchTerm))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sch_term` = @schTermWhere");
                        else
                            sql = string.Concat(sql, " `sch_term` = @schTermWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@schTermWhere", termWhere.SchTerm);
                    }
                    if (!string.IsNullOrEmpty(termWhere.RecommendedPeriods))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `recommended_periods` = @recommendedPeriodsWhere");
                        else
                            sql = string.Concat(sql, " `recommended_periods` = @recommendedPeriodsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@recommendedPeriodsWhere", termWhere.RecommendedPeriods);
                    }
                    if (!string.IsNullOrEmpty(termWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", termWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(termWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", termWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(termWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", termWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(term);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
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

        public List<TermModel> Update(string connectionString, List<TermModel> termList, List<TermModel> termWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
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
                        for (int i = 0; i < termList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_terms SET ";
                                if (!string.IsNullOrEmpty(termList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, termList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(termList[i].SchTerm))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `sch_term` = @schTerm" + i);
                                    else
                                        sql = string.Concat(sql, " `sch_term` = @schTerm" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@schTerm" + i, termList[i].SchTerm);
                                }
                                if (!string.IsNullOrEmpty(termList[i].RecommendedPeriods))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `recommended_periods` = @recommendedPeriods" + i);
                                    else
                                        sql = string.Concat(sql, " `recommended_periods` = @recommendedPeriods" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@recommendedPeriods" + i, termList[i].RecommendedPeriods);
                                }
                                if (!string.IsNullOrEmpty(termList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, termList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(termList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, termList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(termList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, termList[i].UpdatedAt);
                                }
                                if (termWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(termWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, termWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(termWhereList[i].SchTerm))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sch_term` = @schTermWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `sch_term` = @schTermWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@schTermWhere" + i, termWhereList[i].SchTerm);
                                    }
                                    if (!string.IsNullOrEmpty(termWhereList[i].RecommendedPeriods))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `recommended_periods` = @recommendedPeriodsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `recommended_periods` = @recommendedPeriodsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@recommendedPeriodsWhere" + i, termWhereList[i].RecommendedPeriods);
                                    }
                                    if (!string.IsNullOrEmpty(termWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, termWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(termWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, termWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(termWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, termWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(termList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TermModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TermModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
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

        public List<TermModel> Delete(string connectionString, TermModel term)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_terms  ";
                if (term != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(term.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", term.Id);
                    }
                    if (!string.IsNullOrEmpty(term.SchTerm))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sch_term` = @schTerm");
                        else
                            sql = string.Concat(sql, " `sch_term` = @schTerm");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@schTerm", term.SchTerm);
                    }
                    if (!string.IsNullOrEmpty(term.RecommendedPeriods))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `recommended_periods` = @recommendedPeriods");
                        else
                            sql = string.Concat(sql, " `recommended_periods` = @recommendedPeriods");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@recommendedPeriods", term.RecommendedPeriods);
                    }
                    if (!string.IsNullOrEmpty(term.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", term.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(term.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", term.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(term.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", term.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(term);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
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

        public List<TermModel> Delete(string connectionString, List<TermModel> term)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TermModel> response = new List<TermModel>();
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
                        for (int i = 0; i < term.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_terms  ";
                                if (term[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(term[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, term[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(term[i].SchTerm))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sch_term` = @schTerm" + i);
                                        else
                                            sql = string.Concat(sql, " `sch_term` = @schTerm" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@schTerm" + i, term[i].SchTerm);
                                    }
                                    if (!string.IsNullOrEmpty(term[i].RecommendedPeriods))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `recommended_periods` = @recommendedPeriods" + i);
                                        else
                                            sql = string.Concat(sql, " `recommended_periods` = @recommendedPeriods" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@recommendedPeriods" + i, term[i].RecommendedPeriods);
                                    }
                                    if (!string.IsNullOrEmpty(term[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, term[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(term[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, term[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(term[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, term[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(term[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TermModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TermModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TermModel>();
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
