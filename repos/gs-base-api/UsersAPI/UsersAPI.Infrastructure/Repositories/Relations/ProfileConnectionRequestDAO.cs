using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Relations;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Relations
{
	class ProfileConnectionRequestDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, ProfileConnectionRequestModel> ReadD(string connectionString, ProfileConnectionRequestModel profileConnectionRequest, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionRequestModel> response = new Dictionary<string, ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				{
					if (profileConnectionRequest != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requester_id` = @userRequesterId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requester_id` = @userRequesterId ");
							}
							cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requested_id` = @userRequestedId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requested_id` = @userRequestedId ");
							}
							cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
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
									profileConnectionRequest.Id = reader.GetValue(0).ToString();
								else
									profileConnectionRequest.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
								else
									profileConnectionRequest.UserRequesterId = null;
								if (!reader.IsDBNull(2))
									profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
								else
									profileConnectionRequest.UserRequestedId = null;
								if (!reader.IsDBNull(3))
									profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
								else
									profileConnectionRequest.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
								else
									profileConnectionRequest.CreatedAt = null;
								if (!reader.IsDBNull(5))
									profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
								else
									profileConnectionRequest.UpdatedAt = null;
								response.Add(profileConnectionRequest.Id, profileConnectionRequest);
								profileConnectionRequest = new ProfileConnectionRequestModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, ProfileConnectionRequestModel>();
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
				response = new Dictionary<string, ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionRequestModel> ReadD(string connectionString, ProfileConnectionRequestModel profileConnectionRequest, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionRequestModel> response = new Dictionary<string, ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				{
					if (profileConnectionRequest != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requester_id` = @userRequesterId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requester_id` = @userRequesterId ");
							}
							cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requested_id` = @userRequestedId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requested_id` = @userRequestedId ");
							}
							cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
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
									profileConnectionRequest.Id = reader.GetValue(0).ToString();
								else
									profileConnectionRequest.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
								else
									profileConnectionRequest.UserRequesterId = null;
								if (!reader.IsDBNull(2))
									profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
								else
									profileConnectionRequest.UserRequestedId = null;
								if (!reader.IsDBNull(3))
									profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
								else
									profileConnectionRequest.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
								else
									profileConnectionRequest.CreatedAt = null;
								if (!reader.IsDBNull(5))
									profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
								else
									profileConnectionRequest.UpdatedAt = null;
								response.Add(profileConnectionRequest.Id, profileConnectionRequest);
								profileConnectionRequest = new ProfileConnectionRequestModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, ProfileConnectionRequestModel>();
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
				response = new Dictionary<string, ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionRequestModel> ReadD(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequestWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionRequestModel> response = new Dictionary<string, ProfileConnectionRequestModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				if (profileConnectionRequestWhere != null && profileConnectionRequestWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionRequestWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequestWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequesterId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requester_id` = @userRequesterId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequestWhere[i].UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequestedId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requested_id` = @userRequestedId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequestWhere[i].UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequestWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequestWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequestWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionRequestWhere.Count - 1)
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
							ProfileConnectionRequestModel profileConnectionRequest = new ProfileConnectionRequestModel(); if (!reader.IsDBNull(0))
								profileConnectionRequest.Id = reader.GetValue(0).ToString();
							else
								profileConnectionRequest.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
							else
								profileConnectionRequest.UserRequesterId = null;
							if (!reader.IsDBNull(2))
								profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
							else
								profileConnectionRequest.UserRequestedId = null;
							if (!reader.IsDBNull(3))
								profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
							else
								profileConnectionRequest.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
							else
								profileConnectionRequest.CreatedAt = null;
							if (!reader.IsDBNull(5))
								profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
							else
								profileConnectionRequest.UpdatedAt = null;
							response.Add(profileConnectionRequest.Id, profileConnectionRequest);
							profileConnectionRequest = new ProfileConnectionRequestModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, ProfileConnectionRequestModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionRequestModel> ReadD(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequestWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionRequestModel> response = new Dictionary<string, ProfileConnectionRequestModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				if (profileConnectionRequestWhere != null && profileConnectionRequestWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionRequestWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequestWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequesterId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requester_id` = @userRequesterId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequestWhere[i].UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequestedId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requested_id` = @userRequestedId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequestWhere[i].UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequestWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequestWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequestWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionRequestWhere.Count - 1)
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
							ProfileConnectionRequestModel profileConnectionRequest = new ProfileConnectionRequestModel(); if (!reader.IsDBNull(0))
								profileConnectionRequest.Id = reader.GetValue(0).ToString();
							else
								profileConnectionRequest.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
							else
								profileConnectionRequest.UserRequesterId = null;
							if (!reader.IsDBNull(2))
								profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
							else
								profileConnectionRequest.UserRequestedId = null;
							if (!reader.IsDBNull(3))
								profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
							else
								profileConnectionRequest.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
							else
								profileConnectionRequest.CreatedAt = null;
							if (!reader.IsDBNull(5))
								profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
							else
								profileConnectionRequest.UpdatedAt = null;
							response.Add(profileConnectionRequest.Id, profileConnectionRequest);
							profileConnectionRequest = new ProfileConnectionRequestModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, ProfileConnectionRequestModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionRequestModel> Read(string connectionString, ProfileConnectionRequestModel profileConnectionRequest, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				{
					if (profileConnectionRequest != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requester_id` = @userRequesterId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requester_id` = @userRequesterId ");
							}
							cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requested_id` = @userRequestedId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requested_id` = @userRequestedId ");
							}
							cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
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
									profileConnectionRequest.Id = reader.GetValue(0).ToString();
								else
									profileConnectionRequest.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
								else
									profileConnectionRequest.UserRequesterId = null;
								if (!reader.IsDBNull(2))
									profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
								else
									profileConnectionRequest.UserRequestedId = null;
								if (!reader.IsDBNull(3))
									profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
								else
									profileConnectionRequest.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
								else
									profileConnectionRequest.CreatedAt = null;
								if (!reader.IsDBNull(5))
									profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
								else
									profileConnectionRequest.UpdatedAt = null;
								response.Add(profileConnectionRequest);
								profileConnectionRequest = new ProfileConnectionRequestModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ProfileConnectionRequestModel>();
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
				response = new List<ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionRequestModel> Read(string connectionString, ProfileConnectionRequestModel profileConnectionRequest, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				{
					if (profileConnectionRequest != null)
					{
						if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requester_id` = @userRequesterId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requester_id` = @userRequesterId ");
							}
							cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_requested_id` = @userRequestedId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_requested_id` = @userRequestedId ");
							}
							cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
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
									profileConnectionRequest.Id = reader.GetValue(0).ToString();
								else
									profileConnectionRequest.Id = null;
								if (!reader.IsDBNull(1))
									profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
								else
									profileConnectionRequest.UserRequesterId = null;
								if (!reader.IsDBNull(2))
									profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
								else
									profileConnectionRequest.UserRequestedId = null;
								if (!reader.IsDBNull(3))
									profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
								else
									profileConnectionRequest.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
								else
									profileConnectionRequest.CreatedAt = null;
								if (!reader.IsDBNull(5))
									profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
								else
									profileConnectionRequest.UpdatedAt = null;
								response.Add(profileConnectionRequest);
								profileConnectionRequest = new ProfileConnectionRequestModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ProfileConnectionRequestModel>();
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
				response = new List<ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionRequestModel> Read(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequestWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				if (profileConnectionRequestWhere != null && profileConnectionRequestWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionRequestWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequestWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequesterId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requester_id` = @userRequesterId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequestWhere[i].UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequestedId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requested_id` = @userRequestedId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequestWhere[i].UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequestWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequestWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequestWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionRequestWhere.Count - 1)
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
							ProfileConnectionRequestModel profileConnectionRequest = new ProfileConnectionRequestModel(); if (!reader.IsDBNull(0))
								profileConnectionRequest.Id = reader.GetValue(0).ToString();
							else
								profileConnectionRequest.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
							else
								profileConnectionRequest.UserRequesterId = null;
							if (!reader.IsDBNull(2))
								profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
							else
								profileConnectionRequest.UserRequestedId = null;
							if (!reader.IsDBNull(3))
								profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
							else
								profileConnectionRequest.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
							else
								profileConnectionRequest.CreatedAt = null;
							if (!reader.IsDBNull(5))
								profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
							else
								profileConnectionRequest.UpdatedAt = null;
							response.Add(profileConnectionRequest);
							profileConnectionRequest = new ProfileConnectionRequestModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ProfileConnectionRequestModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionRequestModel> Read(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequestWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_requester_id`, `user_requested_id`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connection_requests ";
				if (profileConnectionRequestWhere != null && profileConnectionRequestWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionRequestWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequestWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequesterId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requester_id` = @userRequesterId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequestWhere[i].UserRequesterId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UserRequestedId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_requested_id` = @userRequestedId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequestWhere[i].UserRequestedId);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequestWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequestWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionRequestWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequestWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionRequestWhere.Count - 1)
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
							ProfileConnectionRequestModel profileConnectionRequest = new ProfileConnectionRequestModel(); if (!reader.IsDBNull(0))
								profileConnectionRequest.Id = reader.GetValue(0).ToString();
							else
								profileConnectionRequest.Id = null;
							if (!reader.IsDBNull(1))
								profileConnectionRequest.UserRequesterId = reader.GetValue(1).ToString();
							else
								profileConnectionRequest.UserRequesterId = null;
							if (!reader.IsDBNull(2))
								profileConnectionRequest.UserRequestedId = reader.GetValue(2).ToString();
							else
								profileConnectionRequest.UserRequestedId = null;
							if (!reader.IsDBNull(3))
								profileConnectionRequest.ModifiedBy = reader.GetValue(3).ToString();
							else
								profileConnectionRequest.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								profileConnectionRequest.CreatedAt = reader.GetValue(4).ToString();
							else
								profileConnectionRequest.CreatedAt = null;
							if (!reader.IsDBNull(5))
								profileConnectionRequest.UpdatedAt = reader.GetValue(5).ToString();
							else
								profileConnectionRequest.UpdatedAt = null;
							response.Add(profileConnectionRequest);
							profileConnectionRequest = new ProfileConnectionRequestModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ProfileConnectionRequestModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionRequestModel> Create(string connectionString, ProfileConnectionRequestModel profileConnectionRequest)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO urel_profile_connection_requests (";
				string values = "(";
				if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
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
					cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_requester_id` ");
						values = String.Concat(values, ", @userRequesterId");
					}
					else
					{
						sql = String.Concat(sql, " `user_requester_id` ");
						values = String.Concat(values, " @userRequesterId");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_requested_id` ");
						values = String.Concat(values, ", @userRequestedId");
					}
					else
					{
						sql = String.Concat(sql, " `user_requested_id` ");
						values = String.Concat(values, " @userRequestedId");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				profileConnectionRequest.Id = cmd.LastInsertedId.ToString();
				response.Add(profileConnectionRequest);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
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
		public List<ProfileConnectionRequestModel> Create(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequestList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
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
						for (int i = 0; i < profileConnectionRequestList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO urel_profile_connection_requests (";
								string values = "(";
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequestList[i].Id);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].UserRequesterId))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_requester_id` ");
										values = String.Concat(values, ", @userRequesterId" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_requester_id` ");
										values = String.Concat(values, "@userRequesterId" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequestList[i].UserRequesterId);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].UserRequestedId))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_requested_id` ");
										values = String.Concat(values, ", @userRequestedId" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_requested_id` ");
										values = String.Concat(values, "@userRequestedId" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequestList[i].UserRequestedId);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequestList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequestList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequestList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(profileConnectionRequestList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionRequestModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionRequestModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
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

		public List<ProfileConnectionRequestModel> Update(string connectionString, ProfileConnectionRequestModel profileConnectionRequest, ProfileConnectionRequestModel profileConnectionRequestWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE urel_profile_connection_requests SET ";
				if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_requester_id` = @userRequesterId");
					else
						sql = String.Concat(sql, " `user_requester_id` = @userRequesterId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_requested_id` = @userRequestedId");
					else
						sql = String.Concat(sql, " `user_requested_id` = @userRequestedId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
				}
				if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
				}
				if (profileConnectionRequestWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(profileConnectionRequestWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", profileConnectionRequestWhere.Id);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequestWhere.UserRequesterId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_requester_id` = @userRequesterIdWhere");
						else
							sql = String.Concat(sql, " `user_requester_id` = @userRequesterIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userRequesterIdWhere", profileConnectionRequestWhere.UserRequesterId);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequestWhere.UserRequestedId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_requested_id` = @userRequestedIdWhere");
						else
							sql = String.Concat(sql, " `user_requested_id` = @userRequestedIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userRequestedIdWhere", profileConnectionRequestWhere.UserRequestedId);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequestWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", profileConnectionRequestWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequestWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", profileConnectionRequestWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequestWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", profileConnectionRequestWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(profileConnectionRequest);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
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

		public List<ProfileConnectionRequestModel> Update(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequestList, List<ProfileConnectionRequestModel> profileConnectionRequestWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
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
						for (int i = 0; i < profileConnectionRequestList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE urel_profile_connection_requests SET ";
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequestList[i].Id);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].UserRequesterId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_requester_id` = @userRequesterId" + i);
									else
										sql = String.Concat(sql, " `user_requester_id` = @userRequesterId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequestList[i].UserRequesterId);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].UserRequestedId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_requested_id` = @userRequestedId" + i);
									else
										sql = String.Concat(sql, " `user_requested_id` = @userRequestedId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequestList[i].UserRequestedId);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequestList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequestList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(profileConnectionRequestList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequestList[i].UpdatedAt);
								}
								if (profileConnectionRequestWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(profileConnectionRequestWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, profileConnectionRequestWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequestWhereList[i].UserRequesterId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_requester_id` = @userRequesterIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_requester_id` = @userRequesterIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userRequesterIdWhere" + i, profileConnectionRequestWhereList[i].UserRequesterId);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequestWhereList[i].UserRequestedId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_requested_id` = @userRequestedIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_requested_id` = @userRequestedIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userRequestedIdWhere" + i, profileConnectionRequestWhereList[i].UserRequestedId);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequestWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, profileConnectionRequestWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequestWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, profileConnectionRequestWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequestWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, profileConnectionRequestWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(profileConnectionRequestList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionRequestModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionRequestModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
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

		public List<ProfileConnectionRequestModel> Delete(string connectionString, ProfileConnectionRequestModel profileConnectionRequest)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM urel_profile_connection_requests  ";
				if (profileConnectionRequest != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(profileConnectionRequest.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", profileConnectionRequest.Id);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequesterId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_requester_id` = @userRequesterId");
						else
							sql = String.Concat(sql, " `user_requester_id` = @userRequesterId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userRequesterId", profileConnectionRequest.UserRequesterId);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequest.UserRequestedId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_requested_id` = @userRequestedId");
						else
							sql = String.Concat(sql, " `user_requested_id` = @userRequestedId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userRequestedId", profileConnectionRequest.UserRequestedId);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequest.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", profileConnectionRequest.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequest.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", profileConnectionRequest.CreatedAt);
					}
					if (!String.IsNullOrEmpty(profileConnectionRequest.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", profileConnectionRequest.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(profileConnectionRequest);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
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

		public List<ProfileConnectionRequestModel> Delete(string connectionString, List<ProfileConnectionRequestModel> profileConnectionRequest)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionRequestModel> response = new List<ProfileConnectionRequestModel>();
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
						for (int i = 0; i < profileConnectionRequest.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM urel_profile_connection_requests  ";
								if (profileConnectionRequest[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(profileConnectionRequest[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, profileConnectionRequest[i].Id);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequest[i].UserRequesterId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_requester_id` = @userRequesterId" + i);
										else
											sql = String.Concat(sql, " `user_requester_id` = @userRequesterId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userRequesterId" + i, profileConnectionRequest[i].UserRequesterId);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequest[i].UserRequestedId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_requested_id` = @userRequestedId" + i);
										else
											sql = String.Concat(sql, " `user_requested_id` = @userRequestedId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userRequestedId" + i, profileConnectionRequest[i].UserRequestedId);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequest[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionRequest[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequest[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionRequest[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(profileConnectionRequest[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionRequest[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(profileConnectionRequest[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionRequestModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionRequestModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionRequestModel>();
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
