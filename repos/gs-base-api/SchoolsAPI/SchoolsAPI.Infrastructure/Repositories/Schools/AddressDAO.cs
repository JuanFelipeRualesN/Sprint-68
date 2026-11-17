using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class AddressDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, AddressModel> ReadD(string connectionString, AddressModel address, bool firstOne, bool orderByLastDate, AddressModel addressWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AddressModel> response = new Dictionary<string, AddressModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                {
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.Id))
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
                            cmd.Parameters.AddWithValue("@id", address.Id);
                        }
                        if (!string.IsNullOrEmpty(address.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                        }
                        if (!string.IsNullOrEmpty(address.NameAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name_address` = @nameAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name_address` = @nameAddress ");
                            }
                            cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine1))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_1` = @addressLine1 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_1` = @addressLine1 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine2))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_2` = @addressLine2 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_2` = @addressLine2 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `city` = @city ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `city` = @city ");
                            }
                            cmd.Parameters.AddWithValue("@city", address.City);
                        }
                        if (!string.IsNullOrEmpty(address.StateIsoCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
                            }
                            cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(address.State))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state` = @state ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state` = @state ");
                            }
                            cmd.Parameters.AddWithValue("@state", address.State);
                        }
                        if (!string.IsNullOrEmpty(address.Zip))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `zip` = @zip ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `zip` = @zip ");
                            }
                            cmd.Parameters.AddWithValue("@zip", address.Zip);
                        }
                        if (!string.IsNullOrEmpty(address.RegionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `region_id` = @regionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `region_id` = @regionId ");
                            }
                            cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                        }
                        if (!string.IsNullOrEmpty(address.Country))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country` = @country ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country` = @country ");
                            }
                            cmd.Parameters.AddWithValue("@country", address.Country);
                        }
                        if (!string.IsNullOrEmpty(address.CountryIsoCode2))
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
                            cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(address.AddressExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_extra` = @addressExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_extra` = @addressExtra ");
                            }
                            cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(address.AddressPhone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_phone` = @addressPhone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_phone` = @addressPhone ");
                            }
                            cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(address.MainAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `main_address` = @mainAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `main_address` = @mainAddress ");
                            }
                            cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                        }
                        if (!string.IsNullOrEmpty(address.Enable))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `enable` = @enable ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `enable` = @enable ");
                            }
                            cmd.Parameters.AddWithValue("@enable", address.Enable);
                        }
                        if (!string.IsNullOrEmpty(address.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(address.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(address.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                        }
                    }
                    if (addressWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(addressWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + addressWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + addressWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.NameAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name_address` LIKE @nameAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name_address` LIKE @nameAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameAddressWhereLike", "%" + addressWhereLike.NameAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine1))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine1WhereLike", "%" + addressWhereLike.AddressLine1 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine2WhereLike", "%" + addressWhereLike.AddressLine2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.City))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `city` LIKE @cityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `city` LIKE @cityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cityWhereLike", "%" + addressWhereLike.City + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.StateIsoCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateIsoCodeWhereLike", "%" + addressWhereLike.StateIsoCode + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.State))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state` LIKE @stateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state` LIKE @stateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateWhereLike", "%" + addressWhereLike.State + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Zip))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `zip` LIKE @zipWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `zip` LIKE @zipWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@zipWhereLike", "%" + addressWhereLike.Zip + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.RegionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `region_id` LIKE @regionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `region_id` LIKE @regionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@regionIdWhereLike", "%" + addressWhereLike.RegionId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Country))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country` LIKE @countryWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country` LIKE @countryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryWhereLike", "%" + addressWhereLike.Country + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CountryIsoCode2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2WhereLike", "%" + addressWhereLike.CountryIsoCode2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressExtra))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressExtraWhereLike", "%" + addressWhereLike.AddressExtra + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressPhone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressPhoneWhereLike", "%" + addressWhereLike.AddressPhone + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.MainAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `main_address` LIKE @mainAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `main_address` LIKE @mainAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mainAddressWhereLike", "%" + addressWhereLike.MainAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Enable))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `enable` LIKE @enableWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `enable` LIKE @enableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableWhereLike", "%" + addressWhereLike.Enable + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + addressWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + addressWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + addressWhereLike.UpdatedAt + "%");
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
                                address = new AddressModel();
                                if (!reader.IsDBNull(0))
                                    address.Id = reader.GetValue(0).ToString();
                                else
                                    address.Id = null;
                                if (!reader.IsDBNull(1))
                                    address.EntityId = reader.GetValue(1).ToString();
                                else
                                    address.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    address.NameAddress = reader.GetValue(2).ToString();
                                else
                                    address.NameAddress = null;
                                if (!reader.IsDBNull(3))
                                    address.AddressLine1 = reader.GetValue(3).ToString();
                                else
                                    address.AddressLine1 = null;
                                if (!reader.IsDBNull(4))
                                    address.AddressLine2 = reader.GetValue(4).ToString();
                                else
                                    address.AddressLine2 = null;
                                if (!reader.IsDBNull(5))
                                    address.City = reader.GetValue(5).ToString();
                                else
                                    address.City = null;
                                if (!reader.IsDBNull(6))
                                    address.StateIsoCode = reader.GetValue(6).ToString();
                                else
                                    address.StateIsoCode = null;
                                if (!reader.IsDBNull(7))
                                    address.State = reader.GetValue(7).ToString();
                                else
                                    address.State = null;
                                if (!reader.IsDBNull(8))
                                    address.Zip = reader.GetValue(8).ToString();
                                else
                                    address.Zip = null;
                                if (!reader.IsDBNull(9))
                                    address.RegionId = reader.GetValue(9).ToString();
                                else
                                    address.RegionId = null;
                                if (!reader.IsDBNull(10))
                                    address.Country = reader.GetValue(10).ToString();
                                else
                                    address.Country = null;
                                if (!reader.IsDBNull(11))
                                    address.CountryIsoCode2 = reader.GetValue(11).ToString();
                                else
                                    address.CountryIsoCode2 = null;
                                if (!reader.IsDBNull(12))
                                    address.AddressExtra = reader.GetValue(12).ToString();
                                else
                                    address.AddressExtra = null;
                                if (!reader.IsDBNull(13))
                                    address.AddressPhone = reader.GetValue(13).ToString();
                                else
                                    address.AddressPhone = null;
                                if (!reader.IsDBNull(14))
                                    address.MainAddress = reader.GetValue(14).ToString();
                                else
                                    address.MainAddress = null;
                                if (!reader.IsDBNull(15))
                                    address.Enable = reader.GetValue(15).ToString();
                                else
                                    address.Enable = null;
                                if (!reader.IsDBNull(16))
                                    address.ModifiedBy = reader.GetValue(16).ToString();
                                else
                                    address.ModifiedBy = null;
                                if (!reader.IsDBNull(17))
                                    address.CreatedAt = reader.GetValue(17).ToString();
                                else
                                    address.CreatedAt = null;
                                if (!reader.IsDBNull(18))
                                    address.UpdatedAt = reader.GetValue(18).ToString();
                                else
                                    address.UpdatedAt = null;
                                response.Add(address.Id, address);
                                address = new AddressModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AddressModel>();
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
                response = new Dictionary<string, AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AddressModel> ReadD(string connectionString, AddressModel address, bool orderByLastDate, int fromRow, int limit, AddressModel addressWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AddressModel> response = new Dictionary<string, AddressModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                {
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.Id))
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
                            cmd.Parameters.AddWithValue("@id", address.Id);
                        }
                        if (!string.IsNullOrEmpty(address.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                        }
                        if (!string.IsNullOrEmpty(address.NameAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name_address` = @nameAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name_address` = @nameAddress ");
                            }
                            cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine1))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_1` = @addressLine1 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_1` = @addressLine1 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine2))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_2` = @addressLine2 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_2` = @addressLine2 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `city` = @city ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `city` = @city ");
                            }
                            cmd.Parameters.AddWithValue("@city", address.City);
                        }
                        if (!string.IsNullOrEmpty(address.StateIsoCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
                            }
                            cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(address.State))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state` = @state ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state` = @state ");
                            }
                            cmd.Parameters.AddWithValue("@state", address.State);
                        }
                        if (!string.IsNullOrEmpty(address.Zip))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `zip` = @zip ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `zip` = @zip ");
                            }
                            cmd.Parameters.AddWithValue("@zip", address.Zip);
                        }
                        if (!string.IsNullOrEmpty(address.RegionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `region_id` = @regionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `region_id` = @regionId ");
                            }
                            cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                        }
                        if (!string.IsNullOrEmpty(address.Country))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country` = @country ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country` = @country ");
                            }
                            cmd.Parameters.AddWithValue("@country", address.Country);
                        }
                        if (!string.IsNullOrEmpty(address.CountryIsoCode2))
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
                            cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(address.AddressExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_extra` = @addressExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_extra` = @addressExtra ");
                            }
                            cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(address.AddressPhone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_phone` = @addressPhone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_phone` = @addressPhone ");
                            }
                            cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(address.MainAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `main_address` = @mainAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `main_address` = @mainAddress ");
                            }
                            cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                        }
                        if (!string.IsNullOrEmpty(address.Enable))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `enable` = @enable ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `enable` = @enable ");
                            }
                            cmd.Parameters.AddWithValue("@enable", address.Enable);
                        }
                        if (!string.IsNullOrEmpty(address.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(address.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(address.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                        }
                    }
                    if (addressWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(addressWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + addressWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + addressWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.NameAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name_address` LIKE @nameAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name_address` LIKE @nameAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameAddressWhereLike", "%" + addressWhereLike.NameAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine1))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine1WhereLike", "%" + addressWhereLike.AddressLine1 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine2WhereLike", "%" + addressWhereLike.AddressLine2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.City))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `city` LIKE @cityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `city` LIKE @cityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cityWhereLike", "%" + addressWhereLike.City + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.StateIsoCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateIsoCodeWhereLike", "%" + addressWhereLike.StateIsoCode + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.State))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state` LIKE @stateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state` LIKE @stateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateWhereLike", "%" + addressWhereLike.State + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Zip))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `zip` LIKE @zipWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `zip` LIKE @zipWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@zipWhereLike", "%" + addressWhereLike.Zip + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.RegionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `region_id` LIKE @regionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `region_id` LIKE @regionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@regionIdWhereLike", "%" + addressWhereLike.RegionId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Country))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country` LIKE @countryWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country` LIKE @countryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryWhereLike", "%" + addressWhereLike.Country + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CountryIsoCode2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2WhereLike", "%" + addressWhereLike.CountryIsoCode2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressExtra))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressExtraWhereLike", "%" + addressWhereLike.AddressExtra + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressPhone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressPhoneWhereLike", "%" + addressWhereLike.AddressPhone + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.MainAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `main_address` LIKE @mainAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `main_address` LIKE @mainAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mainAddressWhereLike", "%" + addressWhereLike.MainAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Enable))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `enable` LIKE @enableWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `enable` LIKE @enableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableWhereLike", "%" + addressWhereLike.Enable + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + addressWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + addressWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + addressWhereLike.UpdatedAt + "%");
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
                                address = new AddressModel();
                                if (!reader.IsDBNull(0))
                                    address.Id = reader.GetValue(0).ToString();
                                else
                                    address.Id = null;
                                if (!reader.IsDBNull(1))
                                    address.EntityId = reader.GetValue(1).ToString();
                                else
                                    address.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    address.NameAddress = reader.GetValue(2).ToString();
                                else
                                    address.NameAddress = null;
                                if (!reader.IsDBNull(3))
                                    address.AddressLine1 = reader.GetValue(3).ToString();
                                else
                                    address.AddressLine1 = null;
                                if (!reader.IsDBNull(4))
                                    address.AddressLine2 = reader.GetValue(4).ToString();
                                else
                                    address.AddressLine2 = null;
                                if (!reader.IsDBNull(5))
                                    address.City = reader.GetValue(5).ToString();
                                else
                                    address.City = null;
                                if (!reader.IsDBNull(6))
                                    address.StateIsoCode = reader.GetValue(6).ToString();
                                else
                                    address.StateIsoCode = null;
                                if (!reader.IsDBNull(7))
                                    address.State = reader.GetValue(7).ToString();
                                else
                                    address.State = null;
                                if (!reader.IsDBNull(8))
                                    address.Zip = reader.GetValue(8).ToString();
                                else
                                    address.Zip = null;
                                if (!reader.IsDBNull(9))
                                    address.RegionId = reader.GetValue(9).ToString();
                                else
                                    address.RegionId = null;
                                if (!reader.IsDBNull(10))
                                    address.Country = reader.GetValue(10).ToString();
                                else
                                    address.Country = null;
                                if (!reader.IsDBNull(11))
                                    address.CountryIsoCode2 = reader.GetValue(11).ToString();
                                else
                                    address.CountryIsoCode2 = null;
                                if (!reader.IsDBNull(12))
                                    address.AddressExtra = reader.GetValue(12).ToString();
                                else
                                    address.AddressExtra = null;
                                if (!reader.IsDBNull(13))
                                    address.AddressPhone = reader.GetValue(13).ToString();
                                else
                                    address.AddressPhone = null;
                                if (!reader.IsDBNull(14))
                                    address.MainAddress = reader.GetValue(14).ToString();
                                else
                                    address.MainAddress = null;
                                if (!reader.IsDBNull(15))
                                    address.Enable = reader.GetValue(15).ToString();
                                else
                                    address.Enable = null;
                                if (!reader.IsDBNull(16))
                                    address.ModifiedBy = reader.GetValue(16).ToString();
                                else
                                    address.ModifiedBy = null;
                                if (!reader.IsDBNull(17))
                                    address.CreatedAt = reader.GetValue(17).ToString();
                                else
                                    address.CreatedAt = null;
                                if (!reader.IsDBNull(18))
                                    address.UpdatedAt = reader.GetValue(18).ToString();
                                else
                                    address.UpdatedAt = null;
                                response.Add(address.Id, address);
                                address = new AddressModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AddressModel>();
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
                response = new Dictionary<string, AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AddressModel> ReadD(string connectionString, List<AddressModel> addressWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AddressModel> response = new Dictionary<string, AddressModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                if (addressWhere != null && addressWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < addressWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(addressWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, addressWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, addressWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].NameAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name_address` = @nameAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@nameAddress" + i, addressWhere[i].NameAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine1))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_1` = @addressLine1" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine1" + i, addressWhere[i].AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_2` = @addressLine2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine2" + i, addressWhere[i].AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].City))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `city` = @city" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@city" + i, addressWhere[i].City);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].StateIsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_iso_code` = @stateIsoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateIsoCode" + i, addressWhere[i].StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].State))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state` = @state" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@state" + i, addressWhere[i].State);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Zip))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `zip` = @zip" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@zip" + i, addressWhere[i].Zip);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].RegionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `region_id` = @regionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@regionId" + i, addressWhere[i].RegionId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Country))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country` = @country" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@country" + i, addressWhere[i].Country);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CountryIsoCode2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2" + i, addressWhere[i].CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_extra` = @addressExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressExtra" + i, addressWhere[i].AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressPhone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_phone` = @addressPhone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressPhone" + i, addressWhere[i].AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].MainAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `main_address` = @mainAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mainAddress" + i, addressWhere[i].MainAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Enable))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable` = @enable" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enable" + i, addressWhere[i].Enable);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, addressWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, addressWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, addressWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != addressWhere.Count - 1)
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
                            AddressModel address = new AddressModel(); if (!reader.IsDBNull(0))
                                address.Id = reader.GetValue(0).ToString();
                            else
                                address.Id = null;
                            if (!reader.IsDBNull(1))
                                address.EntityId = reader.GetValue(1).ToString();
                            else
                                address.EntityId = null;
                            if (!reader.IsDBNull(2))
                                address.NameAddress = reader.GetValue(2).ToString();
                            else
                                address.NameAddress = null;
                            if (!reader.IsDBNull(3))
                                address.AddressLine1 = reader.GetValue(3).ToString();
                            else
                                address.AddressLine1 = null;
                            if (!reader.IsDBNull(4))
                                address.AddressLine2 = reader.GetValue(4).ToString();
                            else
                                address.AddressLine2 = null;
                            if (!reader.IsDBNull(5))
                                address.City = reader.GetValue(5).ToString();
                            else
                                address.City = null;
                            if (!reader.IsDBNull(6))
                                address.StateIsoCode = reader.GetValue(6).ToString();
                            else
                                address.StateIsoCode = null;
                            if (!reader.IsDBNull(7))
                                address.State = reader.GetValue(7).ToString();
                            else
                                address.State = null;
                            if (!reader.IsDBNull(8))
                                address.Zip = reader.GetValue(8).ToString();
                            else
                                address.Zip = null;
                            if (!reader.IsDBNull(9))
                                address.RegionId = reader.GetValue(9).ToString();
                            else
                                address.RegionId = null;
                            if (!reader.IsDBNull(10))
                                address.Country = reader.GetValue(10).ToString();
                            else
                                address.Country = null;
                            if (!reader.IsDBNull(11))
                                address.CountryIsoCode2 = reader.GetValue(11).ToString();
                            else
                                address.CountryIsoCode2 = null;
                            if (!reader.IsDBNull(12))
                                address.AddressExtra = reader.GetValue(12).ToString();
                            else
                                address.AddressExtra = null;
                            if (!reader.IsDBNull(13))
                                address.AddressPhone = reader.GetValue(13).ToString();
                            else
                                address.AddressPhone = null;
                            if (!reader.IsDBNull(14))
                                address.MainAddress = reader.GetValue(14).ToString();
                            else
                                address.MainAddress = null;
                            if (!reader.IsDBNull(15))
                                address.Enable = reader.GetValue(15).ToString();
                            else
                                address.Enable = null;
                            if (!reader.IsDBNull(16))
                                address.ModifiedBy = reader.GetValue(16).ToString();
                            else
                                address.ModifiedBy = null;
                            if (!reader.IsDBNull(17))
                                address.CreatedAt = reader.GetValue(17).ToString();
                            else
                                address.CreatedAt = null;
                            if (!reader.IsDBNull(18))
                                address.UpdatedAt = reader.GetValue(18).ToString();
                            else
                                address.UpdatedAt = null;
                            response.Add(address.Id, address);
                            address = new AddressModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AddressModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AddressModel> ReadD(string connectionString, List<AddressModel> addressWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AddressModel> response = new Dictionary<string, AddressModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                if (addressWhere != null && addressWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < addressWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(addressWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, addressWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, addressWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].NameAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name_address` = @nameAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@nameAddress" + i, addressWhere[i].NameAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine1))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_1` = @addressLine1" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine1" + i, addressWhere[i].AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_2` = @addressLine2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine2" + i, addressWhere[i].AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].City))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `city` = @city" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@city" + i, addressWhere[i].City);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].StateIsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_iso_code` = @stateIsoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateIsoCode" + i, addressWhere[i].StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].State))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state` = @state" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@state" + i, addressWhere[i].State);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Zip))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `zip` = @zip" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@zip" + i, addressWhere[i].Zip);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].RegionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `region_id` = @regionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@regionId" + i, addressWhere[i].RegionId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Country))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country` = @country" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@country" + i, addressWhere[i].Country);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CountryIsoCode2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2" + i, addressWhere[i].CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_extra` = @addressExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressExtra" + i, addressWhere[i].AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressPhone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_phone` = @addressPhone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressPhone" + i, addressWhere[i].AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].MainAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `main_address` = @mainAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mainAddress" + i, addressWhere[i].MainAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Enable))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable` = @enable" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enable" + i, addressWhere[i].Enable);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, addressWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, addressWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, addressWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != addressWhere.Count - 1)
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
                            AddressModel address = new AddressModel(); if (!reader.IsDBNull(0))
                                address.Id = reader.GetValue(0).ToString();
                            else
                                address.Id = null;
                            if (!reader.IsDBNull(1))
                                address.EntityId = reader.GetValue(1).ToString();
                            else
                                address.EntityId = null;
                            if (!reader.IsDBNull(2))
                                address.NameAddress = reader.GetValue(2).ToString();
                            else
                                address.NameAddress = null;
                            if (!reader.IsDBNull(3))
                                address.AddressLine1 = reader.GetValue(3).ToString();
                            else
                                address.AddressLine1 = null;
                            if (!reader.IsDBNull(4))
                                address.AddressLine2 = reader.GetValue(4).ToString();
                            else
                                address.AddressLine2 = null;
                            if (!reader.IsDBNull(5))
                                address.City = reader.GetValue(5).ToString();
                            else
                                address.City = null;
                            if (!reader.IsDBNull(6))
                                address.StateIsoCode = reader.GetValue(6).ToString();
                            else
                                address.StateIsoCode = null;
                            if (!reader.IsDBNull(7))
                                address.State = reader.GetValue(7).ToString();
                            else
                                address.State = null;
                            if (!reader.IsDBNull(8))
                                address.Zip = reader.GetValue(8).ToString();
                            else
                                address.Zip = null;
                            if (!reader.IsDBNull(9))
                                address.RegionId = reader.GetValue(9).ToString();
                            else
                                address.RegionId = null;
                            if (!reader.IsDBNull(10))
                                address.Country = reader.GetValue(10).ToString();
                            else
                                address.Country = null;
                            if (!reader.IsDBNull(11))
                                address.CountryIsoCode2 = reader.GetValue(11).ToString();
                            else
                                address.CountryIsoCode2 = null;
                            if (!reader.IsDBNull(12))
                                address.AddressExtra = reader.GetValue(12).ToString();
                            else
                                address.AddressExtra = null;
                            if (!reader.IsDBNull(13))
                                address.AddressPhone = reader.GetValue(13).ToString();
                            else
                                address.AddressPhone = null;
                            if (!reader.IsDBNull(14))
                                address.MainAddress = reader.GetValue(14).ToString();
                            else
                                address.MainAddress = null;
                            if (!reader.IsDBNull(15))
                                address.Enable = reader.GetValue(15).ToString();
                            else
                                address.Enable = null;
                            if (!reader.IsDBNull(16))
                                address.ModifiedBy = reader.GetValue(16).ToString();
                            else
                                address.ModifiedBy = null;
                            if (!reader.IsDBNull(17))
                                address.CreatedAt = reader.GetValue(17).ToString();
                            else
                                address.CreatedAt = null;
                            if (!reader.IsDBNull(18))
                                address.UpdatedAt = reader.GetValue(18).ToString();
                            else
                                address.UpdatedAt = null;
                            response.Add(address.Id, address);
                            address = new AddressModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AddressModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressModel> Read(string connectionString, AddressModel address, bool firstOne, bool orderByLastDate, AddressModel addressWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                {
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.Id))
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
                            cmd.Parameters.AddWithValue("@id", address.Id);
                        }
                        if (!string.IsNullOrEmpty(address.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                        }
                        if (!string.IsNullOrEmpty(address.NameAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name_address` = @nameAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name_address` = @nameAddress ");
                            }
                            cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine1))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_1` = @addressLine1 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_1` = @addressLine1 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine2))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_2` = @addressLine2 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_2` = @addressLine2 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `city` = @city ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `city` = @city ");
                            }
                            cmd.Parameters.AddWithValue("@city", address.City);
                        }
                        if (!string.IsNullOrEmpty(address.StateIsoCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
                            }
                            cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(address.State))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state` = @state ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state` = @state ");
                            }
                            cmd.Parameters.AddWithValue("@state", address.State);
                        }
                        if (!string.IsNullOrEmpty(address.Zip))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `zip` = @zip ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `zip` = @zip ");
                            }
                            cmd.Parameters.AddWithValue("@zip", address.Zip);
                        }
                        if (!string.IsNullOrEmpty(address.RegionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `region_id` = @regionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `region_id` = @regionId ");
                            }
                            cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                        }
                        if (!string.IsNullOrEmpty(address.Country))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country` = @country ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country` = @country ");
                            }
                            cmd.Parameters.AddWithValue("@country", address.Country);
                        }
                        if (!string.IsNullOrEmpty(address.CountryIsoCode2))
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
                            cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(address.AddressExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_extra` = @addressExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_extra` = @addressExtra ");
                            }
                            cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(address.AddressPhone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_phone` = @addressPhone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_phone` = @addressPhone ");
                            }
                            cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(address.MainAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `main_address` = @mainAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `main_address` = @mainAddress ");
                            }
                            cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                        }
                        if (!string.IsNullOrEmpty(address.Enable))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `enable` = @enable ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `enable` = @enable ");
                            }
                            cmd.Parameters.AddWithValue("@enable", address.Enable);
                        }
                        if (!string.IsNullOrEmpty(address.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(address.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(address.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                        }
                    }
                    if (addressWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(addressWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + addressWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + addressWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.NameAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name_address` LIKE @nameAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name_address` LIKE @nameAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameAddressWhereLike", "%" + addressWhereLike.NameAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine1))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine1WhereLike", "%" + addressWhereLike.AddressLine1 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine2WhereLike", "%" + addressWhereLike.AddressLine2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.City))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `city` LIKE @cityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `city` LIKE @cityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cityWhereLike", "%" + addressWhereLike.City + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.StateIsoCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateIsoCodeWhereLike", "%" + addressWhereLike.StateIsoCode + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.State))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state` LIKE @stateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state` LIKE @stateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateWhereLike", "%" + addressWhereLike.State + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Zip))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `zip` LIKE @zipWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `zip` LIKE @zipWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@zipWhereLike", "%" + addressWhereLike.Zip + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.RegionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `region_id` LIKE @regionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `region_id` LIKE @regionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@regionIdWhereLike", "%" + addressWhereLike.RegionId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Country))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country` LIKE @countryWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country` LIKE @countryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryWhereLike", "%" + addressWhereLike.Country + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CountryIsoCode2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2WhereLike", "%" + addressWhereLike.CountryIsoCode2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressExtra))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressExtraWhereLike", "%" + addressWhereLike.AddressExtra + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressPhone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressPhoneWhereLike", "%" + addressWhereLike.AddressPhone + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.MainAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `main_address` LIKE @mainAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `main_address` LIKE @mainAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mainAddressWhereLike", "%" + addressWhereLike.MainAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Enable))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `enable` LIKE @enableWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `enable` LIKE @enableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableWhereLike", "%" + addressWhereLike.Enable + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + addressWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + addressWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + addressWhereLike.UpdatedAt + "%");
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
                                address = new AddressModel();
                                if (!reader.IsDBNull(0))
                                    address.Id = reader.GetValue(0).ToString();
                                else
                                    address.Id = null;
                                if (!reader.IsDBNull(1))
                                    address.EntityId = reader.GetValue(1).ToString();
                                else
                                    address.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    address.NameAddress = reader.GetValue(2).ToString();
                                else
                                    address.NameAddress = null;
                                if (!reader.IsDBNull(3))
                                    address.AddressLine1 = reader.GetValue(3).ToString();
                                else
                                    address.AddressLine1 = null;
                                if (!reader.IsDBNull(4))
                                    address.AddressLine2 = reader.GetValue(4).ToString();
                                else
                                    address.AddressLine2 = null;
                                if (!reader.IsDBNull(5))
                                    address.City = reader.GetValue(5).ToString();
                                else
                                    address.City = null;
                                if (!reader.IsDBNull(6))
                                    address.StateIsoCode = reader.GetValue(6).ToString();
                                else
                                    address.StateIsoCode = null;
                                if (!reader.IsDBNull(7))
                                    address.State = reader.GetValue(7).ToString();
                                else
                                    address.State = null;
                                if (!reader.IsDBNull(8))
                                    address.Zip = reader.GetValue(8).ToString();
                                else
                                    address.Zip = null;
                                if (!reader.IsDBNull(9))
                                    address.RegionId = reader.GetValue(9).ToString();
                                else
                                    address.RegionId = null;
                                if (!reader.IsDBNull(10))
                                    address.Country = reader.GetValue(10).ToString();
                                else
                                    address.Country = null;
                                if (!reader.IsDBNull(11))
                                    address.CountryIsoCode2 = reader.GetValue(11).ToString();
                                else
                                    address.CountryIsoCode2 = null;
                                if (!reader.IsDBNull(12))
                                    address.AddressExtra = reader.GetValue(12).ToString();
                                else
                                    address.AddressExtra = null;
                                if (!reader.IsDBNull(13))
                                    address.AddressPhone = reader.GetValue(13).ToString();
                                else
                                    address.AddressPhone = null;
                                if (!reader.IsDBNull(14))
                                    address.MainAddress = reader.GetValue(14).ToString();
                                else
                                    address.MainAddress = null;
                                if (!reader.IsDBNull(15))
                                    address.Enable = reader.GetValue(15).ToString();
                                else
                                    address.Enable = null;
                                if (!reader.IsDBNull(16))
                                    address.ModifiedBy = reader.GetValue(16).ToString();
                                else
                                    address.ModifiedBy = null;
                                if (!reader.IsDBNull(17))
                                    address.CreatedAt = reader.GetValue(17).ToString();
                                else
                                    address.CreatedAt = null;
                                if (!reader.IsDBNull(18))
                                    address.UpdatedAt = reader.GetValue(18).ToString();
                                else
                                    address.UpdatedAt = null;
                                response.Add(address);
                                address = new AddressModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AddressModel>();
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
                response = new List<AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressModel> Read(string connectionString, AddressModel address, bool orderByLastDate, int fromRow, int limit, AddressModel addressWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                {
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.Id))
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
                            cmd.Parameters.AddWithValue("@id", address.Id);
                        }
                        if (!string.IsNullOrEmpty(address.EntityId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_id` = @entityId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_id` = @entityId ");
                            }
                            cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                        }
                        if (!string.IsNullOrEmpty(address.NameAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name_address` = @nameAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name_address` = @nameAddress ");
                            }
                            cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine1))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_1` = @addressLine1 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_1` = @addressLine1 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(address.AddressLine2))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_line_2` = @addressLine2 ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_line_2` = @addressLine2 ");
                            }
                            cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(address.City))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `city` = @city ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `city` = @city ");
                            }
                            cmd.Parameters.AddWithValue("@city", address.City);
                        }
                        if (!string.IsNullOrEmpty(address.StateIsoCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
                            }
                            cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(address.State))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `state` = @state ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `state` = @state ");
                            }
                            cmd.Parameters.AddWithValue("@state", address.State);
                        }
                        if (!string.IsNullOrEmpty(address.Zip))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `zip` = @zip ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `zip` = @zip ");
                            }
                            cmd.Parameters.AddWithValue("@zip", address.Zip);
                        }
                        if (!string.IsNullOrEmpty(address.RegionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `region_id` = @regionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `region_id` = @regionId ");
                            }
                            cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                        }
                        if (!string.IsNullOrEmpty(address.Country))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country` = @country ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country` = @country ");
                            }
                            cmd.Parameters.AddWithValue("@country", address.Country);
                        }
                        if (!string.IsNullOrEmpty(address.CountryIsoCode2))
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
                            cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(address.AddressExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_extra` = @addressExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_extra` = @addressExtra ");
                            }
                            cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(address.AddressPhone))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `address_phone` = @addressPhone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `address_phone` = @addressPhone ");
                            }
                            cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(address.MainAddress))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `main_address` = @mainAddress ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `main_address` = @mainAddress ");
                            }
                            cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                        }
                        if (!string.IsNullOrEmpty(address.Enable))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `enable` = @enable ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `enable` = @enable ");
                            }
                            cmd.Parameters.AddWithValue("@enable", address.Enable);
                        }
                        if (!string.IsNullOrEmpty(address.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(address.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(address.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                        }
                    }
                    if (addressWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(addressWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + addressWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.EntityId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + addressWhereLike.EntityId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.NameAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `name_address` LIKE @nameAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `name_address` LIKE @nameAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameAddressWhereLike", "%" + addressWhereLike.NameAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine1))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_1` LIKE @addressLine1WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine1WhereLike", "%" + addressWhereLike.AddressLine1 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressLine2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_line_2` LIKE @addressLine2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressLine2WhereLike", "%" + addressWhereLike.AddressLine2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.City))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `city` LIKE @cityWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `city` LIKE @cityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@cityWhereLike", "%" + addressWhereLike.City + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.StateIsoCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state_iso_code` LIKE @stateIsoCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateIsoCodeWhereLike", "%" + addressWhereLike.StateIsoCode + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.State))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `state` LIKE @stateWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `state` LIKE @stateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@stateWhereLike", "%" + addressWhereLike.State + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Zip))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `zip` LIKE @zipWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `zip` LIKE @zipWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@zipWhereLike", "%" + addressWhereLike.Zip + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.RegionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `region_id` LIKE @regionIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `region_id` LIKE @regionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@regionIdWhereLike", "%" + addressWhereLike.RegionId + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Country))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country` LIKE @countryWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country` LIKE @countryWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryWhereLike", "%" + addressWhereLike.Country + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CountryIsoCode2))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `country_iso_code_2` LIKE @countryIsoCode2WhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2WhereLike", "%" + addressWhereLike.CountryIsoCode2 + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressExtra))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_extra` LIKE @addressExtraWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressExtraWhereLike", "%" + addressWhereLike.AddressExtra + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.AddressPhone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `address_phone` LIKE @addressPhoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@addressPhoneWhereLike", "%" + addressWhereLike.AddressPhone + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.MainAddress))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `main_address` LIKE @mainAddressWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `main_address` LIKE @mainAddressWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@mainAddressWhereLike", "%" + addressWhereLike.MainAddress + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.Enable))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `enable` LIKE @enableWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `enable` LIKE @enableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@enableWhereLike", "%" + addressWhereLike.Enable + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + addressWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + addressWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(addressWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + addressWhereLike.UpdatedAt + "%");
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
                                address = new AddressModel();
                                if (!reader.IsDBNull(0))
                                    address.Id = reader.GetValue(0).ToString();
                                else
                                    address.Id = null;
                                if (!reader.IsDBNull(1))
                                    address.EntityId = reader.GetValue(1).ToString();
                                else
                                    address.EntityId = null;
                                if (!reader.IsDBNull(2))
                                    address.NameAddress = reader.GetValue(2).ToString();
                                else
                                    address.NameAddress = null;
                                if (!reader.IsDBNull(3))
                                    address.AddressLine1 = reader.GetValue(3).ToString();
                                else
                                    address.AddressLine1 = null;
                                if (!reader.IsDBNull(4))
                                    address.AddressLine2 = reader.GetValue(4).ToString();
                                else
                                    address.AddressLine2 = null;
                                if (!reader.IsDBNull(5))
                                    address.City = reader.GetValue(5).ToString();
                                else
                                    address.City = null;
                                if (!reader.IsDBNull(6))
                                    address.StateIsoCode = reader.GetValue(6).ToString();
                                else
                                    address.StateIsoCode = null;
                                if (!reader.IsDBNull(7))
                                    address.State = reader.GetValue(7).ToString();
                                else
                                    address.State = null;
                                if (!reader.IsDBNull(8))
                                    address.Zip = reader.GetValue(8).ToString();
                                else
                                    address.Zip = null;
                                if (!reader.IsDBNull(9))
                                    address.RegionId = reader.GetValue(9).ToString();
                                else
                                    address.RegionId = null;
                                if (!reader.IsDBNull(10))
                                    address.Country = reader.GetValue(10).ToString();
                                else
                                    address.Country = null;
                                if (!reader.IsDBNull(11))
                                    address.CountryIsoCode2 = reader.GetValue(11).ToString();
                                else
                                    address.CountryIsoCode2 = null;
                                if (!reader.IsDBNull(12))
                                    address.AddressExtra = reader.GetValue(12).ToString();
                                else
                                    address.AddressExtra = null;
                                if (!reader.IsDBNull(13))
                                    address.AddressPhone = reader.GetValue(13).ToString();
                                else
                                    address.AddressPhone = null;
                                if (!reader.IsDBNull(14))
                                    address.MainAddress = reader.GetValue(14).ToString();
                                else
                                    address.MainAddress = null;
                                if (!reader.IsDBNull(15))
                                    address.Enable = reader.GetValue(15).ToString();
                                else
                                    address.Enable = null;
                                if (!reader.IsDBNull(16))
                                    address.ModifiedBy = reader.GetValue(16).ToString();
                                else
                                    address.ModifiedBy = null;
                                if (!reader.IsDBNull(17))
                                    address.CreatedAt = reader.GetValue(17).ToString();
                                else
                                    address.CreatedAt = null;
                                if (!reader.IsDBNull(18))
                                    address.UpdatedAt = reader.GetValue(18).ToString();
                                else
                                    address.UpdatedAt = null;
                                response.Add(address);
                                address = new AddressModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AddressModel>();
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
                response = new List<AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressModel> Read(string connectionString, List<AddressModel> addressWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                if (addressWhere != null && addressWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < addressWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(addressWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, addressWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, addressWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].NameAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name_address` = @nameAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@nameAddress" + i, addressWhere[i].NameAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine1))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_1` = @addressLine1" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine1" + i, addressWhere[i].AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_2` = @addressLine2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine2" + i, addressWhere[i].AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].City))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `city` = @city" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@city" + i, addressWhere[i].City);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].StateIsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_iso_code` = @stateIsoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateIsoCode" + i, addressWhere[i].StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].State))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state` = @state" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@state" + i, addressWhere[i].State);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Zip))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `zip` = @zip" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@zip" + i, addressWhere[i].Zip);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].RegionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `region_id` = @regionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@regionId" + i, addressWhere[i].RegionId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Country))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country` = @country" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@country" + i, addressWhere[i].Country);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CountryIsoCode2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2" + i, addressWhere[i].CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_extra` = @addressExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressExtra" + i, addressWhere[i].AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressPhone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_phone` = @addressPhone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressPhone" + i, addressWhere[i].AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].MainAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `main_address` = @mainAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mainAddress" + i, addressWhere[i].MainAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Enable))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable` = @enable" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enable" + i, addressWhere[i].Enable);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, addressWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, addressWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, addressWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != addressWhere.Count - 1)
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
                            AddressModel address = new AddressModel(); if (!reader.IsDBNull(0))
                                address.Id = reader.GetValue(0).ToString();
                            else
                                address.Id = null;
                            if (!reader.IsDBNull(1))
                                address.EntityId = reader.GetValue(1).ToString();
                            else
                                address.EntityId = null;
                            if (!reader.IsDBNull(2))
                                address.NameAddress = reader.GetValue(2).ToString();
                            else
                                address.NameAddress = null;
                            if (!reader.IsDBNull(3))
                                address.AddressLine1 = reader.GetValue(3).ToString();
                            else
                                address.AddressLine1 = null;
                            if (!reader.IsDBNull(4))
                                address.AddressLine2 = reader.GetValue(4).ToString();
                            else
                                address.AddressLine2 = null;
                            if (!reader.IsDBNull(5))
                                address.City = reader.GetValue(5).ToString();
                            else
                                address.City = null;
                            if (!reader.IsDBNull(6))
                                address.StateIsoCode = reader.GetValue(6).ToString();
                            else
                                address.StateIsoCode = null;
                            if (!reader.IsDBNull(7))
                                address.State = reader.GetValue(7).ToString();
                            else
                                address.State = null;
                            if (!reader.IsDBNull(8))
                                address.Zip = reader.GetValue(8).ToString();
                            else
                                address.Zip = null;
                            if (!reader.IsDBNull(9))
                                address.RegionId = reader.GetValue(9).ToString();
                            else
                                address.RegionId = null;
                            if (!reader.IsDBNull(10))
                                address.Country = reader.GetValue(10).ToString();
                            else
                                address.Country = null;
                            if (!reader.IsDBNull(11))
                                address.CountryIsoCode2 = reader.GetValue(11).ToString();
                            else
                                address.CountryIsoCode2 = null;
                            if (!reader.IsDBNull(12))
                                address.AddressExtra = reader.GetValue(12).ToString();
                            else
                                address.AddressExtra = null;
                            if (!reader.IsDBNull(13))
                                address.AddressPhone = reader.GetValue(13).ToString();
                            else
                                address.AddressPhone = null;
                            if (!reader.IsDBNull(14))
                                address.MainAddress = reader.GetValue(14).ToString();
                            else
                                address.MainAddress = null;
                            if (!reader.IsDBNull(15))
                                address.Enable = reader.GetValue(15).ToString();
                            else
                                address.Enable = null;
                            if (!reader.IsDBNull(16))
                                address.ModifiedBy = reader.GetValue(16).ToString();
                            else
                                address.ModifiedBy = null;
                            if (!reader.IsDBNull(17))
                                address.CreatedAt = reader.GetValue(17).ToString();
                            else
                                address.CreatedAt = null;
                            if (!reader.IsDBNull(18))
                                address.UpdatedAt = reader.GetValue(18).ToString();
                            else
                                address.UpdatedAt = null;
                            response.Add(address);
                            address = new AddressModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AddressModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressModel> Read(string connectionString, List<AddressModel> addressWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_id`, `name_address`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `region_id`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM sch_addresses ";
                if (addressWhere != null && addressWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < addressWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(addressWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, addressWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].EntityId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_id` = @entityId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityId" + i, addressWhere[i].EntityId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].NameAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name_address` = @nameAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@nameAddress" + i, addressWhere[i].NameAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine1))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_1` = @addressLine1" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine1" + i, addressWhere[i].AddressLine1);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressLine2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_line_2` = @addressLine2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressLine2" + i, addressWhere[i].AddressLine2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].City))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `city` = @city" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@city" + i, addressWhere[i].City);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].StateIsoCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state_iso_code` = @stateIsoCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@stateIsoCode" + i, addressWhere[i].StateIsoCode);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].State))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `state` = @state" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@state" + i, addressWhere[i].State);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Zip))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `zip` = @zip" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@zip" + i, addressWhere[i].Zip);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].RegionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `region_id` = @regionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@regionId" + i, addressWhere[i].RegionId);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Country))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country` = @country" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@country" + i, addressWhere[i].Country);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CountryIsoCode2))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIsoCode2" + i, addressWhere[i].CountryIsoCode2);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_extra` = @addressExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressExtra" + i, addressWhere[i].AddressExtra);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].AddressPhone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `address_phone` = @addressPhone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@addressPhone" + i, addressWhere[i].AddressPhone);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].MainAddress))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `main_address` = @mainAddress" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mainAddress" + i, addressWhere[i].MainAddress);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].Enable))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `enable` = @enable" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@enable" + i, addressWhere[i].Enable);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, addressWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, addressWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(addressWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, addressWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != addressWhere.Count - 1)
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
                            AddressModel address = new AddressModel(); if (!reader.IsDBNull(0))
                                address.Id = reader.GetValue(0).ToString();
                            else
                                address.Id = null;
                            if (!reader.IsDBNull(1))
                                address.EntityId = reader.GetValue(1).ToString();
                            else
                                address.EntityId = null;
                            if (!reader.IsDBNull(2))
                                address.NameAddress = reader.GetValue(2).ToString();
                            else
                                address.NameAddress = null;
                            if (!reader.IsDBNull(3))
                                address.AddressLine1 = reader.GetValue(3).ToString();
                            else
                                address.AddressLine1 = null;
                            if (!reader.IsDBNull(4))
                                address.AddressLine2 = reader.GetValue(4).ToString();
                            else
                                address.AddressLine2 = null;
                            if (!reader.IsDBNull(5))
                                address.City = reader.GetValue(5).ToString();
                            else
                                address.City = null;
                            if (!reader.IsDBNull(6))
                                address.StateIsoCode = reader.GetValue(6).ToString();
                            else
                                address.StateIsoCode = null;
                            if (!reader.IsDBNull(7))
                                address.State = reader.GetValue(7).ToString();
                            else
                                address.State = null;
                            if (!reader.IsDBNull(8))
                                address.Zip = reader.GetValue(8).ToString();
                            else
                                address.Zip = null;
                            if (!reader.IsDBNull(9))
                                address.RegionId = reader.GetValue(9).ToString();
                            else
                                address.RegionId = null;
                            if (!reader.IsDBNull(10))
                                address.Country = reader.GetValue(10).ToString();
                            else
                                address.Country = null;
                            if (!reader.IsDBNull(11))
                                address.CountryIsoCode2 = reader.GetValue(11).ToString();
                            else
                                address.CountryIsoCode2 = null;
                            if (!reader.IsDBNull(12))
                                address.AddressExtra = reader.GetValue(12).ToString();
                            else
                                address.AddressExtra = null;
                            if (!reader.IsDBNull(13))
                                address.AddressPhone = reader.GetValue(13).ToString();
                            else
                                address.AddressPhone = null;
                            if (!reader.IsDBNull(14))
                                address.MainAddress = reader.GetValue(14).ToString();
                            else
                                address.MainAddress = null;
                            if (!reader.IsDBNull(15))
                                address.Enable = reader.GetValue(15).ToString();
                            else
                                address.Enable = null;
                            if (!reader.IsDBNull(16))
                                address.ModifiedBy = reader.GetValue(16).ToString();
                            else
                                address.ModifiedBy = null;
                            if (!reader.IsDBNull(17))
                                address.CreatedAt = reader.GetValue(17).ToString();
                            else
                                address.CreatedAt = null;
                            if (!reader.IsDBNull(18))
                                address.UpdatedAt = reader.GetValue(18).ToString();
                            else
                                address.UpdatedAt = null;
                            response.Add(address);
                            address = new AddressModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AddressModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AddressModel> Create(string connectionString, AddressModel address)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_addresses (";
                string values = "(";
                if (!string.IsNullOrEmpty(address.Id))
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
                    cmd.Parameters.AddWithValue("@id", address.Id);
                }
                if (!string.IsNullOrEmpty(address.EntityId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `entity_id` ");
                        values = string.Concat(values, ", @entityId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `entity_id` ");
                        values = string.Concat(values, " @entityId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                }
                if (!string.IsNullOrEmpty(address.NameAddress))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `name_address` ");
                        values = string.Concat(values, ", @nameAddress");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `name_address` ");
                        values = string.Concat(values, " @nameAddress");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                }
                if (!string.IsNullOrEmpty(address.AddressLine1))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `address_line_1` ");
                        values = string.Concat(values, ", @addressLine1");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `address_line_1` ");
                        values = string.Concat(values, " @addressLine1");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                }
                if (!string.IsNullOrEmpty(address.AddressLine2))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `address_line_2` ");
                        values = string.Concat(values, ", @addressLine2");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `address_line_2` ");
                        values = string.Concat(values, " @addressLine2");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                }
                if (!string.IsNullOrEmpty(address.City))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `city` ");
                        values = string.Concat(values, ", @city");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `city` ");
                        values = string.Concat(values, " @city");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@city", address.City);
                }
                if (!string.IsNullOrEmpty(address.StateIsoCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `state_iso_code` ");
                        values = string.Concat(values, ", @stateIsoCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `state_iso_code` ");
                        values = string.Concat(values, " @stateIsoCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                }
                if (!string.IsNullOrEmpty(address.State))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `state` ");
                        values = string.Concat(values, ", @state");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `state` ");
                        values = string.Concat(values, " @state");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@state", address.State);
                }
                if (!string.IsNullOrEmpty(address.Zip))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `zip` ");
                        values = string.Concat(values, ", @zip");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `zip` ");
                        values = string.Concat(values, " @zip");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@zip", address.Zip);
                }
                if (!string.IsNullOrEmpty(address.RegionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `region_id` ");
                        values = string.Concat(values, ", @regionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `region_id` ");
                        values = string.Concat(values, " @regionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                }
                if (!string.IsNullOrEmpty(address.Country))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country` ");
                        values = string.Concat(values, ", @country");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country` ");
                        values = string.Concat(values, " @country");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@country", address.Country);
                }
                if (!string.IsNullOrEmpty(address.CountryIsoCode2))
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
                    cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                }
                if (!string.IsNullOrEmpty(address.AddressExtra))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `address_extra` ");
                        values = string.Concat(values, ", @addressExtra");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `address_extra` ");
                        values = string.Concat(values, " @addressExtra");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                }
                if (!string.IsNullOrEmpty(address.AddressPhone))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `address_phone` ");
                        values = string.Concat(values, ", @addressPhone");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `address_phone` ");
                        values = string.Concat(values, " @addressPhone");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                }
                if (!string.IsNullOrEmpty(address.MainAddress))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `main_address` ");
                        values = string.Concat(values, ", @mainAddress");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `main_address` ");
                        values = string.Concat(values, " @mainAddress");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                }
                if (!string.IsNullOrEmpty(address.Enable))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `enable` ");
                        values = string.Concat(values, ", @enable");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `enable` ");
                        values = string.Concat(values, " @enable");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@enable", address.Enable);
                }
                if (!string.IsNullOrEmpty(address.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(address.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                }
                if (!string.IsNullOrEmpty(address.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                address.Id = cmd.LastInsertedId.ToString();
                response.Add(address);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
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
        public List<AddressModel> Create(string connectionString, List<AddressModel> addressList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
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
                        for (int i = 0; i < addressList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_addresses (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(addressList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, addressList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].EntityId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `entity_id` ");
                                        values = string.Concat(values, ", @entityId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `entity_id` ");
                                        values = string.Concat(values, "@entityId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityId" + i, addressList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].NameAddress))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `name_address` ");
                                        values = string.Concat(values, ", @nameAddress" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `name_address` ");
                                        values = string.Concat(values, "@nameAddress" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@nameAddress" + i, addressList[i].NameAddress);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressLine1))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `address_line_1` ");
                                        values = string.Concat(values, ", @addressLine1" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `address_line_1` ");
                                        values = string.Concat(values, "@addressLine1" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressLine1" + i, addressList[i].AddressLine1);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressLine2))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `address_line_2` ");
                                        values = string.Concat(values, ", @addressLine2" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `address_line_2` ");
                                        values = string.Concat(values, "@addressLine2" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressLine2" + i, addressList[i].AddressLine2);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].City))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `city` ");
                                        values = string.Concat(values, ", @city" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `city` ");
                                        values = string.Concat(values, "@city" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@city" + i, addressList[i].City);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].StateIsoCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `state_iso_code` ");
                                        values = string.Concat(values, ", @stateIsoCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `state_iso_code` ");
                                        values = string.Concat(values, "@stateIsoCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateIsoCode" + i, addressList[i].StateIsoCode);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].State))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `state` ");
                                        values = string.Concat(values, ", @state" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `state` ");
                                        values = string.Concat(values, "@state" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@state" + i, addressList[i].State);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].Zip))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `zip` ");
                                        values = string.Concat(values, ", @zip" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `zip` ");
                                        values = string.Concat(values, "@zip" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@zip" + i, addressList[i].Zip);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].RegionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `region_id` ");
                                        values = string.Concat(values, ", @regionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `region_id` ");
                                        values = string.Concat(values, "@regionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@regionId" + i, addressList[i].RegionId);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].Country))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country` ");
                                        values = string.Concat(values, ", @country" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country` ");
                                        values = string.Concat(values, "@country" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@country" + i, addressList[i].Country);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].CountryIsoCode2))
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
                                    cmd.Parameters.AddWithValue("@countryIsoCode2" + i, addressList[i].CountryIsoCode2);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressExtra))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `address_extra` ");
                                        values = string.Concat(values, ", @addressExtra" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `address_extra` ");
                                        values = string.Concat(values, "@addressExtra" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressExtra" + i, addressList[i].AddressExtra);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressPhone))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `address_phone` ");
                                        values = string.Concat(values, ", @addressPhone" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `address_phone` ");
                                        values = string.Concat(values, "@addressPhone" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressPhone" + i, addressList[i].AddressPhone);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].MainAddress))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `main_address` ");
                                        values = string.Concat(values, ", @mainAddress" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `main_address` ");
                                        values = string.Concat(values, "@mainAddress" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mainAddress" + i, addressList[i].MainAddress);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].Enable))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `enable` ");
                                        values = string.Concat(values, ", @enable" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `enable` ");
                                        values = string.Concat(values, "@enable" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@enable" + i, addressList[i].Enable);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, addressList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, addressList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, addressList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(addressList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
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

        public List<AddressModel> Update(string connectionString, AddressModel address, AddressModel addressWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_addresses SET ";
                if (!string.IsNullOrEmpty(address.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", address.Id);
                }
                if (!string.IsNullOrEmpty(address.EntityId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `entity_id` = @entityId");
                    else
                        sql = string.Concat(sql, " `entity_id` = @entityId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                }
                if (!string.IsNullOrEmpty(address.NameAddress))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name_address` = @nameAddress");
                    else
                        sql = string.Concat(sql, " `name_address` = @nameAddress");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                }
                if (!string.IsNullOrEmpty(address.AddressLine1))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `address_line_1` = @addressLine1");
                    else
                        sql = string.Concat(sql, " `address_line_1` = @addressLine1");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                }
                if (!string.IsNullOrEmpty(address.AddressLine2))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `address_line_2` = @addressLine2");
                    else
                        sql = string.Concat(sql, " `address_line_2` = @addressLine2");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                }
                if (!string.IsNullOrEmpty(address.City))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `city` = @city");
                    else
                        sql = string.Concat(sql, " `city` = @city");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@city", address.City);
                }
                if (!string.IsNullOrEmpty(address.StateIsoCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `state_iso_code` = @stateIsoCode");
                    else
                        sql = string.Concat(sql, " `state_iso_code` = @stateIsoCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                }
                if (!string.IsNullOrEmpty(address.State))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `state` = @state");
                    else
                        sql = string.Concat(sql, " `state` = @state");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@state", address.State);
                }
                if (!string.IsNullOrEmpty(address.Zip))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `zip` = @zip");
                    else
                        sql = string.Concat(sql, " `zip` = @zip");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@zip", address.Zip);
                }
                if (!string.IsNullOrEmpty(address.RegionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `region_id` = @regionId");
                    else
                        sql = string.Concat(sql, " `region_id` = @regionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                }
                if (!string.IsNullOrEmpty(address.Country))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country` = @country");
                    else
                        sql = string.Concat(sql, " `country` = @country");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@country", address.Country);
                }
                if (!string.IsNullOrEmpty(address.CountryIsoCode2))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso_code_2` = @countryIsoCode2");
                    else
                        sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                }
                if (!string.IsNullOrEmpty(address.AddressExtra))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `address_extra` = @addressExtra");
                    else
                        sql = string.Concat(sql, " `address_extra` = @addressExtra");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                }
                if (!string.IsNullOrEmpty(address.AddressPhone))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `address_phone` = @addressPhone");
                    else
                        sql = string.Concat(sql, " `address_phone` = @addressPhone");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                }
                if (!string.IsNullOrEmpty(address.MainAddress))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `main_address` = @mainAddress");
                    else
                        sql = string.Concat(sql, " `main_address` = @mainAddress");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                }
                if (!string.IsNullOrEmpty(address.Enable))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `enable` = @enable");
                    else
                        sql = string.Concat(sql, " `enable` = @enable");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@enable", address.Enable);
                }
                if (!string.IsNullOrEmpty(address.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(address.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                }
                if (!string.IsNullOrEmpty(address.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                }
                if (addressWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(addressWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", addressWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityIdWhere", addressWhere.EntityId);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.NameAddress))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name_address` = @nameAddressWhere");
                        else
                            sql = string.Concat(sql, " `name_address` = @nameAddressWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameAddressWhere", addressWhere.NameAddress);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.AddressLine1))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_line_1` = @addressLine1Where");
                        else
                            sql = string.Concat(sql, " `address_line_1` = @addressLine1Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressLine1Where", addressWhere.AddressLine1);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.AddressLine2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_line_2` = @addressLine2Where");
                        else
                            sql = string.Concat(sql, " `address_line_2` = @addressLine2Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressLine2Where", addressWhere.AddressLine2);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.City))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `city` = @cityWhere");
                        else
                            sql = string.Concat(sql, " `city` = @cityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@cityWhere", addressWhere.City);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.StateIsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_iso_code` = @stateIsoCodeWhere");
                        else
                            sql = string.Concat(sql, " `state_iso_code` = @stateIsoCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateIsoCodeWhere", addressWhere.StateIsoCode);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.State))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state` = @stateWhere");
                        else
                            sql = string.Concat(sql, " `state` = @stateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateWhere", addressWhere.State);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.Zip))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `zip` = @zipWhere");
                        else
                            sql = string.Concat(sql, " `zip` = @zipWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@zipWhere", addressWhere.Zip);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.RegionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `region_id` = @regionIdWhere");
                        else
                            sql = string.Concat(sql, " `region_id` = @regionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@regionIdWhere", addressWhere.RegionId);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.Country))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country` = @countryWhere");
                        else
                            sql = string.Concat(sql, " `country` = @countryWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryWhere", addressWhere.Country);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.CountryIsoCode2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2Where");
                        else
                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2Where");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCode2Where", addressWhere.CountryIsoCode2);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.AddressExtra))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_extra` = @addressExtraWhere");
                        else
                            sql = string.Concat(sql, " `address_extra` = @addressExtraWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressExtraWhere", addressWhere.AddressExtra);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.AddressPhone))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_phone` = @addressPhoneWhere");
                        else
                            sql = string.Concat(sql, " `address_phone` = @addressPhoneWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressPhoneWhere", addressWhere.AddressPhone);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.MainAddress))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `main_address` = @mainAddressWhere");
                        else
                            sql = string.Concat(sql, " `main_address` = @mainAddressWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mainAddressWhere", addressWhere.MainAddress);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.Enable))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `enable` = @enableWhere");
                        else
                            sql = string.Concat(sql, " `enable` = @enableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@enableWhere", addressWhere.Enable);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", addressWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", addressWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(addressWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", addressWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(address);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
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

        public List<AddressModel> Update(string connectionString, List<AddressModel> addressList, List<AddressModel> addressWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
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
                        for (int i = 0; i < addressList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_addresses SET ";
                                if (!string.IsNullOrEmpty(addressList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, addressList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].EntityId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `entity_id` = @entityId" + i);
                                    else
                                        sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityId" + i, addressList[i].EntityId);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].NameAddress))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name_address` = @nameAddress" + i);
                                    else
                                        sql = string.Concat(sql, " `name_address` = @nameAddress" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@nameAddress" + i, addressList[i].NameAddress);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressLine1))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `address_line_1` = @addressLine1" + i);
                                    else
                                        sql = string.Concat(sql, " `address_line_1` = @addressLine1" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressLine1" + i, addressList[i].AddressLine1);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressLine2))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `address_line_2` = @addressLine2" + i);
                                    else
                                        sql = string.Concat(sql, " `address_line_2` = @addressLine2" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressLine2" + i, addressList[i].AddressLine2);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].City))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `city` = @city" + i);
                                    else
                                        sql = string.Concat(sql, " `city` = @city" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@city" + i, addressList[i].City);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].StateIsoCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `state_iso_code` = @stateIsoCode" + i);
                                    else
                                        sql = string.Concat(sql, " `state_iso_code` = @stateIsoCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@stateIsoCode" + i, addressList[i].StateIsoCode);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].State))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `state` = @state" + i);
                                    else
                                        sql = string.Concat(sql, " `state` = @state" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@state" + i, addressList[i].State);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].Zip))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `zip` = @zip" + i);
                                    else
                                        sql = string.Concat(sql, " `zip` = @zip" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@zip" + i, addressList[i].Zip);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].RegionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `region_id` = @regionId" + i);
                                    else
                                        sql = string.Concat(sql, " `region_id` = @regionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@regionId" + i, addressList[i].RegionId);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].Country))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country` = @country" + i);
                                    else
                                        sql = string.Concat(sql, " `country` = @country" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@country" + i, addressList[i].Country);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].CountryIsoCode2))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso_code_2` = @countryIsoCode2" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIsoCode2" + i, addressList[i].CountryIsoCode2);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressExtra))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `address_extra` = @addressExtra" + i);
                                    else
                                        sql = string.Concat(sql, " `address_extra` = @addressExtra" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressExtra" + i, addressList[i].AddressExtra);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].AddressPhone))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `address_phone` = @addressPhone" + i);
                                    else
                                        sql = string.Concat(sql, " `address_phone` = @addressPhone" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@addressPhone" + i, addressList[i].AddressPhone);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].MainAddress))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `main_address` = @mainAddress" + i);
                                    else
                                        sql = string.Concat(sql, " `main_address` = @mainAddress" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mainAddress" + i, addressList[i].MainAddress);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].Enable))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `enable` = @enable" + i);
                                    else
                                        sql = string.Concat(sql, " `enable` = @enable" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@enable" + i, addressList[i].Enable);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, addressList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, addressList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(addressList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, addressList[i].UpdatedAt);
                                }
                                if (addressWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(addressWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, addressWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityIdWhere" + i, addressWhereList[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].NameAddress))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name_address` = @nameAddressWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name_address` = @nameAddressWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameAddressWhere" + i, addressWhereList[i].NameAddress);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].AddressLine1))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_line_1` = @addressLine1Where" + i);
                                        else
                                            sql = string.Concat(sql, " `address_line_1` = @addressLine1Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressLine1Where" + i, addressWhereList[i].AddressLine1);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].AddressLine2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_line_2` = @addressLine2Where" + i);
                                        else
                                            sql = string.Concat(sql, " `address_line_2` = @addressLine2Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressLine2Where" + i, addressWhereList[i].AddressLine2);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].City))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `city` = @cityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `city` = @cityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@cityWhere" + i, addressWhereList[i].City);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].StateIsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_iso_code` = @stateIsoCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `state_iso_code` = @stateIsoCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateIsoCodeWhere" + i, addressWhereList[i].StateIsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].State))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state` = @stateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `state` = @stateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateWhere" + i, addressWhereList[i].State);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].Zip))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `zip` = @zipWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `zip` = @zipWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@zipWhere" + i, addressWhereList[i].Zip);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].RegionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `region_id` = @regionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `region_id` = @regionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@regionIdWhere" + i, addressWhereList[i].RegionId);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].Country))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country` = @countryWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country` = @countryWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryWhere" + i, addressWhereList[i].Country);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].CountryIsoCode2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2Where" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2Where" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCode2Where" + i, addressWhereList[i].CountryIsoCode2);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].AddressExtra))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_extra` = @addressExtraWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `address_extra` = @addressExtraWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressExtraWhere" + i, addressWhereList[i].AddressExtra);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].AddressPhone))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_phone` = @addressPhoneWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `address_phone` = @addressPhoneWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressPhoneWhere" + i, addressWhereList[i].AddressPhone);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].MainAddress))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `main_address` = @mainAddressWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `main_address` = @mainAddressWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mainAddressWhere" + i, addressWhereList[i].MainAddress);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].Enable))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `enable` = @enableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `enable` = @enableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@enableWhere" + i, addressWhereList[i].Enable);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, addressWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, addressWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(addressWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, addressWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(addressList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
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

        public List<AddressModel> Delete(string connectionString, AddressModel address)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_addresses  ";
                if (address != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(address.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", address.Id);
                    }
                    if (!string.IsNullOrEmpty(address.EntityId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_id` = @entityId");
                        else
                            sql = string.Concat(sql, " `entity_id` = @entityId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityId", address.EntityId);
                    }
                    if (!string.IsNullOrEmpty(address.NameAddress))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name_address` = @nameAddress");
                        else
                            sql = string.Concat(sql, " `name_address` = @nameAddress");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameAddress", address.NameAddress);
                    }
                    if (!string.IsNullOrEmpty(address.AddressLine1))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_line_1` = @addressLine1");
                        else
                            sql = string.Concat(sql, " `address_line_1` = @addressLine1");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                    }
                    if (!string.IsNullOrEmpty(address.AddressLine2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_line_2` = @addressLine2");
                        else
                            sql = string.Concat(sql, " `address_line_2` = @addressLine2");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                    }
                    if (!string.IsNullOrEmpty(address.City))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `city` = @city");
                        else
                            sql = string.Concat(sql, " `city` = @city");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@city", address.City);
                    }
                    if (!string.IsNullOrEmpty(address.StateIsoCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state_iso_code` = @stateIsoCode");
                        else
                            sql = string.Concat(sql, " `state_iso_code` = @stateIsoCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@stateIsoCode", address.StateIsoCode);
                    }
                    if (!string.IsNullOrEmpty(address.State))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `state` = @state");
                        else
                            sql = string.Concat(sql, " `state` = @state");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@state", address.State);
                    }
                    if (!string.IsNullOrEmpty(address.Zip))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `zip` = @zip");
                        else
                            sql = string.Concat(sql, " `zip` = @zip");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@zip", address.Zip);
                    }
                    if (!string.IsNullOrEmpty(address.RegionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `region_id` = @regionId");
                        else
                            sql = string.Concat(sql, " `region_id` = @regionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@regionId", address.RegionId);
                    }
                    if (!string.IsNullOrEmpty(address.Country))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country` = @country");
                        else
                            sql = string.Concat(sql, " `country` = @country");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@country", address.Country);
                    }
                    if (!string.IsNullOrEmpty(address.CountryIsoCode2))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2");
                        else
                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoCode2", address.CountryIsoCode2);
                    }
                    if (!string.IsNullOrEmpty(address.AddressExtra))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_extra` = @addressExtra");
                        else
                            sql = string.Concat(sql, " `address_extra` = @addressExtra");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressExtra", address.AddressExtra);
                    }
                    if (!string.IsNullOrEmpty(address.AddressPhone))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `address_phone` = @addressPhone");
                        else
                            sql = string.Concat(sql, " `address_phone` = @addressPhone");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@addressPhone", address.AddressPhone);
                    }
                    if (!string.IsNullOrEmpty(address.MainAddress))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `main_address` = @mainAddress");
                        else
                            sql = string.Concat(sql, " `main_address` = @mainAddress");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mainAddress", address.MainAddress);
                    }
                    if (!string.IsNullOrEmpty(address.Enable))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `enable` = @enable");
                        else
                            sql = string.Concat(sql, " `enable` = @enable");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@enable", address.Enable);
                    }
                    if (!string.IsNullOrEmpty(address.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", address.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(address.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", address.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(address.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", address.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(address);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
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

        public List<AddressModel> Delete(string connectionString, List<AddressModel> address)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AddressModel> response = new List<AddressModel>();
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
                        for (int i = 0; i < address.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_addresses  ";
                                if (address[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(address[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, address[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].EntityId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_id` = @entityId" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_id` = @entityId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityId" + i, address[i].EntityId);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].NameAddress))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name_address` = @nameAddress" + i);
                                        else
                                            sql = string.Concat(sql, " `name_address` = @nameAddress" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameAddress" + i, address[i].NameAddress);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].AddressLine1))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_line_1` = @addressLine1" + i);
                                        else
                                            sql = string.Concat(sql, " `address_line_1` = @addressLine1" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressLine1" + i, address[i].AddressLine1);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].AddressLine2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_line_2` = @addressLine2" + i);
                                        else
                                            sql = string.Concat(sql, " `address_line_2` = @addressLine2" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressLine2" + i, address[i].AddressLine2);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].City))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `city` = @city" + i);
                                        else
                                            sql = string.Concat(sql, " `city` = @city" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@city" + i, address[i].City);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].StateIsoCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state_iso_code` = @stateIsoCode" + i);
                                        else
                                            sql = string.Concat(sql, " `state_iso_code` = @stateIsoCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@stateIsoCode" + i, address[i].StateIsoCode);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].State))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `state` = @state" + i);
                                        else
                                            sql = string.Concat(sql, " `state` = @state" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@state" + i, address[i].State);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].Zip))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `zip` = @zip" + i);
                                        else
                                            sql = string.Concat(sql, " `zip` = @zip" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@zip" + i, address[i].Zip);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].RegionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `region_id` = @regionId" + i);
                                        else
                                            sql = string.Concat(sql, " `region_id` = @regionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@regionId" + i, address[i].RegionId);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].Country))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country` = @country" + i);
                                        else
                                            sql = string.Concat(sql, " `country` = @country" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@country" + i, address[i].Country);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].CountryIsoCode2))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso_code_2` = @countryIsoCode2" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoCode2" + i, address[i].CountryIsoCode2);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].AddressExtra))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_extra` = @addressExtra" + i);
                                        else
                                            sql = string.Concat(sql, " `address_extra` = @addressExtra" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressExtra" + i, address[i].AddressExtra);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].AddressPhone))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `address_phone` = @addressPhone" + i);
                                        else
                                            sql = string.Concat(sql, " `address_phone` = @addressPhone" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@addressPhone" + i, address[i].AddressPhone);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].MainAddress))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `main_address` = @mainAddress" + i);
                                        else
                                            sql = string.Concat(sql, " `main_address` = @mainAddress" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mainAddress" + i, address[i].MainAddress);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].Enable))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `enable` = @enable" + i);
                                        else
                                            sql = string.Concat(sql, " `enable` = @enable" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@enable" + i, address[i].Enable);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, address[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, address[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(address[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, address[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(address[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AddressModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AddressModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AddressModel>();
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
