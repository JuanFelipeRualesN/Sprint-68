using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Relations;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Relations
{
	class ProfileConnectionDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, ProfileConnectionModel> ReadD(string connectionString, ProfileConnectionModel profileConnection, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionModel> response = new Dictionary<string, ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				{
					if (profileConnection != null)
					{
						if (!String.IsNullOrEmpty(profileConnection.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnection.Id);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdA))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_a` = @userIdA ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_a` = @userIdA ");
							}
							cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdB))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_b` = @userIdB ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_b` = @userIdB ");
							}
							cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_a_alpha` = @userAAlpha ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_a_alpha` = @userAAlpha ");
							}
							cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
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
									profileConnection.Id = reader.GetValue(0).ToString();
								else
									profileConnection.Id = null;
								if (!reader.IsDBNull(1))
									profileConnection.UserIdA = reader.GetValue(1).ToString();
								else
									profileConnection.UserIdA = null;
								if (!reader.IsDBNull(2))
									profileConnection.UserIdB = reader.GetValue(2).ToString();
								else
									profileConnection.UserIdB = null;
								if (!reader.IsDBNull(3))
									profileConnection.UserAAlpha = reader.GetValue(3).ToString();
								else
									profileConnection.UserAAlpha = null;
								if (!reader.IsDBNull(4))
									profileConnection.ModifiedBy = reader.GetValue(4).ToString();
								else
									profileConnection.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									profileConnection.CreatedAt = reader.GetValue(5).ToString();
								else
									profileConnection.CreatedAt = null;
								if (!reader.IsDBNull(6))
									profileConnection.UpdatedAt = reader.GetValue(6).ToString();
								else
									profileConnection.UpdatedAt = null;
								response.Add(profileConnection.Id, profileConnection);
								profileConnection = new ProfileConnectionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, ProfileConnectionModel>();
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
				response = new Dictionary<string, ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionModel> ReadD(string connectionString, ProfileConnectionModel profileConnection, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionModel> response = new Dictionary<string, ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				{
					if (profileConnection != null)
					{
						if (!String.IsNullOrEmpty(profileConnection.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnection.Id);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdA))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_a` = @userIdA ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_a` = @userIdA ");
							}
							cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdB))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_b` = @userIdB ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_b` = @userIdB ");
							}
							cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_a_alpha` = @userAAlpha ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_a_alpha` = @userAAlpha ");
							}
							cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
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
									profileConnection.Id = reader.GetValue(0).ToString();
								else
									profileConnection.Id = null;
								if (!reader.IsDBNull(1))
									profileConnection.UserIdA = reader.GetValue(1).ToString();
								else
									profileConnection.UserIdA = null;
								if (!reader.IsDBNull(2))
									profileConnection.UserIdB = reader.GetValue(2).ToString();
								else
									profileConnection.UserIdB = null;
								if (!reader.IsDBNull(3))
									profileConnection.UserAAlpha = reader.GetValue(3).ToString();
								else
									profileConnection.UserAAlpha = null;
								if (!reader.IsDBNull(4))
									profileConnection.ModifiedBy = reader.GetValue(4).ToString();
								else
									profileConnection.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									profileConnection.CreatedAt = reader.GetValue(5).ToString();
								else
									profileConnection.CreatedAt = null;
								if (!reader.IsDBNull(6))
									profileConnection.UpdatedAt = reader.GetValue(6).ToString();
								else
									profileConnection.UpdatedAt = null;
								response.Add(profileConnection.Id, profileConnection);
								profileConnection = new ProfileConnectionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, ProfileConnectionModel>();
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
				response = new Dictionary<string, ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionModel> ReadD(string connectionString, List<ProfileConnectionModel> profileConnectionWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionModel> response = new Dictionary<string, ProfileConnectionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				if (profileConnectionWhere != null && profileConnectionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdA))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_a` = @userIdA" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdA" + i, profileConnectionWhere[i].UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdB))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_b` = @userIdB" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdB" + i, profileConnectionWhere[i].UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserAAlpha))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_a_alpha` = @userAAlpha" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnectionWhere[i].UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionWhere.Count - 1)
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
							ProfileConnectionModel profileConnection = new ProfileConnectionModel(); if (!reader.IsDBNull(0))
								profileConnection.Id = reader.GetValue(0).ToString();
							else
								profileConnection.Id = null;
							if (!reader.IsDBNull(1))
								profileConnection.UserIdA = reader.GetValue(1).ToString();
							else
								profileConnection.UserIdA = null;
							if (!reader.IsDBNull(2))
								profileConnection.UserIdB = reader.GetValue(2).ToString();
							else
								profileConnection.UserIdB = null;
							if (!reader.IsDBNull(3))
								profileConnection.UserAAlpha = reader.GetValue(3).ToString();
							else
								profileConnection.UserAAlpha = null;
							if (!reader.IsDBNull(4))
								profileConnection.ModifiedBy = reader.GetValue(4).ToString();
							else
								profileConnection.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								profileConnection.CreatedAt = reader.GetValue(5).ToString();
							else
								profileConnection.CreatedAt = null;
							if (!reader.IsDBNull(6))
								profileConnection.UpdatedAt = reader.GetValue(6).ToString();
							else
								profileConnection.UpdatedAt = null;
							response.Add(profileConnection.Id, profileConnection);
							profileConnection = new ProfileConnectionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, ProfileConnectionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, ProfileConnectionModel> ReadD(string connectionString, List<ProfileConnectionModel> profileConnectionWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, ProfileConnectionModel> response = new Dictionary<string, ProfileConnectionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				if (profileConnectionWhere != null && profileConnectionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdA))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_a` = @userIdA" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdA" + i, profileConnectionWhere[i].UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdB))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_b` = @userIdB" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdB" + i, profileConnectionWhere[i].UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserAAlpha))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_a_alpha` = @userAAlpha" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnectionWhere[i].UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionWhere.Count - 1)
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
							ProfileConnectionModel profileConnection = new ProfileConnectionModel(); if (!reader.IsDBNull(0))
								profileConnection.Id = reader.GetValue(0).ToString();
							else
								profileConnection.Id = null;
							if (!reader.IsDBNull(1))
								profileConnection.UserIdA = reader.GetValue(1).ToString();
							else
								profileConnection.UserIdA = null;
							if (!reader.IsDBNull(2))
								profileConnection.UserIdB = reader.GetValue(2).ToString();
							else
								profileConnection.UserIdB = null;
							if (!reader.IsDBNull(3))
								profileConnection.UserAAlpha = reader.GetValue(3).ToString();
							else
								profileConnection.UserAAlpha = null;
							if (!reader.IsDBNull(4))
								profileConnection.ModifiedBy = reader.GetValue(4).ToString();
							else
								profileConnection.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								profileConnection.CreatedAt = reader.GetValue(5).ToString();
							else
								profileConnection.CreatedAt = null;
							if (!reader.IsDBNull(6))
								profileConnection.UpdatedAt = reader.GetValue(6).ToString();
							else
								profileConnection.UpdatedAt = null;
							response.Add(profileConnection.Id, profileConnection);
							profileConnection = new ProfileConnectionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, ProfileConnectionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionModel> Read(string connectionString, ProfileConnectionModel profileConnection, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				{
					if (profileConnection != null)
					{
						if (!String.IsNullOrEmpty(profileConnection.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnection.Id);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdA))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_a` = @userIdA ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_a` = @userIdA ");
							}
							cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdB))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_b` = @userIdB ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_b` = @userIdB ");
							}
							cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_a_alpha` = @userAAlpha ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_a_alpha` = @userAAlpha ");
							}
							cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
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
									profileConnection.Id = reader.GetValue(0).ToString();
								else
									profileConnection.Id = null;
								if (!reader.IsDBNull(1))
									profileConnection.UserIdA = reader.GetValue(1).ToString();
								else
									profileConnection.UserIdA = null;
								if (!reader.IsDBNull(2))
									profileConnection.UserIdB = reader.GetValue(2).ToString();
								else
									profileConnection.UserIdB = null;
								if (!reader.IsDBNull(3))
									profileConnection.UserAAlpha = reader.GetValue(3).ToString();
								else
									profileConnection.UserAAlpha = null;
								if (!reader.IsDBNull(4))
									profileConnection.ModifiedBy = reader.GetValue(4).ToString();
								else
									profileConnection.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									profileConnection.CreatedAt = reader.GetValue(5).ToString();
								else
									profileConnection.CreatedAt = null;
								if (!reader.IsDBNull(6))
									profileConnection.UpdatedAt = reader.GetValue(6).ToString();
								else
									profileConnection.UpdatedAt = null;
								response.Add(profileConnection);
								profileConnection = new ProfileConnectionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ProfileConnectionModel>();
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
				response = new List<ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionModel> Read(string connectionString, ProfileConnectionModel profileConnection, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				{
					if (profileConnection != null)
					{
						if (!String.IsNullOrEmpty(profileConnection.Id))
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
							cmd.Parameters.AddWithValue("@id", profileConnection.Id);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdA))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_a` = @userIdA ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_a` = @userIdA ");
							}
							cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserIdB))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_id_b` = @userIdB ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_id_b` = @userIdB ");
							}
							cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `user_a_alpha` = @userAAlpha ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `user_a_alpha` = @userAAlpha ");
							}
							cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
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
									profileConnection.Id = reader.GetValue(0).ToString();
								else
									profileConnection.Id = null;
								if (!reader.IsDBNull(1))
									profileConnection.UserIdA = reader.GetValue(1).ToString();
								else
									profileConnection.UserIdA = null;
								if (!reader.IsDBNull(2))
									profileConnection.UserIdB = reader.GetValue(2).ToString();
								else
									profileConnection.UserIdB = null;
								if (!reader.IsDBNull(3))
									profileConnection.UserAAlpha = reader.GetValue(3).ToString();
								else
									profileConnection.UserAAlpha = null;
								if (!reader.IsDBNull(4))
									profileConnection.ModifiedBy = reader.GetValue(4).ToString();
								else
									profileConnection.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									profileConnection.CreatedAt = reader.GetValue(5).ToString();
								else
									profileConnection.CreatedAt = null;
								if (!reader.IsDBNull(6))
									profileConnection.UpdatedAt = reader.GetValue(6).ToString();
								else
									profileConnection.UpdatedAt = null;
								response.Add(profileConnection);
								profileConnection = new ProfileConnectionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ProfileConnectionModel>();
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
				response = new List<ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionModel> Read(string connectionString, List<ProfileConnectionModel> profileConnectionWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				if (profileConnectionWhere != null && profileConnectionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdA))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_a` = @userIdA" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdA" + i, profileConnectionWhere[i].UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdB))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_b` = @userIdB" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdB" + i, profileConnectionWhere[i].UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserAAlpha))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_a_alpha` = @userAAlpha" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnectionWhere[i].UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionWhere.Count - 1)
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
							ProfileConnectionModel profileConnection = new ProfileConnectionModel(); if (!reader.IsDBNull(0))
								profileConnection.Id = reader.GetValue(0).ToString();
							else
								profileConnection.Id = null;
							if (!reader.IsDBNull(1))
								profileConnection.UserIdA = reader.GetValue(1).ToString();
							else
								profileConnection.UserIdA = null;
							if (!reader.IsDBNull(2))
								profileConnection.UserIdB = reader.GetValue(2).ToString();
							else
								profileConnection.UserIdB = null;
							if (!reader.IsDBNull(3))
								profileConnection.UserAAlpha = reader.GetValue(3).ToString();
							else
								profileConnection.UserAAlpha = null;
							if (!reader.IsDBNull(4))
								profileConnection.ModifiedBy = reader.GetValue(4).ToString();
							else
								profileConnection.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								profileConnection.CreatedAt = reader.GetValue(5).ToString();
							else
								profileConnection.CreatedAt = null;
							if (!reader.IsDBNull(6))
								profileConnection.UpdatedAt = reader.GetValue(6).ToString();
							else
								profileConnection.UpdatedAt = null;
							response.Add(profileConnection);
							profileConnection = new ProfileConnectionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ProfileConnectionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionModel> Read(string connectionString, List<ProfileConnectionModel> profileConnectionWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id_a`, `user_id_b`, `user_a_alpha`, `modified_by`, `created_at`, `updated_at`  FROM urel_profile_connections ";
				if (profileConnectionWhere != null && profileConnectionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < profileConnectionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, profileConnectionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdA))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_a` = @userIdA" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdA" + i, profileConnectionWhere[i].UserIdA);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserIdB))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id_b` = @userIdB" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userIdB" + i, profileConnectionWhere[i].UserIdB);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UserAAlpha))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_a_alpha` = @userAAlpha" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnectionWhere[i].UserAAlpha);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(profileConnectionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != profileConnectionWhere.Count - 1)
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
							ProfileConnectionModel profileConnection = new ProfileConnectionModel(); if (!reader.IsDBNull(0))
								profileConnection.Id = reader.GetValue(0).ToString();
							else
								profileConnection.Id = null;
							if (!reader.IsDBNull(1))
								profileConnection.UserIdA = reader.GetValue(1).ToString();
							else
								profileConnection.UserIdA = null;
							if (!reader.IsDBNull(2))
								profileConnection.UserIdB = reader.GetValue(2).ToString();
							else
								profileConnection.UserIdB = null;
							if (!reader.IsDBNull(3))
								profileConnection.UserAAlpha = reader.GetValue(3).ToString();
							else
								profileConnection.UserAAlpha = null;
							if (!reader.IsDBNull(4))
								profileConnection.ModifiedBy = reader.GetValue(4).ToString();
							else
								profileConnection.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								profileConnection.CreatedAt = reader.GetValue(5).ToString();
							else
								profileConnection.CreatedAt = null;
							if (!reader.IsDBNull(6))
								profileConnection.UpdatedAt = reader.GetValue(6).ToString();
							else
								profileConnection.UpdatedAt = null;
							response.Add(profileConnection);
							profileConnection = new ProfileConnectionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ProfileConnectionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ProfileConnectionModel> Create(string connectionString, ProfileConnectionModel profileConnection)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO urel_profile_connections (";
				string values = "(";
				if (!String.IsNullOrEmpty(profileConnection.Id))
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
					cmd.Parameters.AddWithValue("@id", profileConnection.Id);
				}
				if (!String.IsNullOrEmpty(profileConnection.UserIdA))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_id_a` ");
						values = String.Concat(values, ", @userIdA");
					}
					else
					{
						sql = String.Concat(sql, " `user_id_a` ");
						values = String.Concat(values, " @userIdA");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
				}
				if (!String.IsNullOrEmpty(profileConnection.UserIdB))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_id_b` ");
						values = String.Concat(values, ", @userIdB");
					}
					else
					{
						sql = String.Concat(sql, " `user_id_b` ");
						values = String.Concat(values, " @userIdB");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
				}
				if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `user_a_alpha` ");
						values = String.Concat(values, ", @userAAlpha");
					}
					else
					{
						sql = String.Concat(sql, " `user_a_alpha` ");
						values = String.Concat(values, " @userAAlpha");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
				}
				if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
				}
				if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				profileConnection.Id = cmd.LastInsertedId.ToString();
				response.Add(profileConnection);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
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
		public List<ProfileConnectionModel> Create(string connectionString, List<ProfileConnectionModel> profileConnectionList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
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
						for (int i = 0; i < profileConnectionList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO urel_profile_connections (";
								string values = "(";
								if (!String.IsNullOrEmpty(profileConnectionList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, profileConnectionList[i].Id);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UserIdA))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_id_a` ");
										values = String.Concat(values, ", @userIdA" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_id_a` ");
										values = String.Concat(values, "@userIdA" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userIdA" + i, profileConnectionList[i].UserIdA);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UserIdB))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_id_b` ");
										values = String.Concat(values, ", @userIdB" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_id_b` ");
										values = String.Concat(values, "@userIdB" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userIdB" + i, profileConnectionList[i].UserIdB);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UserAAlpha))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `user_a_alpha` ");
										values = String.Concat(values, ", @userAAlpha" + i);
									}

									else
									{
										sql = String.Concat(sql, " `user_a_alpha` ");
										values = String.Concat(values, "@userAAlpha" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnectionList[i].UserAAlpha);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(profileConnectionList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
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

		public List<ProfileConnectionModel> Update(string connectionString, ProfileConnectionModel profileConnection, ProfileConnectionModel profileConnectionWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE urel_profile_connections SET ";
				if (!String.IsNullOrEmpty(profileConnection.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", profileConnection.Id);
				}
				if (!String.IsNullOrEmpty(profileConnection.UserIdA))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id_a` = @userIdA");
					else
						sql = String.Concat(sql, " `user_id_a` = @userIdA");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
				}
				if (!String.IsNullOrEmpty(profileConnection.UserIdB))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id_b` = @userIdB");
					else
						sql = String.Concat(sql, " `user_id_b` = @userIdB");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
				}
				if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_a_alpha` = @userAAlpha");
					else
						sql = String.Concat(sql, " `user_a_alpha` = @userAAlpha");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
				}
				if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
				}
				if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
				}
				if (profileConnectionWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(profileConnectionWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", profileConnectionWhere.Id);
					}
					if (!String.IsNullOrEmpty(profileConnectionWhere.UserIdA))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id_a` = @userIdAWhere");
						else
							sql = String.Concat(sql, " `user_id_a` = @userIdAWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdAWhere", profileConnectionWhere.UserIdA);
					}
					if (!String.IsNullOrEmpty(profileConnectionWhere.UserIdB))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id_b` = @userIdBWhere");
						else
							sql = String.Concat(sql, " `user_id_b` = @userIdBWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdBWhere", profileConnectionWhere.UserIdB);
					}
					if (!String.IsNullOrEmpty(profileConnectionWhere.UserAAlpha))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_a_alpha` = @userAAlphaWhere");
						else
							sql = String.Concat(sql, " `user_a_alpha` = @userAAlphaWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userAAlphaWhere", profileConnectionWhere.UserAAlpha);
					}
					if (!String.IsNullOrEmpty(profileConnectionWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", profileConnectionWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(profileConnectionWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", profileConnectionWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(profileConnectionWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", profileConnectionWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(profileConnection);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
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

		public List<ProfileConnectionModel> Update(string connectionString, List<ProfileConnectionModel> profileConnectionList, List<ProfileConnectionModel> profileConnectionWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
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
						for (int i = 0; i < profileConnectionList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE urel_profile_connections SET ";
								if (!String.IsNullOrEmpty(profileConnectionList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, profileConnectionList[i].Id);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UserIdA))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id_a` = @userIdA" + i);
									else
										sql = String.Concat(sql, " `user_id_a` = @userIdA" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userIdA" + i, profileConnectionList[i].UserIdA);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UserIdB))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id_b` = @userIdB" + i);
									else
										sql = String.Concat(sql, " `user_id_b` = @userIdB" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userIdB" + i, profileConnectionList[i].UserIdB);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UserAAlpha))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_a_alpha` = @userAAlpha" + i);
									else
										sql = String.Concat(sql, " `user_a_alpha` = @userAAlpha" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnectionList[i].UserAAlpha);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnectionList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, profileConnectionList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(profileConnectionList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnectionList[i].UpdatedAt);
								}
								if (profileConnectionWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, profileConnectionWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].UserIdA))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id_a` = @userIdAWhere" + i);
										else
											sql = String.Concat(sql, " `user_id_a` = @userIdAWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdAWhere" + i, profileConnectionWhereList[i].UserIdA);
									}
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].UserIdB))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id_b` = @userIdBWhere" + i);
										else
											sql = String.Concat(sql, " `user_id_b` = @userIdBWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdBWhere" + i, profileConnectionWhereList[i].UserIdB);
									}
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].UserAAlpha))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_a_alpha` = @userAAlphaWhere" + i);
										else
											sql = String.Concat(sql, " `user_a_alpha` = @userAAlphaWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userAAlphaWhere" + i, profileConnectionWhereList[i].UserAAlpha);
									}
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, profileConnectionWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, profileConnectionWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(profileConnectionWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, profileConnectionWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(profileConnectionList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
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

		public List<ProfileConnectionModel> Delete(string connectionString, ProfileConnectionModel profileConnection)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM urel_profile_connections  ";
				if (profileConnection != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(profileConnection.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", profileConnection.Id);
					}
					if (!String.IsNullOrEmpty(profileConnection.UserIdA))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id_a` = @userIdA");
						else
							sql = String.Concat(sql, " `user_id_a` = @userIdA");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdA", profileConnection.UserIdA);
					}
					if (!String.IsNullOrEmpty(profileConnection.UserIdB))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id_b` = @userIdB");
						else
							sql = String.Concat(sql, " `user_id_b` = @userIdB");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdB", profileConnection.UserIdB);
					}
					if (!String.IsNullOrEmpty(profileConnection.UserAAlpha))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_a_alpha` = @userAAlpha");
						else
							sql = String.Concat(sql, " `user_a_alpha` = @userAAlpha");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userAAlpha", profileConnection.UserAAlpha);
					}
					if (!String.IsNullOrEmpty(profileConnection.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", profileConnection.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(profileConnection.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", profileConnection.CreatedAt);
					}
					if (!String.IsNullOrEmpty(profileConnection.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", profileConnection.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(profileConnection);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
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

		public List<ProfileConnectionModel> Delete(string connectionString, List<ProfileConnectionModel> profileConnection)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ProfileConnectionModel> response = new List<ProfileConnectionModel>();
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
						for (int i = 0; i < profileConnection.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM urel_profile_connections  ";
								if (profileConnection[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(profileConnection[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, profileConnection[i].Id);
									}
									if (!String.IsNullOrEmpty(profileConnection[i].UserIdA))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id_a` = @userIdA" + i);
										else
											sql = String.Concat(sql, " `user_id_a` = @userIdA" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdA" + i, profileConnection[i].UserIdA);
									}
									if (!String.IsNullOrEmpty(profileConnection[i].UserIdB))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id_b` = @userIdB" + i);
										else
											sql = String.Concat(sql, " `user_id_b` = @userIdB" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdB" + i, profileConnection[i].UserIdB);
									}
									if (!String.IsNullOrEmpty(profileConnection[i].UserAAlpha))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_a_alpha` = @userAAlpha" + i);
										else
											sql = String.Concat(sql, " `user_a_alpha` = @userAAlpha" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userAAlpha" + i, profileConnection[i].UserAAlpha);
									}
									if (!String.IsNullOrEmpty(profileConnection[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, profileConnection[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(profileConnection[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, profileConnection[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(profileConnection[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, profileConnection[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(profileConnection[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ProfileConnectionModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ProfileConnectionModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ProfileConnectionModel>();
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
