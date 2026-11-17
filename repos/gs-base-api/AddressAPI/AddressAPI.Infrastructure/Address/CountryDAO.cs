using AddressAPI.AddressAPI.Domain.Models.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure.Address
{
    class CountryDAO
    {
        private MySqlConnection _connection;
        public List<CountryModel> Read(string connectionString, CountryModel country, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `iso_code_1`, `iso_code_2`, `domestic`, `name`, `available`, `modified_by`, `created_at`, `updated_at`  FROM adrs_countries ";
                {
                    if (country != null)
                    {
                        if (!string.IsNullOrEmpty(country.Id))
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
                            cmd.Parameters.AddWithValue("@id", country.Id);
                        }
                        if (!string.IsNullOrEmpty(country.IsoCode1))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `iso_code_1` = @isoCode1 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `iso_code_1` = @isoCode1 ");
                            }
                            cmd.Parameters.AddWithValue("@isoCode1", country.IsoCode1);
                        }
                        if (!string.IsNullOrEmpty(country.IsoCode2))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `iso_code_2` = @isoCode2 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `iso_code_2` = @isoCode2 ");
                            }
                            cmd.Parameters.AddWithValue("@isoCode2", country.IsoCode2);
                        }
                        if (!string.IsNullOrEmpty(country.Domestic))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `domestic` = @domestic ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `domestic` = @domestic ");
                            }
                            cmd.Parameters.AddWithValue("@domestic", country.Domestic);
                        }
                        if (!string.IsNullOrEmpty(country.Name))
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
                            cmd.Parameters.AddWithValue("@name", country.Name);
                        }
                        if (!string.IsNullOrEmpty(country.Available))
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
                            cmd.Parameters.AddWithValue("@available", country.Available);
                        }
                        if (!string.IsNullOrEmpty(country.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", country.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(country.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", country.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(country.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", country.UpdatedAt);
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
                                    country.Id = reader.GetValue(0).ToString();
                                else
                                    country.Id = null;
                                if (!reader.IsDBNull(1))
                                    country.IsoCode1 = reader.GetValue(1).ToString();
                                else
                                    country.IsoCode1 = null;
                                if (!reader.IsDBNull(2))
                                    country.IsoCode2 = reader.GetValue(2).ToString();
                                else
                                    country.IsoCode2 = null;
                                if (!reader.IsDBNull(3))
                                    country.Domestic = reader.GetValue(3).ToString();
                                else
                                    country.Domestic = null;
                                if (!reader.IsDBNull(4))
                                    country.Name = reader.GetValue(4).ToString();
                                else
                                    country.Name = null;
                                if (!reader.IsDBNull(5))
                                    country.Available = reader.GetValue(5).ToString();
                                else
                                    country.Available = null;
                                if (!reader.IsDBNull(6))
                                    country.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    country.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    country.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    country.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    country.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    country.UpdatedAt = null;
                                response.Add(country);
                                country = new CountryModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<CountryModel>();
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
                response = new List<CountryModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CountryModel> Read(string connectionString, List<CountryModel> countryWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `iso_code_1`, `iso_code_2`, `domestic`, `name`, `available`, `modified_by`, `created_at`, `updated_at`  FROM adrs_countries ";
                if (countryWhere != null && countryWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < countryWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(countryWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, countryWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].IsoCode1))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `iso_code_1` = @isoCode1" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isoCode1" + i, countryWhere[i].IsoCode1);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].IsoCode2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `iso_code_2` = @isoCode2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isoCode2" + i, countryWhere[i].IsoCode2);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].Domestic))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `domestic` = @domestic" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@domestic" + i, countryWhere[i].Domestic);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, countryWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, countryWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, countryWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, countryWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(countryWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, countryWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != countryWhere.Count - 1)
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
                            CountryModel country = new CountryModel(); if (!reader.IsDBNull(0))
                                country.Id = reader.GetValue(0).ToString();
                            else
                                country.Id = null;
                            if (!reader.IsDBNull(1))
                                country.IsoCode1 = reader.GetValue(1).ToString();
                            else
                                country.IsoCode1 = null;
                            if (!reader.IsDBNull(2))
                                country.IsoCode2 = reader.GetValue(2).ToString();
                            else
                                country.IsoCode2 = null;
                            if (!reader.IsDBNull(3))
                                country.Domestic = reader.GetValue(3).ToString();
                            else
                                country.Domestic = null;
                            if (!reader.IsDBNull(4))
                                country.Name = reader.GetValue(4).ToString();
                            else
                                country.Name = null;
                            if (!reader.IsDBNull(5))
                                country.Available = reader.GetValue(5).ToString();
                            else
                                country.Available = null;
                            if (!reader.IsDBNull(6))
                                country.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                country.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                country.CreatedAt = reader.GetValue(7).ToString();
                            else
                                country.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                country.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                country.UpdatedAt = null;
                            response.Add(country);
                            country = new CountryModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<CountryModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<CountryModel> Create(string connectionString, CountryModel country)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO adrs_countries (";
                string values = "(";
                if (!string.IsNullOrEmpty(country.Id))
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
                    cmd.Parameters.AddWithValue("@id", country.Id);
                }
                if (!string.IsNullOrEmpty(country.IsoCode1))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `iso_code_1` ");
                        values = string.Concat(values, ", @isoCode1");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `iso_code_1` ");
                        values = string.Concat(values, " @isoCode1");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode1", country.IsoCode1);
                }
                if (!string.IsNullOrEmpty(country.IsoCode2))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `iso_code_2` ");
                        values = string.Concat(values, ", @isoCode2");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `iso_code_2` ");
                        values = string.Concat(values, " @isoCode2");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode2", country.IsoCode2);
                }
                if (!string.IsNullOrEmpty(country.Domestic))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `domestic` ");
                        values = string.Concat(values, ", @domestic");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `domestic` ");
                        values = string.Concat(values, " @domestic");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@domestic", country.Domestic);
                }
                if (!string.IsNullOrEmpty(country.Name))
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
                    cmd.Parameters.AddWithValue("@name", country.Name);
                }
                if (!string.IsNullOrEmpty(country.Available))
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
                    cmd.Parameters.AddWithValue("@available", country.Available);
                }
                if (!string.IsNullOrEmpty(country.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", country.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(country.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", country.CreatedAt);
                }
                if (!string.IsNullOrEmpty(country.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", country.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                country.Id = cmd.LastInsertedId.ToString();
                response.Add(country);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
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
        public List<CountryModel> Create(string connectionString, List<CountryModel> countryList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO adrs_countries (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(countryList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, countryList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].IsoCode1))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `iso_code_1` ");
                                        values = string.Concat(values, ", @isoCode1" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `iso_code_1` ");
                                        values = string.Concat(values, "@isoCode1" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode1" + i, countryList[i].IsoCode1);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].IsoCode2))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `iso_code_2` ");
                                        values = string.Concat(values, ", @isoCode2" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `iso_code_2` ");
                                        values = string.Concat(values, "@isoCode2" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode2" + i, countryList[i].IsoCode2);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].Domestic))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `domestic` ");
                                        values = string.Concat(values, ", @domestic" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `domestic` ");
                                        values = string.Concat(values, "@domestic" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@domestic" + i, countryList[i].Domestic);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, countryList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, countryList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, countryList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, countryList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, countryList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(countryList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
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

        public List<CountryModel> Update(string connectionString, CountryModel country, CountryModel countryWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE adrs_countries SET ";
                if (!string.IsNullOrEmpty(country.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", country.Id);
                }
                if (!string.IsNullOrEmpty(country.IsoCode1))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `iso_code_1` = @isoCode1");
                    else
                        sql = string.Concat(sql, " `iso_code_1` = @isoCode1");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode1", country.IsoCode1);
                }
                if (!string.IsNullOrEmpty(country.IsoCode2))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `iso_code_2` = @isoCode2");
                    else
                        sql = string.Concat(sql, " `iso_code_2` = @isoCode2");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode2", country.IsoCode2);
                }
                if (!string.IsNullOrEmpty(country.Domestic))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `domestic` = @domestic");
                    else
                        sql = string.Concat(sql, " `domestic` = @domestic");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@domestic", country.Domestic);
                }
                if (!string.IsNullOrEmpty(country.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", country.Name);
                }
                if (!string.IsNullOrEmpty(country.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", country.Available);
                }
                if (!string.IsNullOrEmpty(country.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", country.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(country.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", country.CreatedAt);
                }
                if (!string.IsNullOrEmpty(country.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", country.UpdatedAt);
                }
                if (countryWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(countryWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", countryWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.IsoCode1))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code_1` = @isoCode1Where");
                        else
                            sql = string.Concat(sql, " `iso_code_1` = @isoCode1Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode1Where", countryWhere.IsoCode1);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.IsoCode2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code_2` = @isoCode2Where");
                        else
                            sql = string.Concat(sql, " `iso_code_2` = @isoCode2Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode2Where", countryWhere.IsoCode2);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.Domestic))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `domestic` = @domesticWhere");
                        else
                            sql = string.Concat(sql, " `domestic` = @domesticWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@domesticWhere", countryWhere.Domestic);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", countryWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", countryWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", countryWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", countryWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(countryWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", countryWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(country);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
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

        public List<CountryModel> Update(string connectionString, List<CountryModel> countryList, List<CountryModel> countryWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < countryList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE adrs_countries SET ";
                                if (!string.IsNullOrEmpty(countryList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, countryList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].IsoCode1))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `iso_code_1` = @isoCode1" + i);
                                    else
                                        sql = string.Concat(sql, " `iso_code_1` = @isoCode1" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode1" + i, countryList[i].IsoCode1);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].IsoCode2))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `iso_code_2` = @isoCode2" + i);
                                    else
                                        sql = string.Concat(sql, " `iso_code_2` = @isoCode2" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode2" + i, countryList[i].IsoCode2);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].Domestic))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `domestic` = @domestic" + i);
                                    else
                                        sql = string.Concat(sql, " `domestic` = @domestic" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@domestic" + i, countryList[i].Domestic);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, countryList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, countryList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, countryList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, countryList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(countryList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, countryList[i].UpdatedAt);
                                }
                                if (countryWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(countryWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, countryWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].IsoCode1))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code_1` = @isoCode1Where" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code_1` = @isoCode1Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode1Where" + i, countryWhereList[i].IsoCode1);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].IsoCode2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code_2` = @isoCode2Where" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code_2` = @isoCode2Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode2Where" + i, countryWhereList[i].IsoCode2);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].Domestic))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `domestic` = @domesticWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `domestic` = @domesticWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@domesticWhere" + i, countryWhereList[i].Domestic);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, countryWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, countryWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, countryWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, countryWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(countryWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, countryWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(countryList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
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

        public List<CountryModel> Delete(string connectionString, CountryModel country)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM adrs_countries  ";
                if (country != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(country.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", country.Id);
                    }
                    if (!string.IsNullOrEmpty(country.IsoCode1))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code_1` = @isoCode1");
                        else
                            sql = string.Concat(sql, " `iso_code_1` = @isoCode1");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode1", country.IsoCode1);
                    }
                    if (!string.IsNullOrEmpty(country.IsoCode2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code_2` = @isoCode2");
                        else
                            sql = string.Concat(sql, " `iso_code_2` = @isoCode2");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode2", country.IsoCode2);
                    }
                    if (!string.IsNullOrEmpty(country.Domestic))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `domestic` = @domestic");
                        else
                            sql = string.Concat(sql, " `domestic` = @domestic");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@domestic", country.Domestic);
                    }
                    if (!string.IsNullOrEmpty(country.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", country.Name);
                    }
                    if (!string.IsNullOrEmpty(country.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", country.Available);
                    }
                    if (!string.IsNullOrEmpty(country.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", country.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(country.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", country.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(country.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", country.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(country);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
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

        public List<CountryModel> Delete(string connectionString, List<CountryModel> country)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<CountryModel> response = new List<CountryModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < country.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM adrs_countries  ";
                                if (country[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(country[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, country[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].IsoCode1))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code_1` = @isoCode1" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code_1` = @isoCode1" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode1" + i, country[i].IsoCode1);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].IsoCode2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code_2` = @isoCode2" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code_2` = @isoCode2" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode2" + i, country[i].IsoCode2);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].Domestic))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `domestic` = @domestic" + i);
                                        else
                                            sql = string.Concat(sql, " `domestic` = @domestic" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@domestic" + i, country[i].Domestic);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, country[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, country[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, country[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, country[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(country[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, country[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(country[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<CountryModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<CountryModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<CountryModel>();
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
