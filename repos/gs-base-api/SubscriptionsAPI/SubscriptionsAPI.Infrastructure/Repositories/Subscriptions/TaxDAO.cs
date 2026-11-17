using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class TaxDAO
    {
        private MySqlConnection _connection;
        public List<TaxModel> Read(string connectionString, TaxModel tax, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `tax_name`, `state_iso`, `country_iso`, `percent`, `modified_by`, `created_at`, `updated_at`  FROM subst_taxs ";
                {
                    if (tax != null)
                    {
                        if (!string.IsNullOrEmpty(tax.Id))
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
                            cmd.Parameters.AddWithValue("@id", tax.Id);
                        }
                        if (!string.IsNullOrEmpty(tax.TaxName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `tax_name` = @taxName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `tax_name` = @taxName ");
                            }
                            cmd.Parameters.AddWithValue("@taxName", tax.TaxName);
                        }
                        if (!string.IsNullOrEmpty(tax.StateIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_iso` = @stateIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_iso` = @stateIso ");
                            }
                            cmd.Parameters.AddWithValue("@stateIso", tax.StateIso);
                        }
                        if (!string.IsNullOrEmpty(tax.CountryIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso` = @countryIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso` = @countryIso ");
                            }
                            cmd.Parameters.AddWithValue("@countryIso", tax.CountryIso);
                        }
                        if (!string.IsNullOrEmpty(tax.Percent))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `percent` = @percent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `percent` = @percent ");
                            }
                            cmd.Parameters.AddWithValue("@percent", tax.Percent);
                        }
                        if (!string.IsNullOrEmpty(tax.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", tax.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(tax.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", tax.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(tax.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", tax.UpdatedAt);
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
                                    tax.Id = reader.GetValue(0).ToString();
                                else
                                    tax.Id = null;
                                if (!reader.IsDBNull(1))
                                    tax.TaxName = reader.GetValue(1).ToString();
                                else
                                    tax.TaxName = null;
                                if (!reader.IsDBNull(2))
                                    tax.StateIso = reader.GetValue(2).ToString();
                                else
                                    tax.StateIso = null;
                                if (!reader.IsDBNull(3))
                                    tax.CountryIso = reader.GetValue(3).ToString();
                                else
                                    tax.CountryIso = null;
                                if (!reader.IsDBNull(4))
                                    tax.Percent = reader.GetValue(4).ToString();
                                else
                                    tax.Percent = null;
                                if (!reader.IsDBNull(5))
                                    tax.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    tax.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    tax.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    tax.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    tax.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    tax.UpdatedAt = null;
                                response.Add(tax);
                                tax = new TaxModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TaxModel>();
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
                response = new List<TaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TaxModel> Read(string connectionString, List<TaxModel> taxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `tax_name`, `state_iso`, `country_iso`, `percent`, `modified_by`, `created_at`, `updated_at`  FROM subst_taxs ";
                if (taxWhere != null && taxWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < taxWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(taxWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, taxWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].TaxName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tax_name` = @taxName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@taxName" + i, taxWhere[i].TaxName);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].StateIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_iso` = @stateIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateIso" + i, taxWhere[i].StateIso);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].CountryIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso` = @countryIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIso" + i, taxWhere[i].CountryIso);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].Percent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `percent` = @percent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@percent" + i, taxWhere[i].Percent);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, taxWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, taxWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(taxWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, taxWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != taxWhere.Count - 1)
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
                            TaxModel tax = new TaxModel(); if (!reader.IsDBNull(0))
                                tax.Id = reader.GetValue(0).ToString();
                            else
                                tax.Id = null;
                            if (!reader.IsDBNull(1))
                                tax.TaxName = reader.GetValue(1).ToString();
                            else
                                tax.TaxName = null;
                            if (!reader.IsDBNull(2))
                                tax.StateIso = reader.GetValue(2).ToString();
                            else
                                tax.StateIso = null;
                            if (!reader.IsDBNull(3))
                                tax.CountryIso = reader.GetValue(3).ToString();
                            else
                                tax.CountryIso = null;
                            if (!reader.IsDBNull(4))
                                tax.Percent = reader.GetValue(4).ToString();
                            else
                                tax.Percent = null;
                            if (!reader.IsDBNull(5))
                                tax.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                tax.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                tax.CreatedAt = reader.GetValue(6).ToString();
                            else
                                tax.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                tax.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                tax.UpdatedAt = null;
                            response.Add(tax);
                            tax = new TaxModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TaxModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TaxModel> Create(string connectionString, TaxModel tax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_taxs (";
                string values = "(";
                if (!string.IsNullOrEmpty(tax.Id))
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
                    cmd.Parameters.AddWithValue("@id", tax.Id);
                }
                if (!string.IsNullOrEmpty(tax.TaxName))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `tax_name` ");
                        values = string.Concat(values, ", @taxName");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `tax_name` ");
                        values = string.Concat(values, " @taxName");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxName", tax.TaxName);
                }
                if (!string.IsNullOrEmpty(tax.StateIso))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `state_iso` ");
                        values = string.Concat(values, ", @stateIso");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `state_iso` ");
                        values = string.Concat(values, " @stateIso");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateIso", tax.StateIso);
                }
                if (!string.IsNullOrEmpty(tax.CountryIso))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country_iso` ");
                        values = string.Concat(values, ", @countryIso");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country_iso` ");
                        values = string.Concat(values, " @countryIso");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIso", tax.CountryIso);
                }
                if (!string.IsNullOrEmpty(tax.Percent))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `percent` ");
                        values = string.Concat(values, ", @percent");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `percent` ");
                        values = string.Concat(values, " @percent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@percent", tax.Percent);
                }
                if (!string.IsNullOrEmpty(tax.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", tax.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(tax.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", tax.CreatedAt);
                }
                if (!string.IsNullOrEmpty(tax.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", tax.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                tax.Id = cmd.LastInsertedId.ToString();
                response.Add(tax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
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
        public List<TaxModel> Create(string connectionString, List<TaxModel> taxList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < taxList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_taxs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(taxList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, taxList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].TaxName))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `tax_name` ");
                                        values = string.Concat(values, ", @taxName" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `tax_name` ");
                                        values = string.Concat(values, "@taxName" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxName" + i, taxList[i].TaxName);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].StateIso))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `state_iso` ");
                                        values = string.Concat(values, ", @stateIso" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `state_iso` ");
                                        values = string.Concat(values, "@stateIso" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateIso" + i, taxList[i].StateIso);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].CountryIso))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country_iso` ");
                                        values = string.Concat(values, ", @countryIso" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country_iso` ");
                                        values = string.Concat(values, "@countryIso" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIso" + i, taxList[i].CountryIso);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].Percent))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `percent` ");
                                        values = string.Concat(values, ", @percent" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `percent` ");
                                        values = string.Concat(values, "@percent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@percent" + i, taxList[i].Percent);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, taxList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, taxList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, taxList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(taxList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
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

        public List<TaxModel> Update(string connectionString, TaxModel tax, TaxModel taxWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_taxs SET ";
                if (!string.IsNullOrEmpty(tax.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", tax.Id);
                }
                if (!string.IsNullOrEmpty(tax.TaxName))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `tax_name` = @taxName");
                    else
                        sql = string.Concat(sql, " `tax_name` = @taxName");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@taxName", tax.TaxName);
                }
                if (!string.IsNullOrEmpty(tax.StateIso))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `state_iso` = @stateIso");
                    else
                        sql = string.Concat(sql, " `state_iso` = @stateIso");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateIso", tax.StateIso);
                }
                if (!string.IsNullOrEmpty(tax.CountryIso))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso` = @countryIso");
                    else
                        sql = string.Concat(sql, " `country_iso` = @countryIso");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIso", tax.CountryIso);
                }
                if (!string.IsNullOrEmpty(tax.Percent))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `percent` = @percent");
                    else
                        sql = string.Concat(sql, " `percent` = @percent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@percent", tax.Percent);
                }
                if (!string.IsNullOrEmpty(tax.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", tax.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(tax.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", tax.CreatedAt);
                }
                if (!string.IsNullOrEmpty(tax.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", tax.UpdatedAt);
                }
                if (taxWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(taxWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", taxWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.TaxName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_name` = @taxNameWhere");
                        else
                            sql = string.Concat(sql, " `tax_name` = @taxNameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxNameWhere", taxWhere.TaxName);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.StateIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_iso` = @stateIsoWhere");
                        else
                            sql = string.Concat(sql, " `state_iso` = @stateIsoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateIsoWhere", taxWhere.StateIso);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.CountryIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso` = @countryIsoWhere");
                        else
                            sql = string.Concat(sql, " `country_iso` = @countryIsoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoWhere", taxWhere.CountryIso);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.Percent))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `percent` = @percentWhere");
                        else
                            sql = string.Concat(sql, " `percent` = @percentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@percentWhere", taxWhere.Percent);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", taxWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", taxWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(taxWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", taxWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(tax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
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

        public List<TaxModel> Update(string connectionString, List<TaxModel> taxList, List<TaxModel> taxWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < taxList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_taxs SET ";
                                if (!string.IsNullOrEmpty(taxList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, taxList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].TaxName))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `tax_name` = @taxName" + i);
                                    else
                                        sql = string.Concat(sql, " `tax_name` = @taxName" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@taxName" + i, taxList[i].TaxName);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].StateIso))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `state_iso` = @stateIso" + i);
                                    else
                                        sql = string.Concat(sql, " `state_iso` = @stateIso" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateIso" + i, taxList[i].StateIso);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].CountryIso))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso` = @countryIso" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso` = @countryIso" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIso" + i, taxList[i].CountryIso);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].Percent))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `percent` = @percent" + i);
                                    else
                                        sql = string.Concat(sql, " `percent` = @percent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@percent" + i, taxList[i].Percent);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, taxList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, taxList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(taxList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, taxList[i].UpdatedAt);
                                }
                                if (taxWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(taxWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, taxWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].TaxName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_name` = @taxNameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_name` = @taxNameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxNameWhere" + i, taxWhereList[i].TaxName);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].StateIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_iso` = @stateIsoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `state_iso` = @stateIsoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateIsoWhere" + i, taxWhereList[i].StateIso);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].CountryIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso` = @countryIsoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso` = @countryIsoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoWhere" + i, taxWhereList[i].CountryIso);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].Percent))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `percent` = @percentWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `percent` = @percentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@percentWhere" + i, taxWhereList[i].Percent);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, taxWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, taxWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(taxWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, taxWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(taxList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
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

        public List<TaxModel> Delete(string connectionString, TaxModel tax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_taxs  ";
                if (tax != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(tax.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", tax.Id);
                    }
                    if (!string.IsNullOrEmpty(tax.TaxName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `tax_name` = @taxName");
                        else
                            sql = string.Concat(sql, " `tax_name` = @taxName");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@taxName", tax.TaxName);
                    }
                    if (!string.IsNullOrEmpty(tax.StateIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_iso` = @stateIso");
                        else
                            sql = string.Concat(sql, " `state_iso` = @stateIso");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateIso", tax.StateIso);
                    }
                    if (!string.IsNullOrEmpty(tax.CountryIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso` = @countryIso");
                        else
                            sql = string.Concat(sql, " `country_iso` = @countryIso");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIso", tax.CountryIso);
                    }
                    if (!string.IsNullOrEmpty(tax.Percent))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `percent` = @percent");
                        else
                            sql = string.Concat(sql, " `percent` = @percent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@percent", tax.Percent);
                    }
                    if (!string.IsNullOrEmpty(tax.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", tax.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(tax.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", tax.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(tax.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", tax.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(tax);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
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

        public List<TaxModel> Delete(string connectionString, List<TaxModel> tax)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TaxModel> response = new List<TaxModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < tax.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_taxs  ";
                                if (tax[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(tax[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, tax[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].TaxName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `tax_name` = @taxName" + i);
                                        else
                                            sql = string.Concat(sql, " `tax_name` = @taxName" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@taxName" + i, tax[i].TaxName);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].StateIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_iso` = @stateIso" + i);
                                        else
                                            sql = string.Concat(sql, " `state_iso` = @stateIso" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateIso" + i, tax[i].StateIso);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].CountryIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso` = @countryIso" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso` = @countryIso" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIso" + i, tax[i].CountryIso);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].Percent))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `percent` = @percent" + i);
                                        else
                                            sql = string.Concat(sql, " `percent` = @percent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@percent" + i, tax[i].Percent);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, tax[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, tax[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(tax[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, tax[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(tax[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TaxModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TaxModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TaxModel>();
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
