using AddressAPI.AddressAPI.Domain.Models.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure.Address
{
    class StatesRegionsProvidenceDAO
    {
        private MySqlConnection _connection;
        public List<StatesRegionsProvidenceModel> Read(string connectionString, StatesRegionsProvidenceModel statesRegionsProvidence, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `name`, `iso_code`, `country_iso_code`, `domestic`, `available`, `modified_by`, `created_at`, `updated_at`  FROM adrs_states_regions_providences ";
                {
                    if (statesRegionsProvidence != null)
                    {
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.Id))
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
                            cmd.Parameters.AddWithValue("@id", statesRegionsProvidence.Id);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.Name))
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
                            cmd.Parameters.AddWithValue("@name", statesRegionsProvidence.Name);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.IsoCode))
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
                            cmd.Parameters.AddWithValue("@isoCode", statesRegionsProvidence.IsoCode);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.CountryIsoCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso_code` = @countryIsoCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso_code` = @countryIsoCode ");
                            }
                            cmd.Parameters.AddWithValue("@countryIsoCode", statesRegionsProvidence.CountryIsoCode);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.Domestic))
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
                            cmd.Parameters.AddWithValue("@domestic", statesRegionsProvidence.Domestic);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.Available))
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
                            cmd.Parameters.AddWithValue("@available", statesRegionsProvidence.Available);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", statesRegionsProvidence.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", statesRegionsProvidence.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidence.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", statesRegionsProvidence.UpdatedAt);
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
                                    statesRegionsProvidence.Id = reader.GetValue(0).ToString();
                                else
                                    statesRegionsProvidence.Id = null;
                                if (!reader.IsDBNull(1))
                                    statesRegionsProvidence.Name = reader.GetValue(1).ToString();
                                else
                                    statesRegionsProvidence.Name = null;
                                if (!reader.IsDBNull(2))
                                    statesRegionsProvidence.IsoCode = reader.GetValue(2).ToString();
                                else
                                    statesRegionsProvidence.IsoCode = null;
                                if (!reader.IsDBNull(3))
                                    statesRegionsProvidence.CountryIsoCode = reader.GetValue(3).ToString();
                                else
                                    statesRegionsProvidence.CountryIsoCode = null;
                                if (!reader.IsDBNull(4))
                                    statesRegionsProvidence.Domestic = reader.GetValue(4).ToString();
                                else
                                    statesRegionsProvidence.Domestic = null;
                                if (!reader.IsDBNull(5))
                                    statesRegionsProvidence.Available = reader.GetValue(5).ToString();
                                else
                                    statesRegionsProvidence.Available = null;
                                if (!reader.IsDBNull(6))
                                    statesRegionsProvidence.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    statesRegionsProvidence.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    statesRegionsProvidence.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    statesRegionsProvidence.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    statesRegionsProvidence.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    statesRegionsProvidence.UpdatedAt = null;
                                response.Add(statesRegionsProvidence);
                                statesRegionsProvidence = new StatesRegionsProvidenceModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<StatesRegionsProvidenceModel>();
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
                response = new List<StatesRegionsProvidenceModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatesRegionsProvidenceModel> Read(string connectionString, List<StatesRegionsProvidenceModel> statesRegionsProvidenceWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `iso_code`, `country_iso_code`, `domestic`, `available`, `modified_by`, `created_at`, `updated_at`  FROM adrs_states_regions_providences ";
                if (statesRegionsProvidenceWhere != null && statesRegionsProvidenceWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < statesRegionsProvidenceWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, statesRegionsProvidenceWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, statesRegionsProvidenceWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].IsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `iso_code` = @isoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isoCode" + i, statesRegionsProvidenceWhere[i].IsoCode);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].CountryIsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code` = @countryIsoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode" + i, statesRegionsProvidenceWhere[i].CountryIsoCode);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].Domestic))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `domestic` = @domestic" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@domestic" + i, statesRegionsProvidenceWhere[i].Domestic);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, statesRegionsProvidenceWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, statesRegionsProvidenceWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, statesRegionsProvidenceWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, statesRegionsProvidenceWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != statesRegionsProvidenceWhere.Count - 1)
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
                            StatesRegionsProvidenceModel statesRegionsProvidence = new StatesRegionsProvidenceModel(); if (!reader.IsDBNull(0))
                                statesRegionsProvidence.Id = reader.GetValue(0).ToString();
                            else
                                statesRegionsProvidence.Id = null;
                            if (!reader.IsDBNull(1))
                                statesRegionsProvidence.Name = reader.GetValue(1).ToString();
                            else
                                statesRegionsProvidence.Name = null;
                            if (!reader.IsDBNull(2))
                                statesRegionsProvidence.IsoCode = reader.GetValue(2).ToString();
                            else
                                statesRegionsProvidence.IsoCode = null;
                            if (!reader.IsDBNull(3))
                                statesRegionsProvidence.CountryIsoCode = reader.GetValue(3).ToString();
                            else
                                statesRegionsProvidence.CountryIsoCode = null;
                            if (!reader.IsDBNull(4))
                                statesRegionsProvidence.Domestic = reader.GetValue(4).ToString();
                            else
                                statesRegionsProvidence.Domestic = null;
                            if (!reader.IsDBNull(5))
                                statesRegionsProvidence.Available = reader.GetValue(5).ToString();
                            else
                                statesRegionsProvidence.Available = null;
                            if (!reader.IsDBNull(6))
                                statesRegionsProvidence.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                statesRegionsProvidence.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                statesRegionsProvidence.CreatedAt = reader.GetValue(7).ToString();
                            else
                                statesRegionsProvidence.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                statesRegionsProvidence.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                statesRegionsProvidence.UpdatedAt = null;
                            response.Add(statesRegionsProvidence);
                            statesRegionsProvidence = new StatesRegionsProvidenceModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<StatesRegionsProvidenceModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<StatesRegionsProvidenceModel> Create(string connectionString, StatesRegionsProvidenceModel statesRegionsProvidence)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO adrs_states_regions_providences (";
                string values = "(";
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Id))
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
                    cmd.Parameters.AddWithValue("@id", statesRegionsProvidence.Id);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Name))
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
                    cmd.Parameters.AddWithValue("@name", statesRegionsProvidence.Name);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.IsoCode))
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
                    cmd.Parameters.AddWithValue("@isoCode", statesRegionsProvidence.IsoCode);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.CountryIsoCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country_iso_code` ");
                        values = string.Concat(values, ", @countryIsoCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country_iso_code` ");
                        values = string.Concat(values, " @countryIsoCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIsoCode", statesRegionsProvidence.CountryIsoCode);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Domestic))
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
                    cmd.Parameters.AddWithValue("@domestic", statesRegionsProvidence.Domestic);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Available))
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
                    cmd.Parameters.AddWithValue("@available", statesRegionsProvidence.Available);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", statesRegionsProvidence.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", statesRegionsProvidence.CreatedAt);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", statesRegionsProvidence.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                statesRegionsProvidence.Id = cmd.LastInsertedId.ToString();
                response.Add(statesRegionsProvidence);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
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
        public List<StatesRegionsProvidenceModel> Create(string connectionString, List<StatesRegionsProvidenceModel> statesRegionsProvidenceList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statesRegionsProvidenceList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO adrs_states_regions_providences (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, statesRegionsProvidenceList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, statesRegionsProvidenceList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].IsoCode))
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
                                    cmd.Parameters.AddWithValue("@isoCode" + i, statesRegionsProvidenceList[i].IsoCode);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].CountryIsoCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country_iso_code` ");
                                        values = string.Concat(values, ", @countryIsoCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country_iso_code` ");
                                        values = string.Concat(values, "@countryIsoCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIsoCode" + i, statesRegionsProvidenceList[i].CountryIsoCode);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Domestic))
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
                                    cmd.Parameters.AddWithValue("@domestic" + i, statesRegionsProvidenceList[i].Domestic);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, statesRegionsProvidenceList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, statesRegionsProvidenceList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, statesRegionsProvidenceList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, statesRegionsProvidenceList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(statesRegionsProvidenceList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<StatesRegionsProvidenceModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatesRegionsProvidenceModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
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

        public List<StatesRegionsProvidenceModel> Update(string connectionString, StatesRegionsProvidenceModel statesRegionsProvidence, StatesRegionsProvidenceModel statesRegionsProvidenceWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE adrs_states_regions_providences SET ";
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", statesRegionsProvidence.Id);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", statesRegionsProvidence.Name);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.IsoCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `iso_code` = @isoCode");
                    else
                        sql = string.Concat(sql, " `iso_code` = @isoCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode", statesRegionsProvidence.IsoCode);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.CountryIsoCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso_code` = @countryIsoCode");
                    else
                        sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIsoCode", statesRegionsProvidence.CountryIsoCode);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Domestic))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `domestic` = @domestic");
                    else
                        sql = string.Concat(sql, " `domestic` = @domestic");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@domestic", statesRegionsProvidence.Domestic);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", statesRegionsProvidence.Available);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", statesRegionsProvidence.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", statesRegionsProvidence.CreatedAt);
                }
                if (!string.IsNullOrEmpty(statesRegionsProvidence.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", statesRegionsProvidence.UpdatedAt);
                }
                if (statesRegionsProvidenceWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", statesRegionsProvidenceWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", statesRegionsProvidenceWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.IsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code` = @isoCodeWhere");
                        else
                            sql = string.Concat(sql, " `iso_code` = @isoCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCodeWhere", statesRegionsProvidenceWhere.IsoCode);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.CountryIsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCodeWhere");
                        else
                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCodeWhere", statesRegionsProvidenceWhere.CountryIsoCode);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.Domestic))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `domestic` = @domesticWhere");
                        else
                            sql = string.Concat(sql, " `domestic` = @domesticWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@domesticWhere", statesRegionsProvidenceWhere.Domestic);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", statesRegionsProvidenceWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", statesRegionsProvidenceWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", statesRegionsProvidenceWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", statesRegionsProvidenceWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(statesRegionsProvidence);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
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

        public List<StatesRegionsProvidenceModel> Update(string connectionString, List<StatesRegionsProvidenceModel> statesRegionsProvidenceList, List<StatesRegionsProvidenceModel> statesRegionsProvidenceWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statesRegionsProvidenceList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE adrs_states_regions_providences SET ";
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, statesRegionsProvidenceList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, statesRegionsProvidenceList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].IsoCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `iso_code` = @isoCode" + i);
                                    else
                                        sql = string.Concat(sql, " `iso_code` = @isoCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode" + i, statesRegionsProvidenceList[i].IsoCode);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].CountryIsoCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso_code` = @countryIsoCode" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIsoCode" + i, statesRegionsProvidenceList[i].CountryIsoCode);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Domestic))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `domestic` = @domestic" + i);
                                    else
                                        sql = string.Concat(sql, " `domestic` = @domestic" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@domestic" + i, statesRegionsProvidenceList[i].Domestic);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, statesRegionsProvidenceList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, statesRegionsProvidenceList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, statesRegionsProvidenceList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(statesRegionsProvidenceList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, statesRegionsProvidenceList[i].UpdatedAt);
                                }
                                if (statesRegionsProvidenceWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, statesRegionsProvidenceWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, statesRegionsProvidenceWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].IsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code` = @isoCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code` = @isoCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCodeWhere" + i, statesRegionsProvidenceWhereList[i].IsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].CountryIsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCodeWhere" + i, statesRegionsProvidenceWhereList[i].CountryIsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].Domestic))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `domestic` = @domesticWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `domestic` = @domesticWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@domesticWhere" + i, statesRegionsProvidenceWhereList[i].Domestic);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, statesRegionsProvidenceWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, statesRegionsProvidenceWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, statesRegionsProvidenceWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidenceWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, statesRegionsProvidenceWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(statesRegionsProvidenceList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<StatesRegionsProvidenceModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatesRegionsProvidenceModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
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

        public List<StatesRegionsProvidenceModel> Delete(string connectionString, StatesRegionsProvidenceModel statesRegionsProvidence)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM adrs_states_regions_providences  ";
                if (statesRegionsProvidence != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", statesRegionsProvidence.Id);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", statesRegionsProvidence.Name);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.IsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code` = @isoCode");
                        else
                            sql = string.Concat(sql, " `iso_code` = @isoCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode", statesRegionsProvidence.IsoCode);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.CountryIsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCode");
                        else
                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCode", statesRegionsProvidence.CountryIsoCode);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.Domestic))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `domestic` = @domestic");
                        else
                            sql = string.Concat(sql, " `domestic` = @domestic");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@domestic", statesRegionsProvidence.Domestic);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", statesRegionsProvidence.Available);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", statesRegionsProvidence.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", statesRegionsProvidence.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(statesRegionsProvidence.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", statesRegionsProvidence.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(statesRegionsProvidence);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
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

        public List<StatesRegionsProvidenceModel> Delete(string connectionString, List<StatesRegionsProvidenceModel> statesRegionsProvidence)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<StatesRegionsProvidenceModel> response = new List<StatesRegionsProvidenceModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < statesRegionsProvidence.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM adrs_states_regions_providences  ";
                                if (statesRegionsProvidence[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, statesRegionsProvidence[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, statesRegionsProvidence[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].IsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code` = @isoCode" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code` = @isoCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode" + i, statesRegionsProvidence[i].IsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].CountryIsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCode" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCode" + i, statesRegionsProvidence[i].CountryIsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].Domestic))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `domestic` = @domestic" + i);
                                        else
                                            sql = string.Concat(sql, " `domestic` = @domestic" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@domestic" + i, statesRegionsProvidence[i].Domestic);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, statesRegionsProvidence[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, statesRegionsProvidence[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, statesRegionsProvidence[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(statesRegionsProvidence[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, statesRegionsProvidence[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(statesRegionsProvidence[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<StatesRegionsProvidenceModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<StatesRegionsProvidenceModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<StatesRegionsProvidenceModel>();
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
