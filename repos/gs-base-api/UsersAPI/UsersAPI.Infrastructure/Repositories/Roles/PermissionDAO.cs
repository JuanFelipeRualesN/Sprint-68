using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Services.Roles.CustomClasses;
namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Roles
{
	class PermissionDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, PermissionModel> ReadD(string connectionString, PermissionModel permission, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, PermissionModel> response = new Dictionary<string, PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				{
					if (permission != null)
					{
						if (!String.IsNullOrEmpty(permission.Id))
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
							cmd.Parameters.AddWithValue("@id", permission.Id);
						}
						if (!String.IsNullOrEmpty(permission.Name))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `name` = @name ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `name` = @name ");
							}
							cmd.Parameters.AddWithValue("@name", permission.Name);
						}
						if (!String.IsNullOrEmpty(permission.Desc))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `desc` = @desc ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `desc` = @desc ");
							}
							cmd.Parameters.AddWithValue("@desc", permission.Desc);
						}
						if (!String.IsNullOrEmpty(permission.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permission.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
						}
						if (!String.IsNullOrEmpty(permission.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
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
									permission.Id = reader.GetValue(0).ToString();
								else
									permission.Id = null;
								if (!reader.IsDBNull(1))
									permission.Name = reader.GetValue(1).ToString();
								else
									permission.Name = null;
								if (!reader.IsDBNull(2))
									permission.Desc = reader.GetValue(2).ToString();
								else
									permission.Desc = null;
								if (!reader.IsDBNull(3))
									permission.ModifiedBy = reader.GetValue(3).ToString();
								else
									permission.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									permission.CreatedAt = reader.GetValue(4).ToString();
								else
									permission.CreatedAt = null;
								if (!reader.IsDBNull(5))
									permission.UpdatedAt = reader.GetValue(5).ToString();
								else
									permission.UpdatedAt = null;
								response.Add(permission.Id, permission);
								permission = new PermissionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, PermissionModel>();
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
				response = new Dictionary<string, PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, PermissionModel> ReadD(string connectionString, PermissionModel permission, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, PermissionModel> response = new Dictionary<string, PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				{
					if (permission != null)
					{
						if (!String.IsNullOrEmpty(permission.Id))
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
							cmd.Parameters.AddWithValue("@id", permission.Id);
						}
						if (!String.IsNullOrEmpty(permission.Name))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `name` = @name ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `name` = @name ");
							}
							cmd.Parameters.AddWithValue("@name", permission.Name);
						}
						if (!String.IsNullOrEmpty(permission.Desc))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `desc` = @desc ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `desc` = @desc ");
							}
							cmd.Parameters.AddWithValue("@desc", permission.Desc);
						}
						if (!String.IsNullOrEmpty(permission.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permission.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
						}
						if (!String.IsNullOrEmpty(permission.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
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
									permission.Id = reader.GetValue(0).ToString();
								else
									permission.Id = null;
								if (!reader.IsDBNull(1))
									permission.Name = reader.GetValue(1).ToString();
								else
									permission.Name = null;
								if (!reader.IsDBNull(2))
									permission.Desc = reader.GetValue(2).ToString();
								else
									permission.Desc = null;
								if (!reader.IsDBNull(3))
									permission.ModifiedBy = reader.GetValue(3).ToString();
								else
									permission.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									permission.CreatedAt = reader.GetValue(4).ToString();
								else
									permission.CreatedAt = null;
								if (!reader.IsDBNull(5))
									permission.UpdatedAt = reader.GetValue(5).ToString();
								else
									permission.UpdatedAt = null;
								response.Add(permission.Id, permission);
								permission = new PermissionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, PermissionModel>();
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
				response = new Dictionary<string, PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, PermissionModel> ReadD(string connectionString, List<PermissionModel> permissionWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, PermissionModel> response = new Dictionary<string, PermissionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				if (permissionWhere != null && permissionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < permissionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(permissionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, permissionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Name))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `name` = @name" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@name" + i, permissionWhere[i].Name);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Desc))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `desc` = @desc" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@desc" + i, permissionWhere[i].Desc);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, permissionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, permissionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, permissionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != permissionWhere.Count - 1)
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
							PermissionModel permission = new PermissionModel(); if (!reader.IsDBNull(0))
								permission.Id = reader.GetValue(0).ToString();
							else
								permission.Id = null;
							if (!reader.IsDBNull(1))
								permission.Name = reader.GetValue(1).ToString();
							else
								permission.Name = null;
							if (!reader.IsDBNull(2))
								permission.Desc = reader.GetValue(2).ToString();
							else
								permission.Desc = null;
							if (!reader.IsDBNull(3))
								permission.ModifiedBy = reader.GetValue(3).ToString();
							else
								permission.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								permission.CreatedAt = reader.GetValue(4).ToString();
							else
								permission.CreatedAt = null;
							if (!reader.IsDBNull(5))
								permission.UpdatedAt = reader.GetValue(5).ToString();
							else
								permission.UpdatedAt = null;
							response.Add(permission.Id, permission);
							permission = new PermissionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, PermissionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, PermissionModel> ReadD(string connectionString, List<PermissionModel> permissionWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, PermissionModel> response = new Dictionary<string, PermissionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				if (permissionWhere != null && permissionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < permissionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(permissionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, permissionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Name))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `name` = @name" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@name" + i, permissionWhere[i].Name);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Desc))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `desc` = @desc" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@desc" + i, permissionWhere[i].Desc);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, permissionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, permissionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, permissionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != permissionWhere.Count - 1)
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
							PermissionModel permission = new PermissionModel(); if (!reader.IsDBNull(0))
								permission.Id = reader.GetValue(0).ToString();
							else
								permission.Id = null;
							if (!reader.IsDBNull(1))
								permission.Name = reader.GetValue(1).ToString();
							else
								permission.Name = null;
							if (!reader.IsDBNull(2))
								permission.Desc = reader.GetValue(2).ToString();
							else
								permission.Desc = null;
							if (!reader.IsDBNull(3))
								permission.ModifiedBy = reader.GetValue(3).ToString();
							else
								permission.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								permission.CreatedAt = reader.GetValue(4).ToString();
							else
								permission.CreatedAt = null;
							if (!reader.IsDBNull(5))
								permission.UpdatedAt = reader.GetValue(5).ToString();
							else
								permission.UpdatedAt = null;
							response.Add(permission.Id, permission);
							permission = new PermissionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, PermissionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<PermissionModel> Read(string connectionString, PermissionModel permission, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				{
					if (permission != null)
					{
						if (!String.IsNullOrEmpty(permission.Id))
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
							cmd.Parameters.AddWithValue("@id", permission.Id);
						}
						if (!String.IsNullOrEmpty(permission.Name))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `name` = @name ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `name` = @name ");
							}
							cmd.Parameters.AddWithValue("@name", permission.Name);
						}
						if (!String.IsNullOrEmpty(permission.Desc))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `desc` = @desc ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `desc` = @desc ");
							}
							cmd.Parameters.AddWithValue("@desc", permission.Desc);
						}
						if (!String.IsNullOrEmpty(permission.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permission.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
						}
						if (!String.IsNullOrEmpty(permission.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
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
									permission.Id = reader.GetValue(0).ToString();
								else
									permission.Id = null;
								if (!reader.IsDBNull(1))
									permission.Name = reader.GetValue(1).ToString();
								else
									permission.Name = null;
								if (!reader.IsDBNull(2))
									permission.Desc = reader.GetValue(2).ToString();
								else
									permission.Desc = null;
								if (!reader.IsDBNull(3))
									permission.ModifiedBy = reader.GetValue(3).ToString();
								else
									permission.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									permission.CreatedAt = reader.GetValue(4).ToString();
								else
									permission.CreatedAt = null;
								if (!reader.IsDBNull(5))
									permission.UpdatedAt = reader.GetValue(5).ToString();
								else
									permission.UpdatedAt = null;
								response.Add(permission);
								permission = new PermissionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<PermissionModel>();
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
				response = new List<PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<PermissionModel> Read(string connectionString, PermissionModel permission, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				{
					if (permission != null)
					{
						if (!String.IsNullOrEmpty(permission.Id))
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
							cmd.Parameters.AddWithValue("@id", permission.Id);
						}
						if (!String.IsNullOrEmpty(permission.Name))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `name` = @name ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `name` = @name ");
							}
							cmd.Parameters.AddWithValue("@name", permission.Name);
						}
						if (!String.IsNullOrEmpty(permission.Desc))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `desc` = @desc ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `desc` = @desc ");
							}
							cmd.Parameters.AddWithValue("@desc", permission.Desc);
						}
						if (!String.IsNullOrEmpty(permission.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permission.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
						}
						if (!String.IsNullOrEmpty(permission.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
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
									permission.Id = reader.GetValue(0).ToString();
								else
									permission.Id = null;
								if (!reader.IsDBNull(1))
									permission.Name = reader.GetValue(1).ToString();
								else
									permission.Name = null;
								if (!reader.IsDBNull(2))
									permission.Desc = reader.GetValue(2).ToString();
								else
									permission.Desc = null;
								if (!reader.IsDBNull(3))
									permission.ModifiedBy = reader.GetValue(3).ToString();
								else
									permission.ModifiedBy = null;
								if (!reader.IsDBNull(4))
									permission.CreatedAt = reader.GetValue(4).ToString();
								else
									permission.CreatedAt = null;
								if (!reader.IsDBNull(5))
									permission.UpdatedAt = reader.GetValue(5).ToString();
								else
									permission.UpdatedAt = null;
								response.Add(permission);
								permission = new PermissionModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<PermissionModel>();
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
				response = new List<PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<PermissionModel> Read(string connectionString, List<PermissionModel> permissionWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				if (permissionWhere != null && permissionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < permissionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(permissionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, permissionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Name))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `name` = @name" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@name" + i, permissionWhere[i].Name);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Desc))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `desc` = @desc" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@desc" + i, permissionWhere[i].Desc);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, permissionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, permissionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, permissionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != permissionWhere.Count - 1)
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
							PermissionModel permission = new PermissionModel(); if (!reader.IsDBNull(0))
								permission.Id = reader.GetValue(0).ToString();
							else
								permission.Id = null;
							if (!reader.IsDBNull(1))
								permission.Name = reader.GetValue(1).ToString();
							else
								permission.Name = null;
							if (!reader.IsDBNull(2))
								permission.Desc = reader.GetValue(2).ToString();
							else
								permission.Desc = null;
							if (!reader.IsDBNull(3))
								permission.ModifiedBy = reader.GetValue(3).ToString();
							else
								permission.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								permission.CreatedAt = reader.GetValue(4).ToString();
							else
								permission.CreatedAt = null;
							if (!reader.IsDBNull(5))
								permission.UpdatedAt = reader.GetValue(5).ToString();
							else
								permission.UpdatedAt = null;
							response.Add(permission);
							permission = new PermissionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<PermissionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<PermissionModel> Read(string connectionString, List<PermissionModel> permissionWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `name`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM ur_permissions ";
				if (permissionWhere != null && permissionWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < permissionWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(permissionWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, permissionWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Name))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `name` = @name" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@name" + i, permissionWhere[i].Name);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].Desc))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `desc` = @desc" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@desc" + i, permissionWhere[i].Desc);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, permissionWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, permissionWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(permissionWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, permissionWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != permissionWhere.Count - 1)
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
							PermissionModel permission = new PermissionModel(); if (!reader.IsDBNull(0))
								permission.Id = reader.GetValue(0).ToString();
							else
								permission.Id = null;
							if (!reader.IsDBNull(1))
								permission.Name = reader.GetValue(1).ToString();
							else
								permission.Name = null;
							if (!reader.IsDBNull(2))
								permission.Desc = reader.GetValue(2).ToString();
							else
								permission.Desc = null;
							if (!reader.IsDBNull(3))
								permission.ModifiedBy = reader.GetValue(3).ToString();
							else
								permission.ModifiedBy = null;
							if (!reader.IsDBNull(4))
								permission.CreatedAt = reader.GetValue(4).ToString();
							else
								permission.CreatedAt = null;
							if (!reader.IsDBNull(5))
								permission.UpdatedAt = reader.GetValue(5).ToString();
							else
								permission.UpdatedAt = null;
							response.Add(permission);
							permission = new PermissionModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<PermissionModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<PermissionModel> Create(string connectionString, PermissionModel permission)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO ur_permissions (";
				string values = "(";
				if (!String.IsNullOrEmpty(permission.Id))
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
					cmd.Parameters.AddWithValue("@id", permission.Id);
				}
				if (!String.IsNullOrEmpty(permission.Name))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `name` ");
						values = String.Concat(values, ", @name");
					}
					else
					{
						sql = String.Concat(sql, " `name` ");
						values = String.Concat(values, " @name");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@name", permission.Name);
				}
				if (!String.IsNullOrEmpty(permission.Desc))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `desc` ");
						values = String.Concat(values, ", @desc");
					}
					else
					{
						sql = String.Concat(sql, " `desc` ");
						values = String.Concat(values, " @desc");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@desc", permission.Desc);
				}
				if (!String.IsNullOrEmpty(permission.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(permission.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
				}
				if (!String.IsNullOrEmpty(permission.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				permission.Id = cmd.LastInsertedId.ToString();
				response.Add(permission);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
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
		public List<PermissionModel> Create(string connectionString, List<PermissionModel> permissionList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
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
						for (int i = 0; i < permissionList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO ur_permissions (";
								string values = "(";
								if (!String.IsNullOrEmpty(permissionList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, permissionList[i].Id);
								}
								if (!String.IsNullOrEmpty(permissionList[i].Name))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `name` ");
										values = String.Concat(values, ", @name" + i);
									}

									else
									{
										sql = String.Concat(sql, " `name` ");
										values = String.Concat(values, "@name" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@name" + i, permissionList[i].Name);
								}
								if (!String.IsNullOrEmpty(permissionList[i].Desc))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `desc` ");
										values = String.Concat(values, ", @desc" + i);
									}

									else
									{
										sql = String.Concat(sql, " `desc` ");
										values = String.Concat(values, "@desc" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@desc" + i, permissionList[i].Desc);
								}
								if (!String.IsNullOrEmpty(permissionList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, permissionList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(permissionList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, permissionList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(permissionList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, permissionList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(permissionList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<PermissionModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<PermissionModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
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

		public List<PermissionModel> Update(string connectionString, PermissionModel permission, PermissionModel permissionWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE ur_permissions SET ";
				if (!String.IsNullOrEmpty(permission.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", permission.Id);
				}
				if (!String.IsNullOrEmpty(permission.Name))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `name` = @name");
					else
						sql = String.Concat(sql, " `name` = @name");

					addAnd = true;
					cmd.Parameters.AddWithValue("@name", permission.Name);
				}
				if (!String.IsNullOrEmpty(permission.Desc))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `desc` = @desc");
					else
						sql = String.Concat(sql, " `desc` = @desc");

					addAnd = true;
					cmd.Parameters.AddWithValue("@desc", permission.Desc);
				}
				if (!String.IsNullOrEmpty(permission.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(permission.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
				}
				if (!String.IsNullOrEmpty(permission.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
				}
				if (permissionWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(permissionWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", permissionWhere.Id);
					}
					if (!String.IsNullOrEmpty(permissionWhere.Name))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `name` = @nameWhere");
						else
							sql = String.Concat(sql, " `name` = @nameWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@nameWhere", permissionWhere.Name);
					}
					if (!String.IsNullOrEmpty(permissionWhere.Desc))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `desc` = @descWhere");
						else
							sql = String.Concat(sql, " `desc` = @descWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@descWhere", permissionWhere.Desc);
					}
					if (!String.IsNullOrEmpty(permissionWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", permissionWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(permissionWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", permissionWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(permissionWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", permissionWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(permission);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
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

		public List<PermissionModel> Update(string connectionString, List<PermissionModel> permissionList, List<PermissionModel> permissionWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
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
						for (int i = 0; i < permissionList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE ur_permissions SET ";
								if (!String.IsNullOrEmpty(permissionList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, permissionList[i].Id);
								}
								if (!String.IsNullOrEmpty(permissionList[i].Name))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `name` = @name" + i);
									else
										sql = String.Concat(sql, " `name` = @name" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@name" + i, permissionList[i].Name);
								}
								if (!String.IsNullOrEmpty(permissionList[i].Desc))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `desc` = @desc" + i);
									else
										sql = String.Concat(sql, " `desc` = @desc" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@desc" + i, permissionList[i].Desc);
								}
								if (!String.IsNullOrEmpty(permissionList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, permissionList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(permissionList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, permissionList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(permissionList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, permissionList[i].UpdatedAt);
								}
								if (permissionWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(permissionWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, permissionWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(permissionWhereList[i].Name))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
										else
											sql = String.Concat(sql, " `name` = @nameWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@nameWhere" + i, permissionWhereList[i].Name);
									}
									if (!String.IsNullOrEmpty(permissionWhereList[i].Desc))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
										else
											sql = String.Concat(sql, " `desc` = @descWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@descWhere" + i, permissionWhereList[i].Desc);
									}
									if (!String.IsNullOrEmpty(permissionWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, permissionWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(permissionWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, permissionWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(permissionWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, permissionWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(permissionList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<PermissionModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<PermissionModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
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

		public List<PermissionModel> Delete(string connectionString, PermissionModel permission)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM ur_permissions  ";
				if (permission != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(permission.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", permission.Id);
					}
					if (!String.IsNullOrEmpty(permission.Name))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `name` = @name");
						else
							sql = String.Concat(sql, " `name` = @name");

						addAnd = true;
						cmd.Parameters.AddWithValue("@name", permission.Name);
					}
					if (!String.IsNullOrEmpty(permission.Desc))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `desc` = @desc");
						else
							sql = String.Concat(sql, " `desc` = @desc");

						addAnd = true;
						cmd.Parameters.AddWithValue("@desc", permission.Desc);
					}
					if (!String.IsNullOrEmpty(permission.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", permission.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(permission.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", permission.CreatedAt);
					}
					if (!String.IsNullOrEmpty(permission.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", permission.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(permission);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
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

		public List<PermissionModel> Delete(string connectionString, List<PermissionModel> permission)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<PermissionModel> response = new List<PermissionModel>();
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
						for (int i = 0; i < permission.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM ur_permissions  ";
								if (permission[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(permission[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, permission[i].Id);
									}
									if (!String.IsNullOrEmpty(permission[i].Name))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `name` = @name" + i);
										else
											sql = String.Concat(sql, " `name` = @name" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@name" + i, permission[i].Name);
									}
									if (!String.IsNullOrEmpty(permission[i].Desc))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `desc` = @desc" + i);
										else
											sql = String.Concat(sql, " `desc` = @desc" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@desc" + i, permission[i].Desc);
									}
									if (!String.IsNullOrEmpty(permission[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, permission[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(permission[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, permission[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(permission[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, permission[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(permission[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<PermissionModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<PermissionModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<PermissionModel>();
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
