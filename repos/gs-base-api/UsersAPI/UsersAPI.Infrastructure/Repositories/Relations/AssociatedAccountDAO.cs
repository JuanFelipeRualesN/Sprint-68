using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Relations;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Relations
{
	class AssociatedAccountDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, AssociatedAccountModel> ReadD(string connectionString, AssociatedAccountModel associatedAccount, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, AssociatedAccountModel> response = new Dictionary<string, AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				{
					if (associatedAccount != null)
					{
						if (!String.IsNullOrEmpty(associatedAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
						}
						if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_user_account` = @mainUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_user_account` = @mainUserAccount ");
							}
							cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `profile_user_account` = @profileUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `profile_user_account` = @profileUserAccount ");
							}
							cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.Available))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `available` = @available ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `available` = @available ");
							}
							cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
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
									associatedAccount.Id = reader.GetValue(0).ToString();
								else
									associatedAccount.Id = null;
								if (!reader.IsDBNull(1))
									associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
								else
									associatedAccount.MainUserAccount = null;
								if (!reader.IsDBNull(2))
									associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
								else
									associatedAccount.ProfileUserAccount = null;
								if (!reader.IsDBNull(3))
									associatedAccount.Available = reader.GetValue(3).ToString();
								else
									associatedAccount.Available = null;
								if (!reader.IsDBNull(4))
									associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									associatedAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									associatedAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									associatedAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									associatedAccount.UpdatedAt = null;
								response.Add(associatedAccount.Id, associatedAccount);
								associatedAccount = new AssociatedAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, AssociatedAccountModel>();
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
				response = new Dictionary<string, AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, AssociatedAccountModel> ReadD(string connectionString, AssociatedAccountModel associatedAccount, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, AssociatedAccountModel> response = new Dictionary<string, AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				{
					if (associatedAccount != null)
					{
						if (!String.IsNullOrEmpty(associatedAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
						}
						if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_user_account` = @mainUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_user_account` = @mainUserAccount ");
							}
							cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `profile_user_account` = @profileUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `profile_user_account` = @profileUserAccount ");
							}
							cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.Available))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `available` = @available ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `available` = @available ");
							}
							cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
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
									associatedAccount.Id = reader.GetValue(0).ToString();
								else
									associatedAccount.Id = null;
								if (!reader.IsDBNull(1))
									associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
								else
									associatedAccount.MainUserAccount = null;
								if (!reader.IsDBNull(2))
									associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
								else
									associatedAccount.ProfileUserAccount = null;
								if (!reader.IsDBNull(3))
									associatedAccount.Available = reader.GetValue(3).ToString();
								else
									associatedAccount.Available = null;
								if (!reader.IsDBNull(4))
									associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									associatedAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									associatedAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									associatedAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									associatedAccount.UpdatedAt = null;
								response.Add(associatedAccount.Id, associatedAccount);
								associatedAccount = new AssociatedAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, AssociatedAccountModel>();
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
				response = new Dictionary<string, AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, AssociatedAccountModel> ReadD(string connectionString, List<AssociatedAccountModel> associatedAccountWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, AssociatedAccountModel> response = new Dictionary<string, AssociatedAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				if (associatedAccountWhere != null && associatedAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < associatedAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, associatedAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].MainUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_user_account` = @mainUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccountWhere[i].MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ProfileUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `profile_user_account` = @profileUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccountWhere[i].ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Available))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `available` = @available" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@available" + i, associatedAccountWhere[i].Available);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != associatedAccountWhere.Count - 1)
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
							AssociatedAccountModel associatedAccount = new AssociatedAccountModel(); if (!reader.IsDBNull(0))
								associatedAccount.Id = reader.GetValue(0).ToString();
							else
								associatedAccount.Id = null;
							if (!reader.IsDBNull(1))
								associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
							else
								associatedAccount.MainUserAccount = null;
							if (!reader.IsDBNull(2))
								associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
							else
								associatedAccount.ProfileUserAccount = null;
							if (!reader.IsDBNull(3))
								associatedAccount.Available = reader.GetValue(3).ToString();
							else
								associatedAccount.Available = null;
							if (!reader.IsDBNull(4))
								associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								associatedAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								associatedAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								associatedAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								associatedAccount.UpdatedAt = null;
							response.Add(associatedAccount.Id, associatedAccount);
							associatedAccount = new AssociatedAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, AssociatedAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, AssociatedAccountModel> ReadD(string connectionString, List<AssociatedAccountModel> associatedAccountWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, AssociatedAccountModel> response = new Dictionary<string, AssociatedAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				if (associatedAccountWhere != null && associatedAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < associatedAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, associatedAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].MainUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_user_account` = @mainUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccountWhere[i].MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ProfileUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `profile_user_account` = @profileUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccountWhere[i].ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Available))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `available` = @available" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@available" + i, associatedAccountWhere[i].Available);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != associatedAccountWhere.Count - 1)
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
							AssociatedAccountModel associatedAccount = new AssociatedAccountModel(); if (!reader.IsDBNull(0))
								associatedAccount.Id = reader.GetValue(0).ToString();
							else
								associatedAccount.Id = null;
							if (!reader.IsDBNull(1))
								associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
							else
								associatedAccount.MainUserAccount = null;
							if (!reader.IsDBNull(2))
								associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
							else
								associatedAccount.ProfileUserAccount = null;
							if (!reader.IsDBNull(3))
								associatedAccount.Available = reader.GetValue(3).ToString();
							else
								associatedAccount.Available = null;
							if (!reader.IsDBNull(4))
								associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								associatedAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								associatedAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								associatedAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								associatedAccount.UpdatedAt = null;
							response.Add(associatedAccount.Id, associatedAccount);
							associatedAccount = new AssociatedAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, AssociatedAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<AssociatedAccountModel> Read(string connectionString, AssociatedAccountModel associatedAccount, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				{
					if (associatedAccount != null)
					{
						if (!String.IsNullOrEmpty(associatedAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
						}
						if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_user_account` = @mainUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_user_account` = @mainUserAccount ");
							}
							cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `profile_user_account` = @profileUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `profile_user_account` = @profileUserAccount ");
							}
							cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.Available))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `available` = @available ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `available` = @available ");
							}
							cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
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
									associatedAccount.Id = reader.GetValue(0).ToString();
								else
									associatedAccount.Id = null;
								if (!reader.IsDBNull(1))
									associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
								else
									associatedAccount.MainUserAccount = null;
								if (!reader.IsDBNull(2))
									associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
								else
									associatedAccount.ProfileUserAccount = null;
								if (!reader.IsDBNull(3))
									associatedAccount.Available = reader.GetValue(3).ToString();
								else
									associatedAccount.Available = null;
								if (!reader.IsDBNull(4))
									associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									associatedAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									associatedAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									associatedAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									associatedAccount.UpdatedAt = null;
								response.Add(associatedAccount);
								associatedAccount = new AssociatedAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<AssociatedAccountModel>();
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
				response = new List<AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<AssociatedAccountModel> Read(string connectionString, AssociatedAccountModel associatedAccount, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				{
					if (associatedAccount != null)
					{
						if (!String.IsNullOrEmpty(associatedAccount.Id))
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
							cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
						}
						if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `main_user_account` = @mainUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `main_user_account` = @mainUserAccount ");
							}
							cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `profile_user_account` = @profileUserAccount ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `profile_user_account` = @profileUserAccount ");
							}
							cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccount.Available))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `available` = @available ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `available` = @available ");
							}
							cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
						}
						if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
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
									associatedAccount.Id = reader.GetValue(0).ToString();
								else
									associatedAccount.Id = null;
								if (!reader.IsDBNull(1))
									associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
								else
									associatedAccount.MainUserAccount = null;
								if (!reader.IsDBNull(2))
									associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
								else
									associatedAccount.ProfileUserAccount = null;
								if (!reader.IsDBNull(3))
									associatedAccount.Available = reader.GetValue(3).ToString();
								else
									associatedAccount.Available = null;
								if (!reader.IsDBNull(4))
									associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
								else
									associatedAccount.ModifiedBy = null;
								if (!reader.IsDBNull(5))
									associatedAccount.CreatedAt = reader.GetValue(5).ToString();
								else
									associatedAccount.CreatedAt = null;
								if (!reader.IsDBNull(6))
									associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
								else
									associatedAccount.UpdatedAt = null;
								response.Add(associatedAccount);
								associatedAccount = new AssociatedAccountModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<AssociatedAccountModel>();
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
				response = new List<AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<AssociatedAccountModel> Read(string connectionString, List<AssociatedAccountModel> associatedAccountWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				if (associatedAccountWhere != null && associatedAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < associatedAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, associatedAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].MainUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_user_account` = @mainUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccountWhere[i].MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ProfileUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `profile_user_account` = @profileUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccountWhere[i].ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Available))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `available` = @available" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@available" + i, associatedAccountWhere[i].Available);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != associatedAccountWhere.Count - 1)
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
							AssociatedAccountModel associatedAccount = new AssociatedAccountModel(); if (!reader.IsDBNull(0))
								associatedAccount.Id = reader.GetValue(0).ToString();
							else
								associatedAccount.Id = null;
							if (!reader.IsDBNull(1))
								associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
							else
								associatedAccount.MainUserAccount = null;
							if (!reader.IsDBNull(2))
								associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
							else
								associatedAccount.ProfileUserAccount = null;
							if (!reader.IsDBNull(3))
								associatedAccount.Available = reader.GetValue(3).ToString();
							else
								associatedAccount.Available = null;
							if (!reader.IsDBNull(4))
								associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								associatedAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								associatedAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								associatedAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								associatedAccount.UpdatedAt = null;
							response.Add(associatedAccount);
							associatedAccount = new AssociatedAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<AssociatedAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<AssociatedAccountModel> Read(string connectionString, List<AssociatedAccountModel> associatedAccountWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `main_user_account`, `profile_user_account`, `available`, `modified_by`, `created_at`, `updated_at`  FROM urel_associated_accounts ";
				if (associatedAccountWhere != null && associatedAccountWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < associatedAccountWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, associatedAccountWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].MainUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `main_user_account` = @mainUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccountWhere[i].MainUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ProfileUserAccount))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `profile_user_account` = @profileUserAccount" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccountWhere[i].ProfileUserAccount);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].Available))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `available` = @available" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@available" + i, associatedAccountWhere[i].Available);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccountWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccountWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(associatedAccountWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccountWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != associatedAccountWhere.Count - 1)
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
							AssociatedAccountModel associatedAccount = new AssociatedAccountModel(); if (!reader.IsDBNull(0))
								associatedAccount.Id = reader.GetValue(0).ToString();
							else
								associatedAccount.Id = null;
							if (!reader.IsDBNull(1))
								associatedAccount.MainUserAccount = reader.GetValue(1).ToString();
							else
								associatedAccount.MainUserAccount = null;
							if (!reader.IsDBNull(2))
								associatedAccount.ProfileUserAccount = reader.GetValue(2).ToString();
							else
								associatedAccount.ProfileUserAccount = null;
							if (!reader.IsDBNull(3))
								associatedAccount.Available = reader.GetValue(3).ToString();
							else
								associatedAccount.Available = null;
							if (!reader.IsDBNull(4))
								associatedAccount.ModifiedBy = reader.GetValue(4).ToString();
							else
								associatedAccount.ModifiedBy = null;
							if (!reader.IsDBNull(5))
								associatedAccount.CreatedAt = reader.GetValue(5).ToString();
							else
								associatedAccount.CreatedAt = null;
							if (!reader.IsDBNull(6))
								associatedAccount.UpdatedAt = reader.GetValue(6).ToString();
							else
								associatedAccount.UpdatedAt = null;
							response.Add(associatedAccount);
							associatedAccount = new AssociatedAccountModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<AssociatedAccountModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<AssociatedAccountModel> Create(string connectionString, AssociatedAccountModel associatedAccount)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO urel_associated_accounts (";
				string values = "(";
				if (!String.IsNullOrEmpty(associatedAccount.Id))
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
					cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
				}
				if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `main_user_account` ");
						values = String.Concat(values, ", @mainUserAccount");
					}
					else
					{
						sql = String.Concat(sql, " `main_user_account` ");
						values = String.Concat(values, " @mainUserAccount");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
				}
				if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `profile_user_account` ");
						values = String.Concat(values, ", @profileUserAccount");
					}
					else
					{
						sql = String.Concat(sql, " `profile_user_account` ");
						values = String.Concat(values, " @profileUserAccount");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
				}
				if (!String.IsNullOrEmpty(associatedAccount.Available))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `available` ");
						values = String.Concat(values, ", @available");
					}
					else
					{
						sql = String.Concat(sql, " `available` ");
						values = String.Concat(values, " @available");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
				}
				if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
				}
				if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				associatedAccount.Id = cmd.LastInsertedId.ToString();
				response.Add(associatedAccount);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
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
		public List<AssociatedAccountModel> Create(string connectionString, List<AssociatedAccountModel> associatedAccountList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
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
						for (int i = 0; i < associatedAccountList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO urel_associated_accounts (";
								string values = "(";
								if (!String.IsNullOrEmpty(associatedAccountList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, associatedAccountList[i].Id);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].MainUserAccount))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `main_user_account` ");
										values = String.Concat(values, ", @mainUserAccount" + i);
									}

									else
									{
										sql = String.Concat(sql, " `main_user_account` ");
										values = String.Concat(values, "@mainUserAccount" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccountList[i].MainUserAccount);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].ProfileUserAccount))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `profile_user_account` ");
										values = String.Concat(values, ", @profileUserAccount" + i);
									}

									else
									{
										sql = String.Concat(sql, " `profile_user_account` ");
										values = String.Concat(values, "@profileUserAccount" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccountList[i].ProfileUserAccount);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].Available))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `available` ");
										values = String.Concat(values, ", @available" + i);
									}

									else
									{
										sql = String.Concat(sql, " `available` ");
										values = String.Concat(values, "@available" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@available" + i, associatedAccountList[i].Available);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccountList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccountList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccountList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(associatedAccountList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<AssociatedAccountModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<AssociatedAccountModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
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

		public List<AssociatedAccountModel> Update(string connectionString, AssociatedAccountModel associatedAccount, AssociatedAccountModel associatedAccountWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE urel_associated_accounts SET ";
				if (!String.IsNullOrEmpty(associatedAccount.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
				}
				if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `main_user_account` = @mainUserAccount");
					else
						sql = String.Concat(sql, " `main_user_account` = @mainUserAccount");

					addAnd = true;
					cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
				}
				if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `profile_user_account` = @profileUserAccount");
					else
						sql = String.Concat(sql, " `profile_user_account` = @profileUserAccount");

					addAnd = true;
					cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
				}
				if (!String.IsNullOrEmpty(associatedAccount.Available))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `available` = @available");
					else
						sql = String.Concat(sql, " `available` = @available");

					addAnd = true;
					cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
				}
				if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
				}
				if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
				}
				if (associatedAccountWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(associatedAccountWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", associatedAccountWhere.Id);
					}
					if (!String.IsNullOrEmpty(associatedAccountWhere.MainUserAccount))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `main_user_account` = @mainUserAccountWhere");
						else
							sql = String.Concat(sql, " `main_user_account` = @mainUserAccountWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@mainUserAccountWhere", associatedAccountWhere.MainUserAccount);
					}
					if (!String.IsNullOrEmpty(associatedAccountWhere.ProfileUserAccount))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `profile_user_account` = @profileUserAccountWhere");
						else
							sql = String.Concat(sql, " `profile_user_account` = @profileUserAccountWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@profileUserAccountWhere", associatedAccountWhere.ProfileUserAccount);
					}
					if (!String.IsNullOrEmpty(associatedAccountWhere.Available))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `available` = @availableWhere");
						else
							sql = String.Concat(sql, " `available` = @availableWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@availableWhere", associatedAccountWhere.Available);
					}
					if (!String.IsNullOrEmpty(associatedAccountWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", associatedAccountWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(associatedAccountWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", associatedAccountWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(associatedAccountWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", associatedAccountWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(associatedAccount);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
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

		public List<AssociatedAccountModel> Update(string connectionString, List<AssociatedAccountModel> associatedAccountList, List<AssociatedAccountModel> associatedAccountWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
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
						for (int i = 0; i < associatedAccountList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE urel_associated_accounts SET ";
								if (!String.IsNullOrEmpty(associatedAccountList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, associatedAccountList[i].Id);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].MainUserAccount))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `main_user_account` = @mainUserAccount" + i);
									else
										sql = String.Concat(sql, " `main_user_account` = @mainUserAccount" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccountList[i].MainUserAccount);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].ProfileUserAccount))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `profile_user_account` = @profileUserAccount" + i);
									else
										sql = String.Concat(sql, " `profile_user_account` = @profileUserAccount" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccountList[i].ProfileUserAccount);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].Available))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `available` = @available" + i);
									else
										sql = String.Concat(sql, " `available` = @available" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@available" + i, associatedAccountList[i].Available);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccountList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccountList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(associatedAccountList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccountList[i].UpdatedAt);
								}
								if (associatedAccountWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, associatedAccountWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].MainUserAccount))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `main_user_account` = @mainUserAccountWhere" + i);
										else
											sql = String.Concat(sql, " `main_user_account` = @mainUserAccountWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@mainUserAccountWhere" + i, associatedAccountWhereList[i].MainUserAccount);
									}
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].ProfileUserAccount))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `profile_user_account` = @profileUserAccountWhere" + i);
										else
											sql = String.Concat(sql, " `profile_user_account` = @profileUserAccountWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@profileUserAccountWhere" + i, associatedAccountWhereList[i].ProfileUserAccount);
									}
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].Available))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
										else
											sql = String.Concat(sql, " `available` = @availableWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@availableWhere" + i, associatedAccountWhereList[i].Available);
									}
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, associatedAccountWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, associatedAccountWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(associatedAccountWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, associatedAccountWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(associatedAccountList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<AssociatedAccountModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<AssociatedAccountModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
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

		public List<AssociatedAccountModel> Delete(string connectionString, AssociatedAccountModel associatedAccount)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM urel_associated_accounts  ";
				if (associatedAccount != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(associatedAccount.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", associatedAccount.Id);
					}
					if (!String.IsNullOrEmpty(associatedAccount.MainUserAccount))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `main_user_account` = @mainUserAccount");
						else
							sql = String.Concat(sql, " `main_user_account` = @mainUserAccount");

						addAnd = true;
						cmd.Parameters.AddWithValue("@mainUserAccount", associatedAccount.MainUserAccount);
					}
					if (!String.IsNullOrEmpty(associatedAccount.ProfileUserAccount))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `profile_user_account` = @profileUserAccount");
						else
							sql = String.Concat(sql, " `profile_user_account` = @profileUserAccount");

						addAnd = true;
						cmd.Parameters.AddWithValue("@profileUserAccount", associatedAccount.ProfileUserAccount);
					}
					if (!String.IsNullOrEmpty(associatedAccount.Available))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `available` = @available");
						else
							sql = String.Concat(sql, " `available` = @available");

						addAnd = true;
						cmd.Parameters.AddWithValue("@available", associatedAccount.Available);
					}
					if (!String.IsNullOrEmpty(associatedAccount.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", associatedAccount.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(associatedAccount.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", associatedAccount.CreatedAt);
					}
					if (!String.IsNullOrEmpty(associatedAccount.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", associatedAccount.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(associatedAccount);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
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

		public List<AssociatedAccountModel> Delete(string connectionString, List<AssociatedAccountModel> associatedAccount)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<AssociatedAccountModel> response = new List<AssociatedAccountModel>();
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
						for (int i = 0; i < associatedAccount.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM urel_associated_accounts  ";
								if (associatedAccount[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(associatedAccount[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, associatedAccount[i].Id);
									}
									if (!String.IsNullOrEmpty(associatedAccount[i].MainUserAccount))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `main_user_account` = @mainUserAccount" + i);
										else
											sql = String.Concat(sql, " `main_user_account` = @mainUserAccount" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@mainUserAccount" + i, associatedAccount[i].MainUserAccount);
									}
									if (!String.IsNullOrEmpty(associatedAccount[i].ProfileUserAccount))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `profile_user_account` = @profileUserAccount" + i);
										else
											sql = String.Concat(sql, " `profile_user_account` = @profileUserAccount" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@profileUserAccount" + i, associatedAccount[i].ProfileUserAccount);
									}
									if (!String.IsNullOrEmpty(associatedAccount[i].Available))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `available` = @available" + i);
										else
											sql = String.Concat(sql, " `available` = @available" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@available" + i, associatedAccount[i].Available);
									}
									if (!String.IsNullOrEmpty(associatedAccount[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, associatedAccount[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(associatedAccount[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, associatedAccount[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(associatedAccount[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, associatedAccount[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(associatedAccount[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<AssociatedAccountModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<AssociatedAccountModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<AssociatedAccountModel>();
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
