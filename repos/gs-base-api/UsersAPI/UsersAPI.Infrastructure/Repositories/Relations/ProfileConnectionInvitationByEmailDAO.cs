using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Relations;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Relations
{
	class ProfileConnectionInvitationByEmailDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, ProfileConnectionInvitationByEmailModel> ReadD(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionInvitationByEmailModel> response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				{
					if (profileConnectionInvitationByEmail != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_code_to_connect_with` = @userCodeToConnectWith ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_code_to_connect_with` = @userCodeToConnectWith ");
							}
							cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
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
							cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
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
							cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_by` = @createdBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_by` = @createdBy ");
							}
							cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
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
									profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
								else
									profileConnectionInvitationByEmail.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
								else
									profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
								if (!reader.IsDBNull(2))
									profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
								else
									profileConnectionInvitationByEmail.Email = null;
								if (!reader.IsDBNull(3))
									profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
								else
									profileConnectionInvitationByEmail.Token = null;
								if (!reader.IsDBNull(4))
									profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
								else
									profileConnectionInvitationByEmail.CreatedBy = null;
								if (!reader.IsDBNull(5))
									profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
								else
									profileConnectionInvitationByEmail.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
								else
									profileConnectionInvitationByEmail.CreatedAt = null;
								if (!reader.IsDBNull(7))
									profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
								else
									profileConnectionInvitationByEmail.UpdatedAt = null;
								response.Add(profileConnectionInvitationByEmail.Id, profileConnectionInvitationByEmail);
								profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
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
				response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionInvitationByEmailModel> ReadD(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionInvitationByEmailModel> response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				{
					if (profileConnectionInvitationByEmail != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_code_to_connect_with` = @userCodeToConnectWith ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_code_to_connect_with` = @userCodeToConnectWith ");
							}
							cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
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
							cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
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
							cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_by` = @createdBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_by` = @createdBy ");
							}
							cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
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
									profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
								else
									profileConnectionInvitationByEmail.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
								else
									profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
								if (!reader.IsDBNull(2))
									profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
								else
									profileConnectionInvitationByEmail.Email = null;
								if (!reader.IsDBNull(3))
									profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
								else
									profileConnectionInvitationByEmail.Token = null;
								if (!reader.IsDBNull(4))
									profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
								else
									profileConnectionInvitationByEmail.CreatedBy = null;
								if (!reader.IsDBNull(5))
									profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
								else
									profileConnectionInvitationByEmail.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
								else
									profileConnectionInvitationByEmail.CreatedAt = null;
								if (!reader.IsDBNull(7))
									profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
								else
									profileConnectionInvitationByEmail.UpdatedAt = null;
								response.Add(profileConnectionInvitationByEmail.Id, profileConnectionInvitationByEmail);
								profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
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
				response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionInvitationByEmailModel> ReadD(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionInvitationByEmailModel> response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				if (profileConnectionInvitationByEmailWhere != null && profileConnectionInvitationByEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionInvitationByEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_code_to_connect_with` = @userCodeToConnectWith" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmailWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_by` = @createdBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmailWhere[i].CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionInvitationByEmailWhere.Count - 1)
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
							ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel(); if (!reader.IsDBNull(0))
								profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
							else
								profileConnectionInvitationByEmail.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
							else
								profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
							if (!reader.IsDBNull(2))
								profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
							else
								profileConnectionInvitationByEmail.Email = null;
							if (!reader.IsDBNull(3))
								profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
							else
								profileConnectionInvitationByEmail.Token = null;
							if (!reader.IsDBNull(4))
								profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
							else
								profileConnectionInvitationByEmail.CreatedBy = null;
							if (!reader.IsDBNull(5))
								profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
							else
								profileConnectionInvitationByEmail.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
							else
								profileConnectionInvitationByEmail.CreatedAt = null;
							if (!reader.IsDBNull(7))
								profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
							else
								profileConnectionInvitationByEmail.UpdatedAt = null;
							response.Add(profileConnectionInvitationByEmail.Id, profileConnectionInvitationByEmail);
							profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionInvitationByEmailModel> ReadD(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionInvitationByEmailModel> response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				if (profileConnectionInvitationByEmailWhere != null && profileConnectionInvitationByEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionInvitationByEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_code_to_connect_with` = @userCodeToConnectWith" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmailWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_by` = @createdBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmailWhere[i].CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionInvitationByEmailWhere.Count - 1)
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
							ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel(); if (!reader.IsDBNull(0))
								profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
							else
								profileConnectionInvitationByEmail.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
							else
								profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
							if (!reader.IsDBNull(2))
								profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
							else
								profileConnectionInvitationByEmail.Email = null;
							if (!reader.IsDBNull(3))
								profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
							else
								profileConnectionInvitationByEmail.Token = null;
							if (!reader.IsDBNull(4))
								profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
							else
								profileConnectionInvitationByEmail.CreatedBy = null;
							if (!reader.IsDBNull(5))
								profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
							else
								profileConnectionInvitationByEmail.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
							else
								profileConnectionInvitationByEmail.CreatedAt = null;
							if (!reader.IsDBNull(7))
								profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
							else
								profileConnectionInvitationByEmail.UpdatedAt = null;
							response.Add(profileConnectionInvitationByEmail.Id, profileConnectionInvitationByEmail);
							profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionInvitationByEmailModel> Read(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				{
					if (profileConnectionInvitationByEmail != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_code_to_connect_with` = @userCodeToConnectWith ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_code_to_connect_with` = @userCodeToConnectWith ");
							}
							cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
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
							cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
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
							cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_by` = @createdBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_by` = @createdBy ");
							}
							cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
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
									profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
								else
									profileConnectionInvitationByEmail.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
								else
									profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
								if (!reader.IsDBNull(2))
									profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
								else
									profileConnectionInvitationByEmail.Email = null;
								if (!reader.IsDBNull(3))
									profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
								else
									profileConnectionInvitationByEmail.Token = null;
								if (!reader.IsDBNull(4))
									profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
								else
									profileConnectionInvitationByEmail.CreatedBy = null;
								if (!reader.IsDBNull(5))
									profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
								else
									profileConnectionInvitationByEmail.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
								else
									profileConnectionInvitationByEmail.CreatedAt = null;
								if (!reader.IsDBNull(7))
									profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
								else
									profileConnectionInvitationByEmail.UpdatedAt = null;
								response.Add(profileConnectionInvitationByEmail);
								profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ProfileConnectionInvitationByEmailModel>();
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
				response = new List<ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionInvitationByEmailModel> Read(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				{
					if (profileConnectionInvitationByEmail != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_code_to_connect_with` = @userCodeToConnectWith ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_code_to_connect_with` = @userCodeToConnectWith ");
							}
							cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
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
							cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
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
							cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `created_by` = @createdBy ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `created_by` = @createdBy ");
							}
							cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
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
									profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
								else
									profileConnectionInvitationByEmail.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
								else
									profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
								if (!reader.IsDBNull(2))
									profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
								else
									profileConnectionInvitationByEmail.Email = null;
								if (!reader.IsDBNull(3))
									profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
								else
									profileConnectionInvitationByEmail.Token = null;
								if (!reader.IsDBNull(4))
									profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
								else
									profileConnectionInvitationByEmail.CreatedBy = null;
								if (!reader.IsDBNull(5))
									profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
								else
									profileConnectionInvitationByEmail.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
								else
									profileConnectionInvitationByEmail.CreatedAt = null;
								if (!reader.IsDBNull(7))
									profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
								else
									profileConnectionInvitationByEmail.UpdatedAt = null;
								response.Add(profileConnectionInvitationByEmail);
								profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ProfileConnectionInvitationByEmailModel>();
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
				response = new List<ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionInvitationByEmailModel> Read(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				if (profileConnectionInvitationByEmailWhere != null && profileConnectionInvitationByEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionInvitationByEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_code_to_connect_with` = @userCodeToConnectWith" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmailWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_by` = @createdBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmailWhere[i].CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionInvitationByEmailWhere.Count - 1)
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
							ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel(); if (!reader.IsDBNull(0))
								profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
							else
								profileConnectionInvitationByEmail.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
							else
								profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
							if (!reader.IsDBNull(2))
								profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
							else
								profileConnectionInvitationByEmail.Email = null;
							if (!reader.IsDBNull(3))
								profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
							else
								profileConnectionInvitationByEmail.Token = null;
							if (!reader.IsDBNull(4))
								profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
							else
								profileConnectionInvitationByEmail.CreatedBy = null;
							if (!reader.IsDBNull(5))
								profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
							else
								profileConnectionInvitationByEmail.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
							else
								profileConnectionInvitationByEmail.CreatedAt = null;
							if (!reader.IsDBNull(7))
								profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
							else
								profileConnectionInvitationByEmail.UpdatedAt = null;
							response.Add(profileConnectionInvitationByEmail);
							profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ProfileConnectionInvitationByEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionInvitationByEmailModel> Read(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_code_to_connect_with`, `email`, `token`, `created_by`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_invitation_by_emails ";
				if (profileConnectionInvitationByEmailWhere != null && profileConnectionInvitationByEmailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionInvitationByEmailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_code_to_connect_with` = @userCodeToConnectWith" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmailWhere[i].UserCodeToConnectWith);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Email))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `email` = @email" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmailWhere[i].Email);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].Token))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `token` = @token" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmailWhere[i].Token);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_by` = @createdBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmailWhere[i].CreatedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionInvitationByEmailWhere.Count - 1)
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
							ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel(); if (!reader.IsDBNull(0))
								profileConnectionInvitationByEmail.Id = reader.GetValue(0).ToString();
							else
								profileConnectionInvitationByEmail.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionInvitationByEmail.UserCodeToConnectWith = reader.GetValue(1).ToString();
							else
								profileConnectionInvitationByEmail.UserCodeToConnectWith = null;
							if (!reader.IsDBNull(2))
								profileConnectionInvitationByEmail.Email = reader.GetValue(2).ToString();
							else
								profileConnectionInvitationByEmail.Email = null;
							if (!reader.IsDBNull(3))
								profileConnectionInvitationByEmail.Token = reader.GetValue(3).ToString();
							else
								profileConnectionInvitationByEmail.Token = null;
							if (!reader.IsDBNull(4))
								profileConnectionInvitationByEmail.CreatedBy = reader.GetValue(4).ToString();
							else
								profileConnectionInvitationByEmail.CreatedBy = null;
							if (!reader.IsDBNull(5))
								profileConnectionInvitationByEmail.ModifiedBy = reader.GetValue(5).ToString();
							else
								profileConnectionInvitationByEmail.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								profileConnectionInvitationByEmail.CreatedAt = reader.GetValue(6).ToString();
							else
								profileConnectionInvitationByEmail.CreatedAt = null;
							if (!reader.IsDBNull(7))
								profileConnectionInvitationByEmail.UpdatedAt = reader.GetValue(7).ToString();
							else
								profileConnectionInvitationByEmail.UpdatedAt = null;
							response.Add(profileConnectionInvitationByEmail);
							profileConnectionInvitationByEmail = new ProfileConnectionInvitationByEmailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ProfileConnectionInvitationByEmailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionInvitationByEmailModel> Create(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO urel_profile_connection_invitation_by_emails (";
				string values = "(";
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
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
					cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_code_to_connect_with` ");
						values = String.Concat(values, ", @userCodeToConnectWith");
					}
					else
					{
						sql = String.Concat(sql, " `user_code_to_connect_with` ");
						values = String.Concat(values, " @userCodeToConnectWith");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
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
					cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
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
					cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `created_by` ");
						values = String.Concat(values, ", @createdBy");
					}
					else
					{
						sql = String.Concat(sql, " `created_by` ");
						values = String.Concat(values, " @createdBy");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				profileConnectionInvitationByEmail.Id = cmd.LastInsertedId.ToString();
				response.Add(profileConnectionInvitationByEmail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
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
		public List<ProfileConnectionInvitationByEmailModel> Create(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
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
						for (int i = 0; i < profileConnectionInvitationByEmailList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO urel_profile_connection_invitation_by_emails (";
								string values = "(";
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmailList[i].Id);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].UserCodeToConnectWith))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_code_to_connect_with` ");
										values = String.Concat(values, ", @userCodeToConnectWith" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_code_to_connect_with` ");
										values = String.Concat(values, "@userCodeToConnectWith" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmailList[i].UserCodeToConnectWith);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].Email))
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
									cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmailList[i].Email);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].Token))
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
									cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmailList[i].Token);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].CreatedBy))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `created_by` ");
										values = String.Concat(values, ", @createdBy" + i);
									}

									else
									{
										sql = String.Concat(sql, " `created_by` ");
										values = String.Concat(values, "@createdBy" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmailList[i].CreatedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmailList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmailList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmailList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(profileConnectionInvitationByEmailList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionInvitationByEmailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionInvitationByEmailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
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

		public List<ProfileConnectionInvitationByEmailModel> Update(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE urel_profile_connection_invitation_by_emails SET ";
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_code_to_connect_with` = @userCodeToConnectWith");
					else
						sql = String.Concat(sql, " `user_code_to_connect_with` = @userCodeToConnectWith");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `email` = @email");
					else
						sql = String.Concat(sql, " `email` = @email");

					addAnd = true;
					cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `token` = @token");
					else
						sql = String.Concat(sql, " `token` = @token");

					addAnd = true;
					cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_by` = @createdBy");
					else
						sql = String.Concat(sql, " `created_by` = @createdBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
				}
				if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
				}
				if (profileConnectionInvitationByEmailWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", profileConnectionInvitationByEmailWhere.Id);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.UserCodeToConnectWith))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_code_to_connect_with` = @userCodeToConnectWithWhere");
						else
							sql = String.Concat(sql, " `user_code_to_connect_with` = @userCodeToConnectWithWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userCodeToConnectWithWhere", profileConnectionInvitationByEmailWhere.UserCodeToConnectWith);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.Email))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `email` = @emailWhere");
						else
							sql = String.Concat(sql, " `email` = @emailWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@emailWhere", profileConnectionInvitationByEmailWhere.Email);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.Token))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `token` = @tokenWhere");
						else
							sql = String.Concat(sql, " `token` = @tokenWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@tokenWhere", profileConnectionInvitationByEmailWhere.Token);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.CreatedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_by` = @createdByWhere");
						else
							sql = String.Concat(sql, " `created_by` = @createdByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdByWhere", profileConnectionInvitationByEmailWhere.CreatedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", profileConnectionInvitationByEmailWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", profileConnectionInvitationByEmailWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", profileConnectionInvitationByEmailWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(profileConnectionInvitationByEmail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
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

		public List<ProfileConnectionInvitationByEmailModel> Update(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailList, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmailWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
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
						for (int i = 0; i < profileConnectionInvitationByEmailList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE urel_profile_connection_invitation_by_emails SET ";
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmailList[i].Id);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].UserCodeToConnectWith))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_code_to_connect_with` = @userCodeToConnectWith" + i);
									else
										sql = String.Concat(sql, " `user_code_to_connect_with` = @userCodeToConnectWith" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmailList[i].UserCodeToConnectWith);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].Email))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `email` = @email" + i);
									else
										sql = String.Concat(sql, " `email` = @email" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmailList[i].Email);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].Token))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `token` = @token" + i);
									else
										sql = String.Concat(sql, " `token` = @token" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmailList[i].Token);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].CreatedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_by` = @createdBy" + i);
									else
										sql = String.Concat(sql, " `created_by` = @createdBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmailList[i].CreatedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmailList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmailList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmailList[i].UpdatedAt);
								}
								if (profileConnectionInvitationByEmailWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, profileConnectionInvitationByEmailWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].UserCodeToConnectWith))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_code_to_connect_with` = @userCodeToConnectWithWhere" + i);
										else
											sql = String.Concat(sql, " `user_code_to_connect_with` = @userCodeToConnectWithWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userCodeToConnectWithWhere" + i, profileConnectionInvitationByEmailWhereList[i].UserCodeToConnectWith);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].Email))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `email` = @emailWhere" + i);
										else
											sql = String.Concat(sql, " `email` = @emailWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@emailWhere" + i, profileConnectionInvitationByEmailWhereList[i].Email);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].Token))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `token` = @tokenWhere" + i);
										else
											sql = String.Concat(sql, " `token` = @tokenWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@tokenWhere" + i, profileConnectionInvitationByEmailWhereList[i].Token);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].CreatedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_by` = @createdByWhere" + i);
										else
											sql = String.Concat(sql, " `created_by` = @createdByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdByWhere" + i, profileConnectionInvitationByEmailWhereList[i].CreatedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, profileConnectionInvitationByEmailWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, profileConnectionInvitationByEmailWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmailWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, profileConnectionInvitationByEmailWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(profileConnectionInvitationByEmailList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionInvitationByEmailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionInvitationByEmailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
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

		public List<ProfileConnectionInvitationByEmailModel> Delete(string connectionString, ProfileConnectionInvitationByEmailModel profileConnectionInvitationByEmail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM urel_profile_connection_invitation_by_emails  ";
				if (profileConnectionInvitationByEmail != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", profileConnectionInvitationByEmail.Id);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UserCodeToConnectWith))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_code_to_connect_with` = @userCodeToConnectWith");
						else
							sql = String.Concat(sql, " `user_code_to_connect_with` = @userCodeToConnectWith");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userCodeToConnectWith", profileConnectionInvitationByEmail.UserCodeToConnectWith);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Email))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `email` = @email");
						else
							sql = String.Concat(sql, " `email` = @email");

						addAnd = true;
						cmd.Parameters.AddWithValue("@email", profileConnectionInvitationByEmail.Email);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.Token))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `token` = @token");
						else
							sql = String.Concat(sql, " `token` = @token");

						addAnd = true;
						cmd.Parameters.AddWithValue("@token", profileConnectionInvitationByEmail.Token);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_by` = @createdBy");
						else
							sql = String.Concat(sql, " `created_by` = @createdBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdBy", profileConnectionInvitationByEmail.CreatedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionInvitationByEmail.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", profileConnectionInvitationByEmail.CreatedAt);
					}
					if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", profileConnectionInvitationByEmail.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(profileConnectionInvitationByEmail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
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

		public List<ProfileConnectionInvitationByEmailModel> Delete(string connectionString, List<ProfileConnectionInvitationByEmailModel> profileConnectionInvitationByEmail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionInvitationByEmailModel> response = new List<ProfileConnectionInvitationByEmailModel>();
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
						for (int i = 0; i < profileConnectionInvitationByEmail.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM urel_profile_connection_invitation_by_emails  ";
								if (profileConnectionInvitationByEmail[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, profileConnectionInvitationByEmail[i].Id);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].UserCodeToConnectWith))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_code_to_connect_with` = @userCodeToConnectWith" + i);
										else
											sql = String.Concat(sql, " `user_code_to_connect_with` = @userCodeToConnectWith" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userCodeToConnectWith" + i, profileConnectionInvitationByEmail[i].UserCodeToConnectWith);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].Email))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `email` = @email" + i);
										else
											sql = String.Concat(sql, " `email` = @email" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@email" + i, profileConnectionInvitationByEmail[i].Email);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].Token))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `token` = @token" + i);
										else
											sql = String.Concat(sql, " `token` = @token" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@token" + i, profileConnectionInvitationByEmail[i].Token);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].CreatedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_by` = @createdBy" + i);
										else
											sql = String.Concat(sql, " `created_by` = @createdBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdBy" + i, profileConnectionInvitationByEmail[i].CreatedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionInvitationByEmail[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionInvitationByEmail[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(profileConnectionInvitationByEmail[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionInvitationByEmail[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(profileConnectionInvitationByEmail[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionInvitationByEmailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionInvitationByEmailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionInvitationByEmailModel>();
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
