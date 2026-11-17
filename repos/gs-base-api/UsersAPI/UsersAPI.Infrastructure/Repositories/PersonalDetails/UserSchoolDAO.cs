using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using UsersAPI.UsersAPI.Services.PersonalDetails.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.PersonalDetails
{
	class UserSchoolDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, UserSchoolModel> ReadD(string connectionString, UserSchoolModel userSchool, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserSchoolModel> response = new Dictionary<string, UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				{
					if (userSchool != null)
					{
						if (!String.IsNullOrEmpty(userSchool.Id))
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
							cmd.Parameters.AddWithValue("@id", userSchool.Id);
						}
						if (!String.IsNullOrEmpty(userSchool.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
						}
						if (!String.IsNullOrEmpty(userSchool.SchoolId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `school_id` = @schoolId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `school_id` = @schoolId ");
							}
							cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchool.Identifier))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `identifier` = @identifier ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `identifier` = @identifier ");
							}
							cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
						}
						if (!String.IsNullOrEmpty(userSchool.Confirmed))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `confirmed` = @confirmed ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `confirmed` = @confirmed ");
							}
							cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchool.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
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
									userSchool.Id = reader.GetValue(0).ToString();
								else
									userSchool.Id = null;
								if (!reader.IsDBNull(1))
									userSchool.UserId = reader.GetValue(1).ToString();
								else
									userSchool.UserId = null;
								if (!reader.IsDBNull(2))
									userSchool.SchoolId = reader.GetValue(2).ToString();
								else
									userSchool.SchoolId = null;
								if (!reader.IsDBNull(3))
									userSchool.Identifier = reader.GetValue(3).ToString();
								else
									userSchool.Identifier = null;
								if (!reader.IsDBNull(4))
									userSchool.Confirmed = reader.GetValue(4).ToString();
								else
									userSchool.Confirmed = null;
								if (!reader.IsDBNull(5))
									userSchool.ModifiedBy = reader.GetValue(5).ToString();
								else
									userSchool.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									userSchool.CreatedAt = reader.GetValue(6).ToString();
								else
									userSchool.CreatedAt = null;
								if (!reader.IsDBNull(7))
									userSchool.UpdatedAt = reader.GetValue(7).ToString();
								else
									userSchool.UpdatedAt = null;
								response.Add(userSchool.Id, userSchool);
								userSchool = new UserSchoolModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserSchoolModel>();
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
				response = new Dictionary<string, UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserSchoolModel> ReadD(string connectionString, UserSchoolModel userSchool, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserSchoolModel> response = new Dictionary<string, UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				{
					if (userSchool != null)
					{
						if (!String.IsNullOrEmpty(userSchool.Id))
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
							cmd.Parameters.AddWithValue("@id", userSchool.Id);
						}
						if (!String.IsNullOrEmpty(userSchool.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
						}
						if (!String.IsNullOrEmpty(userSchool.SchoolId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `school_id` = @schoolId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `school_id` = @schoolId ");
							}
							cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchool.Identifier))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `identifier` = @identifier ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `identifier` = @identifier ");
							}
							cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
						}
						if (!String.IsNullOrEmpty(userSchool.Confirmed))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `confirmed` = @confirmed ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `confirmed` = @confirmed ");
							}
							cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchool.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
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
									userSchool.Id = reader.GetValue(0).ToString();
								else
									userSchool.Id = null;
								if (!reader.IsDBNull(1))
									userSchool.UserId = reader.GetValue(1).ToString();
								else
									userSchool.UserId = null;
								if (!reader.IsDBNull(2))
									userSchool.SchoolId = reader.GetValue(2).ToString();
								else
									userSchool.SchoolId = null;
								if (!reader.IsDBNull(3))
									userSchool.Identifier = reader.GetValue(3).ToString();
								else
									userSchool.Identifier = null;
								if (!reader.IsDBNull(4))
									userSchool.Confirmed = reader.GetValue(4).ToString();
								else
									userSchool.Confirmed = null;
								if (!reader.IsDBNull(5))
									userSchool.ModifiedBy = reader.GetValue(5).ToString();
								else
									userSchool.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									userSchool.CreatedAt = reader.GetValue(6).ToString();
								else
									userSchool.CreatedAt = null;
								if (!reader.IsDBNull(7))
									userSchool.UpdatedAt = reader.GetValue(7).ToString();
								else
									userSchool.UpdatedAt = null;
								response.Add(userSchool.Id, userSchool);
								userSchool = new UserSchoolModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, UserSchoolModel>();
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
				response = new Dictionary<string, UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserSchoolModel> ReadD(string connectionString, List<UserSchoolModel> userSchoolWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserSchoolModel> response = new Dictionary<string, UserSchoolModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				if (userSchoolWhere != null && userSchoolWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userSchoolWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userSchoolWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userSchoolWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].SchoolId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `school_id` = @schoolId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@schoolId" + i, userSchoolWhere[i].SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Identifier))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `identifier` = @identifier" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@identifier" + i, userSchoolWhere[i].Identifier);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Confirmed))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `confirmed` = @confirmed" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@confirmed" + i, userSchoolWhere[i].Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchoolWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userSchoolWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userSchoolWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userSchoolWhere.Count - 1)
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
							UserSchoolModel userSchool = new UserSchoolModel(); if (!reader.IsDBNull(0))
								userSchool.Id = reader.GetValue(0).ToString();
							else
								userSchool.Id = null;
							if (!reader.IsDBNull(1))
								userSchool.UserId = reader.GetValue(1).ToString();
							else
								userSchool.UserId = null;
							if (!reader.IsDBNull(2))
								userSchool.SchoolId = reader.GetValue(2).ToString();
							else
								userSchool.SchoolId = null;
							if (!reader.IsDBNull(3))
								userSchool.Identifier = reader.GetValue(3).ToString();
							else
								userSchool.Identifier = null;
							if (!reader.IsDBNull(4))
								userSchool.Confirmed = reader.GetValue(4).ToString();
							else
								userSchool.Confirmed = null;
							if (!reader.IsDBNull(5))
								userSchool.ModifiedBy = reader.GetValue(5).ToString();
							else
								userSchool.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								userSchool.CreatedAt = reader.GetValue(6).ToString();
							else
								userSchool.CreatedAt = null;
							if (!reader.IsDBNull(7))
								userSchool.UpdatedAt = reader.GetValue(7).ToString();
							else
								userSchool.UpdatedAt = null;
							response.Add(userSchool.Id, userSchool);
							userSchool = new UserSchoolModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserSchoolModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, UserSchoolModel> ReadD(string connectionString, List<UserSchoolModel> userSchoolWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, UserSchoolModel> response = new Dictionary<string, UserSchoolModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				if (userSchoolWhere != null && userSchoolWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userSchoolWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userSchoolWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userSchoolWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].SchoolId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `school_id` = @schoolId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@schoolId" + i, userSchoolWhere[i].SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Identifier))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `identifier` = @identifier" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@identifier" + i, userSchoolWhere[i].Identifier);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Confirmed))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `confirmed` = @confirmed" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@confirmed" + i, userSchoolWhere[i].Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchoolWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userSchoolWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userSchoolWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userSchoolWhere.Count - 1)
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
							UserSchoolModel userSchool = new UserSchoolModel(); if (!reader.IsDBNull(0))
								userSchool.Id = reader.GetValue(0).ToString();
							else
								userSchool.Id = null;
							if (!reader.IsDBNull(1))
								userSchool.UserId = reader.GetValue(1).ToString();
							else
								userSchool.UserId = null;
							if (!reader.IsDBNull(2))
								userSchool.SchoolId = reader.GetValue(2).ToString();
							else
								userSchool.SchoolId = null;
							if (!reader.IsDBNull(3))
								userSchool.Identifier = reader.GetValue(3).ToString();
							else
								userSchool.Identifier = null;
							if (!reader.IsDBNull(4))
								userSchool.Confirmed = reader.GetValue(4).ToString();
							else
								userSchool.Confirmed = null;
							if (!reader.IsDBNull(5))
								userSchool.ModifiedBy = reader.GetValue(5).ToString();
							else
								userSchool.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								userSchool.CreatedAt = reader.GetValue(6).ToString();
							else
								userSchool.CreatedAt = null;
							if (!reader.IsDBNull(7))
								userSchool.UpdatedAt = reader.GetValue(7).ToString();
							else
								userSchool.UpdatedAt = null;
							response.Add(userSchool.Id, userSchool);
							userSchool = new UserSchoolModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, UserSchoolModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserSchoolModel> Read(string connectionString, UserSchoolModel userSchool, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				{
					if (userSchool != null)
					{
						if (!String.IsNullOrEmpty(userSchool.Id))
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
							cmd.Parameters.AddWithValue("@id", userSchool.Id);
						}
						if (!String.IsNullOrEmpty(userSchool.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
						}
						if (!String.IsNullOrEmpty(userSchool.SchoolId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `school_id` = @schoolId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `school_id` = @schoolId ");
							}
							cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchool.Identifier))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `identifier` = @identifier ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `identifier` = @identifier ");
							}
							cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
						}
						if (!String.IsNullOrEmpty(userSchool.Confirmed))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `confirmed` = @confirmed ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `confirmed` = @confirmed ");
							}
							cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchool.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
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
									userSchool.Id = reader.GetValue(0).ToString();
								else
									userSchool.Id = null;
								if (!reader.IsDBNull(1))
									userSchool.UserId = reader.GetValue(1).ToString();
								else
									userSchool.UserId = null;
								if (!reader.IsDBNull(2))
									userSchool.SchoolId = reader.GetValue(2).ToString();
								else
									userSchool.SchoolId = null;
								if (!reader.IsDBNull(3))
									userSchool.Identifier = reader.GetValue(3).ToString();
								else
									userSchool.Identifier = null;
								if (!reader.IsDBNull(4))
									userSchool.Confirmed = reader.GetValue(4).ToString();
								else
									userSchool.Confirmed = null;
								if (!reader.IsDBNull(5))
									userSchool.ModifiedBy = reader.GetValue(5).ToString();
								else
									userSchool.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									userSchool.CreatedAt = reader.GetValue(6).ToString();
								else
									userSchool.CreatedAt = null;
								if (!reader.IsDBNull(7))
									userSchool.UpdatedAt = reader.GetValue(7).ToString();
								else
									userSchool.UpdatedAt = null;
								response.Add(userSchool);
								userSchool = new UserSchoolModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserSchoolModel>();
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
				response = new List<UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserSchoolModel> Read(string connectionString, UserSchoolModel userSchool, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				{
					if (userSchool != null)
					{
						if (!String.IsNullOrEmpty(userSchool.Id))
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
							cmd.Parameters.AddWithValue("@id", userSchool.Id);
						}
						if (!String.IsNullOrEmpty(userSchool.UserId))
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
							cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
						}
						if (!String.IsNullOrEmpty(userSchool.SchoolId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `school_id` = @schoolId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `school_id` = @schoolId ");
							}
							cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchool.Identifier))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `identifier` = @identifier ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `identifier` = @identifier ");
							}
							cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
						}
						if (!String.IsNullOrEmpty(userSchool.Confirmed))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `confirmed` = @confirmed ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `confirmed` = @confirmed ");
							}
							cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchool.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
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
									userSchool.Id = reader.GetValue(0).ToString();
								else
									userSchool.Id = null;
								if (!reader.IsDBNull(1))
									userSchool.UserId = reader.GetValue(1).ToString();
								else
									userSchool.UserId = null;
								if (!reader.IsDBNull(2))
									userSchool.SchoolId = reader.GetValue(2).ToString();
								else
									userSchool.SchoolId = null;
								if (!reader.IsDBNull(3))
									userSchool.Identifier = reader.GetValue(3).ToString();
								else
									userSchool.Identifier = null;
								if (!reader.IsDBNull(4))
									userSchool.Confirmed = reader.GetValue(4).ToString();
								else
									userSchool.Confirmed = null;
								if (!reader.IsDBNull(5))
									userSchool.ModifiedBy = reader.GetValue(5).ToString();
								else
									userSchool.ModifiedBy = null;
								if (!reader.IsDBNull(6))
									userSchool.CreatedAt = reader.GetValue(6).ToString();
								else
									userSchool.CreatedAt = null;
								if (!reader.IsDBNull(7))
									userSchool.UpdatedAt = reader.GetValue(7).ToString();
								else
									userSchool.UpdatedAt = null;
								response.Add(userSchool);
								userSchool = new UserSchoolModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<UserSchoolModel>();
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
				response = new List<UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserSchoolModel> Read(string connectionString, List<UserSchoolModel> userSchoolWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				if (userSchoolWhere != null && userSchoolWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userSchoolWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userSchoolWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userSchoolWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].SchoolId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `school_id` = @schoolId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@schoolId" + i, userSchoolWhere[i].SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Identifier))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `identifier` = @identifier" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@identifier" + i, userSchoolWhere[i].Identifier);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Confirmed))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `confirmed` = @confirmed" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@confirmed" + i, userSchoolWhere[i].Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchoolWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userSchoolWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userSchoolWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userSchoolWhere.Count - 1)
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
							UserSchoolModel userSchool = new UserSchoolModel(); if (!reader.IsDBNull(0))
								userSchool.Id = reader.GetValue(0).ToString();
							else
								userSchool.Id = null;
							if (!reader.IsDBNull(1))
								userSchool.UserId = reader.GetValue(1).ToString();
							else
								userSchool.UserId = null;
							if (!reader.IsDBNull(2))
								userSchool.SchoolId = reader.GetValue(2).ToString();
							else
								userSchool.SchoolId = null;
							if (!reader.IsDBNull(3))
								userSchool.Identifier = reader.GetValue(3).ToString();
							else
								userSchool.Identifier = null;
							if (!reader.IsDBNull(4))
								userSchool.Confirmed = reader.GetValue(4).ToString();
							else
								userSchool.Confirmed = null;
							if (!reader.IsDBNull(5))
								userSchool.ModifiedBy = reader.GetValue(5).ToString();
							else
								userSchool.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								userSchool.CreatedAt = reader.GetValue(6).ToString();
							else
								userSchool.CreatedAt = null;
							if (!reader.IsDBNull(7))
								userSchool.UpdatedAt = reader.GetValue(7).ToString();
							else
								userSchool.UpdatedAt = null;
							response.Add(userSchool);
							userSchool = new UserSchoolModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserSchoolModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserSchoolModel> Read(string connectionString, List<UserSchoolModel> userSchoolWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `user_id`, `school_id`, `identifier`, `confirmed`, `modified_by`, `created_at`, `updated_at`  FROM upd_user_schools ";
				if (userSchoolWhere != null && userSchoolWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < userSchoolWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, userSchoolWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, userSchoolWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].SchoolId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `school_id` = @schoolId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@schoolId" + i, userSchoolWhere[i].SchoolId);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Identifier))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `identifier` = @identifier" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@identifier" + i, userSchoolWhere[i].Identifier);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].Confirmed))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `confirmed` = @confirmed" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@confirmed" + i, userSchoolWhere[i].Confirmed);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchoolWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, userSchoolWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(userSchoolWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, userSchoolWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != userSchoolWhere.Count - 1)
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
							UserSchoolModel userSchool = new UserSchoolModel(); if (!reader.IsDBNull(0))
								userSchool.Id = reader.GetValue(0).ToString();
							else
								userSchool.Id = null;
							if (!reader.IsDBNull(1))
								userSchool.UserId = reader.GetValue(1).ToString();
							else
								userSchool.UserId = null;
							if (!reader.IsDBNull(2))
								userSchool.SchoolId = reader.GetValue(2).ToString();
							else
								userSchool.SchoolId = null;
							if (!reader.IsDBNull(3))
								userSchool.Identifier = reader.GetValue(3).ToString();
							else
								userSchool.Identifier = null;
							if (!reader.IsDBNull(4))
								userSchool.Confirmed = reader.GetValue(4).ToString();
							else
								userSchool.Confirmed = null;
							if (!reader.IsDBNull(5))
								userSchool.ModifiedBy = reader.GetValue(5).ToString();
							else
								userSchool.ModifiedBy = null;
							if (!reader.IsDBNull(6))
								userSchool.CreatedAt = reader.GetValue(6).ToString();
							else
								userSchool.CreatedAt = null;
							if (!reader.IsDBNull(7))
								userSchool.UpdatedAt = reader.GetValue(7).ToString();
							else
								userSchool.UpdatedAt = null;
							response.Add(userSchool);
							userSchool = new UserSchoolModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<UserSchoolModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<UserSchoolModel> Create(string connectionString, UserSchoolModel userSchool)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO upd_user_schools (";
				string values = "(";
				if (!String.IsNullOrEmpty(userSchool.Id))
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
					cmd.Parameters.AddWithValue("@id", userSchool.Id);
				}
				if (!String.IsNullOrEmpty(userSchool.UserId))
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
					cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
				}
				if (!String.IsNullOrEmpty(userSchool.SchoolId))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `school_id` ");
						values = String.Concat(values, ", @schoolId");
					}
					else
					{
						sql = String.Concat(sql, " `school_id` ");
						values = String.Concat(values, " @schoolId");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
				}
				if (!String.IsNullOrEmpty(userSchool.Identifier))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `identifier` ");
						values = String.Concat(values, ", @identifier");
					}
					else
					{
						sql = String.Concat(sql, " `identifier` ");
						values = String.Concat(values, " @identifier");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
				}
				if (!String.IsNullOrEmpty(userSchool.Confirmed))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `confirmed` ");
						values = String.Concat(values, ", @confirmed");
					}
					else
					{
						sql = String.Concat(sql, " `confirmed` ");
						values = String.Concat(values, " @confirmed");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
				}
				if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userSchool.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				userSchool.Id = cmd.LastInsertedId.ToString();
				response.Add(userSchool);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
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
		public List<UserSchoolModel> Create(string connectionString, List<UserSchoolModel> userSchoolList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
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
						for (int i = 0; i < userSchoolList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO upd_user_schools (";
								string values = "(";
								if (!String.IsNullOrEmpty(userSchoolList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, userSchoolList[i].Id);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].UserId))
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
									cmd.Parameters.AddWithValue("@userId" + i, userSchoolList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].SchoolId))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `school_id` ");
										values = String.Concat(values, ", @schoolId" + i);
									}

									else
									{
										sql = String.Concat(sql, " `school_id` ");
										values = String.Concat(values, "@schoolId" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@schoolId" + i, userSchoolList[i].SchoolId);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].Identifier))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `identifier` ");
										values = String.Concat(values, ", @identifier" + i);
									}

									else
									{
										sql = String.Concat(sql, " `identifier` ");
										values = String.Concat(values, "@identifier" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@identifier" + i, userSchoolList[i].Identifier);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].Confirmed))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `confirmed` ");
										values = String.Concat(values, ", @confirmed" + i);
									}

									else
									{
										sql = String.Concat(sql, " `confirmed` ");
										values = String.Concat(values, "@confirmed" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@confirmed" + i, userSchoolList[i].Confirmed);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchoolList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, userSchoolList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, userSchoolList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(userSchoolList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<UserSchoolModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserSchoolModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
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

		public List<UserSchoolModel> Update(string connectionString, UserSchoolModel userSchool, UserSchoolModel userSchoolWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE upd_user_schools SET ";
				if (!String.IsNullOrEmpty(userSchool.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", userSchool.Id);
				}
				if (!String.IsNullOrEmpty(userSchool.UserId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id` = @userId");
					else
						sql = String.Concat(sql, " `user_id` = @userId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
				}
				if (!String.IsNullOrEmpty(userSchool.SchoolId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `school_id` = @schoolId");
					else
						sql = String.Concat(sql, " `school_id` = @schoolId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
				}
				if (!String.IsNullOrEmpty(userSchool.Identifier))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `identifier` = @identifier");
					else
						sql = String.Concat(sql, " `identifier` = @identifier");

					addAnd = true;
					cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
				}
				if (!String.IsNullOrEmpty(userSchool.Confirmed))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `confirmed` = @confirmed");
					else
						sql = String.Concat(sql, " `confirmed` = @confirmed");

					addAnd = true;
					cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
				}
				if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(userSchool.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
				}
				if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
				}
				if (userSchoolWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(userSchoolWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", userSchoolWhere.Id);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
						else
							sql = String.Concat(sql, " `user_id` = @userIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdWhere", userSchoolWhere.UserId);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.SchoolId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `school_id` = @schoolIdWhere");
						else
							sql = String.Concat(sql, " `school_id` = @schoolIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@schoolIdWhere", userSchoolWhere.SchoolId);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.Identifier))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `identifier` = @identifierWhere");
						else
							sql = String.Concat(sql, " `identifier` = @identifierWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@identifierWhere", userSchoolWhere.Identifier);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.Confirmed))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `confirmed` = @confirmedWhere");
						else
							sql = String.Concat(sql, " `confirmed` = @confirmedWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@confirmedWhere", userSchoolWhere.Confirmed);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", userSchoolWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", userSchoolWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userSchoolWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", userSchoolWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(userSchool);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
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

		public List<UserSchoolModel> Update(string connectionString, List<UserSchoolModel> userSchoolList, List<UserSchoolModel> userSchoolWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
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
						for (int i = 0; i < userSchoolList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE upd_user_schools SET ";
								if (!String.IsNullOrEmpty(userSchoolList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, userSchoolList[i].Id);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].UserId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id` = @userId" + i);
									else
										sql = String.Concat(sql, " `user_id` = @userId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, userSchoolList[i].UserId);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].SchoolId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `school_id` = @schoolId" + i);
									else
										sql = String.Concat(sql, " `school_id` = @schoolId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@schoolId" + i, userSchoolList[i].SchoolId);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].Identifier))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `identifier` = @identifier" + i);
									else
										sql = String.Concat(sql, " `identifier` = @identifier" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@identifier" + i, userSchoolList[i].Identifier);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].Confirmed))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `confirmed` = @confirmed" + i);
									else
										sql = String.Concat(sql, " `confirmed` = @confirmed" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@confirmed" + i, userSchoolList[i].Confirmed);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchoolList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, userSchoolList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(userSchoolList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, userSchoolList[i].UpdatedAt);
								}
								if (userSchoolWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, userSchoolWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdWhere" + i, userSchoolWhereList[i].UserId);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].SchoolId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `school_id` = @schoolIdWhere" + i);
										else
											sql = String.Concat(sql, " `school_id` = @schoolIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@schoolIdWhere" + i, userSchoolWhereList[i].SchoolId);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].Identifier))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `identifier` = @identifierWhere" + i);
										else
											sql = String.Concat(sql, " `identifier` = @identifierWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@identifierWhere" + i, userSchoolWhereList[i].Identifier);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].Confirmed))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `confirmed` = @confirmedWhere" + i);
										else
											sql = String.Concat(sql, " `confirmed` = @confirmedWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@confirmedWhere" + i, userSchoolWhereList[i].Confirmed);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userSchoolWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, userSchoolWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userSchoolWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userSchoolWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userSchoolList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserSchoolModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserSchoolModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
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

		public List<UserSchoolModel> Delete(string connectionString, UserSchoolModel userSchool)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM upd_user_schools  ";
				if (userSchool != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(userSchool.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", userSchool.Id);
					}
					if (!String.IsNullOrEmpty(userSchool.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userId");
						else
							sql = String.Concat(sql, " `user_id` = @userId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userId", userSchool.UserId);
					}
					if (!String.IsNullOrEmpty(userSchool.SchoolId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `school_id` = @schoolId");
						else
							sql = String.Concat(sql, " `school_id` = @schoolId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@schoolId", userSchool.SchoolId);
					}
					if (!String.IsNullOrEmpty(userSchool.Identifier))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `identifier` = @identifier");
						else
							sql = String.Concat(sql, " `identifier` = @identifier");

						addAnd = true;
						cmd.Parameters.AddWithValue("@identifier", userSchool.Identifier);
					}
					if (!String.IsNullOrEmpty(userSchool.Confirmed))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `confirmed` = @confirmed");
						else
							sql = String.Concat(sql, " `confirmed` = @confirmed");

						addAnd = true;
						cmd.Parameters.AddWithValue("@confirmed", userSchool.Confirmed);
					}
					if (!String.IsNullOrEmpty(userSchool.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", userSchool.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(userSchool.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", userSchool.CreatedAt);
					}
					if (!String.IsNullOrEmpty(userSchool.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", userSchool.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(userSchool);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
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

		public List<UserSchoolModel> Delete(string connectionString, List<UserSchoolModel> userSchool)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<UserSchoolModel> response = new List<UserSchoolModel>();
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
						for (int i = 0; i < userSchool.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM upd_user_schools  ";
								if (userSchool[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(userSchool[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, userSchool[i].Id);
									}
									if (!String.IsNullOrEmpty(userSchool[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userId" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userId" + i, userSchool[i].UserId);
									}
									if (!String.IsNullOrEmpty(userSchool[i].SchoolId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `school_id` = @schoolId" + i);
										else
											sql = String.Concat(sql, " `school_id` = @schoolId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@schoolId" + i, userSchool[i].SchoolId);
									}
									if (!String.IsNullOrEmpty(userSchool[i].Identifier))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `identifier` = @identifier" + i);
										else
											sql = String.Concat(sql, " `identifier` = @identifier" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@identifier" + i, userSchool[i].Identifier);
									}
									if (!String.IsNullOrEmpty(userSchool[i].Confirmed))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `confirmed` = @confirmed" + i);
										else
											sql = String.Concat(sql, " `confirmed` = @confirmed" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@confirmed" + i, userSchool[i].Confirmed);
									}
									if (!String.IsNullOrEmpty(userSchool[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, userSchool[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(userSchool[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, userSchool[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(userSchool[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, userSchool[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(userSchool[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<UserSchoolModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<UserSchoolModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<UserSchoolModel>();
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
