using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails
{
	class UserPhoneNumberDAO
	{
		private MySqlConnection _connection;
		public List<UserPhoneNumberModel> Read(string connectionString, UserPhoneNumberModel userPhoneNumber, bool firstOne, bool orderByLastDate, UserPhoneNumberModel userPhoneNumberWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `user_id`, `phone_number_type_code`, `phone_number`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_phone_numbers ";
				{
					if (userPhoneNumber != null)
					{
						if (!String.IsNullOrEmpty(userPhoneNumber.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userPhoneNumber.UserId);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumberTypeCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `phone_number_type_code` = @phoneNumberTypeCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `phone_number_type_code` = @phoneNumberTypeCode ");
							}
							cmd.Parameters.AddWithValue("@phoneNumberTypeCode", userPhoneNumber.PhoneNumberTypeCode);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumber))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `phone_number` = @phoneNumber ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `phone_number` = @phoneNumber ");
							}
							cmd.Parameters.AddWithValue("@phoneNumber", userPhoneNumber.PhoneNumber);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userPhoneNumber.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userPhoneNumber.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userPhoneNumber.UpdatedAt);
						}
					}
					if (userPhoneNumberWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.UserId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userPhoneNumberWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.PhoneNumberTypeCode))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `phone_number_type_code` LIKE @phoneNumberTypeCodeWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `phone_number_type_code` LIKE @phoneNumberTypeCodeWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@phoneNumberTypeCodeWhereLike", "%" + userPhoneNumberWhereLike.PhoneNumberTypeCode + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.PhoneNumber))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `phone_number` LIKE @phoneNumberWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `phone_number` LIKE @phoneNumberWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@phoneNumberWhereLike", "%" + userPhoneNumberWhereLike.PhoneNumber + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.ModifiedBy))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userPhoneNumberWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.CreatedAt))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userPhoneNumberWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.UpdatedAt))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userPhoneNumberWhereLike.UpdatedAt + "%");
						}
					}
					if (orderByLastDate)
						sql = String.Concat(sql, " ORDER BY updated_at DESC");
					if (firstOne)
						sql = String.Concat(sql, " LIMIT 1 ");
					cmd.CommandText = sql;
					MySqlDataReader reader = cmd.ExecuteReader();
					try
					{
						if (reader.HasRows)
							while (reader.Read())
							{
								userPhoneNumber = new UserPhoneNumberModel();
								if (!reader.IsDBNull(0))
									userPhoneNumber.UserId = reader.GetValue(0).ToString();
								else
									userPhoneNumber.UserId = null;
								if (!reader.IsDBNull(1))
									userPhoneNumber.PhoneNumberTypeCode = reader.GetValue(1).ToString();
								else
									userPhoneNumber.PhoneNumberTypeCode = null;
								if (!reader.IsDBNull(2))
									userPhoneNumber.PhoneNumber = reader.GetValue(2).ToString();
								else
									userPhoneNumber.PhoneNumber = null;
								if (!reader.IsDBNull(3))
									userPhoneNumber.ModifiedBy = reader.GetValue(3).ToString();
								else
									userPhoneNumber.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									userPhoneNumber.CreatedAt = reader.GetValue(4).ToString();
								else
									userPhoneNumber.CreatedAt = null;
								if (!reader.IsDBNull(5))
									userPhoneNumber.UpdatedAt = reader.GetValue(5).ToString();
								else
									userPhoneNumber.UpdatedAt = null;
								response.Add(userPhoneNumber);
								userPhoneNumber = new UserPhoneNumberModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserPhoneNumberModel>();
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
				response = new List<UserPhoneNumberModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserPhoneNumberModel> Read(string connectionString, UserPhoneNumberModel userPhoneNumber, bool orderByLastDate, int fromRow, int limit, UserPhoneNumberModel userPhoneNumberWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `user_id`, `phone_number_type_code`, `phone_number`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_phone_numbers ";
				{
					if (userPhoneNumber != null)
					{
						if (!String.IsNullOrEmpty(userPhoneNumber.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userPhoneNumber.UserId);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumberTypeCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `phone_number_type_code` = @phoneNumberTypeCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `phone_number_type_code` = @phoneNumberTypeCode ");
							}
							cmd.Parameters.AddWithValue("@phoneNumberTypeCode", userPhoneNumber.PhoneNumberTypeCode);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumber))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `phone_number` = @phoneNumber ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `phone_number` = @phoneNumber ");
							}
							cmd.Parameters.AddWithValue("@phoneNumber", userPhoneNumber.PhoneNumber);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userPhoneNumber.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userPhoneNumber.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userPhoneNumber.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userPhoneNumber.UpdatedAt);
						}
					}
					if (userPhoneNumberWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.UserId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userPhoneNumberWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.PhoneNumberTypeCode))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `phone_number_type_code` LIKE @phoneNumberTypeCodeWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `phone_number_type_code` LIKE @phoneNumberTypeCodeWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@phoneNumberTypeCodeWhereLike", "%" + userPhoneNumberWhereLike.PhoneNumberTypeCode + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.PhoneNumber))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `phone_number` LIKE @phoneNumberWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `phone_number` LIKE @phoneNumberWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@phoneNumberWhereLike", "%" + userPhoneNumberWhereLike.PhoneNumber + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.ModifiedBy))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userPhoneNumberWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.CreatedAt))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userPhoneNumberWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhereLike.UpdatedAt))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userPhoneNumberWhereLike.UpdatedAt + "%");
						}
					}
					if (orderByLastDate)
						sql = String.Concat(sql, " ORDER BY updated_at DESC");
					sql = String.Concat(sql, " LIMIT @fromRow, @limit");
					cmd.Parameters.AddWithValue("@fromRow", fromRow);
					cmd.Parameters.AddWithValue("@limit", limit);
					cmd.CommandText = sql;
					MySqlDataReader reader = cmd.ExecuteReader();
					try
					{
						if (reader.HasRows)
							while (reader.Read())
							{
								userPhoneNumber = new UserPhoneNumberModel();
								if (!reader.IsDBNull(0))
									userPhoneNumber.UserId = reader.GetValue(0).ToString();
								else
									userPhoneNumber.UserId = null;
								if (!reader.IsDBNull(1))
									userPhoneNumber.PhoneNumberTypeCode = reader.GetValue(1).ToString();
								else
									userPhoneNumber.PhoneNumberTypeCode = null;
								if (!reader.IsDBNull(2))
									userPhoneNumber.PhoneNumber = reader.GetValue(2).ToString();
								else
									userPhoneNumber.PhoneNumber = null;
								if (!reader.IsDBNull(3))
									userPhoneNumber.ModifiedBy = reader.GetValue(3).ToString();
								else
									userPhoneNumber.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									userPhoneNumber.CreatedAt = reader.GetValue(4).ToString();
								else
									userPhoneNumber.CreatedAt = null;
								if (!reader.IsDBNull(5))
									userPhoneNumber.UpdatedAt = reader.GetValue(5).ToString();
								else
									userPhoneNumber.UpdatedAt = null;
								response.Add(userPhoneNumber);
								userPhoneNumber = new UserPhoneNumberModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserPhoneNumberModel>();
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
				response = new List<UserPhoneNumberModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserPhoneNumberModel> Read(string connectionString, List<UserPhoneNumberModel> userPhoneNumberWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `user_id`, `phone_number_type_code`, `phone_number`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_phone_numbers ";
				if (userPhoneNumberWhere != null && userPhoneNumberWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userPhoneNumberWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userPhoneNumberWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].PhoneNumberTypeCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `phone_number_type_code` = @phoneNumberTypeCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@phoneNumberTypeCode" + i, userPhoneNumberWhere[i].PhoneNumberTypeCode);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].PhoneNumber))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `phone_number` = @phoneNumber" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@phoneNumber" + i, userPhoneNumberWhere[i].PhoneNumber);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userPhoneNumberWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userPhoneNumberWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userPhoneNumberWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userPhoneNumberWhere.Count - 1)
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
							UserPhoneNumberModel userPhoneNumber = new UserPhoneNumberModel(); if (!reader.IsDBNull(0))
								userPhoneNumber.UserId = reader.GetValue(0).ToString();
							else
								userPhoneNumber.UserId = null;
							if (!reader.IsDBNull(1))
								userPhoneNumber.PhoneNumberTypeCode = reader.GetValue(1).ToString();
							else
								userPhoneNumber.PhoneNumberTypeCode = null;
							if (!reader.IsDBNull(2))
								userPhoneNumber.PhoneNumber = reader.GetValue(2).ToString();
							else
								userPhoneNumber.PhoneNumber = null;
							if (!reader.IsDBNull(3))
								userPhoneNumber.ModifiedBy = reader.GetValue(3).ToString();
							else
								userPhoneNumber.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								userPhoneNumber.CreatedAt = reader.GetValue(4).ToString();
							else
								userPhoneNumber.CreatedAt = null;
							if (!reader.IsDBNull(5))
								userPhoneNumber.UpdatedAt = reader.GetValue(5).ToString();
							else
								userPhoneNumber.UpdatedAt = null;
							response.Add(userPhoneNumber);
							userPhoneNumber = new UserPhoneNumberModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserPhoneNumberModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserPhoneNumberModel> Read(string connectionString, List<UserPhoneNumberModel> userPhoneNumberWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `user_id`, `phone_number_type_code`, `phone_number`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_phone_numbers ";
				if (userPhoneNumberWhere != null && userPhoneNumberWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userPhoneNumberWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userPhoneNumberWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].PhoneNumberTypeCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `phone_number_type_code` = @phoneNumberTypeCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@phoneNumberTypeCode" + i, userPhoneNumberWhere[i].PhoneNumberTypeCode);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].PhoneNumber))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `phone_number` = @phoneNumber" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@phoneNumber" + i, userPhoneNumberWhere[i].PhoneNumber);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userPhoneNumberWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userPhoneNumberWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userPhoneNumberWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userPhoneNumberWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userPhoneNumberWhere.Count - 1)
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
							UserPhoneNumberModel userPhoneNumber = new UserPhoneNumberModel(); if (!reader.IsDBNull(0))
								userPhoneNumber.UserId = reader.GetValue(0).ToString();
							else
								userPhoneNumber.UserId = null;
							if (!reader.IsDBNull(1))
								userPhoneNumber.PhoneNumberTypeCode = reader.GetValue(1).ToString();
							else
								userPhoneNumber.PhoneNumberTypeCode = null;
							if (!reader.IsDBNull(2))
								userPhoneNumber.PhoneNumber = reader.GetValue(2).ToString();
							else
								userPhoneNumber.PhoneNumber = null;
							if (!reader.IsDBNull(3))
								userPhoneNumber.ModifiedBy = reader.GetValue(3).ToString();
							else
								userPhoneNumber.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								userPhoneNumber.CreatedAt = reader.GetValue(4).ToString();
							else
								userPhoneNumber.CreatedAt = null;
							if (!reader.IsDBNull(5))
								userPhoneNumber.UpdatedAt = reader.GetValue(5).ToString();
							else
								userPhoneNumber.UpdatedAt = null;
							response.Add(userPhoneNumber);
							userPhoneNumber = new UserPhoneNumberModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserPhoneNumberModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserPhoneNumberModel> Create(string connectionString, UserPhoneNumberModel userPhoneNumber)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO upd_user_phone_numbers (";
				string values = "(";
				if (!String.IsNullOrEmpty(userPhoneNumber.UserId))
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
					cmd.Parameters.AddWithValue("@userId", userPhoneNumber.UserId);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumberTypeCode))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `phone_number_type_code` ");
						values = String.Concat(values, ", @phoneNumberTypeCode");
					}
					else
					{
						sql = String.Concat(sql, " `phone_number_type_code` ");
						values = String.Concat(values, " @phoneNumberTypeCode");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@phoneNumberTypeCode", userPhoneNumber.PhoneNumberTypeCode);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumber))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `phone_number` ");
						values = String.Concat(values, ", @phoneNumber");
					}
					else
					{
						sql = String.Concat(sql, " `phone_number` ");
						values = String.Concat(values, " @phoneNumber");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@phoneNumber", userPhoneNumber.PhoneNumber);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", userPhoneNumber.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", userPhoneNumber.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", userPhoneNumber.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				response.Add(userPhoneNumber);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
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
		public List<UserPhoneNumberModel> Create(string connectionString, List<UserPhoneNumberModel> userPhoneNumberList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
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
						for (int i = 0; i < userPhoneNumberList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO upd_user_phone_numbers (";
								string values = "(";
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].UserId))
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
									cmd.Parameters.AddWithValue("@userId" + i, userPhoneNumberList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].PhoneNumberTypeCode))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `phone_number_type_code` ");
										values = String.Concat(values, ", @phoneNumberTypeCode" + i);
									}

									else
									{
										sql = String.Concat(sql, " `phone_number_type_code` ");
										values = String.Concat(values, "@phoneNumberTypeCode" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@phoneNumberTypeCode" + i, userPhoneNumberList[i].PhoneNumberTypeCode);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].PhoneNumber))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `phone_number` ");
										values = String.Concat(values, ", @phoneNumber" + i);
									}

									else
									{
										sql = String.Concat(sql, " `phone_number` ");
										values = String.Concat(values, "@phoneNumber" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@phoneNumber" + i, userPhoneNumberList[i].PhoneNumber);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userPhoneNumberList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, userPhoneNumberList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, userPhoneNumberList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(userPhoneNumberList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<UserPhoneNumberModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserPhoneNumberModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
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

		public List<UserPhoneNumberModel> Update(string connectionString, UserPhoneNumberModel userPhoneNumber, UserPhoneNumberModel userPhoneNumberWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE upd_user_phone_numbers SET ";
				if (!String.IsNullOrEmpty(userPhoneNumber.UserId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id` = @userId");
					else
						sql = String.Concat(sql, " `user_id` = @userId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", userPhoneNumber.UserId);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumberTypeCode))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `phone_number_type_code` = @phoneNumberTypeCode");
					else
						sql = String.Concat(sql, " `phone_number_type_code` = @phoneNumberTypeCode");

					addAnd = true;
					cmd.Parameters.AddWithValue("@phoneNumberTypeCode", userPhoneNumber.PhoneNumberTypeCode);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumber))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `phone_number` = @phoneNumber");
					else
						sql = String.Concat(sql, " `phone_number` = @phoneNumber");

					addAnd = true;
					cmd.Parameters.AddWithValue("@phoneNumber", userPhoneNumber.PhoneNumber);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userPhoneNumber.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userPhoneNumber.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userPhoneNumber.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userPhoneNumber.UpdatedAt);
				}
				if (userPhoneNumberWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(userPhoneNumberWhere.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
						else
							sql = String.Concat(sql, " `user_id` = @userIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdWhere", userPhoneNumberWhere.UserId);
					}
					if (!String.IsNullOrEmpty(userPhoneNumberWhere.PhoneNumberTypeCode))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `phone_number_type_code` = @phoneNumberTypeCodeWhere");
						else
							sql = String.Concat(sql, " `phone_number_type_code` = @phoneNumberTypeCodeWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@phoneNumberTypeCodeWhere", userPhoneNumberWhere.PhoneNumberTypeCode);
					}
					if (!String.IsNullOrEmpty(userPhoneNumberWhere.PhoneNumber))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `phone_number` = @phoneNumberWhere");
						else
							sql = String.Concat(sql, " `phone_number` = @phoneNumberWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@phoneNumberWhere", userPhoneNumberWhere.PhoneNumber);
					}
					if (!String.IsNullOrEmpty(userPhoneNumberWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", userPhoneNumberWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userPhoneNumberWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", userPhoneNumberWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userPhoneNumberWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", userPhoneNumberWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(userPhoneNumber);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
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

		public List<UserPhoneNumberModel> Update(string connectionString, List<UserPhoneNumberModel> userPhoneNumberList, List<UserPhoneNumberModel> userPhoneNumberWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
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
						for (int i = 0; i < userPhoneNumberList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE upd_user_phone_numbers SET ";
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].UserId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id` = @userId" + i);
									else
										sql = String.Concat(sql, " `user_id` = @userId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, userPhoneNumberList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].PhoneNumberTypeCode))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `phone_number_type_code` = @phoneNumberTypeCode" + i);
									else
										sql = String.Concat(sql, " `phone_number_type_code` = @phoneNumberTypeCode" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@phoneNumberTypeCode" + i, userPhoneNumberList[i].PhoneNumberTypeCode);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].PhoneNumber))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `phone_number` = @phoneNumber" + i);
									else
										sql = String.Concat(sql, " `phone_number` = @phoneNumber" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@phoneNumber" + i, userPhoneNumberList[i].PhoneNumber);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userPhoneNumberList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userPhoneNumberList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userPhoneNumberList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userPhoneNumberList[i].UpdatedAt);
								}
								if (userPhoneNumberWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(userPhoneNumberWhereList[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdWhere" + i, userPhoneNumberWhereList[i].UserId);
									}
									if (!String.IsNullOrEmpty(userPhoneNumberWhereList[i].PhoneNumberTypeCode))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `phone_number_type_code` = @phoneNumberTypeCodeWhere" + i);
										else
											sql = String.Concat(sql, " `phone_number_type_code` = @phoneNumberTypeCodeWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@phoneNumberTypeCodeWhere" + i, userPhoneNumberWhereList[i].PhoneNumberTypeCode);
									}
									if (!String.IsNullOrEmpty(userPhoneNumberWhereList[i].PhoneNumber))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `phone_number` = @phoneNumberWhere" + i);
										else
											sql = String.Concat(sql, " `phone_number` = @phoneNumberWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@phoneNumberWhere" + i, userPhoneNumberWhereList[i].PhoneNumber);
									}
									if (!String.IsNullOrEmpty(userPhoneNumberWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userPhoneNumberWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userPhoneNumberWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, userPhoneNumberWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userPhoneNumberWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userPhoneNumberWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userPhoneNumberList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserPhoneNumberModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserPhoneNumberModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
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

		public List<UserPhoneNumberModel> Delete(string connectionString, UserPhoneNumberModel userPhoneNumber)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM upd_user_phone_numbers  ";
				if (userPhoneNumber != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(userPhoneNumber.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userId");
						else
							sql = String.Concat(sql, " `user_id` = @userId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userId", userPhoneNumber.UserId);
					}
					if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumberTypeCode))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `phone_number_type_code` = @phoneNumberTypeCode");
						else
							sql = String.Concat(sql, " `phone_number_type_code` = @phoneNumberTypeCode");

						addAnd = true;
						cmd.Parameters.AddWithValue("@phoneNumberTypeCode", userPhoneNumber.PhoneNumberTypeCode);
					}
					if (!String.IsNullOrEmpty(userPhoneNumber.PhoneNumber))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `phone_number` = @phoneNumber");
						else
							sql = String.Concat(sql, " `phone_number` = @phoneNumber");

						addAnd = true;
						cmd.Parameters.AddWithValue("@phoneNumber", userPhoneNumber.PhoneNumber);
					}
					if (!String.IsNullOrEmpty(userPhoneNumber.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", userPhoneNumber.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userPhoneNumber.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", userPhoneNumber.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userPhoneNumber.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", userPhoneNumber.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(userPhoneNumber);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
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

		public List<UserPhoneNumberModel> Delete(string connectionString, List<UserPhoneNumberModel> userPhoneNumber)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserPhoneNumberModel> response = new List<UserPhoneNumberModel>();
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
						for (int i = 0; i < userPhoneNumber.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM upd_user_phone_numbers  ";
								if (userPhoneNumber[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(userPhoneNumber[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userId" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userId" + i, userPhoneNumber[i].UserId);
									}
									if (!String.IsNullOrEmpty(userPhoneNumber[i].PhoneNumberTypeCode))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `phone_number_type_code` = @phoneNumberTypeCode" + i);
										else
											sql = String.Concat(sql, " `phone_number_type_code` = @phoneNumberTypeCode" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@phoneNumberTypeCode" + i, userPhoneNumber[i].PhoneNumberTypeCode);
									}
									if (!String.IsNullOrEmpty(userPhoneNumber[i].PhoneNumber))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `phone_number` = @phoneNumber" + i);
										else
											sql = String.Concat(sql, " `phone_number` = @phoneNumber" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@phoneNumber" + i, userPhoneNumber[i].PhoneNumber);
									}
									if (!String.IsNullOrEmpty(userPhoneNumber[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, userPhoneNumber[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userPhoneNumber[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, userPhoneNumber[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userPhoneNumber[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, userPhoneNumber[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userPhoneNumber[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserPhoneNumberModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserPhoneNumberModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserPhoneNumberModel>();
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
