using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Services.Accounts.CustomClasses;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts
{
	class UserAccountDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, UserAccountModel> ReadD(string connectionString, UserAccountModel userAccount, bool firstOne, bool orderByLastDate, UserAccountModel userAccountWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAccountModel> response = new Dictionary<string, UserAccountModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				{
					if (userAccount != null)
					{
						if (!String.IsNullOrEmpty(userAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", userAccount.Id);
						}
						if (!String.IsNullOrEmpty(userAccount.Username))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `username` = @username ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `username` = @username ");
							}
							cmd.Parameters.AddWithValue("@username", userAccount.Username);
						}
						if (!String.IsNullOrEmpty(userAccount.Password))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `password` = @password ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `password` = @password ");
							}
							cmd.Parameters.AddWithValue("@password", userAccount.Password);
						}
						if (!String.IsNullOrEmpty(userAccount.Token))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `token` = @token ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `token` = @token ");
							}
							cmd.Parameters.AddWithValue("@token", userAccount.Token);
						}
						if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
						}
					}
					if (userAccountWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userAccountWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userAccountWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Username))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `username` LIKE @usernameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `username` LIKE @usernameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@usernameWhereLike", "%" + userAccountWhereLike.Username + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Password))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `password` LIKE @passwordWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `password` LIKE @passwordWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@passwordWhereLike", "%" + userAccountWhereLike.Password + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Token))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `token` LIKE @tokenWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `token` LIKE @tokenWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + userAccountWhereLike.Token + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userAccountWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userAccountWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userAccountWhereLike.UpdatedAt + "%");
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
								userAccount = new UserAccountModel();
								if (!reader.IsDBNull(0))
									userAccount.Id = reader.GetValue(0).ToString();
								else
									userAccount.Id = null;
								if (!reader.IsDBNull(1))
									userAccount.Username = reader.GetValue(1).ToString();
								else
									userAccount.Username = null;
								if (!reader.IsDBNull(2))
									userAccount.Password = reader.GetValue(2).ToString();
								else
									userAccount.Password = null;
								if (!reader.IsDBNull(3))
									userAccount.Token = reader.GetValue(3).ToString();
								else
									userAccount.Token = null;
								if (!reader.IsDBNull(4))
									userAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									userAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									userAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									userAccount.UpdatedAt = null;
								response.Add(userAccount.Id, userAccount);
								userAccount = new UserAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserAccountModel>();
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
				response = new Dictionary<string, UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserAccountModel> ReadD(string connectionString, UserAccountModel userAccount, bool orderByLastDate, int fromRow, int limit, UserAccountModel userAccountWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAccountModel> response = new Dictionary<string, UserAccountModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				{
					if (userAccount != null)
					{
						if (!String.IsNullOrEmpty(userAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", userAccount.Id);
						}
						if (!String.IsNullOrEmpty(userAccount.Username))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `username` = @username ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `username` = @username ");
							}
							cmd.Parameters.AddWithValue("@username", userAccount.Username);
						}
						if (!String.IsNullOrEmpty(userAccount.Password))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `password` = @password ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `password` = @password ");
							}
							cmd.Parameters.AddWithValue("@password", userAccount.Password);
						}
						if (!String.IsNullOrEmpty(userAccount.Token))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `token` = @token ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `token` = @token ");
							}
							cmd.Parameters.AddWithValue("@token", userAccount.Token);
						}
						if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
						}
					}
					if (userAccountWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userAccountWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userAccountWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Username))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `username` LIKE @usernameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `username` LIKE @usernameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@usernameWhereLike", "%" + userAccountWhereLike.Username + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Password))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `password` LIKE @passwordWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `password` LIKE @passwordWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@passwordWhereLike", "%" + userAccountWhereLike.Password + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Token))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `token` LIKE @tokenWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `token` LIKE @tokenWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + userAccountWhereLike.Token + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userAccountWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userAccountWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userAccountWhereLike.UpdatedAt + "%");
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
								userAccount = new UserAccountModel();
								if (!reader.IsDBNull(0))
									userAccount.Id = reader.GetValue(0).ToString();
								else
									userAccount.Id = null;
								if (!reader.IsDBNull(1))
									userAccount.Username = reader.GetValue(1).ToString();
								else
									userAccount.Username = null;
								if (!reader.IsDBNull(2))
									userAccount.Password = reader.GetValue(2).ToString();
								else
									userAccount.Password = null;
								if (!reader.IsDBNull(3))
									userAccount.Token = reader.GetValue(3).ToString();
								else
									userAccount.Token = null;
								if (!reader.IsDBNull(4))
									userAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									userAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									userAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									userAccount.UpdatedAt = null;
								response.Add(userAccount.Id, userAccount);
								userAccount = new UserAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserAccountModel>();
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
				response = new Dictionary<string, UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserAccountModel> ReadD(string connectionString, List<UserAccountModel> userAccountWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAccountModel> response = new Dictionary<string, UserAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				if (userAccountWhere != null && userAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Username))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `username` = @username" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@username" + i, userAccountWhere[i].Username);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Password))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `password` = @password" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@password" + i, userAccountWhere[i].Password);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, userAccountWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAccountWhere.Count - 1)
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
							UserAccountModel userAccount = new UserAccountModel(); if (!reader.IsDBNull(0))
								userAccount.Id = reader.GetValue(0).ToString();
							else
								userAccount.Id = null;
							if (!reader.IsDBNull(1))
								userAccount.Username = reader.GetValue(1).ToString();
							else
								userAccount.Username = null;
							if (!reader.IsDBNull(2))
								userAccount.Password = reader.GetValue(2).ToString();
							else
								userAccount.Password = null;
							if (!reader.IsDBNull(3))
								userAccount.Token = reader.GetValue(3).ToString();
							else
								userAccount.Token = null;
							if (!reader.IsDBNull(4))
								userAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								userAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								userAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								userAccount.UpdatedAt = null;
							response.Add(userAccount.Id, userAccount);
							userAccount = new UserAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserAccountModel> ReadD(string connectionString, List<UserAccountModel> userAccountWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserAccountModel> response = new Dictionary<string, UserAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				if (userAccountWhere != null && userAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Username))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `username` = @username" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@username" + i, userAccountWhere[i].Username);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Password))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `password` = @password" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@password" + i, userAccountWhere[i].Password);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, userAccountWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAccountWhere.Count - 1)
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
							UserAccountModel userAccount = new UserAccountModel(); if (!reader.IsDBNull(0))
								userAccount.Id = reader.GetValue(0).ToString();
							else
								userAccount.Id = null;
							if (!reader.IsDBNull(1))
								userAccount.Username = reader.GetValue(1).ToString();
							else
								userAccount.Username = null;
							if (!reader.IsDBNull(2))
								userAccount.Password = reader.GetValue(2).ToString();
							else
								userAccount.Password = null;
							if (!reader.IsDBNull(3))
								userAccount.Token = reader.GetValue(3).ToString();
							else
								userAccount.Token = null;
							if (!reader.IsDBNull(4))
								userAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								userAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								userAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								userAccount.UpdatedAt = null;
							response.Add(userAccount.Id, userAccount);
							userAccount = new UserAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAccountModel> Read(string connectionString, UserAccountModel userAccount, bool firstOne, bool orderByLastDate, UserAccountModel userAccountWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				{
					if (userAccount != null)
					{
						if (!String.IsNullOrEmpty(userAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", userAccount.Id);
						}
						if (!String.IsNullOrEmpty(userAccount.Username))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `username` = @username ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `username` = @username ");
							}
							cmd.Parameters.AddWithValue("@username", userAccount.Username);
						}
						if (!String.IsNullOrEmpty(userAccount.Password))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `password` = @password ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `password` = @password ");
							}
							cmd.Parameters.AddWithValue("@password", userAccount.Password);
						}
						if (!String.IsNullOrEmpty(userAccount.Token))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `token` = @token ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `token` = @token ");
							}
							cmd.Parameters.AddWithValue("@token", userAccount.Token);
						}
						if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
						}
					}
					if (userAccountWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userAccountWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userAccountWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Username))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `username` LIKE @usernameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `username` LIKE @usernameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@usernameWhereLike", "%" + userAccountWhereLike.Username + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Password))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `password` LIKE @passwordWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `password` LIKE @passwordWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@passwordWhereLike", "%" + userAccountWhereLike.Password + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Token))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `token` LIKE @tokenWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `token` LIKE @tokenWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + userAccountWhereLike.Token + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userAccountWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userAccountWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userAccountWhereLike.UpdatedAt + "%");
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
								userAccount = new UserAccountModel();
								if (!reader.IsDBNull(0))
									userAccount.Id = reader.GetValue(0).ToString();
								else
									userAccount.Id = null;
								if (!reader.IsDBNull(1))
									userAccount.Username = reader.GetValue(1).ToString();
								else
									userAccount.Username = null;
								if (!reader.IsDBNull(2))
									userAccount.Password = reader.GetValue(2).ToString();
								else
									userAccount.Password = null;
								if (!reader.IsDBNull(3))
									userAccount.Token = reader.GetValue(3).ToString();
								else
									userAccount.Token = null;
								if (!reader.IsDBNull(4))
									userAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									userAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									userAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									userAccount.UpdatedAt = null;
								response.Add(userAccount);
								userAccount = new UserAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserAccountModel>();
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
				response = new List<UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAccountModel> Read(string connectionString, UserAccountModel userAccount, bool orderByLastDate, int fromRow, int limit, UserAccountModel userAccountWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				{
					if (userAccount != null)
					{
						if (!String.IsNullOrEmpty(userAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", userAccount.Id);
						}
						if (!String.IsNullOrEmpty(userAccount.Username))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `username` = @username ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `username` = @username ");
							}
							cmd.Parameters.AddWithValue("@username", userAccount.Username);
						}
						if (!String.IsNullOrEmpty(userAccount.Password))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `password` = @password ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `password` = @password ");
							}
							cmd.Parameters.AddWithValue("@password", userAccount.Password);
						}
						if (!String.IsNullOrEmpty(userAccount.Token))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `token` = @token ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `token` = @token ");
							}
							cmd.Parameters.AddWithValue("@token", userAccount.Token);
						}
						if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
						}
					}
					if (userAccountWhereLike != null)
					{
						if (!String.IsNullOrEmpty(userAccountWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + userAccountWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Username))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `username` LIKE @usernameWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `username` LIKE @usernameWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@usernameWhereLike", "%" + userAccountWhereLike.Username + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Password))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `password` LIKE @passwordWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `password` LIKE @passwordWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@passwordWhereLike", "%" + userAccountWhereLike.Password + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.Token))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `token` LIKE @tokenWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `token` LIKE @tokenWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@tokenWhereLike", "%" + userAccountWhereLike.Token + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userAccountWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userAccountWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(userAccountWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userAccountWhereLike.UpdatedAt + "%");
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
								userAccount = new UserAccountModel();
								if (!reader.IsDBNull(0))
									userAccount.Id = reader.GetValue(0).ToString();
								else
									userAccount.Id = null;
								if (!reader.IsDBNull(1))
									userAccount.Username = reader.GetValue(1).ToString();
								else
									userAccount.Username = null;
								if (!reader.IsDBNull(2))
									userAccount.Password = reader.GetValue(2).ToString();
								else
									userAccount.Password = null;
								if (!reader.IsDBNull(3))
									userAccount.Token = reader.GetValue(3).ToString();
								else
									userAccount.Token = null;
								if (!reader.IsDBNull(4))
									userAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									userAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									userAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									userAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									userAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									userAccount.UpdatedAt = null;
								response.Add(userAccount);
								userAccount = new UserAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserAccountModel>();
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
				response = new List<UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAccountModel> Read(string connectionString, List<UserAccountModel> userAccountWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				if (userAccountWhere != null && userAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Username))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `username` = @username" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@username" + i, userAccountWhere[i].Username);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Password))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `password` = @password" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@password" + i, userAccountWhere[i].Password);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, userAccountWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAccountWhere.Count - 1)
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
							UserAccountModel userAccount = new UserAccountModel(); if (!reader.IsDBNull(0))
								userAccount.Id = reader.GetValue(0).ToString();
							else
								userAccount.Id = null;
							if (!reader.IsDBNull(1))
								userAccount.Username = reader.GetValue(1).ToString();
							else
								userAccount.Username = null;
							if (!reader.IsDBNull(2))
								userAccount.Password = reader.GetValue(2).ToString();
							else
								userAccount.Password = null;
							if (!reader.IsDBNull(3))
								userAccount.Token = reader.GetValue(3).ToString();
							else
								userAccount.Token = null;
							if (!reader.IsDBNull(4))
								userAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								userAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								userAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								userAccount.UpdatedAt = null;
							response.Add(userAccount);
							userAccount = new UserAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAccountModel> Read(string connectionString, List<UserAccountModel> userAccountWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `username`, `password`, `token`, `modified_by`, `created_at`, `updated_at`  FROM ul_user_accounts ";
				if (userAccountWhere != null && userAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Username))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `username` = @username" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@username" + i, userAccountWhere[i].Username);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Password))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `password` = @password" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@password" + i, userAccountWhere[i].Password);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, userAccountWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userAccountWhere.Count - 1)
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
							UserAccountModel userAccount = new UserAccountModel(); if (!reader.IsDBNull(0))
								userAccount.Id = reader.GetValue(0).ToString();
							else
								userAccount.Id = null;
							if (!reader.IsDBNull(1))
								userAccount.Username = reader.GetValue(1).ToString();
							else
								userAccount.Username = null;
							if (!reader.IsDBNull(2))
								userAccount.Password = reader.GetValue(2).ToString();
							else
								userAccount.Password = null;
							if (!reader.IsDBNull(3))
								userAccount.Token = reader.GetValue(3).ToString();
							else
								userAccount.Token = null;
							if (!reader.IsDBNull(4))
								userAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								userAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								userAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								userAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								userAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								userAccount.UpdatedAt = null;
							response.Add(userAccount);
							userAccount = new UserAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserAccountModel> Create(string connectionString, UserAccountModel userAccount)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO ul_user_accounts (";
				string values = "(";
				if (!String.IsNullOrEmpty(userAccount.Id))
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
					cmd.Parameters.AddWithValue("@id", userAccount.Id);
				}
				if (!String.IsNullOrEmpty(userAccount.Username))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `username` ");
						values = String.Concat(values, ", @username");
					}
					else
					{
						sql = String.Concat(sql, " `username` ");
						values = String.Concat(values, " @username");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@username", userAccount.Username);
				}
				if (!String.IsNullOrEmpty(userAccount.Password))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `password` ");
						values = String.Concat(values, ", @password");
					}
					else
					{
						sql = String.Concat(sql, " `password` ");
						values = String.Concat(values, " @password");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@password", userAccount.Password);
				}
				if (!String.IsNullOrEmpty(userAccount.Token))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `token` ");
						values = String.Concat(values, ", @token");
					}
					else
					{
						sql = String.Concat(sql, " `token` ");
						values = String.Concat(values, " @token");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@token", userAccount.Token);
				}
				if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userAccount.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				userAccount.Id = cmd.LastInsertedId.ToString();
				response.Add(userAccount);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
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
		public List<UserAccountModel> Create(string connectionString, List<UserAccountModel> userAccountList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
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
						for (int i = 0; i < userAccountList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO ul_user_accounts (";
								string values = "(";
								if (!String.IsNullOrEmpty(userAccountList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, userAccountList[i].Id);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].Username))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `username` ");
										values = String.Concat(values, ", @username" + i);
									}

									else
									{
										sql = String.Concat(sql, " `username` ");
										values = String.Concat(values, "@username" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@username" + i, userAccountList[i].Username);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].Password))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `password` ");
										values = String.Concat(values, ", @password" + i);
									}

									else
									{
										sql = String.Concat(sql, " `password` ");
										values = String.Concat(values, "@password" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@password" + i, userAccountList[i].Password);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].Token))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `token` ");
										values = String.Concat(values, ", @token" + i);
									}

									else
									{
										sql = String.Concat(sql, " `token` ");
										values = String.Concat(values, "@token" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@token" + i, userAccountList[i].Token);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, userAccountList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(userAccountList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<UserAccountModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserAccountModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
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

		public List<UserAccountModel> Update(string connectionString, UserAccountModel userAccount, UserAccountModel userAccountWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE ul_user_accounts SET ";
				if (!String.IsNullOrEmpty(userAccount.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", userAccount.Id);
				}
				if (!String.IsNullOrEmpty(userAccount.Username))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `username` = @username");
					else
						sql = String.Concat(sql, " `username` = @username");

					addAnd = true;
					cmd.Parameters.AddWithValue("@username", userAccount.Username);
				}
				if (!String.IsNullOrEmpty(userAccount.Password))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `password` = @password");
					else
						sql = String.Concat(sql, " `password` = @password");

					addAnd = true;
					cmd.Parameters.AddWithValue("@password", userAccount.Password);
				}
				if (!String.IsNullOrEmpty(userAccount.Token))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `token` = @token");
					else
						sql = String.Concat(sql, " `token` = @token");

					addAnd = true;
					cmd.Parameters.AddWithValue("@token", userAccount.Token);
				}
				if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userAccount.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
				}
				if (userAccountWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(userAccountWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", userAccountWhere.Id);
					}
					if (!String.IsNullOrEmpty(userAccountWhere.Username))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `username` = @usernameWhere");
						else
							sql = String.Concat(sql, " `username` = @usernameWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@usernameWhere", userAccountWhere.Username);
					}
					if (!String.IsNullOrEmpty(userAccountWhere.Password))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `password` = @passwordWhere");
						else
							sql = String.Concat(sql, " `password` = @passwordWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@passwordWhere", userAccountWhere.Password);
					}
					if (!String.IsNullOrEmpty(userAccountWhere.Token))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `token` = @tokenWhere");
						else
							sql = String.Concat(sql, " `token` = @tokenWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@tokenWhere", userAccountWhere.Token);
					}
					if (!String.IsNullOrEmpty(userAccountWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", userAccountWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userAccountWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", userAccountWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userAccountWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", userAccountWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(userAccount);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
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

		public List<UserAccountModel> Update(string connectionString, List<UserAccountModel> userAccountList, List<UserAccountModel> userAccountWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
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
						for (int i = 0; i < userAccountList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE ul_user_accounts SET ";
								if (!String.IsNullOrEmpty(userAccountList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, userAccountList[i].Id);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].Username))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `username` = @username" + i);
									else
										sql = String.Concat(sql, " `username` = @username" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@username" + i, userAccountList[i].Username);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].Password))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `password` = @password" + i);
									else
										sql = String.Concat(sql, " `password` = @password" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@password" + i, userAccountList[i].Password);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].Token))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `token` = @token" + i);
									else
										sql = String.Concat(sql, " `token` = @token" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@token" + i, userAccountList[i].Token);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccountList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userAccountList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userAccountList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userAccountList[i].UpdatedAt);
								}
								if (userAccountWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(userAccountWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, userAccountWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(userAccountWhereList[i].Username))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `username` = @usernameWhere" + i);
										else
											sql = String.Concat(sql, " `username` = @usernameWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@usernameWhere" + i, userAccountWhereList[i].Username);
									}
									if (!String.IsNullOrEmpty(userAccountWhereList[i].Password))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `password` = @passwordWhere" + i);
										else
											sql = String.Concat(sql, " `password` = @passwordWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@passwordWhere" + i, userAccountWhereList[i].Password);
									}
									if (!String.IsNullOrEmpty(userAccountWhereList[i].Token))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `token` = @tokenWhere" + i);
										else
											sql = String.Concat(sql, " `token` = @tokenWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@tokenWhere" + i, userAccountWhereList[i].Token);
									}
									if (!String.IsNullOrEmpty(userAccountWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userAccountWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userAccountWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, userAccountWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userAccountWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userAccountWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userAccountList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserAccountModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserAccountModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
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

		public List<UserAccountModel> Delete(string connectionString, UserAccountModel userAccount)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM ul_user_accounts  ";
				if (userAccount != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(userAccount.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", userAccount.Id);
					}
					if (!String.IsNullOrEmpty(userAccount.Username))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `username` = @username");
						else
							sql = String.Concat(sql, " `username` = @username");

						addAnd = true;
						cmd.Parameters.AddWithValue("@username", userAccount.Username);
					}
					if (!String.IsNullOrEmpty(userAccount.Password))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `password` = @password");
						else
							sql = String.Concat(sql, " `password` = @password");

						addAnd = true;
						cmd.Parameters.AddWithValue("@password", userAccount.Password);
					}
					if (!String.IsNullOrEmpty(userAccount.Token))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `token` = @token");
						else
							sql = String.Concat(sql, " `token` = @token");

						addAnd = true;
						cmd.Parameters.AddWithValue("@token", userAccount.Token);
					}
					if (!String.IsNullOrEmpty(userAccount.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", userAccount.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userAccount.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", userAccount.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userAccount.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", userAccount.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(userAccount);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
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

		public List<UserAccountModel> Delete(string connectionString, List<UserAccountModel> userAccount)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserAccountModel> response = new List<UserAccountModel>();
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
						for (int i = 0; i < userAccount.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM ul_user_accounts  ";
								if (userAccount[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(userAccount[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, userAccount[i].Id);
									}
									if (!String.IsNullOrEmpty(userAccount[i].Username))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `username` = @username" + i);
										else
											sql = String.Concat(sql, " `username` = @username" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@username" + i, userAccount[i].Username);
									}
									if (!String.IsNullOrEmpty(userAccount[i].Password))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `password` = @password" + i);
										else
											sql = String.Concat(sql, " `password` = @password" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@password" + i, userAccount[i].Password);
									}
									if (!String.IsNullOrEmpty(userAccount[i].Token))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `token` = @token" + i);
										else
											sql = String.Concat(sql, " `token` = @token" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@token" + i, userAccount[i].Token);
									}
									if (!String.IsNullOrEmpty(userAccount[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, userAccount[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userAccount[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, userAccount[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userAccount[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, userAccount[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userAccount[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserAccountModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserAccountModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserAccountModel>();
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
