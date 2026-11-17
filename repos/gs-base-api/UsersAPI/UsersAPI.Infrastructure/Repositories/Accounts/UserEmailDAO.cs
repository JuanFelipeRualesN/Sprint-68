using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
	class UserEmailDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, UserEmailModel> ReadD(string connectionString, UserEmailModel userEmail, bool firstOne, bool orderByLastDate, UserEmailModel userEmailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserEmailModel> response = new Dictionary<string, UserEmailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				{
					if (userEmail != null)
					{
						if (!String.IsNullOrEmpty(userEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", userEmail.Id);
						}
						if (!String.IsNullOrEmpty(userEmail.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
						}
						if (!String.IsNullOrEmpty(userEmail.Email))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `email` = @email ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `email` = @email ");
							}
							cmd.Parameters.AddWithValue("@email", userEmail.Email);
						}
						if (!String.IsNullOrEmpty(userEmail.Validated))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `validated` = @validated ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `validated` = @validated ");
							}
							cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
						}
						if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
						}
					}
					if (userEmailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userEmailWhereLike.Id))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userEmailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UserId))
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
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userEmailWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Email))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `email` LIKE @emailWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `email` LIKE @emailWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@emailWhereLike", "%" + userEmailWhereLike.Email + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Validated))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `validated` LIKE @validatedWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `validated` LIKE @validatedWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@validatedWhereLike", "%" + userEmailWhereLike.Validated + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userEmailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userEmailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userEmailWhereLike.UpdatedAt + "%");
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
								userEmail = new UserEmailModel();
								if (!reader.IsDBNull(0))
									userEmail.Id = reader.GetValue(0).ToString();
								else
									userEmail.Id = null;
								if (!reader.IsDBNull(1))
									userEmail.UserId = reader.GetValue(1).ToString();
								else
									userEmail.UserId = null;
								if (!reader.IsDBNull(2))
									userEmail.Email = reader.GetValue(2).ToString();
								else
									userEmail.Email = null;
								if (!reader.IsDBNull(3))
									userEmail.Validated = reader.GetValue(3).ToString();
								else
									userEmail.Validated = null;
								if (!reader.IsDBNull(4))
									userEmail.ModifiedBy = reader.GetValue(4).ToString();
								else
									userEmail.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userEmail.CreatedAt = reader.GetValue(5).ToString();
								else
									userEmail.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userEmail.UpdatedAt = reader.GetValue(6).ToString();
								else
									userEmail.UpdatedAt = null;
								response.Add(userEmail.Id, userEmail);
								userEmail = new UserEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserEmailModel>();
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
				response = new Dictionary<string, UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserEmailModel> ReadD(string connectionString, UserEmailModel userEmail, bool orderByLastDate, int fromRow, int limit, UserEmailModel userEmailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserEmailModel> response = new Dictionary<string, UserEmailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				{
					if (userEmail != null)
					{
						if (!String.IsNullOrEmpty(userEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", userEmail.Id);
						}
						if (!String.IsNullOrEmpty(userEmail.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
						}
						if (!String.IsNullOrEmpty(userEmail.Email))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `email` = @email ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `email` = @email ");
							}
							cmd.Parameters.AddWithValue("@email", userEmail.Email);
						}
						if (!String.IsNullOrEmpty(userEmail.Validated))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `validated` = @validated ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `validated` = @validated ");
							}
							cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
						}
						if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
						}
					}
					if (userEmailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userEmailWhereLike.Id))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userEmailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UserId))
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
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userEmailWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Email))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `email` LIKE @emailWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `email` LIKE @emailWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@emailWhereLike", "%" + userEmailWhereLike.Email + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Validated))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `validated` LIKE @validatedWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `validated` LIKE @validatedWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@validatedWhereLike", "%" + userEmailWhereLike.Validated + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userEmailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userEmailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userEmailWhereLike.UpdatedAt + "%");
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
								userEmail = new UserEmailModel();
								if (!reader.IsDBNull(0))
									userEmail.Id = reader.GetValue(0).ToString();
								else
									userEmail.Id = null;
								if (!reader.IsDBNull(1))
									userEmail.UserId = reader.GetValue(1).ToString();
								else
									userEmail.UserId = null;
								if (!reader.IsDBNull(2))
									userEmail.Email = reader.GetValue(2).ToString();
								else
									userEmail.Email = null;
								if (!reader.IsDBNull(3))
									userEmail.Validated = reader.GetValue(3).ToString();
								else
									userEmail.Validated = null;
								if (!reader.IsDBNull(4))
									userEmail.ModifiedBy = reader.GetValue(4).ToString();
								else
									userEmail.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userEmail.CreatedAt = reader.GetValue(5).ToString();
								else
									userEmail.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userEmail.UpdatedAt = reader.GetValue(6).ToString();
								else
									userEmail.UpdatedAt = null;
								response.Add(userEmail.Id, userEmail);
								userEmail = new UserEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserEmailModel>();
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
				response = new Dictionary<string, UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserEmailModel> ReadD(string connectionString, List<UserEmailModel> userEmailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserEmailModel> response = new Dictionary<string, UserEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				if (userEmailWhere != null && userEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userEmailWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, userEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Validated))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `validated` = @validated" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@validated" + i, userEmailWhere[i].Validated);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userEmailWhere.Count - 1)
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
							UserEmailModel userEmail = new UserEmailModel(); if (!reader.IsDBNull(0))
								userEmail.Id = reader.GetValue(0).ToString();
							else
								userEmail.Id = null;
							if (!reader.IsDBNull(1))
								userEmail.UserId = reader.GetValue(1).ToString();
							else
								userEmail.UserId = null;
							if (!reader.IsDBNull(2))
								userEmail.Email = reader.GetValue(2).ToString();
							else
								userEmail.Email = null;
							if (!reader.IsDBNull(3))
								userEmail.Validated = reader.GetValue(3).ToString();
							else
								userEmail.Validated = null;
							if (!reader.IsDBNull(4))
								userEmail.ModifiedBy = reader.GetValue(4).ToString();
							else
								userEmail.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userEmail.CreatedAt = reader.GetValue(5).ToString();
							else
								userEmail.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userEmail.UpdatedAt = reader.GetValue(6).ToString();
							else
								userEmail.UpdatedAt = null;
							response.Add(userEmail.Id, userEmail);
							userEmail = new UserEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserEmailModel> ReadD(string connectionString, List<UserEmailModel> userEmailWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserEmailModel> response = new Dictionary<string, UserEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				if (userEmailWhere != null && userEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userEmailWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, userEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Validated))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `validated` = @validated" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@validated" + i, userEmailWhere[i].Validated);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userEmailWhere.Count - 1)
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
							UserEmailModel userEmail = new UserEmailModel(); if (!reader.IsDBNull(0))
								userEmail.Id = reader.GetValue(0).ToString();
							else
								userEmail.Id = null;
							if (!reader.IsDBNull(1))
								userEmail.UserId = reader.GetValue(1).ToString();
							else
								userEmail.UserId = null;
							if (!reader.IsDBNull(2))
								userEmail.Email = reader.GetValue(2).ToString();
							else
								userEmail.Email = null;
							if (!reader.IsDBNull(3))
								userEmail.Validated = reader.GetValue(3).ToString();
							else
								userEmail.Validated = null;
							if (!reader.IsDBNull(4))
								userEmail.ModifiedBy = reader.GetValue(4).ToString();
							else
								userEmail.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userEmail.CreatedAt = reader.GetValue(5).ToString();
							else
								userEmail.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userEmail.UpdatedAt = reader.GetValue(6).ToString();
							else
								userEmail.UpdatedAt = null;
							response.Add(userEmail.Id, userEmail);
							userEmail = new UserEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserEmailModel> Read(string connectionString, UserEmailModel userEmail, bool firstOne, bool orderByLastDate, UserEmailModel userEmailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				{
					if (userEmail != null)
					{
						if (!String.IsNullOrEmpty(userEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", userEmail.Id);
						}
						if (!String.IsNullOrEmpty(userEmail.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
						}
						if (!String.IsNullOrEmpty(userEmail.Email))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `email` = @email ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `email` = @email ");
							}
							cmd.Parameters.AddWithValue("@email", userEmail.Email);
						}
						if (!String.IsNullOrEmpty(userEmail.Validated))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `validated` = @validated ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `validated` = @validated ");
							}
							cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
						}
						if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
						}
					}
					if (userEmailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userEmailWhereLike.Id))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userEmailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UserId))
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
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userEmailWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Email))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `email` LIKE @emailWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `email` LIKE @emailWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@emailWhereLike", "%" + userEmailWhereLike.Email + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Validated))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `validated` LIKE @validatedWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `validated` LIKE @validatedWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@validatedWhereLike", "%" + userEmailWhereLike.Validated + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userEmailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userEmailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userEmailWhereLike.UpdatedAt + "%");
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
								userEmail = new UserEmailModel();
								if (!reader.IsDBNull(0))
									userEmail.Id = reader.GetValue(0).ToString();
								else
									userEmail.Id = null;
								if (!reader.IsDBNull(1))
									userEmail.UserId = reader.GetValue(1).ToString();
								else
									userEmail.UserId = null;
								if (!reader.IsDBNull(2))
									userEmail.Email = reader.GetValue(2).ToString();
								else
									userEmail.Email = null;
								if (!reader.IsDBNull(3))
									userEmail.Validated = reader.GetValue(3).ToString();
								else
									userEmail.Validated = null;
								if (!reader.IsDBNull(4))
									userEmail.ModifiedBy = reader.GetValue(4).ToString();
								else
									userEmail.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userEmail.CreatedAt = reader.GetValue(5).ToString();
								else
									userEmail.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userEmail.UpdatedAt = reader.GetValue(6).ToString();
								else
									userEmail.UpdatedAt = null;
								response.Add(userEmail);
								userEmail = new UserEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserEmailModel>();
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
				response = new List<UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserEmailModel> Read(string connectionString, UserEmailModel userEmail, bool orderByLastDate, int fromRow, int limit, UserEmailModel userEmailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				{
					if (userEmail != null)
					{
						if (!String.IsNullOrEmpty(userEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", userEmail.Id);
						}
						if (!String.IsNullOrEmpty(userEmail.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
						}
						if (!String.IsNullOrEmpty(userEmail.Email))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `email` = @email ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `email` = @email ");
							}
							cmd.Parameters.AddWithValue("@email", userEmail.Email);
						}
						if (!String.IsNullOrEmpty(userEmail.Validated))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `validated` = @validated ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `validated` = @validated ");
							}
							cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
						}
						if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
						}
					}
					if (userEmailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userEmailWhereLike.Id))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userEmailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UserId))
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
							cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userEmailWhereLike.UserId + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Email))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `email` LIKE @emailWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `email` LIKE @emailWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@emailWhereLike", "%" + userEmailWhereLike.Email + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.Validated))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `validated` LIKE @validatedWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `validated` LIKE @validatedWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@validatedWhereLike", "%" + userEmailWhereLike.Validated + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userEmailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userEmailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userEmailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userEmailWhereLike.UpdatedAt + "%");
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
								userEmail = new UserEmailModel();
								if (!reader.IsDBNull(0))
									userEmail.Id = reader.GetValue(0).ToString();
								else
									userEmail.Id = null;
								if (!reader.IsDBNull(1))
									userEmail.UserId = reader.GetValue(1).ToString();
								else
									userEmail.UserId = null;
								if (!reader.IsDBNull(2))
									userEmail.Email = reader.GetValue(2).ToString();
								else
									userEmail.Email = null;
								if (!reader.IsDBNull(3))
									userEmail.Validated = reader.GetValue(3).ToString();
								else
									userEmail.Validated = null;
								if (!reader.IsDBNull(4))
									userEmail.ModifiedBy = reader.GetValue(4).ToString();
								else
									userEmail.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userEmail.CreatedAt = reader.GetValue(5).ToString();
								else
									userEmail.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userEmail.UpdatedAt = reader.GetValue(6).ToString();
								else
									userEmail.UpdatedAt = null;
								response.Add(userEmail);
								userEmail = new UserEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserEmailModel>();
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
				response = new List<UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserEmailModel> Read(string connectionString, List<UserEmailModel> userEmailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				if (userEmailWhere != null && userEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userEmailWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, userEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Validated))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `validated` = @validated" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@validated" + i, userEmailWhere[i].Validated);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userEmailWhere.Count - 1)
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
							UserEmailModel userEmail = new UserEmailModel(); if (!reader.IsDBNull(0))
								userEmail.Id = reader.GetValue(0).ToString();
							else
								userEmail.Id = null;
							if (!reader.IsDBNull(1))
								userEmail.UserId = reader.GetValue(1).ToString();
							else
								userEmail.UserId = null;
							if (!reader.IsDBNull(2))
								userEmail.Email = reader.GetValue(2).ToString();
							else
								userEmail.Email = null;
							if (!reader.IsDBNull(3))
								userEmail.Validated = reader.GetValue(3).ToString();
							else
								userEmail.Validated = null;
							if (!reader.IsDBNull(4))
								userEmail.ModifiedBy = reader.GetValue(4).ToString();
							else
								userEmail.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userEmail.CreatedAt = reader.GetValue(5).ToString();
							else
								userEmail.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userEmail.UpdatedAt = reader.GetValue(6).ToString();
							else
								userEmail.UpdatedAt = null;
							response.Add(userEmail);
							userEmail = new UserEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserEmailModel> Read(string connectionString, List<UserEmailModel> userEmailWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `email`, `validated`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_emails ";
				if (userEmailWhere != null && userEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userEmailWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, userEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].Validated))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `validated` = @validated" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@validated" + i, userEmailWhere[i].Validated);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userEmailWhere.Count - 1)
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
							UserEmailModel userEmail = new UserEmailModel(); if (!reader.IsDBNull(0))
								userEmail.Id = reader.GetValue(0).ToString();
							else
								userEmail.Id = null;
							if (!reader.IsDBNull(1))
								userEmail.UserId = reader.GetValue(1).ToString();
							else
								userEmail.UserId = null;
							if (!reader.IsDBNull(2))
								userEmail.Email = reader.GetValue(2).ToString();
							else
								userEmail.Email = null;
							if (!reader.IsDBNull(3))
								userEmail.Validated = reader.GetValue(3).ToString();
							else
								userEmail.Validated = null;
							if (!reader.IsDBNull(4))
								userEmail.ModifiedBy = reader.GetValue(4).ToString();
							else
								userEmail.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userEmail.CreatedAt = reader.GetValue(5).ToString();
							else
								userEmail.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userEmail.UpdatedAt = reader.GetValue(6).ToString();
							else
								userEmail.UpdatedAt = null;
							response.Add(userEmail);
							userEmail = new UserEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserEmailModel> Create(string connectionString, UserEmailModel userEmail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO ul_user_emails (";
				string values = "(";
				if (!String.IsNullOrEmpty(userEmail.Id))
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
					cmd.Parameters.AddWithValue("@id", userEmail.Id);
				}
				if (!String.IsNullOrEmpty(userEmail.UserId))
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
					cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
				}
				if (!String.IsNullOrEmpty(userEmail.Email))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `email` ");
						values = String.Concat(values, ", @email");
					}
					else
					{
						sql = String.Concat(sql, " `email` ");
						values = String.Concat(values, " @email");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@email", userEmail.Email);
				}
				if (!String.IsNullOrEmpty(userEmail.Validated))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `validated` ");
						values = String.Concat(values, ", @validated");
					}
					else
					{
						sql = String.Concat(sql, " `validated` ");
						values = String.Concat(values, " @validated");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
				}
				if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userEmail.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				userEmail.Id = cmd.LastInsertedId.ToString();
				response.Add(userEmail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
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
		public List<UserEmailModel> Create(string connectionString, List<UserEmailModel> userEmailList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
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
						for (int i = 0; i < userEmailList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO ul_user_emails (";
								string values = "(";
								if (!String.IsNullOrEmpty(userEmailList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, userEmailList[i].Id);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].UserId))
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
									cmd.Parameters.AddWithValue("@userId" + i, userEmailList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].Email))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `email` ");
										values = String.Concat(values, ", @email" + i);
									}

									else
									{
										sql = String.Concat(sql, " `email` ");
										values = String.Concat(values, "@email" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@email" + i, userEmailList[i].Email);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].Validated))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `validated` ");
										values = String.Concat(values, ", @validated" + i);
									}

									else
									{
										sql = String.Concat(sql, " `validated` ");
										values = String.Concat(values, "@validated" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@validated" + i, userEmailList[i].Validated);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmailList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, userEmailList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, userEmailList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(userEmailList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<UserEmailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserEmailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
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

		public List<UserEmailModel> Update(string connectionString, UserEmailModel userEmail, UserEmailModel userEmailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE ul_user_emails SET ";
				if (!String.IsNullOrEmpty(userEmail.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", userEmail.Id);
				}
				if (!String.IsNullOrEmpty(userEmail.UserId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id` = @userId");
					else
						sql = String.Concat(sql, " `user_id` = @userId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
				}
				if (!String.IsNullOrEmpty(userEmail.Email))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `email` = @email");
					else
						sql = String.Concat(sql, " `email` = @email");

					addAnd = true;
					cmd.Parameters.AddWithValue("@email", userEmail.Email);
				}
				if (!String.IsNullOrEmpty(userEmail.Validated))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `validated` = @validated");
					else
						sql = String.Concat(sql, " `validated` = @validated");

					addAnd = true;
					cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
				}
				if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userEmail.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
				}
				if (userEmailWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(userEmailWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", userEmailWhere.Id);
					}
					if (!String.IsNullOrEmpty(userEmailWhere.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
						else
							sql = String.Concat(sql, " `user_id` = @userIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdWhere", userEmailWhere.UserId);
					}
					if (!String.IsNullOrEmpty(userEmailWhere.Email))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `email` = @emailWhere");
						else
							sql = String.Concat(sql, " `email` = @emailWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@emailWhere", userEmailWhere.Email);
					}
					if (!String.IsNullOrEmpty(userEmailWhere.Validated))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `validated` = @validatedWhere");
						else
							sql = String.Concat(sql, " `validated` = @validatedWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@validatedWhere", userEmailWhere.Validated);
					}
					if (!String.IsNullOrEmpty(userEmailWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", userEmailWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userEmailWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", userEmailWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userEmailWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", userEmailWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(userEmail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
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

		public List<UserEmailModel> Update(string connectionString, List<UserEmailModel> userEmailList, List<UserEmailModel> userEmailWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
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
						for (int i = 0; i < userEmailList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE ul_user_emails SET ";
								if (!String.IsNullOrEmpty(userEmailList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, userEmailList[i].Id);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].UserId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id` = @userId" + i);
									else
										sql = String.Concat(sql, " `user_id` = @userId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, userEmailList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].Email))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `email` = @email" + i);
									else
										sql = String.Concat(sql, " `email` = @email" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@email" + i, userEmailList[i].Email);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].Validated))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `validated` = @validated" + i);
									else
										sql = String.Concat(sql, " `validated` = @validated" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@validated" + i, userEmailList[i].Validated);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmailList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userEmailList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userEmailList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userEmailList[i].UpdatedAt);
								}
								if (userEmailWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(userEmailWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, userEmailWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(userEmailWhereList[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdWhere" + i, userEmailWhereList[i].UserId);
									}
									if (!String.IsNullOrEmpty(userEmailWhereList[i].Email))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `email` = @emailWhere" + i);
										else
											sql = String.Concat(sql, " `email` = @emailWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@emailWhere" + i, userEmailWhereList[i].Email);
									}
									if (!String.IsNullOrEmpty(userEmailWhereList[i].Validated))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `validated` = @validatedWhere" + i);
										else
											sql = String.Concat(sql, " `validated` = @validatedWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@validatedWhere" + i, userEmailWhereList[i].Validated);
									}
									if (!String.IsNullOrEmpty(userEmailWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userEmailWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userEmailWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, userEmailWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userEmailWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userEmailWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userEmailList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserEmailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserEmailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
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

		public List<UserEmailModel> Delete(string connectionString, UserEmailModel userEmail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM ul_user_emails  ";
				if (userEmail != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(userEmail.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", userEmail.Id);
					}
					if (!String.IsNullOrEmpty(userEmail.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userId");
						else
							sql = String.Concat(sql, " `user_id` = @userId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userId", userEmail.UserId);
					}
					if (!String.IsNullOrEmpty(userEmail.Email))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `email` = @email");
						else
							sql = String.Concat(sql, " `email` = @email");

						addAnd = true;
						cmd.Parameters.AddWithValue("@email", userEmail.Email);
					}
					if (!String.IsNullOrEmpty(userEmail.Validated))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `validated` = @validated");
						else
							sql = String.Concat(sql, " `validated` = @validated");

						addAnd = true;
						cmd.Parameters.AddWithValue("@validated", userEmail.Validated);
					}
					if (!String.IsNullOrEmpty(userEmail.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", userEmail.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userEmail.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", userEmail.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userEmail.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", userEmail.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(userEmail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
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

		public List<UserEmailModel> Delete(string connectionString, List<UserEmailModel> userEmail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserEmailModel> response = new List<UserEmailModel>();
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
						for (int i = 0; i < userEmail.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM ul_user_emails  ";
								if (userEmail[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(userEmail[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, userEmail[i].Id);
									}
									if (!String.IsNullOrEmpty(userEmail[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userId" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userId" + i, userEmail[i].UserId);
									}
									if (!String.IsNullOrEmpty(userEmail[i].Email))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `email` = @email" + i);
										else
											sql = String.Concat(sql, " `email` = @email" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@email" + i, userEmail[i].Email);
									}
									if (!String.IsNullOrEmpty(userEmail[i].Validated))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `validated` = @validated" + i);
										else
											sql = String.Concat(sql, " `validated` = @validated" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@validated" + i, userEmail[i].Validated);
									}
									if (!String.IsNullOrEmpty(userEmail[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, userEmail[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userEmail[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, userEmail[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userEmail[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, userEmail[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userEmail[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserEmailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserEmailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserEmailModel>();
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
