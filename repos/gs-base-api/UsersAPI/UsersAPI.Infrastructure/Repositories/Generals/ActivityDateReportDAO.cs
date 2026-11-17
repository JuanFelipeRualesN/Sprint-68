using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Generals;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Generals
{
    class ActivityDateReportDAO
	{
		private MySqlConnection _connection;
		public List<ActivityDateReportModel> Read(string connectionString, ActivityDateReportModel activityDateReport, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `user_id`, `created_at`, `updated_at`  FROM gus_activity_date_reports ";
				{
					if (activityDateReport != null)
					{
						if (!String.IsNullOrEmpty(activityDateReport.UserId))
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
							cmd.Parameters.AddWithValue("@userId", activityDateReport.UserId);
						}
						if (!String.IsNullOrEmpty(activityDateReport.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", activityDateReport.CreatedAt);
						}
						if (!String.IsNullOrEmpty(activityDateReport.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", activityDateReport.UpdatedAt);
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
									activityDateReport.UserId = reader.GetValue(0).ToString();
								else
									activityDateReport.UserId = null;
								if (!reader.IsDBNull(1))
									activityDateReport.CreatedAt = reader.GetValue(1).ToString();
								else
									activityDateReport.CreatedAt = null;
								if (!reader.IsDBNull(2))
									activityDateReport.UpdatedAt = reader.GetValue(2).ToString();
								else
									activityDateReport.UpdatedAt = null;
								response.Add(activityDateReport);
								activityDateReport = new ActivityDateReportModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ActivityDateReportModel>();
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
				response = new List<ActivityDateReportModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ActivityDateReportModel> Read(string connectionString, ActivityDateReportModel activityDateReport, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `user_id`, `created_at`, `updated_at`  FROM gus_activity_date_reports ";
				{
					if (activityDateReport != null)
					{
						if (!String.IsNullOrEmpty(activityDateReport.UserId))
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
							cmd.Parameters.AddWithValue("@userId", activityDateReport.UserId);
						}
						if (!String.IsNullOrEmpty(activityDateReport.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", activityDateReport.CreatedAt);
						}
						if (!String.IsNullOrEmpty(activityDateReport.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", activityDateReport.UpdatedAt);
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
									activityDateReport.UserId = reader.GetValue(0).ToString();
								else
									activityDateReport.UserId = null;
								if (!reader.IsDBNull(1))
									activityDateReport.CreatedAt = reader.GetValue(1).ToString();
								else
									activityDateReport.CreatedAt = null;
								if (!reader.IsDBNull(2))
									activityDateReport.UpdatedAt = reader.GetValue(2).ToString();
								else
									activityDateReport.UpdatedAt = null;
								response.Add(activityDateReport);
								activityDateReport = new ActivityDateReportModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<ActivityDateReportModel>();
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
				response = new List<ActivityDateReportModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ActivityDateReportModel> Read(string connectionString, List<ActivityDateReportModel> activityDateReportWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `user_id`, `created_at`, `updated_at`  FROM gus_activity_date_reports ";
				if (activityDateReportWhere != null && activityDateReportWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < activityDateReportWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(activityDateReportWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, activityDateReportWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(activityDateReportWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, activityDateReportWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(activityDateReportWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, activityDateReportWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != activityDateReportWhere.Count - 1)
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
							ActivityDateReportModel activityDateReport = new ActivityDateReportModel(); if (!reader.IsDBNull(0))
								activityDateReport.UserId = reader.GetValue(0).ToString();
							else
								activityDateReport.UserId = null;
							if (!reader.IsDBNull(1))
								activityDateReport.CreatedAt = reader.GetValue(1).ToString();
							else
								activityDateReport.CreatedAt = null;
							if (!reader.IsDBNull(2))
								activityDateReport.UpdatedAt = reader.GetValue(2).ToString();
							else
								activityDateReport.UpdatedAt = null;
							response.Add(activityDateReport);
							activityDateReport = new ActivityDateReportModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ActivityDateReportModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ActivityDateReportModel> Read(string connectionString, List<ActivityDateReportModel> activityDateReportWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `user_id`, `created_at`, `updated_at`  FROM gus_activity_date_reports ";
				if (activityDateReportWhere != null && activityDateReportWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < activityDateReportWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(activityDateReportWhere[i].UserId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `user_id` = @userId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@userId" + i, activityDateReportWhere[i].UserId);
						}
						if (!String.IsNullOrEmpty(activityDateReportWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, activityDateReportWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(activityDateReportWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, activityDateReportWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != activityDateReportWhere.Count - 1)
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
							ActivityDateReportModel activityDateReport = new ActivityDateReportModel(); if (!reader.IsDBNull(0))
								activityDateReport.UserId = reader.GetValue(0).ToString();
							else
								activityDateReport.UserId = null;
							if (!reader.IsDBNull(1))
								activityDateReport.CreatedAt = reader.GetValue(1).ToString();
							else
								activityDateReport.CreatedAt = null;
							if (!reader.IsDBNull(2))
								activityDateReport.UpdatedAt = reader.GetValue(2).ToString();
							else
								activityDateReport.UpdatedAt = null;
							response.Add(activityDateReport);
							activityDateReport = new ActivityDateReportModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<ActivityDateReportModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<ActivityDateReportModel> Create(string connectionString, ActivityDateReportModel activityDateReport)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO gus_activity_date_reports (";
				string values = "(";
				if (!String.IsNullOrEmpty(activityDateReport.UserId))
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
					cmd.Parameters.AddWithValue("@userId", activityDateReport.UserId);
				}
				if (!String.IsNullOrEmpty(activityDateReport.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", activityDateReport.CreatedAt);
				}
				if (!String.IsNullOrEmpty(activityDateReport.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", activityDateReport.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				response.Add(activityDateReport);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
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
		public List<ActivityDateReportModel> Create(string connectionString, List<ActivityDateReportModel> activityDateReportList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
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
						for (int i = 0; i < activityDateReportList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO gus_activity_date_reports (";
								string values = "(";
								if (!String.IsNullOrEmpty(activityDateReportList[i].UserId))
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
									cmd.Parameters.AddWithValue("@userId" + i, activityDateReportList[i].UserId);
								}
								if (!String.IsNullOrEmpty(activityDateReportList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, activityDateReportList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(activityDateReportList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, activityDateReportList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(activityDateReportList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<ActivityDateReportModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ActivityDateReportModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
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

		public List<ActivityDateReportModel> Update(string connectionString, ActivityDateReportModel activityDateReport, ActivityDateReportModel activityDateReportWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE gus_activity_date_reports SET ";
				if (!String.IsNullOrEmpty(activityDateReport.UserId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `user_id` = @userId");
					else
						sql = String.Concat(sql, " `user_id` = @userId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@userId", activityDateReport.UserId);
				}
				if (!String.IsNullOrEmpty(activityDateReport.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", activityDateReport.CreatedAt);
				}
				if (!String.IsNullOrEmpty(activityDateReport.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", activityDateReport.UpdatedAt);
				}
				if (activityDateReportWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(activityDateReportWhere.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
						else
							sql = String.Concat(sql, " `user_id` = @userIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userIdWhere", activityDateReportWhere.UserId);
					}
					if (!String.IsNullOrEmpty(activityDateReportWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", activityDateReportWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(activityDateReportWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", activityDateReportWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(activityDateReport);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
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

		public List<ActivityDateReportModel> Update(string connectionString, List<ActivityDateReportModel> activityDateReportList, List<ActivityDateReportModel> activityDateReportWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
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
						for (int i = 0; i < activityDateReportList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE gus_activity_date_reports SET ";
								if (!String.IsNullOrEmpty(activityDateReportList[i].UserId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `user_id` = @userId" + i);
									else
										sql = String.Concat(sql, " `user_id` = @userId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@userId" + i, activityDateReportList[i].UserId);
								}
								if (!String.IsNullOrEmpty(activityDateReportList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, activityDateReportList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(activityDateReportList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, activityDateReportList[i].UpdatedAt);
								}
								if (activityDateReportWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(activityDateReportWhereList[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userIdWhere" + i, activityDateReportWhereList[i].UserId);
									}
									if (!String.IsNullOrEmpty(activityDateReportWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, activityDateReportWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(activityDateReportWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, activityDateReportWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(activityDateReportList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ActivityDateReportModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ActivityDateReportModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
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

		public List<ActivityDateReportModel> Delete(string connectionString, ActivityDateReportModel activityDateReport)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM gus_activity_date_reports  ";
				if (activityDateReport != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(activityDateReport.UserId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `user_id` = @userId");
						else
							sql = String.Concat(sql, " `user_id` = @userId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@userId", activityDateReport.UserId);
					}
					if (!String.IsNullOrEmpty(activityDateReport.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", activityDateReport.CreatedAt);
					}
					if (!String.IsNullOrEmpty(activityDateReport.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", activityDateReport.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(activityDateReport);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
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

		public List<ActivityDateReportModel> Delete(string connectionString, List<ActivityDateReportModel> activityDateReport)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<ActivityDateReportModel> response = new List<ActivityDateReportModel>();
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
						for (int i = 0; i < activityDateReport.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM gus_activity_date_reports  ";
								if (activityDateReport[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(activityDateReport[i].UserId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `user_id` = @userId" + i);
										else
											sql = String.Concat(sql, " `user_id` = @userId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@userId" + i, activityDateReport[i].UserId);
									}
									if (!String.IsNullOrEmpty(activityDateReport[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, activityDateReport[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(activityDateReport[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, activityDateReport[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(activityDateReport[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<ActivityDateReportModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<ActivityDateReportModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<ActivityDateReportModel>();
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
