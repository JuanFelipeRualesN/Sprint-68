using AddressAPI.AddressAPI.Domain.Models.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Infrastructure.Address
{
    class AddressExtraIdentifierLangDAO
    {
        private MySqlConnection _connection;
        public List<AddressExtraIdentifierLangModel> Read(string connectionString, AddressExtraIdentifierLangModel addressExtraIdentifierLang, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `aei_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM adrs_address_extra_identifier_langs ";
                {
                    if (addressExtraIdentifierLang != null)
                    {
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLang.AeiId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `aei_id` = @aeiId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `aei_id` = @aeiId ");
                            }
                            cmd.Parameters.AddWithValue("@aeiId", addressExtraIdentifierLang.AeiId);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLang.LangCode))
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
                            cmd.Parameters.AddWithValue("@langCode", addressExtraIdentifierLang.LangCode);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLang.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", addressExtraIdentifierLang.Desc);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLang.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifierLang.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLang.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifierLang.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLang.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifierLang.UpdatedAt);
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
                                    addressExtraIdentifierLang.AeiId = reader.GetValue(0).ToString();
                                else
                                    addressExtraIdentifierLang.AeiId = null;
                                if (!reader.IsDBNull(1))
                                    addressExtraIdentifierLang.LangCode = reader.GetValue(1).ToString();
                                else
                                    addressExtraIdentifierLang.LangCode = null;
                                if (!reader.IsDBNull(2))
                                    addressExtraIdentifierLang.Desc = reader.GetValue(2).ToString();
                                else
                                    addressExtraIdentifierLang.Desc = null;
                                if (!reader.IsDBNull(3))
                                    addressExtraIdentifierLang.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    addressExtraIdentifierLang.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    addressExtraIdentifierLang.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    addressExtraIdentifierLang.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    addressExtraIdentifierLang.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    addressExtraIdentifierLang.UpdatedAt = null;
                                response.Add(addressExtraIdentifierLang);
                                addressExtraIdentifierLang = new AddressExtraIdentifierLangModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AddressExtraIdentifierLangModel>();
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
                response = new List<AddressExtraIdentifierLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressExtraIdentifierLangModel> Read(string connectionString, List<AddressExtraIdentifierLangModel> addressExtraIdentifierLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `aei_id`, `lang_code`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM adrs_address_extra_identifier_langs ";
                if (addressExtraIdentifierLangWhere != null && addressExtraIdentifierLangWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < addressExtraIdentifierLangWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere[i].AeiId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `aei_id` = @aeiId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@aeiId" + i, addressExtraIdentifierLangWhere[i].AeiId);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere[i].LangCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lang_code` = @langCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@langCode" + i, addressExtraIdentifierLangWhere[i].LangCode);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierLangWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierLangWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierLangWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierLangWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != addressExtraIdentifierLangWhere.Count - 1)
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
                            AddressExtraIdentifierLangModel addressExtraIdentifierLang = new AddressExtraIdentifierLangModel(); if (!reader.IsDBNull(0))
                                addressExtraIdentifierLang.AeiId = reader.GetValue(0).ToString();
                            else
                                addressExtraIdentifierLang.AeiId = null;
                            if (!reader.IsDBNull(1))
                                addressExtraIdentifierLang.LangCode = reader.GetValue(1).ToString();
                            else
                                addressExtraIdentifierLang.LangCode = null;
                            if (!reader.IsDBNull(2))
                                addressExtraIdentifierLang.Desc = reader.GetValue(2).ToString();
                            else
                                addressExtraIdentifierLang.Desc = null;
                            if (!reader.IsDBNull(3))
                                addressExtraIdentifierLang.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                addressExtraIdentifierLang.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                addressExtraIdentifierLang.CreatedAt = reader.GetValue(4).ToString();
                            else
                                addressExtraIdentifierLang.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                addressExtraIdentifierLang.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                addressExtraIdentifierLang.UpdatedAt = null;
                            response.Add(addressExtraIdentifierLang);
                            addressExtraIdentifierLang = new AddressExtraIdentifierLangModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AddressExtraIdentifierLangModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressExtraIdentifierLangModel> Create(string connectionString, AddressExtraIdentifierLangModel addressExtraIdentifierLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO adrs_address_extra_identifier_langs (";
                string values = "(";
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.AeiId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `aei_id` ");
                        values = string.Concat(values, ", @aeiId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `aei_id` ");
                        values = string.Concat(values, " @aeiId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@aeiId", addressExtraIdentifierLang.AeiId);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.LangCode))
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
                    cmd.Parameters.AddWithValue("@langCode", addressExtraIdentifierLang.LangCode);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.Desc))
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
                    cmd.Parameters.AddWithValue("@desc", addressExtraIdentifierLang.Desc);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifierLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifierLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifierLang.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                response.Add(addressExtraIdentifierLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
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
        public List<AddressExtraIdentifierLangModel> Create(string connectionString, List<AddressExtraIdentifierLangModel> addressExtraIdentifierLangList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < addressExtraIdentifierLangList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO adrs_address_extra_identifier_langs (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].AeiId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `aei_id` ");
                                        values = string.Concat(values, ", @aeiId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `aei_id` ");
                                        values = string.Concat(values, "@aeiId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@aeiId" + i, addressExtraIdentifierLangList[i].AeiId);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].LangCode))
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
                                    cmd.Parameters.AddWithValue("@langCode" + i, addressExtraIdentifierLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].Desc))
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
                                    cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierLangList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(addressExtraIdentifierLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressExtraIdentifierLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressExtraIdentifierLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
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

        public List<AddressExtraIdentifierLangModel> Update(string connectionString, AddressExtraIdentifierLangModel addressExtraIdentifierLang, AddressExtraIdentifierLangModel addressExtraIdentifierLangWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE adrs_address_extra_identifier_langs SET ";
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.AeiId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `aei_id` = @aeiId");
                    else
                        sql = string.Concat(sql, " `aei_id` = @aeiId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@aeiId", addressExtraIdentifierLang.AeiId);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.LangCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `lang_code` = @langCode");
                    else
                        sql = string.Concat(sql, " `lang_code` = @langCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@langCode", addressExtraIdentifierLang.LangCode);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.Desc))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `desc` = @desc");
                    else
                        sql = string.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", addressExtraIdentifierLang.Desc);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifierLang.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifierLang.CreatedAt);
                }
                if (!string.IsNullOrEmpty(addressExtraIdentifierLang.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifierLang.UpdatedAt);
                }
                if (addressExtraIdentifierLangWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere.AeiId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `aei_id` = @aeiIdWhere");
                        else
                            sql = string.Concat(sql, " `aei_id` = @aeiIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@aeiIdWhere", addressExtraIdentifierLangWhere.AeiId);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCodeWhere", addressExtraIdentifierLangWhere.LangCode);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = string.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", addressExtraIdentifierLangWhere.Desc);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", addressExtraIdentifierLangWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", addressExtraIdentifierLangWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", addressExtraIdentifierLangWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(addressExtraIdentifierLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
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

        public List<AddressExtraIdentifierLangModel> Update(string connectionString, List<AddressExtraIdentifierLangModel> addressExtraIdentifierLangList, List<AddressExtraIdentifierLangModel> addressExtraIdentifierLangWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < addressExtraIdentifierLangList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE adrs_address_extra_identifier_langs SET ";
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].AeiId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `aei_id` = @aeiId" + i);
                                    else
                                        sql = string.Concat(sql, " `aei_id` = @aeiId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@aeiId" + i, addressExtraIdentifierLangList[i].AeiId);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].LangCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `lang_code` = @langCode" + i);
                                    else
                                        sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@langCode" + i, addressExtraIdentifierLangList[i].LangCode);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = string.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierLangList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierLangList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierLangList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(addressExtraIdentifierLangList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierLangList[i].UpdatedAt);
                                }
                                if (addressExtraIdentifierLangWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhereList[i].AeiId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `aei_id` = @aeiIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `aei_id` = @aeiIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@aeiIdWhere" + i, addressExtraIdentifierLangWhereList[i].AeiId);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhereList[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCodeWhere" + i, addressExtraIdentifierLangWhereList[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, addressExtraIdentifierLangWhereList[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, addressExtraIdentifierLangWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, addressExtraIdentifierLangWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLangWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, addressExtraIdentifierLangWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(addressExtraIdentifierLangList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressExtraIdentifierLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressExtraIdentifierLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
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

        public List<AddressExtraIdentifierLangModel> Delete(string connectionString, AddressExtraIdentifierLangModel addressExtraIdentifierLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM adrs_address_extra_identifier_langs  ";
                if (addressExtraIdentifierLang != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang.AeiId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `aei_id` = @aeiId");
                        else
                            sql = string.Concat(sql, " `aei_id` = @aeiId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@aeiId", addressExtraIdentifierLang.AeiId);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang.LangCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `lang_code` = @langCode");
                        else
                            sql = string.Concat(sql, " `lang_code` = @langCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@langCode", addressExtraIdentifierLang.LangCode);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = string.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", addressExtraIdentifierLang.Desc);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", addressExtraIdentifierLang.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", addressExtraIdentifierLang.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", addressExtraIdentifierLang.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(addressExtraIdentifierLang);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
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

        public List<AddressExtraIdentifierLangModel> Delete(string connectionString, List<AddressExtraIdentifierLangModel> addressExtraIdentifierLang)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressExtraIdentifierLangModel> response = new List<AddressExtraIdentifierLangModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < addressExtraIdentifierLang.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM adrs_address_extra_identifier_langs  ";
                                if (addressExtraIdentifierLang[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang[i].AeiId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `aei_id` = @aeiId" + i);
                                        else
                                            sql = string.Concat(sql, " `aei_id` = @aeiId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@aeiId" + i, addressExtraIdentifierLang[i].AeiId);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang[i].LangCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `lang_code` = @langCode" + i);
                                        else
                                            sql = string.Concat(sql, " `lang_code` = @langCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@langCode" + i, addressExtraIdentifierLang[i].LangCode);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, addressExtraIdentifierLang[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, addressExtraIdentifierLang[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, addressExtraIdentifierLang[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(addressExtraIdentifierLang[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, addressExtraIdentifierLang[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(addressExtraIdentifierLang[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressExtraIdentifierLangModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressExtraIdentifierLangModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressExtraIdentifierLangModel>();
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
