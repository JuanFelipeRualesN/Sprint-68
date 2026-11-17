using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails
{
	class UserFullNameDAO
	{
		private MySqlConnection _connection;
		public List<UserFullNameModel> Read(string connectionString, UserFullNameModel userFullName, bool firstOne, bool orderByLastDate, UserFullNameModel userFullNameWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `user_id`, `given_name`, `surname`, `suffix`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_full_names ";
				{
					if (userFullName != null)
					{
						if (!String.IsNullOrEmpty(userFullName.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userFullName.UserId);
						}
						if (!String.IsNullOrEmpty(userFullName.GivenName))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `given_name` = @givenName ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `given_name` = @givenName ");
							}
							cmd.Parameters.AddWithValue("@givenName", userFullName.GivenName);
						}
						if (!String.IsNullOrEmpty(userFullName.Surname))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `surname` = @surname ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `surname` = @surname ");
							}
							cmd.Parameters.AddWithValue("@surname", userFullName.Surname);
						}
						if (!String.IsNullOrEmpty(userFullName.Suffix))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `suffix` = @suffix ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `suffix` = @suffix ");
							}
							cmd.Parameters.AddWithValue("@suffix", userFullName.Suffix);
						}
						if (!String.IsNullOrEmpty(userFullName.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userFullName.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userFullName.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userFullName.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userFullName.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userFullName.UpdatedAt);
						}
					}
					if (userFullNameWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userFullNameWhereLike.UserId))
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
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userFullNameWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.GivenName))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `given_name` LIKE @givenNameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `given_name` LIKE @givenNameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@givenNameWhereLike", "%" + userFullNameWhereLike.GivenName + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.Surname))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `surname` LIKE @surnameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `surname` LIKE @surnameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@surnameWhereLike", "%" + userFullNameWhereLike.Surname + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.Suffix))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `suffix` LIKE @suffixWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `suffix` LIKE @suffixWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@suffixWhereLike", "%" + userFullNameWhereLike.Suffix + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userFullNameWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userFullNameWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userFullNameWhereLike.UpdatedAt + "%");
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
								userFullName = new UserFullNameModel();
								if (!reader.IsDBNull(0))
									userFullName.UserId = reader.GetValue(0).ToString();
								else
									userFullName.UserId = null;
								if (!reader.IsDBNull(1))
									userFullName.GivenName = reader.GetValue(1).ToString();
								else
									userFullName.GivenName = null;
								if (!reader.IsDBNull(2))
									userFullName.Surname = reader.GetValue(2).ToString();
								else
									userFullName.Surname = null;
								if (!reader.IsDBNull(3))
									userFullName.Suffix = reader.GetValue(3).ToString();
								else
									userFullName.Suffix = null;
								if (!reader.IsDBNull(4))
									userFullName.ModifiedBy = reader.GetValue(4).ToString();
								else
									userFullName.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userFullName.CreatedAt = reader.GetValue(5).ToString();
								else
									userFullName.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userFullName.UpdatedAt = reader.GetValue(6).ToString();
								else
									userFullName.UpdatedAt = null;
								response.Add(userFullName);
								userFullName = new UserFullNameModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserFullNameModel>();
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
				response = new List<UserFullNameModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserFullNameModel> Read(string connectionString, UserFullNameModel userFullName, bool orderByLastDate, int fromRow, int limit, UserFullNameModel userFullNameWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `user_id`, `given_name`, `surname`, `suffix`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_full_names ";
				{
					if (userFullName != null)
					{
						if (!String.IsNullOrEmpty(userFullName.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userFullName.UserId);
						}
						if (!String.IsNullOrEmpty(userFullName.GivenName))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `given_name` = @givenName ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `given_name` = @givenName ");
							}
							cmd.Parameters.AddWithValue("@givenName", userFullName.GivenName);
						}
						if (!String.IsNullOrEmpty(userFullName.Surname))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `surname` = @surname ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `surname` = @surname ");
							}
							cmd.Parameters.AddWithValue("@surname", userFullName.Surname);
						}
						if (!String.IsNullOrEmpty(userFullName.Suffix))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `suffix` = @suffix ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `suffix` = @suffix ");
							}
							cmd.Parameters.AddWithValue("@suffix", userFullName.Suffix);
						}
						if (!String.IsNullOrEmpty(userFullName.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userFullName.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userFullName.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userFullName.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userFullName.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userFullName.UpdatedAt);
						}
					}
					if (userFullNameWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userFullNameWhereLike.UserId))
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
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userFullNameWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.GivenName))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `given_name` LIKE @givenNameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `given_name` LIKE @givenNameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@givenNameWhereLike", "%" + userFullNameWhereLike.GivenName + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.Surname))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `surname` LIKE @surnameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `surname` LIKE @surnameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@surnameWhereLike", "%" + userFullNameWhereLike.Surname + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.Suffix))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `suffix` LIKE @suffixWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `suffix` LIKE @suffixWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@suffixWhereLike", "%" + userFullNameWhereLike.Suffix + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userFullNameWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userFullNameWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userFullNameWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userFullNameWhereLike.UpdatedAt + "%");
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
								userFullName = new UserFullNameModel();
								if (!reader.IsDBNull(0))
									userFullName.UserId = reader.GetValue(0).ToString();
								else
									userFullName.UserId = null;
								if (!reader.IsDBNull(1))
									userFullName.GivenName = reader.GetValue(1).ToString();
								else
									userFullName.GivenName = null;
								if (!reader.IsDBNull(2))
									userFullName.Surname = reader.GetValue(2).ToString();
								else
									userFullName.Surname = null;
								if (!reader.IsDBNull(3))
									userFullName.Suffix = reader.GetValue(3).ToString();
								else
									userFullName.Suffix = null;
								if (!reader.IsDBNull(4))
									userFullName.ModifiedBy = reader.GetValue(4).ToString();
								else
									userFullName.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userFullName.CreatedAt = reader.GetValue(5).ToString();
								else
									userFullName.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userFullName.UpdatedAt = reader.GetValue(6).ToString();
								else
									userFullName.UpdatedAt = null;
								response.Add(userFullName);
								userFullName = new UserFullNameModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserFullNameModel>();
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
				response = new List<UserFullNameModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserFullNameModel> Read(string connectionString, List<UserFullNameModel> userFullNameWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `user_id`, `given_name`, `surname`, `suffix`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_full_names ";
				if (userFullNameWhere != null && userFullNameWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userFullNameWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userFullNameWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userFullNameWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].GivenName))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `given_name` = @givenName" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@givenName" + i, userFullNameWhere[i].GivenName);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].Surname))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `surname` = @surname" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@surname" + i, userFullNameWhere[i].Surname);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].Suffix))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `suffix` = @suffix" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@suffix" + i, userFullNameWhere[i].Suffix);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userFullNameWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userFullNameWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userFullNameWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userFullNameWhere.Count - 1)
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
							UserFullNameModel userFullName = new UserFullNameModel(); if (!reader.IsDBNull(0))
								userFullName.UserId = reader.GetValue(0).ToString();
							else
								userFullName.UserId = null;
							if (!reader.IsDBNull(1))
								userFullName.GivenName = reader.GetValue(1).ToString();
							else
								userFullName.GivenName = null;
							if (!reader.IsDBNull(2))
								userFullName.Surname = reader.GetValue(2).ToString();
							else
								userFullName.Surname = null;
							if (!reader.IsDBNull(3))
								userFullName.Suffix = reader.GetValue(3).ToString();
							else
								userFullName.Suffix = null;
							if (!reader.IsDBNull(4))
								userFullName.ModifiedBy = reader.GetValue(4).ToString();
							else
								userFullName.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userFullName.CreatedAt = reader.GetValue(5).ToString();
							else
								userFullName.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userFullName.UpdatedAt = reader.GetValue(6).ToString();
							else
								userFullName.UpdatedAt = null;
							response.Add(userFullName);
							userFullName = new UserFullNameModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserFullNameModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserFullNameModel> Read(string connectionString, List<UserFullNameModel> userFullNameWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `user_id`, `given_name`, `surname`, `suffix`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_full_names ";
				if (userFullNameWhere != null && userFullNameWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userFullNameWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userFullNameWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userFullNameWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].GivenName))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `given_name` = @givenName" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@givenName" + i, userFullNameWhere[i].GivenName);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].Surname))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `surname` = @surname" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@surname" + i, userFullNameWhere[i].Surname);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].Suffix))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `suffix` = @suffix" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@suffix" + i, userFullNameWhere[i].Suffix);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userFullNameWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userFullNameWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userFullNameWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userFullNameWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userFullNameWhere.Count - 1)
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
							UserFullNameModel userFullName = new UserFullNameModel(); if (!reader.IsDBNull(0))
								userFullName.UserId = reader.GetValue(0).ToString();
							else
								userFullName.UserId = null;
							if (!reader.IsDBNull(1))
								userFullName.GivenName = reader.GetValue(1).ToString();
							else
								userFullName.GivenName = null;
							if (!reader.IsDBNull(2))
								userFullName.Surname = reader.GetValue(2).ToString();
							else
								userFullName.Surname = null;
							if (!reader.IsDBNull(3))
								userFullName.Suffix = reader.GetValue(3).ToString();
							else
								userFullName.Suffix = null;
							if (!reader.IsDBNull(4))
								userFullName.ModifiedBy = reader.GetValue(4).ToString();
							else
								userFullName.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userFullName.CreatedAt = reader.GetValue(5).ToString();
							else
								userFullName.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userFullName.UpdatedAt = reader.GetValue(6).ToString();
							else
								userFullName.UpdatedAt = null;
							response.Add(userFullName);
							userFullName = new UserFullNameModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserFullNameModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserFullNameModel> Create(string connectionString, UserFullNameModel userFullName)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO upd_user_full_names (";
				string values = "(";
				if (!String.IsNullOrEmpty(userFullName.UserId))
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
					cmd.Parameters.AddWithValue("@userId", userFullName.UserId);
				}
				if (!String.IsNullOrEmpty(userFullName.GivenName))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `given_name` ");
						values = String.Concat(values, ", @givenName");
					}
					else
					{
						sql = String.Concat(sql, " `given_name` ");
						values = String.Concat(values, " @givenName");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@givenName", userFullName.GivenName);
				}
				if (!String.IsNullOrEmpty(userFullName.Surname))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `surname` ");
						values = String.Concat(values, ", @surname");
					}
					else
					{
						sql = String.Concat(sql, " `surname` ");
						values = String.Concat(values, " @surname");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@surname", userFullName.Surname);
				}
				if (!String.IsNullOrEmpty(userFullName.Suffix))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `suffix` ");
						values = String.Concat(values, ", @suffix");
					}
					else
					{
						sql = String.Concat(sql, " `suffix` ");
						values = String.Concat(values, " @suffix");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@suffix", userFullName.Suffix);
				}
				if (!String.IsNullOrEmpty(userFullName.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", userFullName.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userFullName.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", userFullName.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userFullName.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", userFullName.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				response.Add(userFullName);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
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
		public List<UserFullNameModel> Create(string connectionString, List<UserFullNameModel> userFullNameList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
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
						for (int i = 0; i < userFullNameList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO upd_user_full_names (";
								string values = "(";
								if (!String.IsNullOrEmpty(userFullNameList[i].UserId))
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
									cmd.Parameters.AddWithValue("@userId" + i, userFullNameList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].GivenName))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `given_name` ");
										values = String.Concat(values, ", @givenName" + i);
									}

									else
									{
										sql = String.Concat(sql, " `given_name` ");
										values = String.Concat(values, "@givenName" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@givenName" + i, userFullNameList[i].GivenName);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].Surname))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `surname` ");
										values = String.Concat(values, ", @surname" + i);
									}

									else
									{
										sql = String.Concat(sql, " `surname` ");
										values = String.Concat(values, "@surname" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@surname" + i, userFullNameList[i].Surname);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].Suffix))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `suffix` ");
										values = String.Concat(values, ", @suffix" + i);
									}

									else
									{
										sql = String.Concat(sql, " `suffix` ");
										values = String.Concat(values, "@suffix" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@suffix" + i, userFullNameList[i].Suffix);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userFullNameList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, userFullNameList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, userFullNameList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(userFullNameList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<UserFullNameModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserFullNameModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
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

		public List<UserFullNameModel> Update(string connectionString, UserFullNameModel userFullName, UserFullNameModel userFullNameWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE upd_user_full_names SET ";
				if (!String.IsNullOrEmpty(userFullName.UserId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id` = @userId");
					else
						sql = String.Concat(sql, " `user_id` = @userId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", userFullName.UserId);
				}
				if (!String.IsNullOrEmpty(userFullName.GivenName))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `given_name` = @givenName");
					else
						sql = String.Concat(sql, " `given_name` = @givenName");

					addAnd = true;
					cmd.Parameters.AddWithValue("@givenName", userFullName.GivenName);
				}
				if (!String.IsNullOrEmpty(userFullName.Surname))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `surname` = @surname");
					else
						sql = String.Concat(sql, " `surname` = @surname");

					addAnd = true;
					cmd.Parameters.AddWithValue("@surname", userFullName.Surname);
				}
				if (!String.IsNullOrEmpty(userFullName.Suffix))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `suffix` = @suffix");
					else
						sql = String.Concat(sql, " `suffix` = @suffix");

					addAnd = true;
					cmd.Parameters.AddWithValue("@suffix", userFullName.Suffix);
				}
				if (!String.IsNullOrEmpty(userFullName.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userFullName.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userFullName.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userFullName.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userFullName.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userFullName.UpdatedAt);
				}
				if (userFullNameWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(userFullNameWhere.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
						else
							sql = String.Concat(sql, " `user_id` = @userIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdWhere", userFullNameWhere.UserId);
					}
					if (!String.IsNullOrEmpty(userFullNameWhere.GivenName))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `given_name` = @givenNameWhere");
						else
							sql = String.Concat(sql, " `given_name` = @givenNameWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@givenNameWhere", userFullNameWhere.GivenName);
					}
					if (!String.IsNullOrEmpty(userFullNameWhere.Surname))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `surname` = @surnameWhere");
						else
							sql = String.Concat(sql, " `surname` = @surnameWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@surnameWhere", userFullNameWhere.Surname);
					}
					if (!String.IsNullOrEmpty(userFullNameWhere.Suffix))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `suffix` = @suffixWhere");
						else
							sql = String.Concat(sql, " `suffix` = @suffixWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@suffixWhere", userFullNameWhere.Suffix);
					}
					if (!String.IsNullOrEmpty(userFullNameWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", userFullNameWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userFullNameWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", userFullNameWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userFullNameWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", userFullNameWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(userFullName);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
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

		public List<UserFullNameModel> Update(string connectionString, List<UserFullNameModel> userFullNameList, List<UserFullNameModel> userFullNameWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
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
						for (int i = 0; i < userFullNameList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE upd_user_full_names SET ";
								if (!String.IsNullOrEmpty(userFullNameList[i].UserId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id` = @userId" + i);
									else
										sql = String.Concat(sql, " `user_id` = @userId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, userFullNameList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].GivenName))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `given_name` = @givenName" + i);
									else
										sql = String.Concat(sql, " `given_name` = @givenName" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@givenName" + i, userFullNameList[i].GivenName);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].Surname))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `surname` = @surname" + i);
									else
										sql = String.Concat(sql, " `surname` = @surname" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@surname" + i, userFullNameList[i].Surname);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].Suffix))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `suffix` = @suffix" + i);
									else
										sql = String.Concat(sql, " `suffix` = @suffix" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@suffix" + i, userFullNameList[i].Suffix);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userFullNameList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userFullNameList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userFullNameList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userFullNameList[i].UpdatedAt);
								}
								if (userFullNameWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdWhere" + i, userFullNameWhereList[i].UserId);
									}
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].GivenName))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `given_name` = @givenNameWhere" + i);
										else
											sql = String.Concat(sql, " `given_name` = @givenNameWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@givenNameWhere" + i, userFullNameWhereList[i].GivenName);
									}
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].Surname))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `surname` = @surnameWhere" + i);
										else
											sql = String.Concat(sql, " `surname` = @surnameWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@surnameWhere" + i, userFullNameWhereList[i].Surname);
									}
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].Suffix))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `suffix` = @suffixWhere" + i);
										else
											sql = String.Concat(sql, " `suffix` = @suffixWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@suffixWhere" + i, userFullNameWhereList[i].Suffix);
									}
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userFullNameWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, userFullNameWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userFullNameWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userFullNameWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userFullNameList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserFullNameModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserFullNameModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
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

		public List<UserFullNameModel> Delete(string connectionString, UserFullNameModel userFullName)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM upd_user_full_names  ";
				if (userFullName != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(userFullName.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userId");
						else
							sql = String.Concat(sql, " `user_id` = @userId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userId", userFullName.UserId);
					}
					if (!String.IsNullOrEmpty(userFullName.GivenName))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `given_name` = @givenName");
						else
							sql = String.Concat(sql, " `given_name` = @givenName");

						addAnd = true;
						cmd.Parameters.AddWithValue("@givenName", userFullName.GivenName);
					}
					if (!String.IsNullOrEmpty(userFullName.Surname))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `surname` = @surname");
						else
							sql = String.Concat(sql, " `surname` = @surname");

						addAnd = true;
						cmd.Parameters.AddWithValue("@surname", userFullName.Surname);
					}
					if (!String.IsNullOrEmpty(userFullName.Suffix))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `suffix` = @suffix");
						else
							sql = String.Concat(sql, " `suffix` = @suffix");

						addAnd = true;
						cmd.Parameters.AddWithValue("@suffix", userFullName.Suffix);
					}
					if (!String.IsNullOrEmpty(userFullName.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", userFullName.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userFullName.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", userFullName.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userFullName.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", userFullName.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(userFullName);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
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

		public List<UserFullNameModel> Delete(string connectionString, List<UserFullNameModel> userFullName)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserFullNameModel> response = new List<UserFullNameModel>();
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
						for (int i = 0; i < userFullName.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM upd_user_full_names  ";
								if (userFullName[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(userFullName[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userId" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userId" + i, userFullName[i].UserId);
									}
									if (!String.IsNullOrEmpty(userFullName[i].GivenName))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `given_name` = @givenName" + i);
										else
											sql = String.Concat(sql, " `given_name` = @givenName" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@givenName" + i, userFullName[i].GivenName);
									}
									if (!String.IsNullOrEmpty(userFullName[i].Surname))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `surname` = @surname" + i);
										else
											sql = String.Concat(sql, " `surname` = @surname" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@surname" + i, userFullName[i].Surname);
									}
									if (!String.IsNullOrEmpty(userFullName[i].Suffix))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `suffix` = @suffix" + i);
										else
											sql = String.Concat(sql, " `suffix` = @suffix" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@suffix" + i, userFullName[i].Suffix);
									}
									if (!String.IsNullOrEmpty(userFullName[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, userFullName[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userFullName[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, userFullName[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userFullName[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, userFullName[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userFullName[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserFullNameModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserFullNameModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserFullNameModel>();
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
