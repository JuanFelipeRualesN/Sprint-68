using AddressAPI.AddressAPI.Domain.Models.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure.Address
{
    class ZipCodeIsoDAO
    {
        private MySqlConnection _connection;
        public List<ZipCodeIsoModel> Read(string connectionString, ZipCodeIsoModel zipCodeIso, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `state_name`, `zip_code_from`, `zip_code_to`, `iso_code`, `country_iso_code`, `modified_by`, `created_at`, `updated_at`  FROM adrs_zip_code_isos ";
                {
                    if (zipCodeIso != null)
                    {
                        if (!string.IsNullOrEmpty(zipCodeIso.Id))
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
                            cmd.Parameters.AddWithValue("@id", zipCodeIso.Id);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.StateName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_name` = @stateName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_name` = @stateName ");
                            }
                            cmd.Parameters.AddWithValue("@stateName", zipCodeIso.StateName);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `zip_code_from` = @zipCodeFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `zip_code_from` = @zipCodeFrom ");
                            }
                            cmd.Parameters.AddWithValue("@zipCodeFrom", zipCodeIso.ZipCodeFrom);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeTo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `zip_code_to` = @zipCodeTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `zip_code_to` = @zipCodeTo ");
                            }
                            cmd.Parameters.AddWithValue("@zipCodeTo", zipCodeIso.ZipCodeTo);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.IsoCode))
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
                            cmd.Parameters.AddWithValue("@isoCode", zipCodeIso.IsoCode);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.CountryIsoCode))
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
                            cmd.Parameters.AddWithValue("@countryIsoCode", zipCodeIso.CountryIsoCode);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", zipCodeIso.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", zipCodeIso.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIso.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", zipCodeIso.UpdatedAt);
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
                                    zipCodeIso.Id = reader.GetValue(0).ToString();
                                else
                                    zipCodeIso.Id = null;
                                if (!reader.IsDBNull(1))
                                    zipCodeIso.StateName = reader.GetValue(1).ToString();
                                else
                                    zipCodeIso.StateName = null;
                                if (!reader.IsDBNull(2))
                                    zipCodeIso.ZipCodeFrom = reader.GetValue(2).ToString();
                                else
                                    zipCodeIso.ZipCodeFrom = null;
                                if (!reader.IsDBNull(3))
                                    zipCodeIso.ZipCodeTo = reader.GetValue(3).ToString();
                                else
                                    zipCodeIso.ZipCodeTo = null;
                                if (!reader.IsDBNull(4))
                                    zipCodeIso.IsoCode = reader.GetValue(4).ToString();
                                else
                                    zipCodeIso.IsoCode = null;
                                if (!reader.IsDBNull(5))
                                    zipCodeIso.CountryIsoCode = reader.GetValue(5).ToString();
                                else
                                    zipCodeIso.CountryIsoCode = null;
                                if (!reader.IsDBNull(6))
                                    zipCodeIso.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    zipCodeIso.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    zipCodeIso.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    zipCodeIso.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    zipCodeIso.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    zipCodeIso.UpdatedAt = null;
                                response.Add(zipCodeIso);
                                zipCodeIso = new ZipCodeIsoModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<ZipCodeIsoModel>();
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
                response = new List<ZipCodeIsoModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ZipCodeIsoModel> Read(string connectionString, List<ZipCodeIsoModel> zipCodeIsoWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `state_name`, `zip_code_from`, `zip_code_to`, `iso_code`, `country_iso_code`, `modified_by`, `created_at`, `updated_at`  FROM adrs_zip_code_isos ";
                if (zipCodeIsoWhere != null && zipCodeIsoWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < zipCodeIsoWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, zipCodeIsoWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].StateName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_name` = @stateName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateName" + i, zipCodeIsoWhere[i].StateName);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].ZipCodeFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `zip_code_from` = @zipCodeFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@zipCodeFrom" + i, zipCodeIsoWhere[i].ZipCodeFrom);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].ZipCodeTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `zip_code_to` = @zipCodeTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@zipCodeTo" + i, zipCodeIsoWhere[i].ZipCodeTo);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].IsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `iso_code` = @isoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isoCode" + i, zipCodeIsoWhere[i].IsoCode);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].CountryIsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code` = @countryIsoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode" + i, zipCodeIsoWhere[i].CountryIsoCode);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, zipCodeIsoWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, zipCodeIsoWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(zipCodeIsoWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, zipCodeIsoWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != zipCodeIsoWhere.Count - 1)
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
                            ZipCodeIsoModel zipCodeIso = new ZipCodeIsoModel(); if (!reader.IsDBNull(0))
                                zipCodeIso.Id = reader.GetValue(0).ToString();
                            else
                                zipCodeIso.Id = null;
                            if (!reader.IsDBNull(1))
                                zipCodeIso.StateName = reader.GetValue(1).ToString();
                            else
                                zipCodeIso.StateName = null;
                            if (!reader.IsDBNull(2))
                                zipCodeIso.ZipCodeFrom = reader.GetValue(2).ToString();
                            else
                                zipCodeIso.ZipCodeFrom = null;
                            if (!reader.IsDBNull(3))
                                zipCodeIso.ZipCodeTo = reader.GetValue(3).ToString();
                            else
                                zipCodeIso.ZipCodeTo = null;
                            if (!reader.IsDBNull(4))
                                zipCodeIso.IsoCode = reader.GetValue(4).ToString();
                            else
                                zipCodeIso.IsoCode = null;
                            if (!reader.IsDBNull(5))
                                zipCodeIso.CountryIsoCode = reader.GetValue(5).ToString();
                            else
                                zipCodeIso.CountryIsoCode = null;
                            if (!reader.IsDBNull(6))
                                zipCodeIso.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                zipCodeIso.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                zipCodeIso.CreatedAt = reader.GetValue(7).ToString();
                            else
                                zipCodeIso.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                zipCodeIso.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                zipCodeIso.UpdatedAt = null;
                            response.Add(zipCodeIso);
                            zipCodeIso = new ZipCodeIsoModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<ZipCodeIsoModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<ZipCodeIsoModel> Create(string connectionString, ZipCodeIsoModel zipCodeIso)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO adrs_zip_code_isos (";
                string values = "(";
                if (!string.IsNullOrEmpty(zipCodeIso.Id))
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
                    cmd.Parameters.AddWithValue("@id", zipCodeIso.Id);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.StateName))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `state_name` ");
                        values = string.Concat(values, ", @stateName");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `state_name` ");
                        values = string.Concat(values, " @stateName");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateName", zipCodeIso.StateName);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeFrom))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `zip_code_from` ");
                        values = string.Concat(values, ", @zipCodeFrom");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `zip_code_from` ");
                        values = string.Concat(values, " @zipCodeFrom");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@zipCodeFrom", zipCodeIso.ZipCodeFrom);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeTo))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `zip_code_to` ");
                        values = string.Concat(values, ", @zipCodeTo");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `zip_code_to` ");
                        values = string.Concat(values, " @zipCodeTo");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@zipCodeTo", zipCodeIso.ZipCodeTo);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.IsoCode))
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
                    cmd.Parameters.AddWithValue("@isoCode", zipCodeIso.IsoCode);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.CountryIsoCode))
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
                    cmd.Parameters.AddWithValue("@countryIsoCode", zipCodeIso.CountryIsoCode);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", zipCodeIso.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", zipCodeIso.CreatedAt);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", zipCodeIso.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                zipCodeIso.Id = cmd.LastInsertedId.ToString();
                response.Add(zipCodeIso);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
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
        public List<ZipCodeIsoModel> Create(string connectionString, List<ZipCodeIsoModel> zipCodeIsoList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < zipCodeIsoList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO adrs_zip_code_isos (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, zipCodeIsoList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].StateName))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `state_name` ");
                                        values = string.Concat(values, ", @stateName" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `state_name` ");
                                        values = string.Concat(values, "@stateName" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateName" + i, zipCodeIsoList[i].StateName);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].ZipCodeFrom))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `zip_code_from` ");
                                        values = string.Concat(values, ", @zipCodeFrom" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `zip_code_from` ");
                                        values = string.Concat(values, "@zipCodeFrom" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@zipCodeFrom" + i, zipCodeIsoList[i].ZipCodeFrom);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].ZipCodeTo))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `zip_code_to` ");
                                        values = string.Concat(values, ", @zipCodeTo" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `zip_code_to` ");
                                        values = string.Concat(values, "@zipCodeTo" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@zipCodeTo" + i, zipCodeIsoList[i].ZipCodeTo);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].IsoCode))
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
                                    cmd.Parameters.AddWithValue("@isoCode" + i, zipCodeIsoList[i].IsoCode);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].CountryIsoCode))
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
                                    cmd.Parameters.AddWithValue("@countryIsoCode" + i, zipCodeIsoList[i].CountryIsoCode);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, zipCodeIsoList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, zipCodeIsoList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, zipCodeIsoList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(zipCodeIsoList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<ZipCodeIsoModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ZipCodeIsoModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
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

        public List<ZipCodeIsoModel> Update(string connectionString, ZipCodeIsoModel zipCodeIso, ZipCodeIsoModel zipCodeIsoWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE adrs_zip_code_isos SET ";
                if (!string.IsNullOrEmpty(zipCodeIso.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", zipCodeIso.Id);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.StateName))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `state_name` = @stateName");
                    else
                        sql = string.Concat(sql, " `state_name` = @stateName");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateName", zipCodeIso.StateName);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeFrom))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `zip_code_from` = @zipCodeFrom");
                    else
                        sql = string.Concat(sql, " `zip_code_from` = @zipCodeFrom");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@zipCodeFrom", zipCodeIso.ZipCodeFrom);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeTo))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `zip_code_to` = @zipCodeTo");
                    else
                        sql = string.Concat(sql, " `zip_code_to` = @zipCodeTo");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@zipCodeTo", zipCodeIso.ZipCodeTo);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.IsoCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `iso_code` = @isoCode");
                    else
                        sql = string.Concat(sql, " `iso_code` = @isoCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isoCode", zipCodeIso.IsoCode);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.CountryIsoCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso_code` = @countryIsoCode");
                    else
                        sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIsoCode", zipCodeIso.CountryIsoCode);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", zipCodeIso.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", zipCodeIso.CreatedAt);
                }
                if (!string.IsNullOrEmpty(zipCodeIso.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", zipCodeIso.UpdatedAt);
                }
                if (zipCodeIsoWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", zipCodeIsoWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.StateName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_name` = @stateNameWhere");
                        else
                            sql = string.Concat(sql, " `state_name` = @stateNameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateNameWhere", zipCodeIsoWhere.StateName);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.ZipCodeFrom))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `zip_code_from` = @zipCodeFromWhere");
                        else
                            sql = string.Concat(sql, " `zip_code_from` = @zipCodeFromWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@zipCodeFromWhere", zipCodeIsoWhere.ZipCodeFrom);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.ZipCodeTo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `zip_code_to` = @zipCodeToWhere");
                        else
                            sql = string.Concat(sql, " `zip_code_to` = @zipCodeToWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@zipCodeToWhere", zipCodeIsoWhere.ZipCodeTo);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.IsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code` = @isoCodeWhere");
                        else
                            sql = string.Concat(sql, " `iso_code` = @isoCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCodeWhere", zipCodeIsoWhere.IsoCode);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.CountryIsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCodeWhere");
                        else
                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCodeWhere", zipCodeIsoWhere.CountryIsoCode);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", zipCodeIsoWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", zipCodeIsoWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIsoWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", zipCodeIsoWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(zipCodeIso);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
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

        public List<ZipCodeIsoModel> Update(string connectionString, List<ZipCodeIsoModel> zipCodeIsoList, List<ZipCodeIsoModel> zipCodeIsoWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < zipCodeIsoList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE adrs_zip_code_isos SET ";
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, zipCodeIsoList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].StateName))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `state_name` = @stateName" + i);
                                    else
                                        sql = string.Concat(sql, " `state_name` = @stateName" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateName" + i, zipCodeIsoList[i].StateName);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].ZipCodeFrom))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `zip_code_from` = @zipCodeFrom" + i);
                                    else
                                        sql = string.Concat(sql, " `zip_code_from` = @zipCodeFrom" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@zipCodeFrom" + i, zipCodeIsoList[i].ZipCodeFrom);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].ZipCodeTo))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `zip_code_to` = @zipCodeTo" + i);
                                    else
                                        sql = string.Concat(sql, " `zip_code_to` = @zipCodeTo" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@zipCodeTo" + i, zipCodeIsoList[i].ZipCodeTo);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].IsoCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `iso_code` = @isoCode" + i);
                                    else
                                        sql = string.Concat(sql, " `iso_code` = @isoCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isoCode" + i, zipCodeIsoList[i].IsoCode);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].CountryIsoCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso_code` = @countryIsoCode" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIsoCode" + i, zipCodeIsoList[i].CountryIsoCode);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, zipCodeIsoList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, zipCodeIsoList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(zipCodeIsoList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, zipCodeIsoList[i].UpdatedAt);
                                }
                                if (zipCodeIsoWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, zipCodeIsoWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].StateName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_name` = @stateNameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `state_name` = @stateNameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateNameWhere" + i, zipCodeIsoWhereList[i].StateName);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].ZipCodeFrom))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `zip_code_from` = @zipCodeFromWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `zip_code_from` = @zipCodeFromWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@zipCodeFromWhere" + i, zipCodeIsoWhereList[i].ZipCodeFrom);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].ZipCodeTo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `zip_code_to` = @zipCodeToWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `zip_code_to` = @zipCodeToWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@zipCodeToWhere" + i, zipCodeIsoWhereList[i].ZipCodeTo);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].IsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code` = @isoCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code` = @isoCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCodeWhere" + i, zipCodeIsoWhereList[i].IsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].CountryIsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCodeWhere" + i, zipCodeIsoWhereList[i].CountryIsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, zipCodeIsoWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, zipCodeIsoWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIsoWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, zipCodeIsoWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(zipCodeIsoList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ZipCodeIsoModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ZipCodeIsoModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
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

        public List<ZipCodeIsoModel> Delete(string connectionString, ZipCodeIsoModel zipCodeIso)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM adrs_zip_code_isos  ";
                if (zipCodeIso != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(zipCodeIso.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", zipCodeIso.Id);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.StateName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_name` = @stateName");
                        else
                            sql = string.Concat(sql, " `state_name` = @stateName");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateName", zipCodeIso.StateName);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeFrom))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `zip_code_from` = @zipCodeFrom");
                        else
                            sql = string.Concat(sql, " `zip_code_from` = @zipCodeFrom");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@zipCodeFrom", zipCodeIso.ZipCodeFrom);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.ZipCodeTo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `zip_code_to` = @zipCodeTo");
                        else
                            sql = string.Concat(sql, " `zip_code_to` = @zipCodeTo");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@zipCodeTo", zipCodeIso.ZipCodeTo);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.IsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `iso_code` = @isoCode");
                        else
                            sql = string.Concat(sql, " `iso_code` = @isoCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isoCode", zipCodeIso.IsoCode);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.CountryIsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCode");
                        else
                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCode", zipCodeIso.CountryIsoCode);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", zipCodeIso.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", zipCodeIso.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(zipCodeIso.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", zipCodeIso.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(zipCodeIso);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
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

        public List<ZipCodeIsoModel> Delete(string connectionString, List<ZipCodeIsoModel> zipCodeIso)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<ZipCodeIsoModel> response = new List<ZipCodeIsoModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < zipCodeIso.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM adrs_zip_code_isos  ";
                                if (zipCodeIso[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, zipCodeIso[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].StateName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_name` = @stateName" + i);
                                        else
                                            sql = string.Concat(sql, " `state_name` = @stateName" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateName" + i, zipCodeIso[i].StateName);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].ZipCodeFrom))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `zip_code_from` = @zipCodeFrom" + i);
                                        else
                                            sql = string.Concat(sql, " `zip_code_from` = @zipCodeFrom" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@zipCodeFrom" + i, zipCodeIso[i].ZipCodeFrom);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].ZipCodeTo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `zip_code_to` = @zipCodeTo" + i);
                                        else
                                            sql = string.Concat(sql, " `zip_code_to` = @zipCodeTo" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@zipCodeTo" + i, zipCodeIso[i].ZipCodeTo);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].IsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `iso_code` = @isoCode" + i);
                                        else
                                            sql = string.Concat(sql, " `iso_code` = @isoCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isoCode" + i, zipCodeIso[i].IsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].CountryIsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code` = @countryIsoCode" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code` = @countryIsoCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCode" + i, zipCodeIso[i].CountryIsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, zipCodeIso[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, zipCodeIso[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(zipCodeIso[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, zipCodeIso[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(zipCodeIso[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<ZipCodeIsoModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<ZipCodeIsoModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<ZipCodeIsoModel>();
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
