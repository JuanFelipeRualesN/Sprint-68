using AddressAPI.AddressAPI.Domain.Models.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure.Address
{
    class AddressExtraIdentifierDAO
    {
        private MySqlConnection _connection;
        public List<AddressExtraIdentifierModel> Read(string connectionString, AddressExtraIdentifierModel addressExtraIdentifier, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `country_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM adrs_address_extra_identifiers ";
                {
                    if (addressExtraIdentifier != null)
                    {
                        if (!string.IsNullOrEmpty(addressExtraIdentifier.Id))
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
                            cmd.Parameters.AddWithValue("@id", addressExtraIdentifier.Id);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifier.CountryId))
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
                            cmd.Parameters.AddWithValue("@countryId", addressExtraIdentifier.CountryId);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifier.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", addressExtraIdentifier.Desc);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifier.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifier.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifier.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifier.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifier.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifier.UpdatedAt);
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
                                    addressExtraIdentifier.Id = reader.GetValue(0).ToString();
                                else
                                    addressExtraIdentifier.Id = null;
                                if (!reader.IsDBNull(1))
                                    addressExtraIdentifier.CountryId = reader.GetValue(1).ToString();
                                else
                                    addressExtraIdentifier.CountryId = null;
                                if (!reader.IsDBNull(2))
                                    addressExtraIdentifier.Desc = reader.GetValue(2).ToString();
                                else
                                    addressExtraIdentifier.Desc = null;
                                if (!reader.IsDBNull(3))
                                    addressExtraIdentifier.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    addressExtraIdentifier.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    addressExtraIdentifier.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    addressExtraIdentifier.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    addressExtraIdentifier.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    addressExtraIdentifier.UpdatedAt = null;
                                response.Add(addressExtraIdentifier);
                                addressExtraIdentifier = new AddressExtraIdentifierModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AddressExtraIdentifierModel>();
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
                response = new List<AddressExtraIdentifierModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressExtraIdentifierModel> Read(string connectionString, List<AddressExtraIdentifierModel> addressExtraIdentifierWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `country_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM adrs_address_extra_identifiers ";
                if (addressExtraIdentifierWhere != null && addressExtraIdentifierWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < addressExtraIdentifierWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(addressExtraIdentifierWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, addressExtraIdentifierWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierWhere[i].CountryId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_id` = @countryId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryId" + i, addressExtraIdentifierWhere[i].CountryId);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != addressExtraIdentifierWhere.Count - 1)
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
                            AddressExtraIdentifierModel addressExtraIdentifier = new AddressExtraIdentifierModel(); if (!reader.IsDBNull(0))
                                addressExtraIdentifier.Id = reader.GetValue(0).ToString();
                            else
                                addressExtraIdentifier.Id = null;
                            if (!reader.IsDBNull(1))
                                addressExtraIdentifier.CountryId = reader.GetValue(1).ToString();
                            else
                                addressExtraIdentifier.CountryId = null;
                            if (!reader.IsDBNull(2))
                                addressExtraIdentifier.Desc = reader.GetValue(2).ToString();
                            else
                                addressExtraIdentifier.Desc = null;
                            if (!reader.IsDBNull(3))
                                addressExtraIdentifier.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                addressExtraIdentifier.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                addressExtraIdentifier.CreatedAt = reader.GetValue(4).ToString();
                            else
                                addressExtraIdentifier.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                addressExtraIdentifier.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                addressExtraIdentifier.UpdatedAt = null;
                            response.Add(addressExtraIdentifier);
                            addressExtraIdentifier = new AddressExtraIdentifierModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AddressExtraIdentifierModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressExtraIdentifierModel> Create(string connectionString, AddressExtraIdentifierModel addressExtraIdentifier)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO adrs_address_extra_identifiers (";
                string values = "(";
                if (!string.IsNullOrEmpty(addressExtraIdentifier.Id))
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
                    cmd.Parameters.AddWithValue("@id", addressExtraIdentifier.Id);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.CountryId))
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
                    cmd.Parameters.AddWithValue("@countryId", addressExtraIdentifier.CountryId);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.Desc))
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
                    cmd.Parameters.AddWithValue("@desc", addressExtraIdentifier.Desc);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifier.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifier.CreatedAt);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifier.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                addressExtraIdentifier.Id = cmd.LastInsertedId.ToString();
                response.Add(addressExtraIdentifier);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
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
        public List<AddressExtraIdentifierModel> Create(string connectionString, List<AddressExtraIdentifierModel> addressExtraIdentifierList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < addressExtraIdentifierList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO adrs_address_extra_identifiers (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, addressExtraIdentifierList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].CountryId))
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
                                    cmd.Parameters.AddWithValue("@countryId" + i, addressExtraIdentifierList[i].CountryId);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].Desc))
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
                                    cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(addressExtraIdentifierList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressExtraIdentifierModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressExtraIdentifierModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
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

        public List<AddressExtraIdentifierModel> Update(string connectionString, AddressExtraIdentifierModel addressExtraIdentifier, AddressExtraIdentifierModel addressExtraIdentifierWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE adrs_address_extra_identifiers SET ";
                if (!string.IsNullOrEmpty(addressExtraIdentifier.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", addressExtraIdentifier.Id);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.CountryId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_id` = @countryId");
                    else
                        sql = string.Concat(sql, " `country_id` = @countryId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryId", addressExtraIdentifier.CountryId);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.Desc))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `desc` = @desc");
                    else
                        sql = string.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", addressExtraIdentifier.Desc);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifier.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifier.CreatedAt);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifier.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifier.UpdatedAt);
                }
                if (addressExtraIdentifierWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", addressExtraIdentifierWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhere.CountryId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_id` = @countryIdWhere");
                        else
                            sql = string.Concat(sql, " `country_id` = @countryIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIdWhere", addressExtraIdentifierWhere.CountryId);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhere.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = string.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", addressExtraIdentifierWhere.Desc);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", addressExtraIdentifierWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", addressExtraIdentifierWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", addressExtraIdentifierWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(addressExtraIdentifier);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
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

        public List<AddressExtraIdentifierModel> Update(string connectionString, List<AddressExtraIdentifierModel> addressExtraIdentifierList, List<AddressExtraIdentifierModel> addressExtraIdentifierWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < addressExtraIdentifierList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE adrs_address_extra_identifiers SET ";
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, addressExtraIdentifierList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].CountryId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_id` = @countryId" + i);
                                    else
                                        sql = string.Concat(sql, " `country_id` = @countryId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryId" + i, addressExtraIdentifierList[i].CountryId);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = string.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierList[i].UpdatedAt);
                                }
                                if (addressExtraIdentifierWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, addressExtraIdentifierWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhereList[i].CountryId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_id` = @countryIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_id` = @countryIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIdWhere" + i, addressExtraIdentifierWhereList[i].CountryId);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, addressExtraIdentifierWhereList[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, addressExtraIdentifierWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, addressExtraIdentifierWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, addressExtraIdentifierWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(addressExtraIdentifierList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressExtraIdentifierModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressExtraIdentifierModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
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

        public List<AddressExtraIdentifierModel> Delete(string connectionString, AddressExtraIdentifierModel addressExtraIdentifier)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM adrs_address_extra_identifiers  ";
                if (addressExtraIdentifier != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(addressExtraIdentifier.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", addressExtraIdentifier.Id);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifier.CountryId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_id` = @countryId");
                        else
                            sql = string.Concat(sql, " `country_id` = @countryId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryId", addressExtraIdentifier.CountryId);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifier.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = string.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", addressExtraIdentifier.Desc);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifier.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifier.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifier.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifier.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifier.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifier.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(addressExtraIdentifier);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
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

        public List<AddressExtraIdentifierModel> Delete(string connectionString, List<AddressExtraIdentifierModel> addressExtraIdentifier)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierModel> response = new List<AddressExtraIdentifierModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < addressExtraIdentifier.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM adrs_address_extra_identifiers  ";
                                if (addressExtraIdentifier[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(addressExtraIdentifier[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, addressExtraIdentifier[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifier[i].CountryId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_id` = @countryId" + i);
                                        else
                                            sql = string.Concat(sql, " `country_id` = @countryId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryId" + i, addressExtraIdentifier[i].CountryId);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifier[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifier[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifier[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifier[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifier[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifier[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifier[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifier[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(addressExtraIdentifier[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressExtraIdentifierModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressExtraIdentifierModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierModel>();
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
