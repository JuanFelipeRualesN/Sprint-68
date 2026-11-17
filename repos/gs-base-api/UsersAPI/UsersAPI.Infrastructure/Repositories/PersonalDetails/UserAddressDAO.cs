using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails
{
	class UserAddressDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, UserAddressModel> ReadD(string connectionString, UserAddressModel userAddress, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAddressModel> response = new Dictionary<string, UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				{
					if (userAddress != null)
					{
						if (!String.IsNullOrEmpty(userAddress.Id))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `id` = @id ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `id` = @id ");
							}
							cmd.Parameters.AddWithValue("@id", userAddress.Id);
						}
						if (!String.IsNullOrEmpty(userAddress.UserId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id` = @userId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id` = @userId ");
							}
							cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
						}
						if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `person_address_name` = @personAddressName ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `person_address_name` = @personAddressName ");
							}
							cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine1))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_1` = @addressLine1 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_1` = @addressLine1 ");
							}
							cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_2` = @addressLine2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_2` = @addressLine2 ");
							}
							cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddress.City))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `city` = @city ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `city` = @city ");
							}
							cmd.Parameters.AddWithValue("@city", userAddress.City);
						}
						if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
							}
							cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddress.State))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state` = @state ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state` = @state ");
							}
							cmd.Parameters.AddWithValue("@state", userAddress.State);
						}
						if (!String.IsNullOrEmpty(userAddress.Zip))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `zip` = @zip ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `zip` = @zip ");
							}
							cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
						}
						if (!String.IsNullOrEmpty(userAddress.Country))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country` = @country ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country` = @country ");
							}
							cmd.Parameters.AddWithValue("@country", userAddress.Country);
						}
						if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country_iso_code_2` = @countryIsoCode2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country_iso_code_2` = @countryIsoCode2 ");
							}
							cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressExtra))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_extra` = @addressExtra ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_extra` = @addressExtra ");
							}
							cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressPhone))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_phone` = @addressPhone ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_phone` = @addressPhone ");
							}
							cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddress.MainAddress))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_address` = @mainAddress ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_address` = @mainAddress ");
							}
							cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddress.Enable))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `enable` = @enable ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `enable` = @enable ");
							}
							cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
						}
						if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
							}
							cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddress.CreatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_at` = @createdAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_at` = @createdAt ");
							}
							cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
							}
							cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
						}
						if (orderByLastDate)
							sql = String.Concat(sql, " ORDER BY updated_at DESC");
						if (firstOne)
							sql = String.Concat(sql, " LIMIT 1 ");
					}
					cmd.CommandText = sql;
					MySqlDataReader reader = cmd.ExecuteReader();
					try
					{
						if (reader.HasRows)
							while (reader.Read())
							{
								if (!reader.IsDBNull(0))
									userAddress.Id = reader.GetValue(0).ToString();
								else
									userAddress.Id = null;
								if (!reader.IsDBNull(1))
									userAddress.UserId = reader.GetValue(1).ToString();
								else
									userAddress.UserId = null;
								if (!reader.IsDBNull(2))
									userAddress.PersonAddressName = reader.GetValue(2).ToString();
								else
									userAddress.PersonAddressName = null;
								if (!reader.IsDBNull(3))
									userAddress.AddressLine1 = reader.GetValue(3).ToString();
								else
									userAddress.AddressLine1 = null;
								if (!reader.IsDBNull(4))
									userAddress.AddressLine2 = reader.GetValue(4).ToString();
								else
									userAddress.AddressLine2 = null;
								if (!reader.IsDBNull(5))
									userAddress.City = reader.GetValue(5).ToString();
								else
									userAddress.City = null;
								if (!reader.IsDBNull(6))
									userAddress.StateIsoCode = reader.GetValue(6).ToString();
								else
									userAddress.StateIsoCode = null;
								if (!reader.IsDBNull(7))
									userAddress.State = reader.GetValue(7).ToString();
								else
									userAddress.State = null;
								if (!reader.IsDBNull(8))
									userAddress.Zip = reader.GetValue(8).ToString();
								else
									userAddress.Zip = null;
								if (!reader.IsDBNull(9))
									userAddress.Country = reader.GetValue(9).ToString();
								else
									userAddress.Country = null;
								if (!reader.IsDBNull(10))
									userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
								else
									userAddress.CountryIsoCode2 = null;
								if (!reader.IsDBNull(11))
									userAddress.AddressExtra = reader.GetValue(11).ToString();
								else
									userAddress.AddressExtra = null;
								if (!reader.IsDBNull(12))
									userAddress.AddressPhone = reader.GetValue(12).ToString();
								else
									userAddress.AddressPhone = null;
								if (!reader.IsDBNull(13))
									userAddress.MainAddress = reader.GetValue(13).ToString();
								else
									userAddress.MainAddress = null;
								if (!reader.IsDBNull(14))
									userAddress.Enable = reader.GetValue(14).ToString();
								else
									userAddress.Enable = null;
								if (!reader.IsDBNull(15))
									userAddress.ModifiedBy = reader.GetValue(15).ToString();
								else
									userAddress.ModifiedBy = null;
								if (!reader.IsDBNull(16))
									userAddress.CreatedAt = reader.GetValue(16).ToString();
								else
									userAddress.CreatedAt = null;
								if (!reader.IsDBNull(17))
									userAddress.UpdatedAt = reader.GetValue(17).ToString();
								else
									userAddress.UpdatedAt = null;
								response.Add(userAddress.Id, userAddress);
								userAddress = new UserAddressModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserAddressModel>();
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
				response = new Dictionary<string, UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserAddressModel> ReadD(string connectionString, UserAddressModel userAddress, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAddressModel> response = new Dictionary<string, UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				{
					if (userAddress != null)
					{
						if (!String.IsNullOrEmpty(userAddress.Id))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `id` = @id ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `id` = @id ");
							}
							cmd.Parameters.AddWithValue("@id", userAddress.Id);
						}
						if (!String.IsNullOrEmpty(userAddress.UserId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id` = @userId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id` = @userId ");
							}
							cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
						}
						if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `person_address_name` = @personAddressName ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `person_address_name` = @personAddressName ");
							}
							cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine1))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_1` = @addressLine1 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_1` = @addressLine1 ");
							}
							cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_2` = @addressLine2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_2` = @addressLine2 ");
							}
							cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddress.City))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `city` = @city ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `city` = @city ");
							}
							cmd.Parameters.AddWithValue("@city", userAddress.City);
						}
						if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
							}
							cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddress.State))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state` = @state ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state` = @state ");
							}
							cmd.Parameters.AddWithValue("@state", userAddress.State);
						}
						if (!String.IsNullOrEmpty(userAddress.Zip))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `zip` = @zip ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `zip` = @zip ");
							}
							cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
						}
						if (!String.IsNullOrEmpty(userAddress.Country))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country` = @country ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country` = @country ");
							}
							cmd.Parameters.AddWithValue("@country", userAddress.Country);
						}
						if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country_iso_code_2` = @countryIsoCode2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country_iso_code_2` = @countryIsoCode2 ");
							}
							cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressExtra))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_extra` = @addressExtra ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_extra` = @addressExtra ");
							}
							cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressPhone))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_phone` = @addressPhone ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_phone` = @addressPhone ");
							}
							cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddress.MainAddress))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_address` = @mainAddress ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_address` = @mainAddress ");
							}
							cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddress.Enable))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `enable` = @enable ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `enable` = @enable ");
							}
							cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
						}
						if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
							}
							cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddress.CreatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_at` = @createdAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_at` = @createdAt ");
							}
							cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
							}
							cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
						}
						if (orderByLastDate)
							sql = String.Concat(sql, " ORDER BY updated_at DESC");
						sql = String.Concat(sql, " LIMIT @fromRow, @limit");
						cmd.Parameters.AddWithValue("@fromRow", fromRow);
						cmd.Parameters.AddWithValue("@limit", limit);
					}
					cmd.CommandText = sql;
					MySqlDataReader reader = cmd.ExecuteReader();
					try
					{
						if (reader.HasRows)
							while (reader.Read())
							{
								if (!reader.IsDBNull(0))
									userAddress.Id = reader.GetValue(0).ToString();
								else
									userAddress.Id = null;
								if (!reader.IsDBNull(1))
									userAddress.UserId = reader.GetValue(1).ToString();
								else
									userAddress.UserId = null;
								if (!reader.IsDBNull(2))
									userAddress.PersonAddressName = reader.GetValue(2).ToString();
								else
									userAddress.PersonAddressName = null;
								if (!reader.IsDBNull(3))
									userAddress.AddressLine1 = reader.GetValue(3).ToString();
								else
									userAddress.AddressLine1 = null;
								if (!reader.IsDBNull(4))
									userAddress.AddressLine2 = reader.GetValue(4).ToString();
								else
									userAddress.AddressLine2 = null;
								if (!reader.IsDBNull(5))
									userAddress.City = reader.GetValue(5).ToString();
								else
									userAddress.City = null;
								if (!reader.IsDBNull(6))
									userAddress.StateIsoCode = reader.GetValue(6).ToString();
								else
									userAddress.StateIsoCode = null;
								if (!reader.IsDBNull(7))
									userAddress.State = reader.GetValue(7).ToString();
								else
									userAddress.State = null;
								if (!reader.IsDBNull(8))
									userAddress.Zip = reader.GetValue(8).ToString();
								else
									userAddress.Zip = null;
								if (!reader.IsDBNull(9))
									userAddress.Country = reader.GetValue(9).ToString();
								else
									userAddress.Country = null;
								if (!reader.IsDBNull(10))
									userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
								else
									userAddress.CountryIsoCode2 = null;
								if (!reader.IsDBNull(11))
									userAddress.AddressExtra = reader.GetValue(11).ToString();
								else
									userAddress.AddressExtra = null;
								if (!reader.IsDBNull(12))
									userAddress.AddressPhone = reader.GetValue(12).ToString();
								else
									userAddress.AddressPhone = null;
								if (!reader.IsDBNull(13))
									userAddress.MainAddress = reader.GetValue(13).ToString();
								else
									userAddress.MainAddress = null;
								if (!reader.IsDBNull(14))
									userAddress.Enable = reader.GetValue(14).ToString();
								else
									userAddress.Enable = null;
								if (!reader.IsDBNull(15))
									userAddress.ModifiedBy = reader.GetValue(15).ToString();
								else
									userAddress.ModifiedBy = null;
								if (!reader.IsDBNull(16))
									userAddress.CreatedAt = reader.GetValue(16).ToString();
								else
									userAddress.CreatedAt = null;
								if (!reader.IsDBNull(17))
									userAddress.UpdatedAt = reader.GetValue(17).ToString();
								else
									userAddress.UpdatedAt = null;
								response.Add(userAddress.Id, userAddress);
								userAddress = new UserAddressModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserAddressModel>();
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
				response = new Dictionary<string, UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserAddressModel> ReadD(string connectionString, List<UserAddressModel> userAddressWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAddressModel> response = new Dictionary<string, UserAddressModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				if (userAddressWhere != null && userAddressWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAddressWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAddressWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAddressWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userAddressWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].PersonAddressName))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `person_address_name` = @personAddressName" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@personAddressName" + i, userAddressWhere[i].PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine1))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_1` = @addressLine1" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine1" + i, userAddressWhere[i].AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_2` = @addressLine2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine2" + i, userAddressWhere[i].AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].City))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `city` = @city" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@city" + i, userAddressWhere[i].City);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].StateIsoCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state_iso_code` = @stateIsoCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddressWhere[i].StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].State))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state` = @state" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@state" + i, userAddressWhere[i].State);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Zip))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `zip` = @zip" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@zip" + i, userAddressWhere[i].Zip);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Country))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country` = @country" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@country" + i, userAddressWhere[i].Country);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CountryIsoCode2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddressWhere[i].CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressExtra))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_extra` = @addressExtra" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressExtra" + i, userAddressWhere[i].AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressPhone))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_phone` = @addressPhone" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressPhone" + i, userAddressWhere[i].AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].MainAddress))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_address` = @mainAddress" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainAddress" + i, userAddressWhere[i].MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Enable))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `enable` = @enable" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@enable" + i, userAddressWhere[i].Enable);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddressWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAddressWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAddressWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAddressWhere.Count - 1)
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
							UserAddressModel userAddress = new UserAddressModel(); if (!reader.IsDBNull(0))
								userAddress.Id = reader.GetValue(0).ToString();
							else
								userAddress.Id = null;
							if (!reader.IsDBNull(1))
								userAddress.UserId = reader.GetValue(1).ToString();
							else
								userAddress.UserId = null;
							if (!reader.IsDBNull(2))
								userAddress.PersonAddressName = reader.GetValue(2).ToString();
							else
								userAddress.PersonAddressName = null;
							if (!reader.IsDBNull(3))
								userAddress.AddressLine1 = reader.GetValue(3).ToString();
							else
								userAddress.AddressLine1 = null;
							if (!reader.IsDBNull(4))
								userAddress.AddressLine2 = reader.GetValue(4).ToString();
							else
								userAddress.AddressLine2 = null;
							if (!reader.IsDBNull(5))
								userAddress.City = reader.GetValue(5).ToString();
							else
								userAddress.City = null;
							if (!reader.IsDBNull(6))
								userAddress.StateIsoCode = reader.GetValue(6).ToString();
							else
								userAddress.StateIsoCode = null;
							if (!reader.IsDBNull(7))
								userAddress.State = reader.GetValue(7).ToString();
							else
								userAddress.State = null;
							if (!reader.IsDBNull(8))
								userAddress.Zip = reader.GetValue(8).ToString();
							else
								userAddress.Zip = null;
							if (!reader.IsDBNull(9))
								userAddress.Country = reader.GetValue(9).ToString();
							else
								userAddress.Country = null;
							if (!reader.IsDBNull(10))
								userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
							else
								userAddress.CountryIsoCode2 = null;
							if (!reader.IsDBNull(11))
								userAddress.AddressExtra = reader.GetValue(11).ToString();
							else
								userAddress.AddressExtra = null;
							if (!reader.IsDBNull(12))
								userAddress.AddressPhone = reader.GetValue(12).ToString();
							else
								userAddress.AddressPhone = null;
							if (!reader.IsDBNull(13))
								userAddress.MainAddress = reader.GetValue(13).ToString();
							else
								userAddress.MainAddress = null;
							if (!reader.IsDBNull(14))
								userAddress.Enable = reader.GetValue(14).ToString();
							else
								userAddress.Enable = null;
							if (!reader.IsDBNull(15))
								userAddress.ModifiedBy = reader.GetValue(15).ToString();
							else
								userAddress.ModifiedBy = null;
							if (!reader.IsDBNull(16))
								userAddress.CreatedAt = reader.GetValue(16).ToString();
							else
								userAddress.CreatedAt = null;
							if (!reader.IsDBNull(17))
								userAddress.UpdatedAt = reader.GetValue(17).ToString();
							else
								userAddress.UpdatedAt = null;
							response.Add(userAddress.Id, userAddress);
							userAddress = new UserAddressModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserAddressModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserAddressModel> ReadD(string connectionString, List<UserAddressModel> userAddressWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAddressModel> response = new Dictionary<string, UserAddressModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				if (userAddressWhere != null && userAddressWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAddressWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAddressWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAddressWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userAddressWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].PersonAddressName))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `person_address_name` = @personAddressName" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@personAddressName" + i, userAddressWhere[i].PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine1))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_1` = @addressLine1" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine1" + i, userAddressWhere[i].AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_2` = @addressLine2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine2" + i, userAddressWhere[i].AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].City))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `city` = @city" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@city" + i, userAddressWhere[i].City);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].StateIsoCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state_iso_code` = @stateIsoCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddressWhere[i].StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].State))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state` = @state" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@state" + i, userAddressWhere[i].State);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Zip))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `zip` = @zip" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@zip" + i, userAddressWhere[i].Zip);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Country))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country` = @country" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@country" + i, userAddressWhere[i].Country);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CountryIsoCode2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddressWhere[i].CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressExtra))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_extra` = @addressExtra" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressExtra" + i, userAddressWhere[i].AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressPhone))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_phone` = @addressPhone" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressPhone" + i, userAddressWhere[i].AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].MainAddress))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_address` = @mainAddress" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainAddress" + i, userAddressWhere[i].MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Enable))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `enable` = @enable" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@enable" + i, userAddressWhere[i].Enable);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddressWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAddressWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAddressWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAddressWhere.Count - 1)
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
							UserAddressModel userAddress = new UserAddressModel(); if (!reader.IsDBNull(0))
								userAddress.Id = reader.GetValue(0).ToString();
							else
								userAddress.Id = null;
							if (!reader.IsDBNull(1))
								userAddress.UserId = reader.GetValue(1).ToString();
							else
								userAddress.UserId = null;
							if (!reader.IsDBNull(2))
								userAddress.PersonAddressName = reader.GetValue(2).ToString();
							else
								userAddress.PersonAddressName = null;
							if (!reader.IsDBNull(3))
								userAddress.AddressLine1 = reader.GetValue(3).ToString();
							else
								userAddress.AddressLine1 = null;
							if (!reader.IsDBNull(4))
								userAddress.AddressLine2 = reader.GetValue(4).ToString();
							else
								userAddress.AddressLine2 = null;
							if (!reader.IsDBNull(5))
								userAddress.City = reader.GetValue(5).ToString();
							else
								userAddress.City = null;
							if (!reader.IsDBNull(6))
								userAddress.StateIsoCode = reader.GetValue(6).ToString();
							else
								userAddress.StateIsoCode = null;
							if (!reader.IsDBNull(7))
								userAddress.State = reader.GetValue(7).ToString();
							else
								userAddress.State = null;
							if (!reader.IsDBNull(8))
								userAddress.Zip = reader.GetValue(8).ToString();
							else
								userAddress.Zip = null;
							if (!reader.IsDBNull(9))
								userAddress.Country = reader.GetValue(9).ToString();
							else
								userAddress.Country = null;
							if (!reader.IsDBNull(10))
								userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
							else
								userAddress.CountryIsoCode2 = null;
							if (!reader.IsDBNull(11))
								userAddress.AddressExtra = reader.GetValue(11).ToString();
							else
								userAddress.AddressExtra = null;
							if (!reader.IsDBNull(12))
								userAddress.AddressPhone = reader.GetValue(12).ToString();
							else
								userAddress.AddressPhone = null;
							if (!reader.IsDBNull(13))
								userAddress.MainAddress = reader.GetValue(13).ToString();
							else
								userAddress.MainAddress = null;
							if (!reader.IsDBNull(14))
								userAddress.Enable = reader.GetValue(14).ToString();
							else
								userAddress.Enable = null;
							if (!reader.IsDBNull(15))
								userAddress.ModifiedBy = reader.GetValue(15).ToString();
							else
								userAddress.ModifiedBy = null;
							if (!reader.IsDBNull(16))
								userAddress.CreatedAt = reader.GetValue(16).ToString();
							else
								userAddress.CreatedAt = null;
							if (!reader.IsDBNull(17))
								userAddress.UpdatedAt = reader.GetValue(17).ToString();
							else
								userAddress.UpdatedAt = null;
							response.Add(userAddress.Id, userAddress);
							userAddress = new UserAddressModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserAddressModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAddressModel> Read(string connectionString, UserAddressModel userAddress, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				{
					if (userAddress != null)
					{
						if (!String.IsNullOrEmpty(userAddress.Id))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `id` = @id ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `id` = @id ");
							}
							cmd.Parameters.AddWithValue("@id", userAddress.Id);
						}
						if (!String.IsNullOrEmpty(userAddress.UserId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id` = @userId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id` = @userId ");
							}
							cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
						}
						if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `person_address_name` = @personAddressName ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `person_address_name` = @personAddressName ");
							}
							cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine1))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_1` = @addressLine1 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_1` = @addressLine1 ");
							}
							cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_2` = @addressLine2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_2` = @addressLine2 ");
							}
							cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddress.City))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `city` = @city ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `city` = @city ");
							}
							cmd.Parameters.AddWithValue("@city", userAddress.City);
						}
						if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
							}
							cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddress.State))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state` = @state ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state` = @state ");
							}
							cmd.Parameters.AddWithValue("@state", userAddress.State);
						}
						if (!String.IsNullOrEmpty(userAddress.Zip))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `zip` = @zip ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `zip` = @zip ");
							}
							cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
						}
						if (!String.IsNullOrEmpty(userAddress.Country))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country` = @country ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country` = @country ");
							}
							cmd.Parameters.AddWithValue("@country", userAddress.Country);
						}
						if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country_iso_code_2` = @countryIsoCode2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country_iso_code_2` = @countryIsoCode2 ");
							}
							cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressExtra))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_extra` = @addressExtra ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_extra` = @addressExtra ");
							}
							cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressPhone))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_phone` = @addressPhone ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_phone` = @addressPhone ");
							}
							cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddress.MainAddress))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_address` = @mainAddress ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_address` = @mainAddress ");
							}
							cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddress.Enable))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `enable` = @enable ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `enable` = @enable ");
							}
							cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
						}
						if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
							}
							cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddress.CreatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_at` = @createdAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_at` = @createdAt ");
							}
							cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
							}
							cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
						}
						if (orderByLastDate)
							sql = String.Concat(sql, " ORDER BY updated_at DESC");
						if (firstOne)
							sql = String.Concat(sql, " LIMIT 1 ");
					}
					cmd.CommandText = sql;
					MySqlDataReader reader = cmd.ExecuteReader();
					try
					{
						if (reader.HasRows)
							while (reader.Read())
							{
								if (!reader.IsDBNull(0))
									userAddress.Id = reader.GetValue(0).ToString();
								else
									userAddress.Id = null;
								if (!reader.IsDBNull(1))
									userAddress.UserId = reader.GetValue(1).ToString();
								else
									userAddress.UserId = null;
								if (!reader.IsDBNull(2))
									userAddress.PersonAddressName = reader.GetValue(2).ToString();
								else
									userAddress.PersonAddressName = null;
								if (!reader.IsDBNull(3))
									userAddress.AddressLine1 = reader.GetValue(3).ToString();
								else
									userAddress.AddressLine1 = null;
								if (!reader.IsDBNull(4))
									userAddress.AddressLine2 = reader.GetValue(4).ToString();
								else
									userAddress.AddressLine2 = null;
								if (!reader.IsDBNull(5))
									userAddress.City = reader.GetValue(5).ToString();
								else
									userAddress.City = null;
								if (!reader.IsDBNull(6))
									userAddress.StateIsoCode = reader.GetValue(6).ToString();
								else
									userAddress.StateIsoCode = null;
								if (!reader.IsDBNull(7))
									userAddress.State = reader.GetValue(7).ToString();
								else
									userAddress.State = null;
								if (!reader.IsDBNull(8))
									userAddress.Zip = reader.GetValue(8).ToString();
								else
									userAddress.Zip = null;
								if (!reader.IsDBNull(9))
									userAddress.Country = reader.GetValue(9).ToString();
								else
									userAddress.Country = null;
								if (!reader.IsDBNull(10))
									userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
								else
									userAddress.CountryIsoCode2 = null;
								if (!reader.IsDBNull(11))
									userAddress.AddressExtra = reader.GetValue(11).ToString();
								else
									userAddress.AddressExtra = null;
								if (!reader.IsDBNull(12))
									userAddress.AddressPhone = reader.GetValue(12).ToString();
								else
									userAddress.AddressPhone = null;
								if (!reader.IsDBNull(13))
									userAddress.MainAddress = reader.GetValue(13).ToString();
								else
									userAddress.MainAddress = null;
								if (!reader.IsDBNull(14))
									userAddress.Enable = reader.GetValue(14).ToString();
								else
									userAddress.Enable = null;
								if (!reader.IsDBNull(15))
									userAddress.ModifiedBy = reader.GetValue(15).ToString();
								else
									userAddress.ModifiedBy = null;
								if (!reader.IsDBNull(16))
									userAddress.CreatedAt = reader.GetValue(16).ToString();
								else
									userAddress.CreatedAt = null;
								if (!reader.IsDBNull(17))
									userAddress.UpdatedAt = reader.GetValue(17).ToString();
								else
									userAddress.UpdatedAt = null;
								response.Add(userAddress);
								userAddress = new UserAddressModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserAddressModel>();
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
				response = new List<UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAddressModel> Read(string connectionString, UserAddressModel userAddress, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				{
					if (userAddress != null)
					{
						if (!String.IsNullOrEmpty(userAddress.Id))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `id` = @id ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `id` = @id ");
							}
							cmd.Parameters.AddWithValue("@id", userAddress.Id);
						}
						if (!String.IsNullOrEmpty(userAddress.UserId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id` = @userId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id` = @userId ");
							}
							cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
						}
						if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `person_address_name` = @personAddressName ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `person_address_name` = @personAddressName ");
							}
							cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine1))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_1` = @addressLine1 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_1` = @addressLine1 ");
							}
							cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressLine2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_line_2` = @addressLine2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_line_2` = @addressLine2 ");
							}
							cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddress.City))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `city` = @city ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `city` = @city ");
							}
							cmd.Parameters.AddWithValue("@city", userAddress.City);
						}
						if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state_iso_code` = @stateIsoCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state_iso_code` = @stateIsoCode ");
							}
							cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddress.State))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `state` = @state ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `state` = @state ");
							}
							cmd.Parameters.AddWithValue("@state", userAddress.State);
						}
						if (!String.IsNullOrEmpty(userAddress.Zip))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `zip` = @zip ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `zip` = @zip ");
							}
							cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
						}
						if (!String.IsNullOrEmpty(userAddress.Country))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country` = @country ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country` = @country ");
							}
							cmd.Parameters.AddWithValue("@country", userAddress.Country);
						}
						if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `country_iso_code_2` = @countryIsoCode2 ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `country_iso_code_2` = @countryIsoCode2 ");
							}
							cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressExtra))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_extra` = @addressExtra ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_extra` = @addressExtra ");
							}
							cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddress.AddressPhone))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `address_phone` = @addressPhone ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `address_phone` = @addressPhone ");
							}
							cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddress.MainAddress))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_address` = @mainAddress ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_address` = @mainAddress ");
							}
							cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddress.Enable))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `enable` = @enable ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `enable` = @enable ");
							}
							cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
						}
						if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
							}
							cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddress.CreatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_at` = @createdAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_at` = @createdAt ");
							}
							cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
							}
							cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
						}
						if (orderByLastDate)
							sql = String.Concat(sql, " ORDER BY updated_at DESC");
						sql = String.Concat(sql, " LIMIT @fromRow, @limit");
						cmd.Parameters.AddWithValue("@fromRow", fromRow);
						cmd.Parameters.AddWithValue("@limit", limit);
					}
					cmd.CommandText = sql;
					MySqlDataReader reader = cmd.ExecuteReader();
					try
					{
						if (reader.HasRows)
							while (reader.Read())
							{
								if (!reader.IsDBNull(0))
									userAddress.Id = reader.GetValue(0).ToString();
								else
									userAddress.Id = null;
								if (!reader.IsDBNull(1))
									userAddress.UserId = reader.GetValue(1).ToString();
								else
									userAddress.UserId = null;
								if (!reader.IsDBNull(2))
									userAddress.PersonAddressName = reader.GetValue(2).ToString();
								else
									userAddress.PersonAddressName = null;
								if (!reader.IsDBNull(3))
									userAddress.AddressLine1 = reader.GetValue(3).ToString();
								else
									userAddress.AddressLine1 = null;
								if (!reader.IsDBNull(4))
									userAddress.AddressLine2 = reader.GetValue(4).ToString();
								else
									userAddress.AddressLine2 = null;
								if (!reader.IsDBNull(5))
									userAddress.City = reader.GetValue(5).ToString();
								else
									userAddress.City = null;
								if (!reader.IsDBNull(6))
									userAddress.StateIsoCode = reader.GetValue(6).ToString();
								else
									userAddress.StateIsoCode = null;
								if (!reader.IsDBNull(7))
									userAddress.State = reader.GetValue(7).ToString();
								else
									userAddress.State = null;
								if (!reader.IsDBNull(8))
									userAddress.Zip = reader.GetValue(8).ToString();
								else
									userAddress.Zip = null;
								if (!reader.IsDBNull(9))
									userAddress.Country = reader.GetValue(9).ToString();
								else
									userAddress.Country = null;
								if (!reader.IsDBNull(10))
									userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
								else
									userAddress.CountryIsoCode2 = null;
								if (!reader.IsDBNull(11))
									userAddress.AddressExtra = reader.GetValue(11).ToString();
								else
									userAddress.AddressExtra = null;
								if (!reader.IsDBNull(12))
									userAddress.AddressPhone = reader.GetValue(12).ToString();
								else
									userAddress.AddressPhone = null;
								if (!reader.IsDBNull(13))
									userAddress.MainAddress = reader.GetValue(13).ToString();
								else
									userAddress.MainAddress = null;
								if (!reader.IsDBNull(14))
									userAddress.Enable = reader.GetValue(14).ToString();
								else
									userAddress.Enable = null;
								if (!reader.IsDBNull(15))
									userAddress.ModifiedBy = reader.GetValue(15).ToString();
								else
									userAddress.ModifiedBy = null;
								if (!reader.IsDBNull(16))
									userAddress.CreatedAt = reader.GetValue(16).ToString();
								else
									userAddress.CreatedAt = null;
								if (!reader.IsDBNull(17))
									userAddress.UpdatedAt = reader.GetValue(17).ToString();
								else
									userAddress.UpdatedAt = null;
								response.Add(userAddress);
								userAddress = new UserAddressModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserAddressModel>();
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
				response = new List<UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAddressModel> Read(string connectionString, List<UserAddressModel> userAddressWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				if (userAddressWhere != null && userAddressWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAddressWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAddressWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAddressWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userAddressWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].PersonAddressName))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `person_address_name` = @personAddressName" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@personAddressName" + i, userAddressWhere[i].PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine1))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_1` = @addressLine1" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine1" + i, userAddressWhere[i].AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_2` = @addressLine2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine2" + i, userAddressWhere[i].AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].City))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `city` = @city" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@city" + i, userAddressWhere[i].City);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].StateIsoCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state_iso_code` = @stateIsoCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddressWhere[i].StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].State))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state` = @state" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@state" + i, userAddressWhere[i].State);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Zip))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `zip` = @zip" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@zip" + i, userAddressWhere[i].Zip);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Country))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country` = @country" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@country" + i, userAddressWhere[i].Country);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CountryIsoCode2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddressWhere[i].CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressExtra))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_extra` = @addressExtra" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressExtra" + i, userAddressWhere[i].AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressPhone))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_phone` = @addressPhone" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressPhone" + i, userAddressWhere[i].AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].MainAddress))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_address` = @mainAddress" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainAddress" + i, userAddressWhere[i].MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Enable))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `enable` = @enable" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@enable" + i, userAddressWhere[i].Enable);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddressWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAddressWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAddressWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAddressWhere.Count - 1)
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
							UserAddressModel userAddress = new UserAddressModel(); if (!reader.IsDBNull(0))
								userAddress.Id = reader.GetValue(0).ToString();
							else
								userAddress.Id = null;
							if (!reader.IsDBNull(1))
								userAddress.UserId = reader.GetValue(1).ToString();
							else
								userAddress.UserId = null;
							if (!reader.IsDBNull(2))
								userAddress.PersonAddressName = reader.GetValue(2).ToString();
							else
								userAddress.PersonAddressName = null;
							if (!reader.IsDBNull(3))
								userAddress.AddressLine1 = reader.GetValue(3).ToString();
							else
								userAddress.AddressLine1 = null;
							if (!reader.IsDBNull(4))
								userAddress.AddressLine2 = reader.GetValue(4).ToString();
							else
								userAddress.AddressLine2 = null;
							if (!reader.IsDBNull(5))
								userAddress.City = reader.GetValue(5).ToString();
							else
								userAddress.City = null;
							if (!reader.IsDBNull(6))
								userAddress.StateIsoCode = reader.GetValue(6).ToString();
							else
								userAddress.StateIsoCode = null;
							if (!reader.IsDBNull(7))
								userAddress.State = reader.GetValue(7).ToString();
							else
								userAddress.State = null;
							if (!reader.IsDBNull(8))
								userAddress.Zip = reader.GetValue(8).ToString();
							else
								userAddress.Zip = null;
							if (!reader.IsDBNull(9))
								userAddress.Country = reader.GetValue(9).ToString();
							else
								userAddress.Country = null;
							if (!reader.IsDBNull(10))
								userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
							else
								userAddress.CountryIsoCode2 = null;
							if (!reader.IsDBNull(11))
								userAddress.AddressExtra = reader.GetValue(11).ToString();
							else
								userAddress.AddressExtra = null;
							if (!reader.IsDBNull(12))
								userAddress.AddressPhone = reader.GetValue(12).ToString();
							else
								userAddress.AddressPhone = null;
							if (!reader.IsDBNull(13))
								userAddress.MainAddress = reader.GetValue(13).ToString();
							else
								userAddress.MainAddress = null;
							if (!reader.IsDBNull(14))
								userAddress.Enable = reader.GetValue(14).ToString();
							else
								userAddress.Enable = null;
							if (!reader.IsDBNull(15))
								userAddress.ModifiedBy = reader.GetValue(15).ToString();
							else
								userAddress.ModifiedBy = null;
							if (!reader.IsDBNull(16))
								userAddress.CreatedAt = reader.GetValue(16).ToString();
							else
								userAddress.CreatedAt = null;
							if (!reader.IsDBNull(17))
								userAddress.UpdatedAt = reader.GetValue(17).ToString();
							else
								userAddress.UpdatedAt = null;
							response.Add(userAddress);
							userAddress = new UserAddressModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserAddressModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAddressModel> Read(string connectionString, List<UserAddressModel> userAddressWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `person_address_name`, `address_line_1`, `address_line_2`, `city`, `state_iso_code`, `state`, `zip`, `country`, `country_iso_code_2`, `address_extra`, `address_phone`, `main_address`, `enable`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_addresses ";
				if (userAddressWhere != null && userAddressWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAddressWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAddressWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAddressWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userAddressWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].PersonAddressName))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `person_address_name` = @personAddressName" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@personAddressName" + i, userAddressWhere[i].PersonAddressName);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine1))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_1` = @addressLine1" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine1" + i, userAddressWhere[i].AddressLine1);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressLine2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_line_2` = @addressLine2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressLine2" + i, userAddressWhere[i].AddressLine2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].City))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `city` = @city" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@city" + i, userAddressWhere[i].City);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].StateIsoCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state_iso_code` = @stateIsoCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddressWhere[i].StateIsoCode);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].State))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `state` = @state" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@state" + i, userAddressWhere[i].State);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Zip))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `zip` = @zip" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@zip" + i, userAddressWhere[i].Zip);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Country))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country` = @country" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@country" + i, userAddressWhere[i].Country);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CountryIsoCode2))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `country_iso_code_2` = @countryIsoCode2" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddressWhere[i].CountryIsoCode2);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressExtra))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_extra` = @addressExtra" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressExtra" + i, userAddressWhere[i].AddressExtra);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].AddressPhone))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `address_phone` = @addressPhone" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@addressPhone" + i, userAddressWhere[i].AddressPhone);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].MainAddress))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_address` = @mainAddress" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainAddress" + i, userAddressWhere[i].MainAddress);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].Enable))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `enable` = @enable" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@enable" + i, userAddressWhere[i].Enable);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddressWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAddressWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAddressWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAddressWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAddressWhere.Count - 1)
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
							UserAddressModel userAddress = new UserAddressModel(); if (!reader.IsDBNull(0))
								userAddress.Id = reader.GetValue(0).ToString();
							else
								userAddress.Id = null;
							if (!reader.IsDBNull(1))
								userAddress.UserId = reader.GetValue(1).ToString();
							else
								userAddress.UserId = null;
							if (!reader.IsDBNull(2))
								userAddress.PersonAddressName = reader.GetValue(2).ToString();
							else
								userAddress.PersonAddressName = null;
							if (!reader.IsDBNull(3))
								userAddress.AddressLine1 = reader.GetValue(3).ToString();
							else
								userAddress.AddressLine1 = null;
							if (!reader.IsDBNull(4))
								userAddress.AddressLine2 = reader.GetValue(4).ToString();
							else
								userAddress.AddressLine2 = null;
							if (!reader.IsDBNull(5))
								userAddress.City = reader.GetValue(5).ToString();
							else
								userAddress.City = null;
							if (!reader.IsDBNull(6))
								userAddress.StateIsoCode = reader.GetValue(6).ToString();
							else
								userAddress.StateIsoCode = null;
							if (!reader.IsDBNull(7))
								userAddress.State = reader.GetValue(7).ToString();
							else
								userAddress.State = null;
							if (!reader.IsDBNull(8))
								userAddress.Zip = reader.GetValue(8).ToString();
							else
								userAddress.Zip = null;
							if (!reader.IsDBNull(9))
								userAddress.Country = reader.GetValue(9).ToString();
							else
								userAddress.Country = null;
							if (!reader.IsDBNull(10))
								userAddress.CountryIsoCode2 = reader.GetValue(10).ToString();
							else
								userAddress.CountryIsoCode2 = null;
							if (!reader.IsDBNull(11))
								userAddress.AddressExtra = reader.GetValue(11).ToString();
							else
								userAddress.AddressExtra = null;
							if (!reader.IsDBNull(12))
								userAddress.AddressPhone = reader.GetValue(12).ToString();
							else
								userAddress.AddressPhone = null;
							if (!reader.IsDBNull(13))
								userAddress.MainAddress = reader.GetValue(13).ToString();
							else
								userAddress.MainAddress = null;
							if (!reader.IsDBNull(14))
								userAddress.Enable = reader.GetValue(14).ToString();
							else
								userAddress.Enable = null;
							if (!reader.IsDBNull(15))
								userAddress.ModifiedBy = reader.GetValue(15).ToString();
							else
								userAddress.ModifiedBy = null;
							if (!reader.IsDBNull(16))
								userAddress.CreatedAt = reader.GetValue(16).ToString();
							else
								userAddress.CreatedAt = null;
							if (!reader.IsDBNull(17))
								userAddress.UpdatedAt = reader.GetValue(17).ToString();
							else
								userAddress.UpdatedAt = null;
							response.Add(userAddress);
							userAddress = new UserAddressModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserAddressModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAddressModel> Create(string connectionString, UserAddressModel userAddress)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO upd_user_addresses (";
				string values = "(";
				if (!String.IsNullOrEmpty(userAddress.Id))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `id` ");
						values = String.Concat(values, ", @id");
					}
					else
					{
						sql = String.Concat(sql, " `id` ");
						values = String.Concat(values, " @id");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@id", userAddress.Id);
				}
				if (!String.IsNullOrEmpty(userAddress.UserId))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_id` ");
						values = String.Concat(values, ", @userId");
					}
					else
					{
						sql = String.Concat(sql, " `user_id` ");
						values = String.Concat(values, " @userId");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
				}
				if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `person_address_name` ");
						values = String.Concat(values, ", @personAddressName");
					}
					else
					{
						sql = String.Concat(sql, " `person_address_name` ");
						values = String.Concat(values, " @personAddressName");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressLine1))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `address_line_1` ");
						values = String.Concat(values, ", @addressLine1");
					}
					else
					{
						sql = String.Concat(sql, " `address_line_1` ");
						values = String.Concat(values, " @addressLine1");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressLine2))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `address_line_2` ");
						values = String.Concat(values, ", @addressLine2");
					}
					else
					{
						sql = String.Concat(sql, " `address_line_2` ");
						values = String.Concat(values, " @addressLine2");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
				}
				if (!String.IsNullOrEmpty(userAddress.City))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `city` ");
						values = String.Concat(values, ", @city");
					}
					else
					{
						sql = String.Concat(sql, " `city` ");
						values = String.Concat(values, " @city");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@city", userAddress.City);
				}
				if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `state_iso_code` ");
						values = String.Concat(values, ", @stateIsoCode");
					}
					else
					{
						sql = String.Concat(sql, " `state_iso_code` ");
						values = String.Concat(values, " @stateIsoCode");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
				}
				if (!String.IsNullOrEmpty(userAddress.State))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `state` ");
						values = String.Concat(values, ", @state");
					}
					else
					{
						sql = String.Concat(sql, " `state` ");
						values = String.Concat(values, " @state");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@state", userAddress.State);
				}
				if (!String.IsNullOrEmpty(userAddress.Zip))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `zip` ");
						values = String.Concat(values, ", @zip");
					}
					else
					{
						sql = String.Concat(sql, " `zip` ");
						values = String.Concat(values, " @zip");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
				}
				if (!String.IsNullOrEmpty(userAddress.Country))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `country` ");
						values = String.Concat(values, ", @country");
					}
					else
					{
						sql = String.Concat(sql, " `country` ");
						values = String.Concat(values, " @country");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@country", userAddress.Country);
				}
				if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `country_iso_code_2` ");
						values = String.Concat(values, ", @countryIsoCode2");
					}
					else
					{
						sql = String.Concat(sql, " `country_iso_code_2` ");
						values = String.Concat(values, " @countryIsoCode2");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressExtra))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `address_extra` ");
						values = String.Concat(values, ", @addressExtra");
					}
					else
					{
						sql = String.Concat(sql, " `address_extra` ");
						values = String.Concat(values, " @addressExtra");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressPhone))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `address_phone` ");
						values = String.Concat(values, ", @addressPhone");
					}
					else
					{
						sql = String.Concat(sql, " `address_phone` ");
						values = String.Concat(values, " @addressPhone");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
				}
				if (!String.IsNullOrEmpty(userAddress.MainAddress))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `main_address` ");
						values = String.Concat(values, ", @mainAddress");
					}
					else
					{
						sql = String.Concat(sql, " `main_address` ");
						values = String.Concat(values, " @mainAddress");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
				}
				if (!String.IsNullOrEmpty(userAddress.Enable))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `enable` ");
						values = String.Concat(values, ", @enable");
					}
					else
					{
						sql = String.Concat(sql, " `enable` ");
						values = String.Concat(values, " @enable");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
				}
				if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `modified_by` ");
						values = String.Concat(values, ", @modifiedBy");
					}
					else
					{
						sql = String.Concat(sql, " `modified_by` ");
						values = String.Concat(values, " @modifiedBy");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userAddress.CreatedAt))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `created_at` ");
						values = String.Concat(values, ", @createdAt");
					}
					else
					{
						sql = String.Concat(sql, " `created_at` ");
						values = String.Concat(values, " @createdAt");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `updated_at` ");
						values = String.Concat(values, ", @updatedAt");
					}
					else
					{
						sql = String.Concat(sql, " `updated_at` ");
						values = String.Concat(values, " @updatedAt");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				userAddress.Id = cmd.LastInsertedId.ToString();
				response.Add(userAddress);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
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
		public List<UserAddressModel> Create(string connectionString, List<UserAddressModel> userAddressList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
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
						for (int i = 0; i < userAddressList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO upd_user_addresses (";
								string values = "(";
								if (!String.IsNullOrEmpty(userAddressList[i].Id))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `id` ");
										values = String.Concat(values, ", @id" + i);
									}

									else
									{
										sql = String.Concat(sql, " `id` ");
										values = String.Concat(values, "@id" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, userAddressList[i].Id);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].UserId))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_id` ");
										values = String.Concat(values, ", @userId" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_id` ");
										values = String.Concat(values, "@userId" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, userAddressList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].PersonAddressName))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `person_address_name` ");
										values = String.Concat(values, ", @personAddressName" + i);
									}

									else
									{
										sql = String.Concat(sql, " `person_address_name` ");
										values = String.Concat(values, "@personAddressName" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@personAddressName" + i, userAddressList[i].PersonAddressName);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressLine1))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `address_line_1` ");
										values = String.Concat(values, ", @addressLine1" + i);
									}

									else
									{
										sql = String.Concat(sql, " `address_line_1` ");
										values = String.Concat(values, "@addressLine1" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@addressLine1" + i, userAddressList[i].AddressLine1);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressLine2))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `address_line_2` ");
										values = String.Concat(values, ", @addressLine2" + i);
									}

									else
									{
										sql = String.Concat(sql, " `address_line_2` ");
										values = String.Concat(values, "@addressLine2" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@addressLine2" + i, userAddressList[i].AddressLine2);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].City))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `city` ");
										values = String.Concat(values, ", @city" + i);
									}

									else
									{
										sql = String.Concat(sql, " `city` ");
										values = String.Concat(values, "@city" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@city" + i, userAddressList[i].City);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].StateIsoCode))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `state_iso_code` ");
										values = String.Concat(values, ", @stateIsoCode" + i);
									}

									else
									{
										sql = String.Concat(sql, " `state_iso_code` ");
										values = String.Concat(values, "@stateIsoCode" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddressList[i].StateIsoCode);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].State))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `state` ");
										values = String.Concat(values, ", @state" + i);
									}

									else
									{
										sql = String.Concat(sql, " `state` ");
										values = String.Concat(values, "@state" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@state" + i, userAddressList[i].State);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].Zip))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `zip` ");
										values = String.Concat(values, ", @zip" + i);
									}

									else
									{
										sql = String.Concat(sql, " `zip` ");
										values = String.Concat(values, "@zip" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@zip" + i, userAddressList[i].Zip);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].Country))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `country` ");
										values = String.Concat(values, ", @country" + i);
									}

									else
									{
										sql = String.Concat(sql, " `country` ");
										values = String.Concat(values, "@country" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@country" + i, userAddressList[i].Country);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].CountryIsoCode2))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `country_iso_code_2` ");
										values = String.Concat(values, ", @countryIsoCode2" + i);
									}

									else
									{
										sql = String.Concat(sql, " `country_iso_code_2` ");
										values = String.Concat(values, "@countryIsoCode2" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddressList[i].CountryIsoCode2);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressExtra))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `address_extra` ");
										values = String.Concat(values, ", @addressExtra" + i);
									}

									else
									{
										sql = String.Concat(sql, " `address_extra` ");
										values = String.Concat(values, "@addressExtra" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@addressExtra" + i, userAddressList[i].AddressExtra);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressPhone))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `address_phone` ");
										values = String.Concat(values, ", @addressPhone" + i);
									}

									else
									{
										sql = String.Concat(sql, " `address_phone` ");
										values = String.Concat(values, "@addressPhone" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@addressPhone" + i, userAddressList[i].AddressPhone);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].MainAddress))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `main_address` ");
										values = String.Concat(values, ", @mainAddress" + i);
									}

									else
									{
										sql = String.Concat(sql, " `main_address` ");
										values = String.Concat(values, "@mainAddress" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@mainAddress" + i, userAddressList[i].MainAddress);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].Enable))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `enable` ");
										values = String.Concat(values, ", @enable" + i);
									}

									else
									{
										sql = String.Concat(sql, " `enable` ");
										values = String.Concat(values, "@enable" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@enable" + i, userAddressList[i].Enable);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].ModifiedBy))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `modified_by` ");
										values = String.Concat(values, ", @modifiedBy" + i);
									}

									else
									{
										sql = String.Concat(sql, " `modified_by` ");
										values = String.Concat(values, "@modifiedBy" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddressList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].CreatedAt))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `created_at` ");
										values = String.Concat(values, ", @createdAt" + i);
									}

									else
									{
										sql = String.Concat(sql, " `created_at` ");
										values = String.Concat(values, "@createdAt" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userAddressList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].UpdatedAt))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `updated_at` ");
										values = String.Concat(values, ", @updatedAt" + i);
									}

									else
									{
										sql = String.Concat(sql, " `updated_at` ");
										values = String.Concat(values, "@updatedAt" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userAddressList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(userAddressList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<UserAddressModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserAddressModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
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

		public List<UserAddressModel> Update(string connectionString, UserAddressModel userAddress, UserAddressModel userAddressWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE upd_user_addresses SET ";
				if (!String.IsNullOrEmpty(userAddress.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", userAddress.Id);
				}
				if (!String.IsNullOrEmpty(userAddress.UserId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id` = @userId");
					else
						sql = String.Concat(sql, " `user_id` = @userId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
				}
				if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `person_address_name` = @personAddressName");
					else
						sql = String.Concat(sql, " `person_address_name` = @personAddressName");

					addAnd = true;
					cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressLine1))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `address_line_1` = @addressLine1");
					else
						sql = String.Concat(sql, " `address_line_1` = @addressLine1");

					addAnd = true;
					cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressLine2))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `address_line_2` = @addressLine2");
					else
						sql = String.Concat(sql, " `address_line_2` = @addressLine2");

					addAnd = true;
					cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
				}
				if (!String.IsNullOrEmpty(userAddress.City))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `city` = @city");
					else
						sql = String.Concat(sql, " `city` = @city");

					addAnd = true;
					cmd.Parameters.AddWithValue("@city", userAddress.City);
				}
				if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `state_iso_code` = @stateIsoCode");
					else
						sql = String.Concat(sql, " `state_iso_code` = @stateIsoCode");

					addAnd = true;
					cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
				}
				if (!String.IsNullOrEmpty(userAddress.State))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `state` = @state");
					else
						sql = String.Concat(sql, " `state` = @state");

					addAnd = true;
					cmd.Parameters.AddWithValue("@state", userAddress.State);
				}
				if (!String.IsNullOrEmpty(userAddress.Zip))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `zip` = @zip");
					else
						sql = String.Concat(sql, " `zip` = @zip");

					addAnd = true;
					cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
				}
				if (!String.IsNullOrEmpty(userAddress.Country))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `country` = @country");
					else
						sql = String.Concat(sql, " `country` = @country");

					addAnd = true;
					cmd.Parameters.AddWithValue("@country", userAddress.Country);
				}
				if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `country_iso_code_2` = @countryIsoCode2");
					else
						sql = String.Concat(sql, " `country_iso_code_2` = @countryIsoCode2");

					addAnd = true;
					cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressExtra))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `address_extra` = @addressExtra");
					else
						sql = String.Concat(sql, " `address_extra` = @addressExtra");

					addAnd = true;
					cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
				}
				if (!String.IsNullOrEmpty(userAddress.AddressPhone))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `address_phone` = @addressPhone");
					else
						sql = String.Concat(sql, " `address_phone` = @addressPhone");

					addAnd = true;
					cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
				}
				if (!String.IsNullOrEmpty(userAddress.MainAddress))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `main_address` = @mainAddress");
					else
						sql = String.Concat(sql, " `main_address` = @mainAddress");

					addAnd = true;
					cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
				}
				if (!String.IsNullOrEmpty(userAddress.Enable))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `enable` = @enable");
					else
						sql = String.Concat(sql, " `enable` = @enable");

					addAnd = true;
					cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
				}
				if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userAddress.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
				}
				if (userAddressWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(userAddressWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", userAddressWhere.Id);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
						else
							sql = String.Concat(sql, " `user_id` = @userIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdWhere", userAddressWhere.UserId);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.PersonAddressName))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `person_address_name` = @personAddressNameWhere");
						else
							sql = String.Concat(sql, " `person_address_name` = @personAddressNameWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@personAddressNameWhere", userAddressWhere.PersonAddressName);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.AddressLine1))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_line_1` = @addressLine1Where");
						else
							sql = String.Concat(sql, " `address_line_1` = @addressLine1Where");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressLine1Where", userAddressWhere.AddressLine1);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.AddressLine2))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_line_2` = @addressLine2Where");
						else
							sql = String.Concat(sql, " `address_line_2` = @addressLine2Where");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressLine2Where", userAddressWhere.AddressLine2);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.City))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `city` = @cityWhere");
						else
							sql = String.Concat(sql, " `city` = @cityWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@cityWhere", userAddressWhere.City);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.StateIsoCode))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `state_iso_code` = @stateIsoCodeWhere");
						else
							sql = String.Concat(sql, " `state_iso_code` = @stateIsoCodeWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@stateIsoCodeWhere", userAddressWhere.StateIsoCode);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.State))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `state` = @stateWhere");
						else
							sql = String.Concat(sql, " `state` = @stateWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@stateWhere", userAddressWhere.State);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.Zip))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `zip` = @zipWhere");
						else
							sql = String.Concat(sql, " `zip` = @zipWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@zipWhere", userAddressWhere.Zip);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.Country))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `country` = @countryWhere");
						else
							sql = String.Concat(sql, " `country` = @countryWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@countryWhere", userAddressWhere.Country);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.CountryIsoCode2))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2Where");
						else
							sql = String.Concat(sql, " `country_iso_code_2` = @countryIsoCode2Where");

						addAnd = true;
						cmd.Parameters.AddWithValue("@countryIsoCode2Where", userAddressWhere.CountryIsoCode2);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.AddressExtra))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_extra` = @addressExtraWhere");
						else
							sql = String.Concat(sql, " `address_extra` = @addressExtraWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressExtraWhere", userAddressWhere.AddressExtra);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.AddressPhone))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_phone` = @addressPhoneWhere");
						else
							sql = String.Concat(sql, " `address_phone` = @addressPhoneWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressPhoneWhere", userAddressWhere.AddressPhone);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.MainAddress))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `main_address` = @mainAddressWhere");
						else
							sql = String.Concat(sql, " `main_address` = @mainAddressWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@mainAddressWhere", userAddressWhere.MainAddress);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.Enable))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `enable` = @enableWhere");
						else
							sql = String.Concat(sql, " `enable` = @enableWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@enableWhere", userAddressWhere.Enable);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", userAddressWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", userAddressWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userAddressWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", userAddressWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(userAddress);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
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

		public List<UserAddressModel> Update(string connectionString, List<UserAddressModel> userAddressList, List<UserAddressModel> userAddressWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
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
						for (int i = 0; i < userAddressList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE upd_user_addresses SET ";
								if (!String.IsNullOrEmpty(userAddressList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, userAddressList[i].Id);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].UserId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id` = @userId" + i);
									else
										sql = String.Concat(sql, " `user_id` = @userId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, userAddressList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].PersonAddressName))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `person_address_name` = @personAddressName" + i);
									else
										sql = String.Concat(sql, " `person_address_name` = @personAddressName" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@personAddressName" + i, userAddressList[i].PersonAddressName);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressLine1))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `address_line_1` = @addressLine1" + i);
									else
										sql = String.Concat(sql, " `address_line_1` = @addressLine1" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@addressLine1" + i, userAddressList[i].AddressLine1);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressLine2))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `address_line_2` = @addressLine2" + i);
									else
										sql = String.Concat(sql, " `address_line_2` = @addressLine2" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@addressLine2" + i, userAddressList[i].AddressLine2);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].City))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `city` = @city" + i);
									else
										sql = String.Concat(sql, " `city` = @city" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@city" + i, userAddressList[i].City);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].StateIsoCode))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `state_iso_code` = @stateIsoCode" + i);
									else
										sql = String.Concat(sql, " `state_iso_code` = @stateIsoCode" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddressList[i].StateIsoCode);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].State))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `state` = @state" + i);
									else
										sql = String.Concat(sql, " `state` = @state" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@state" + i, userAddressList[i].State);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].Zip))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `zip` = @zip" + i);
									else
										sql = String.Concat(sql, " `zip` = @zip" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@zip" + i, userAddressList[i].Zip);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].Country))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `country` = @country" + i);
									else
										sql = String.Concat(sql, " `country` = @country" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@country" + i, userAddressList[i].Country);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].CountryIsoCode2))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `country_iso_code_2` = @countryIsoCode2" + i);
									else
										sql = String.Concat(sql, " `country_iso_code_2` = @countryIsoCode2" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddressList[i].CountryIsoCode2);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressExtra))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `address_extra` = @addressExtra" + i);
									else
										sql = String.Concat(sql, " `address_extra` = @addressExtra" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@addressExtra" + i, userAddressList[i].AddressExtra);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].AddressPhone))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `address_phone` = @addressPhone" + i);
									else
										sql = String.Concat(sql, " `address_phone` = @addressPhone" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@addressPhone" + i, userAddressList[i].AddressPhone);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].MainAddress))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `main_address` = @mainAddress" + i);
									else
										sql = String.Concat(sql, " `main_address` = @mainAddress" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@mainAddress" + i, userAddressList[i].MainAddress);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].Enable))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `enable` = @enable" + i);
									else
										sql = String.Concat(sql, " `enable` = @enable" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@enable" + i, userAddressList[i].Enable);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddressList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userAddressList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userAddressList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userAddressList[i].UpdatedAt);
								}
								if (userAddressWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(userAddressWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, userAddressWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdWhere" + i, userAddressWhereList[i].UserId);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].PersonAddressName))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `person_address_name` = @personAddressNameWhere" + i);
										else
											sql = String.Concat(sql, " `person_address_name` = @personAddressNameWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@personAddressNameWhere" + i, userAddressWhereList[i].PersonAddressName);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].AddressLine1))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_line_1` = @addressLine1Where" + i);
										else
											sql = String.Concat(sql, " `address_line_1` = @addressLine1Where" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressLine1Where" + i, userAddressWhereList[i].AddressLine1);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].AddressLine2))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_line_2` = @addressLine2Where" + i);
										else
											sql = String.Concat(sql, " `address_line_2` = @addressLine2Where" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressLine2Where" + i, userAddressWhereList[i].AddressLine2);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].City))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `city` = @cityWhere" + i);
										else
											sql = String.Concat(sql, " `city` = @cityWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@cityWhere" + i, userAddressWhereList[i].City);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].StateIsoCode))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `state_iso_code` = @stateIsoCodeWhere" + i);
										else
											sql = String.Concat(sql, " `state_iso_code` = @stateIsoCodeWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@stateIsoCodeWhere" + i, userAddressWhereList[i].StateIsoCode);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].State))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `state` = @stateWhere" + i);
										else
											sql = String.Concat(sql, " `state` = @stateWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@stateWhere" + i, userAddressWhereList[i].State);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].Zip))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `zip` = @zipWhere" + i);
										else
											sql = String.Concat(sql, " `zip` = @zipWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@zipWhere" + i, userAddressWhereList[i].Zip);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].Country))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `country` = @countryWhere" + i);
										else
											sql = String.Concat(sql, " `country` = @countryWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@countryWhere" + i, userAddressWhereList[i].Country);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].CountryIsoCode2))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2Where" + i);
										else
											sql = String.Concat(sql, " `country_iso_code_2` = @countryIsoCode2Where" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@countryIsoCode2Where" + i, userAddressWhereList[i].CountryIsoCode2);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].AddressExtra))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_extra` = @addressExtraWhere" + i);
										else
											sql = String.Concat(sql, " `address_extra` = @addressExtraWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressExtraWhere" + i, userAddressWhereList[i].AddressExtra);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].AddressPhone))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_phone` = @addressPhoneWhere" + i);
										else
											sql = String.Concat(sql, " `address_phone` = @addressPhoneWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressPhoneWhere" + i, userAddressWhereList[i].AddressPhone);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].MainAddress))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `main_address` = @mainAddressWhere" + i);
										else
											sql = String.Concat(sql, " `main_address` = @mainAddressWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@mainAddressWhere" + i, userAddressWhereList[i].MainAddress);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].Enable))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `enable` = @enableWhere" + i);
										else
											sql = String.Concat(sql, " `enable` = @enableWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@enableWhere" + i, userAddressWhereList[i].Enable);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userAddressWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, userAddressWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userAddressWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userAddressWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userAddressList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserAddressModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserAddressModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
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

		public List<UserAddressModel> Delete(string connectionString, UserAddressModel userAddress)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM upd_user_addresses  ";
				if (userAddress != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(userAddress.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", userAddress.Id);
					}
					if (!String.IsNullOrEmpty(userAddress.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userId");
						else
							sql = String.Concat(sql, " `user_id` = @userId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userId", userAddress.UserId);
					}
					if (!String.IsNullOrEmpty(userAddress.PersonAddressName))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `person_address_name` = @personAddressName");
						else
							sql = String.Concat(sql, " `person_address_name` = @personAddressName");

						addAnd = true;
						cmd.Parameters.AddWithValue("@personAddressName", userAddress.PersonAddressName);
					}
					if (!String.IsNullOrEmpty(userAddress.AddressLine1))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_line_1` = @addressLine1");
						else
							sql = String.Concat(sql, " `address_line_1` = @addressLine1");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressLine1", userAddress.AddressLine1);
					}
					if (!String.IsNullOrEmpty(userAddress.AddressLine2))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_line_2` = @addressLine2");
						else
							sql = String.Concat(sql, " `address_line_2` = @addressLine2");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressLine2", userAddress.AddressLine2);
					}
					if (!String.IsNullOrEmpty(userAddress.City))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `city` = @city");
						else
							sql = String.Concat(sql, " `city` = @city");

						addAnd = true;
						cmd.Parameters.AddWithValue("@city", userAddress.City);
					}
					if (!String.IsNullOrEmpty(userAddress.StateIsoCode))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `state_iso_code` = @stateIsoCode");
						else
							sql = String.Concat(sql, " `state_iso_code` = @stateIsoCode");

						addAnd = true;
						cmd.Parameters.AddWithValue("@stateIsoCode", userAddress.StateIsoCode);
					}
					if (!String.IsNullOrEmpty(userAddress.State))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `state` = @state");
						else
							sql = String.Concat(sql, " `state` = @state");

						addAnd = true;
						cmd.Parameters.AddWithValue("@state", userAddress.State);
					}
					if (!String.IsNullOrEmpty(userAddress.Zip))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `zip` = @zip");
						else
							sql = String.Concat(sql, " `zip` = @zip");

						addAnd = true;
						cmd.Parameters.AddWithValue("@zip", userAddress.Zip);
					}
					if (!String.IsNullOrEmpty(userAddress.Country))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `country` = @country");
						else
							sql = String.Concat(sql, " `country` = @country");

						addAnd = true;
						cmd.Parameters.AddWithValue("@country", userAddress.Country);
					}
					if (!String.IsNullOrEmpty(userAddress.CountryIsoCode2))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2");
						else
							sql = String.Concat(sql, " `country_iso_code_2` = @countryIsoCode2");

						addAnd = true;
						cmd.Parameters.AddWithValue("@countryIsoCode2", userAddress.CountryIsoCode2);
					}
					if (!String.IsNullOrEmpty(userAddress.AddressExtra))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_extra` = @addressExtra");
						else
							sql = String.Concat(sql, " `address_extra` = @addressExtra");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressExtra", userAddress.AddressExtra);
					}
					if (!String.IsNullOrEmpty(userAddress.AddressPhone))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `address_phone` = @addressPhone");
						else
							sql = String.Concat(sql, " `address_phone` = @addressPhone");

						addAnd = true;
						cmd.Parameters.AddWithValue("@addressPhone", userAddress.AddressPhone);
					}
					if (!String.IsNullOrEmpty(userAddress.MainAddress))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `main_address` = @mainAddress");
						else
							sql = String.Concat(sql, " `main_address` = @mainAddress");

						addAnd = true;
						cmd.Parameters.AddWithValue("@mainAddress", userAddress.MainAddress);
					}
					if (!String.IsNullOrEmpty(userAddress.Enable))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `enable` = @enable");
						else
							sql = String.Concat(sql, " `enable` = @enable");

						addAnd = true;
						cmd.Parameters.AddWithValue("@enable", userAddress.Enable);
					}
					if (!String.IsNullOrEmpty(userAddress.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", userAddress.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userAddress.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", userAddress.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userAddress.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", userAddress.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(userAddress);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
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

		public List<UserAddressModel> Delete(string connectionString, List<UserAddressModel> userAddress)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAddressModel> response = new List<UserAddressModel>();
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
						for (int i = 0; i < userAddress.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM upd_user_addresses  ";
								if (userAddress[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(userAddress[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, userAddress[i].Id);
									}
									if (!String.IsNullOrEmpty(userAddress[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userId" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userId" + i, userAddress[i].UserId);
									}
									if (!String.IsNullOrEmpty(userAddress[i].PersonAddressName))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `person_address_name` = @personAddressName" + i);
										else
											sql = String.Concat(sql, " `person_address_name` = @personAddressName" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@personAddressName" + i, userAddress[i].PersonAddressName);
									}
									if (!String.IsNullOrEmpty(userAddress[i].AddressLine1))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_line_1` = @addressLine1" + i);
										else
											sql = String.Concat(sql, " `address_line_1` = @addressLine1" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressLine1" + i, userAddress[i].AddressLine1);
									}
									if (!String.IsNullOrEmpty(userAddress[i].AddressLine2))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_line_2` = @addressLine2" + i);
										else
											sql = String.Concat(sql, " `address_line_2` = @addressLine2" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressLine2" + i, userAddress[i].AddressLine2);
									}
									if (!String.IsNullOrEmpty(userAddress[i].City))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `city` = @city" + i);
										else
											sql = String.Concat(sql, " `city` = @city" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@city" + i, userAddress[i].City);
									}
									if (!String.IsNullOrEmpty(userAddress[i].StateIsoCode))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `state_iso_code` = @stateIsoCode" + i);
										else
											sql = String.Concat(sql, " `state_iso_code` = @stateIsoCode" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@stateIsoCode" + i, userAddress[i].StateIsoCode);
									}
									if (!String.IsNullOrEmpty(userAddress[i].State))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `state` = @state" + i);
										else
											sql = String.Concat(sql, " `state` = @state" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@state" + i, userAddress[i].State);
									}
									if (!String.IsNullOrEmpty(userAddress[i].Zip))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `zip` = @zip" + i);
										else
											sql = String.Concat(sql, " `zip` = @zip" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@zip" + i, userAddress[i].Zip);
									}
									if (!String.IsNullOrEmpty(userAddress[i].Country))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `country` = @country" + i);
										else
											sql = String.Concat(sql, " `country` = @country" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@country" + i, userAddress[i].Country);
									}
									if (!String.IsNullOrEmpty(userAddress[i].CountryIsoCode2))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `country_iso_code_2` = @countryIsoCode2" + i);
										else
											sql = String.Concat(sql, " `country_iso_code_2` = @countryIsoCode2" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@countryIsoCode2" + i, userAddress[i].CountryIsoCode2);
									}
									if (!String.IsNullOrEmpty(userAddress[i].AddressExtra))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_extra` = @addressExtra" + i);
										else
											sql = String.Concat(sql, " `address_extra` = @addressExtra" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressExtra" + i, userAddress[i].AddressExtra);
									}
									if (!String.IsNullOrEmpty(userAddress[i].AddressPhone))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `address_phone` = @addressPhone" + i);
										else
											sql = String.Concat(sql, " `address_phone` = @addressPhone" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@addressPhone" + i, userAddress[i].AddressPhone);
									}
									if (!String.IsNullOrEmpty(userAddress[i].MainAddress))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `main_address` = @mainAddress" + i);
										else
											sql = String.Concat(sql, " `main_address` = @mainAddress" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@mainAddress" + i, userAddress[i].MainAddress);
									}
									if (!String.IsNullOrEmpty(userAddress[i].Enable))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `enable` = @enable" + i);
										else
											sql = String.Concat(sql, " `enable` = @enable" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@enable" + i, userAddress[i].Enable);
									}
									if (!String.IsNullOrEmpty(userAddress[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, userAddress[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userAddress[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, userAddress[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userAddress[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, userAddress[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userAddress[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserAddressModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserAddressModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAddressModel>();
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
