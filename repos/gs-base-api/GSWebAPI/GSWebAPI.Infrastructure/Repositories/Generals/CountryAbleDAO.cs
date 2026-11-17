using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.Generals;
using GSWebAPI.GSWebAPI.Domain.Models.General;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals
{
    class CountryAbleDAO
    {
        private MySqlConnection _connection;
        public List<CountryAbleModel> Read(string connectionString, CountryAbleModel countryAble, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `country_iso_code_2`, `modified_by`, `created_at`, `updated_at`  FROM gwp_country_ables ";
                {
                    if (countryAble != null)
                    {
                        if (!string.IsNullOrEmpty(countryAble.Id))
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
                            cmd.Parameters.AddWithValue("@id", countryAble.Id);
                        }
                        if (!string.IsNullOrEmpty(countryAble.CountryIsoCode2))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso_code_2` = @countryIsoCode2 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso_code_2` = @countryIsoCode2 ");
                            }
                            cmd.Parameters.AddWithValue("@countryIsoCode2", countryAble.CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(countryAble.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", countryAble.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(countryAble.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", countryAble.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(countryAble.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", countryAble.UpdatedAt);
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
                                    countryAble.Id = reader.GetValue(0).ToString();
                                else
                                    countryAble.Id = null;
                                if (!reader.IsDBNull(1))
                                    countryAble.CountryIsoCode2 = reader.GetValue(1).ToString();
                                else
                                    countryAble.CountryIsoCode2 = null;
                                if (!reader.IsDBNull(2))
                                    countryAble.ModifiedBy = reader.GetValue(2).ToString();
                                else
                                    countryAble.ModifiedBy = null;
                                if (!reader.IsDBNull(3))
                                    countryAble.CreatedAt = reader.GetValue(3).ToString();
                                else
                                    countryAble.CreatedAt = null;
                                if (!reader.IsDBNull(4))
                                    countryAble.UpdatedAt = reader.GetValue(4).ToString();
                                else
                                    countryAble.UpdatedAt = null;
                                response.Add(countryAble);
                                countryAble = new CountryAbleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CountryAbleModel>();
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
                response = new List<CountryAbleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CountryAbleModel> Read(string connectionString, List<CountryAbleModel> countryAbleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `country_iso_code_2`, `modified_by`, `created_at`, `updated_at`  FROM gwp_country_ables ";
                if (countryAbleWhere != null && countryAbleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < countryAbleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(countryAbleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, countryAbleWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(countryAbleWhere[i].CountryIsoCode2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2" + i, countryAbleWhere[i].CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(countryAbleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, countryAbleWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(countryAbleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, countryAbleWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(countryAbleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, countryAbleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != countryAbleWhere.Count - 1)
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
                            CountryAbleModel countryAble = new CountryAbleModel(); if (!reader.IsDBNull(0))
                                countryAble.Id = reader.GetValue(0).ToString();
                            else
                                countryAble.Id = null;
                            if (!reader.IsDBNull(1))
                                countryAble.CountryIsoCode2 = reader.GetValue(1).ToString();
                            else
                                countryAble.CountryIsoCode2 = null;
                            if (!reader.IsDBNull(2))
                                countryAble.ModifiedBy = reader.GetValue(2).ToString();
                            else
                                countryAble.ModifiedBy = null;
                            if (!reader.IsDBNull(3))
                                countryAble.CreatedAt = reader.GetValue(3).ToString();
                            else
                                countryAble.CreatedAt = null;
                            if (!reader.IsDBNull(4))
                                countryAble.UpdatedAt = reader.GetValue(4).ToString();
                            else
                                countryAble.UpdatedAt = null;
                            response.Add(countryAble);
                            countryAble = new CountryAbleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CountryAbleModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CountryAbleModel> Create(string connectionString, CountryAbleModel countryAble)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gwp_country_ables (";
                string values = "(";
                if (!string.IsNullOrEmpty(countryAble.Id))
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
                    cmd.Parameters.AddWithValue("@id", countryAble.Id);
                }
                if (!string.IsNullOrEmpty(countryAble.CountryIsoCode2))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country_iso_code_2` ");
                        values = string.Concat(values, ", @countryIsoCode2");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country_iso_code_2` ");
                        values = string.Concat(values, " @countryIsoCode2");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIsoCode2", countryAble.CountryIsoCode2);
                }
                if (!string.IsNullOrEmpty(countryAble.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", countryAble.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(countryAble.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", countryAble.CreatedAt);
                }
                if (!string.IsNullOrEmpty(countryAble.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", countryAble.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                countryAble.Id = cmd.LastInsertedId.ToString();
                response.Add(countryAble);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
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
        public List<CountryAbleModel> Create(string connectionString, List<CountryAbleModel> countryAbleList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryAbleList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gwp_country_ables (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(countryAbleList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, countryAbleList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].CountryIsoCode2))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country_iso_code_2` ");
                                        values = string.Concat(values, ", @countryIsoCode2" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country_iso_code_2` ");
                                        values = string.Concat(values, "@countryIsoCode2" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIsoCode2" + i, countryAbleList[i].CountryIsoCode2);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, countryAbleList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, countryAbleList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, countryAbleList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(countryAbleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryAbleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryAbleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
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

        public List<CountryAbleModel> Update(string connectionString, CountryAbleModel countryAble, CountryAbleModel countryAbleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gwp_country_ables SET ";
                if (!string.IsNullOrEmpty(countryAble.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", countryAble.Id);
                }
                if (!string.IsNullOrEmpty(countryAble.CountryIsoCode2))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso_code_2` = @countryIsoCode2");
                    else
                        sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIsoCode2", countryAble.CountryIsoCode2);
                }
                if (!string.IsNullOrEmpty(countryAble.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", countryAble.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(countryAble.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", countryAble.CreatedAt);
                }
                if (!string.IsNullOrEmpty(countryAble.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", countryAble.UpdatedAt);
                }
                if (countryAbleWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(countryAbleWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", countryAbleWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(countryAbleWhere.CountryIsoCode2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2Where");
                        else
                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCode2Where", countryAbleWhere.CountryIsoCode2);
                    }
                    if (!string.IsNullOrEmpty(countryAbleWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", countryAbleWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(countryAbleWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", countryAbleWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(countryAbleWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", countryAbleWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(countryAble);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
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

        public List<CountryAbleModel> Update(string connectionString, List<CountryAbleModel> countryAbleList, List<CountryAbleModel> countryAbleWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryAbleList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gwp_country_ables SET ";
                                if (!string.IsNullOrEmpty(countryAbleList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, countryAbleList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].CountryIsoCode2))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso_code_2` = @countryIsoCode2" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIsoCode2" + i, countryAbleList[i].CountryIsoCode2);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, countryAbleList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, countryAbleList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(countryAbleList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, countryAbleList[i].UpdatedAt);
                                }
                                if (countryAbleWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(countryAbleWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, countryAbleWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(countryAbleWhereList[i].CountryIsoCode2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2Where" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCode2Where" + i, countryAbleWhereList[i].CountryIsoCode2);
                                    }
                                    if (!string.IsNullOrEmpty(countryAbleWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, countryAbleWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(countryAbleWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, countryAbleWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(countryAbleWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, countryAbleWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(countryAbleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryAbleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryAbleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
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

        public List<CountryAbleModel> Delete(string connectionString, CountryAbleModel countryAble)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gwp_country_ables  ";
                if (countryAble != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(countryAble.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", countryAble.Id);
                    }
                    if (!string.IsNullOrEmpty(countryAble.CountryIsoCode2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2");
                        else
                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCode2", countryAble.CountryIsoCode2);
                    }
                    if (!string.IsNullOrEmpty(countryAble.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", countryAble.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(countryAble.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", countryAble.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(countryAble.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", countryAble.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(countryAble);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
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

        public List<CountryAbleModel> Delete(string connectionString, List<CountryAbleModel> countryAble)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryAbleModel> response = new List<CountryAbleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryAble.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gwp_country_ables  ";
                                if (countryAble[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(countryAble[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, countryAble[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(countryAble[i].CountryIsoCode2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCode2" + i, countryAble[i].CountryIsoCode2);
                                    }
                                    if (!string.IsNullOrEmpty(countryAble[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, countryAble[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(countryAble[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, countryAble[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(countryAble[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, countryAble[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(countryAble[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryAbleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryAbleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryAbleModel>();
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
