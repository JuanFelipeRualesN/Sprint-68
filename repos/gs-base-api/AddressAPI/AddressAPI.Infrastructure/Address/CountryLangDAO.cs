using AddressAPI.AddressAPI.Domain.Models.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure.Address
{
    class CountryLangDAO
    {
        private MySqlConnection _connection;
        public List<CountryLangModel> Read(string connectionString, CountryLangModel countryLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `country_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM adrs_country_langs ";
                {
                    if (countryLang != null)
                    {
                        if (!string.IsNullOrEmpty(countryLang.CountryId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_id` = @countryId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_id` = @countryId ");
                            }
                            cmd.Parameters.AddWithValue("@countryId", countryLang.CountryId);
                        }
                        if (!string.IsNullOrEmpty(countryLang.LangCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lang_code` = @langCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lang_code` = @langCode ");
                            }
                            cmd.Parameters.AddWithValue("@langCode", countryLang.LangCode);
                        }
                        if (!string.IsNullOrEmpty(countryLang.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", countryLang.Desc);
                        }
                        if (!string.IsNullOrEmpty(countryLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", countryLang.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(countryLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", countryLang.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(countryLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", countryLang.UpdatedAt);
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
                                    countryLang.CountryId = reader.GetValue(0).ToString();
                                else
                                    countryLang.CountryId = null;
                                if (!reader.IsDBNull(1))
                                    countryLang.LangCode = reader.GetValue(1).ToString();
                                else
                                    countryLang.LangCode = null;
                                if (!reader.IsDBNull(2))
                                    countryLang.Desc = reader.GetValue(2).ToString();
                                else
                                    countryLang.Desc = null;
                                if (!reader.IsDBNull(3))
                                    countryLang.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    countryLang.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    countryLang.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    countryLang.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    countryLang.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    countryLang.UpdatedAt = null;
                                response.Add(countryLang);
                                countryLang = new CountryLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CountryLangModel>();
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
                response = new List<CountryLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CountryLangModel> Read(string connectionString, List<CountryLangModel> countryLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `country_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM adrs_country_langs ";
                if (countryLangWhere != null && countryLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < countryLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(countryLangWhere[i].CountryId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_id` = @countryId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryId" + i, countryLangWhere[i].CountryId);
                        }
                        if (!string.IsNullOrEmpty(countryLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, countryLangWhere[i].LangCode);
                        }
                        if (!string.IsNullOrEmpty(countryLangWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, countryLangWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(countryLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, countryLangWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(countryLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, countryLangWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(countryLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, countryLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != countryLangWhere.Count - 1)
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
                            CountryLangModel countryLang = new CountryLangModel(); if (!reader.IsDBNull(0))
                                countryLang.CountryId = reader.GetValue(0).ToString();
                            else
                                countryLang.CountryId = null;
                            if (!reader.IsDBNull(1))
                                countryLang.LangCode = reader.GetValue(1).ToString();
                            else
                                countryLang.LangCode = null;
                            if (!reader.IsDBNull(2))
                                countryLang.Desc = reader.GetValue(2).ToString();
                            else
                                countryLang.Desc = null;
                            if (!reader.IsDBNull(3))
                                countryLang.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                countryLang.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                countryLang.CreatedAt = reader.GetValue(4).ToString();
                            else
                                countryLang.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                countryLang.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                countryLang.UpdatedAt = null;
                            response.Add(countryLang);
                            countryLang = new CountryLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CountryLangModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CountryLangModel> Create(string connectionString, CountryLangModel countryLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO adrs_country_langs (";
                string values = "(";
                if (!string.IsNullOrEmpty(countryLang.CountryId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country_id` ");
                        values = string.Concat(values, ", @countryId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country_id` ");
                        values = string.Concat(values, " @countryId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryId", countryLang.CountryId);
                }
                if (!string.IsNullOrEmpty(countryLang.LangCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `lang_code` ");
                        values = string.Concat(values, ", @langCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `lang_code` ");
                        values = string.Concat(values, " @langCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", countryLang.LangCode);
                }
                if (!string.IsNullOrEmpty(countryLang.Desc))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `desc` ");
                        values = string.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `desc` ");
                        values = string.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", countryLang.Desc);
                }
                if (!string.IsNullOrEmpty(countryLang.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", countryLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(countryLang.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", countryLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(countryLang.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", countryLang.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                response.Add(countryLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
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
        public List<CountryLangModel> Create(string connectionString, List<CountryLangModel> countryLangList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryLangList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO adrs_country_langs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(countryLangList[i].CountryId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country_id` ");
                                        values = string.Concat(values, ", @countryId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country_id` ");
                                        values = string.Concat(values, "@countryId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryId" + i, countryLangList[i].CountryId);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].LangCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `lang_code` ");
                                        values = string.Concat(values, ", @langCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `lang_code` ");
                                        values = string.Concat(values, "@langCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, countryLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `desc` ");
                                        values = string.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `desc` ");
                                        values = string.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, countryLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, countryLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, countryLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, countryLangList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(countryLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
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

        public List<CountryLangModel> Update(string connectionString, CountryLangModel countryLang, CountryLangModel countryLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE adrs_country_langs SET ";
                if (!string.IsNullOrEmpty(countryLang.CountryId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_id` = @countryId");
                    else
                        sql = string.Concat(sql, " `country_id` = @countryId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryId", countryLang.CountryId);
                }
                if (!string.IsNullOrEmpty(countryLang.LangCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = string.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", countryLang.LangCode);
                }
                if (!string.IsNullOrEmpty(countryLang.Desc))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `desc` = @desc");
                    else
                        sql = string.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", countryLang.Desc);
                }
                if (!string.IsNullOrEmpty(countryLang.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", countryLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(countryLang.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", countryLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(countryLang.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", countryLang.UpdatedAt);
                }
                if (countryLangWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(countryLangWhere.CountryId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_id` = @countryIdWhere");
                        else
                            sql = string.Concat(sql, " `country_id` = @countryIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIdWhere", countryLangWhere.CountryId);
                    }
                    if (!string.IsNullOrEmpty(countryLangWhere.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", countryLangWhere.LangCode);
                    }
                    if (!string.IsNullOrEmpty(countryLangWhere.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = string.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", countryLangWhere.Desc);
                    }
                    if (!string.IsNullOrEmpty(countryLangWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", countryLangWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(countryLangWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", countryLangWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(countryLangWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", countryLangWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(countryLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
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

        public List<CountryLangModel> Update(string connectionString, List<CountryLangModel> countryLangList, List<CountryLangModel> countryLangWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryLangList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE adrs_country_langs SET ";
                                if (!string.IsNullOrEmpty(countryLangList[i].CountryId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_id` = @countryId" + i);
                                    else
                                        sql = string.Concat(sql, " `country_id` = @countryId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryId" + i, countryLangList[i].CountryId);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, countryLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = string.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, countryLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, countryLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, countryLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(countryLangList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, countryLangList[i].UpdatedAt);
                                }
                                if (countryLangWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(countryLangWhereList[i].CountryId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_id` = @countryIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_id` = @countryIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIdWhere" + i, countryLangWhereList[i].CountryId);
                                    }
                                    if (!string.IsNullOrEmpty(countryLangWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, countryLangWhereList[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(countryLangWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, countryLangWhereList[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(countryLangWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, countryLangWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(countryLangWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, countryLangWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(countryLangWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, countryLangWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(countryLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
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

        public List<CountryLangModel> Delete(string connectionString, CountryLangModel countryLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM adrs_country_langs  ";
                if (countryLang != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(countryLang.CountryId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_id` = @countryId");
                        else
                            sql = string.Concat(sql, " `country_id` = @countryId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryId", countryLang.CountryId);
                    }
                    if (!string.IsNullOrEmpty(countryLang.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", countryLang.LangCode);
                    }
                    if (!string.IsNullOrEmpty(countryLang.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = string.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", countryLang.Desc);
                    }
                    if (!string.IsNullOrEmpty(countryLang.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", countryLang.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(countryLang.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", countryLang.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(countryLang.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", countryLang.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(countryLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
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

        public List<CountryLangModel> Delete(string connectionString, List<CountryLangModel> countryLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryLangModel> response = new List<CountryLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryLang.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM adrs_country_langs  ";
                                if (countryLang[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(countryLang[i].CountryId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_id` = @countryId" + i);
                                        else
                                            sql = string.Concat(sql, " `country_id` = @countryId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryId" + i, countryLang[i].CountryId);
                                    }
                                    if (!string.IsNullOrEmpty(countryLang[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, countryLang[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(countryLang[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, countryLang[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(countryLang[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, countryLang[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(countryLang[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, countryLang[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(countryLang[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, countryLang[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(countryLang[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryLangModel>();
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
