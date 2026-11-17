using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Currencies;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Currencies
{
    class CurrencyDAO
    {
        private MySqlConnection _connection;
        public List<CurrencyModel> Read(string connectionString, CurrencyModel currency, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `iso_code`, `symbol`, `name`, `decimal_places`, `available`, `modified_by`, `created_at`, `updated_at`  FROM cur_currencies ";
                {
                    if (currency != null)
                    {
                        if (!string.IsNullOrEmpty(currency.Id))
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
                            cmd.Parameters.AddWithValue("@id", currency.Id);
                        }
                        if (!string.IsNullOrEmpty(currency.IsoCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `iso_code` = @isoCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `iso_code` = @isoCode ");
                            }
                            cmd.Parameters.AddWithValue("@isoCode", currency.IsoCode);
                        }
                        if (!string.IsNullOrEmpty(currency.Symbol))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `symbol` = @symbol ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `symbol` = @symbol ");
                            }
                            cmd.Parameters.AddWithValue("@symbol", currency.Symbol);
                        }
                        if (!string.IsNullOrEmpty(currency.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", currency.Name);
                        }
                        if (!string.IsNullOrEmpty(currency.DecimalPlaces))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `decimal_places` = @decimalPlaces ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `decimal_places` = @decimalPlaces ");
                            }
                            cmd.Parameters.AddWithValue("@decimalPlaces", currency.DecimalPlaces);
                        }
                        if (!string.IsNullOrEmpty(currency.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", currency.Available);
                        }
                        if (!string.IsNullOrEmpty(currency.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", currency.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(currency.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", currency.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(currency.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", currency.UpdatedAt);
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
                                    currency.Id = reader.GetValue(0).ToString();
                                else
                                    currency.Id = null;
                                if (!reader.IsDBNull(1))
                                    currency.IsoCode = reader.GetValue(1).ToString();
                                else
                                    currency.IsoCode = null;
                                if (!reader.IsDBNull(2))
                                    currency.Symbol = reader.GetValue(2).ToString();
                                else
                                    currency.Symbol = null;
                                if (!reader.IsDBNull(3))
                                    currency.Name = reader.GetValue(3).ToString();
                                else
                                    currency.Name = null;
                                if (!reader.IsDBNull(4))
                                    currency.DecimalPlaces = reader.GetValue(4).ToString();
                                else
                                    currency.DecimalPlaces = null;
                                if (!reader.IsDBNull(5))
                                    currency.Available = reader.GetValue(5).ToString();
                                else
                                    currency.Available = null;
                                if (!reader.IsDBNull(6))
                                    currency.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    currency.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    currency.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    currency.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    currency.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    currency.UpdatedAt = null;
                                response.Add(currency);
                                currency = new CurrencyModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CurrencyModel>();
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
                response = new List<CurrencyModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CurrencyModel> Read(string connectionString, List<CurrencyModel> currencyWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `iso_code`, `symbol`, `name`, `decimal_places`, `available`, `modified_by`, `created_at`, `updated_at`  FROM cur_currencies ";
                if (currencyWhere != null && currencyWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < currencyWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(currencyWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, currencyWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].IsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `iso_code` = @isoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isoCode" + i, currencyWhere[i].IsoCode);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].Symbol))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `symbol` = @symbol" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@symbol" + i, currencyWhere[i].Symbol);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, currencyWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].DecimalPlaces))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `decimal_places` = @decimalPlaces" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@decimalPlaces" + i, currencyWhere[i].DecimalPlaces);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, currencyWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, currencyWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, currencyWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(currencyWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, currencyWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != currencyWhere.Count - 1)
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
                            CurrencyModel currency = new CurrencyModel(); if (!reader.IsDBNull(0))
                                currency.Id = reader.GetValue(0).ToString();
                            else
                                currency.Id = null;
                            if (!reader.IsDBNull(1))
                                currency.IsoCode = reader.GetValue(1).ToString();
                            else
                                currency.IsoCode = null;
                            if (!reader.IsDBNull(2))
                                currency.Symbol = reader.GetValue(2).ToString();
                            else
                                currency.Symbol = null;
                            if (!reader.IsDBNull(3))
                                currency.Name = reader.GetValue(3).ToString();
                            else
                                currency.Name = null;
                            if (!reader.IsDBNull(4))
                                currency.DecimalPlaces = reader.GetValue(4).ToString();
                            else
                                currency.DecimalPlaces = null;
                            if (!reader.IsDBNull(5))
                                currency.Available = reader.GetValue(5).ToString();
                            else
                                currency.Available = null;
                            if (!reader.IsDBNull(6))
                                currency.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                currency.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                currency.CreatedAt = reader.GetValue(7).ToString();
                            else
                                currency.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                currency.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                currency.UpdatedAt = null;
                            response.Add(currency);
                            currency = new CurrencyModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CurrencyModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CurrencyModel> Create(string connectionString, CurrencyModel currency)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO cur_currencies (";
                string values = "(";
                if (!string.IsNullOrEmpty(currency.Id))
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
                    cmd.Parameters.AddWithValue("@id", currency.Id);
                }
                if (!string.IsNullOrEmpty(currency.IsoCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `iso_code` ");
                        values = string.Concat(values, ", @isoCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `iso_code` ");
                        values = string.Concat(values, " @isoCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode", currency.IsoCode);
                }
                if (!string.IsNullOrEmpty(currency.Symbol))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `symbol` ");
                        values = string.Concat(values, ", @symbol");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `symbol` ");
                        values = string.Concat(values, " @symbol");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@symbol", currency.Symbol);
                }
                if (!string.IsNullOrEmpty(currency.Name))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `name` ");
                        values = string.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `name` ");
                        values = string.Concat(values, " @name");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", currency.Name);
                }
                if (!string.IsNullOrEmpty(currency.DecimalPlaces))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `decimal_places` ");
                        values = string.Concat(values, ", @decimalPlaces");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `decimal_places` ");
                        values = string.Concat(values, " @decimalPlaces");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@decimalPlaces", currency.DecimalPlaces);
                }
                if (!string.IsNullOrEmpty(currency.Available))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `available` ");
                        values = string.Concat(values, ", @available");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `available` ");
                        values = string.Concat(values, " @available");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", currency.Available);
                }
                if (!string.IsNullOrEmpty(currency.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", currency.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(currency.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", currency.CreatedAt);
                }
                if (!string.IsNullOrEmpty(currency.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", currency.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                currency.Id = cmd.LastInsertedId.ToString();
                response.Add(currency);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
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
        public List<CurrencyModel> Create(string connectionString, List<CurrencyModel> currencyList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < currencyList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO cur_currencies (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(currencyList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, currencyList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].IsoCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `iso_code` ");
                                        values = string.Concat(values, ", @isoCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `iso_code` ");
                                        values = string.Concat(values, "@isoCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode" + i, currencyList[i].IsoCode);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].Symbol))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `symbol` ");
                                        values = string.Concat(values, ", @symbol" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `symbol` ");
                                        values = string.Concat(values, "@symbol" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@symbol" + i, currencyList[i].Symbol);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `name` ");
                                        values = string.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `name` ");
                                        values = string.Concat(values, "@name" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, currencyList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].DecimalPlaces))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `decimal_places` ");
                                        values = string.Concat(values, ", @decimalPlaces" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `decimal_places` ");
                                        values = string.Concat(values, "@decimalPlaces" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@decimalPlaces" + i, currencyList[i].DecimalPlaces);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].Available))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `available` ");
                                        values = string.Concat(values, ", @available" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `available` ");
                                        values = string.Concat(values, "@available" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, currencyList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, currencyList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, currencyList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, currencyList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(currencyList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CurrencyModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CurrencyModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
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

        public List<CurrencyModel> Update(string connectionString, CurrencyModel currency, CurrencyModel currencyWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE cur_currencies SET ";
                if (!string.IsNullOrEmpty(currency.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", currency.Id);
                }
                if (!string.IsNullOrEmpty(currency.IsoCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `iso_code` = @isoCode");
                    else
                        sql = string.Concat(sql, " `iso_code` = @isoCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode", currency.IsoCode);
                }
                if (!string.IsNullOrEmpty(currency.Symbol))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `symbol` = @symbol");
                    else
                        sql = string.Concat(sql, " `symbol` = @symbol");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@symbol", currency.Symbol);
                }
                if (!string.IsNullOrEmpty(currency.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", currency.Name);
                }
                if (!string.IsNullOrEmpty(currency.DecimalPlaces))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `decimal_places` = @decimalPlaces");
                    else
                        sql = string.Concat(sql, " `decimal_places` = @decimalPlaces");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@decimalPlaces", currency.DecimalPlaces);
                }
                if (!string.IsNullOrEmpty(currency.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", currency.Available);
                }
                if (!string.IsNullOrEmpty(currency.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", currency.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(currency.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", currency.CreatedAt);
                }
                if (!string.IsNullOrEmpty(currency.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", currency.UpdatedAt);
                }
                if (currencyWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(currencyWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", currencyWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.IsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code` = @isoCodeWhere");
                        else
                            sql = string.Concat(sql, " `iso_code` = @isoCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCodeWhere", currencyWhere.IsoCode);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.Symbol))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `symbol` = @symbolWhere");
                        else
                            sql = string.Concat(sql, " `symbol` = @symbolWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@symbolWhere", currencyWhere.Symbol);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", currencyWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.DecimalPlaces))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `decimal_places` = @decimalPlacesWhere");
                        else
                            sql = string.Concat(sql, " `decimal_places` = @decimalPlacesWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@decimalPlacesWhere", currencyWhere.DecimalPlaces);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", currencyWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", currencyWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", currencyWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(currencyWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", currencyWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(currency);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
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

        public List<CurrencyModel> Update(string connectionString, List<CurrencyModel> currencyList, List<CurrencyModel> currencyWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < currencyList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE cur_currencies SET ";
                                if (!string.IsNullOrEmpty(currencyList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, currencyList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].IsoCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `iso_code` = @isoCode" + i);
                                    else
                                        sql = string.Concat(sql, " `iso_code` = @isoCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode" + i, currencyList[i].IsoCode);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].Symbol))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `symbol` = @symbol" + i);
                                    else
                                        sql = string.Concat(sql, " `symbol` = @symbol" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@symbol" + i, currencyList[i].Symbol);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, currencyList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].DecimalPlaces))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `decimal_places` = @decimalPlaces" + i);
                                    else
                                        sql = string.Concat(sql, " `decimal_places` = @decimalPlaces" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@decimalPlaces" + i, currencyList[i].DecimalPlaces);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, currencyList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, currencyList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, currencyList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(currencyList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, currencyList[i].UpdatedAt);
                                }
                                if (currencyWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, currencyWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].IsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code` = @isoCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code` = @isoCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCodeWhere" + i, currencyWhereList[i].IsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].Symbol))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `symbol` = @symbolWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `symbol` = @symbolWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@symbolWhere" + i, currencyWhereList[i].Symbol);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, currencyWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].DecimalPlaces))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `decimal_places` = @decimalPlacesWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `decimal_places` = @decimalPlacesWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@decimalPlacesWhere" + i, currencyWhereList[i].DecimalPlaces);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, currencyWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, currencyWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, currencyWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(currencyWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, currencyWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(currencyList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CurrencyModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CurrencyModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
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

        public List<CurrencyModel> Delete(string connectionString, CurrencyModel currency)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM cur_currencies  ";
                if (currency != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(currency.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", currency.Id);
                    }
                    if (!string.IsNullOrEmpty(currency.IsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code` = @isoCode");
                        else
                            sql = string.Concat(sql, " `iso_code` = @isoCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode", currency.IsoCode);
                    }
                    if (!string.IsNullOrEmpty(currency.Symbol))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `symbol` = @symbol");
                        else
                            sql = string.Concat(sql, " `symbol` = @symbol");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@symbol", currency.Symbol);
                    }
                    if (!string.IsNullOrEmpty(currency.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", currency.Name);
                    }
                    if (!string.IsNullOrEmpty(currency.DecimalPlaces))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `decimal_places` = @decimalPlaces");
                        else
                            sql = string.Concat(sql, " `decimal_places` = @decimalPlaces");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@decimalPlaces", currency.DecimalPlaces);
                    }
                    if (!string.IsNullOrEmpty(currency.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", currency.Available);
                    }
                    if (!string.IsNullOrEmpty(currency.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", currency.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(currency.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", currency.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(currency.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", currency.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(currency);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
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

        public List<CurrencyModel> Delete(string connectionString, List<CurrencyModel> currency)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CurrencyModel> response = new List<CurrencyModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < currency.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM cur_currencies  ";
                                if (currency[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(currency[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, currency[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].IsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code` = @isoCode" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code` = @isoCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode" + i, currency[i].IsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].Symbol))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `symbol` = @symbol" + i);
                                        else
                                            sql = string.Concat(sql, " `symbol` = @symbol" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@symbol" + i, currency[i].Symbol);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, currency[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].DecimalPlaces))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `decimal_places` = @decimalPlaces" + i);
                                        else
                                            sql = string.Concat(sql, " `decimal_places` = @decimalPlaces" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@decimalPlaces" + i, currency[i].DecimalPlaces);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, currency[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, currency[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, currency[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(currency[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, currency[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(currency[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CurrencyModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CurrencyModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CurrencyModel>();
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
