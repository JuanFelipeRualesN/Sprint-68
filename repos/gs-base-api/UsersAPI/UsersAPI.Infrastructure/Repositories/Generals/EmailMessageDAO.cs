using System;
using System.Collections.Generic;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Generals;

namespace UsersAPI.UsersAPI.Infrastructure.Repositories.Generals
{
    class EmailMessageDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, EmailMessageModel> ReadD(string connectionString, EmailMessageModel emailMessage, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				{
					if (emailMessage != null)
					{
						if (!String.IsNullOrEmpty(emailMessage.Id))
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
							cmd.Parameters.AddWithValue("@id", emailMessage.Id);
						}
						if (!String.IsNullOrEmpty(emailMessage.Subject))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `subject` = @subject ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `subject` = @subject ");
							}
							cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
						}
						if (!String.IsNullOrEmpty(emailMessage.Header))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `header` = @header ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `header` = @header ");
							}
							cmd.Parameters.AddWithValue("@header", emailMessage.Header);
						}
						if (!String.IsNullOrEmpty(emailMessage.Body))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `body` = @body ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `body` = @body ");
							}
							cmd.Parameters.AddWithValue("@body", emailMessage.Body);
						}
						if (!String.IsNullOrEmpty(emailMessage.Url))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `url` = @url ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `url` = @url ");
							}
							cmd.Parameters.AddWithValue("@url", emailMessage.Url);
						}
						if (!String.IsNullOrEmpty(emailMessage.Footer))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `footer` = @footer ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `footer` = @footer ");
							}
							cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
						}
						if (!String.IsNullOrEmpty(emailMessage.LangCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `lang_code` = @langCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `lang_code` = @langCode ");
							}
							cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessage.Type))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `type` = @type ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `type` = @type ");
							}
							cmd.Parameters.AddWithValue("@type", emailMessage.Type);
						}
						if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
									emailMessage.Id = reader.GetValue(0).ToString();
								else
									emailMessage.Id = null;
								if (!reader.IsDBNull(1))
									emailMessage.Subject = reader.GetValue(1).ToString();
								else
									emailMessage.Subject = null;
								if (!reader.IsDBNull(2))
									emailMessage.Header = reader.GetValue(2).ToString();
								else
									emailMessage.Header = null;
								if (!reader.IsDBNull(3))
									emailMessage.Body = reader.GetValue(3).ToString();
								else
									emailMessage.Body = null;
								if (!reader.IsDBNull(4))
									emailMessage.Url = reader.GetValue(4).ToString();
								else
									emailMessage.Url = null;
								if (!reader.IsDBNull(5))
									emailMessage.Footer = reader.GetValue(5).ToString();
								else
									emailMessage.Footer = null;
								if (!reader.IsDBNull(6))
									emailMessage.LangCode = reader.GetValue(6).ToString();
								else
									emailMessage.LangCode = null;
								if (!reader.IsDBNull(7))
									emailMessage.Type = reader.GetValue(7).ToString();
								else
									emailMessage.Type = null;
								if (!reader.IsDBNull(8))
									emailMessage.ModifiedBy = reader.GetValue(8).ToString();
								else
									emailMessage.ModifiedBy = null;
								if (!reader.IsDBNull(9))
									emailMessage.CreatedAt = reader.GetValue(9).ToString();
								else
									emailMessage.CreatedAt = null;
								if (!reader.IsDBNull(10))
									emailMessage.UpdatedAt = reader.GetValue(10).ToString();
								else
									emailMessage.UpdatedAt = null;
								response.Add(emailMessage.Id, emailMessage);
								emailMessage = new EmailMessageModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, EmailMessageModel>();
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
				response = new Dictionary<string, EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, EmailMessageModel> ReadD(string connectionString, EmailMessageModel emailMessage, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				{
					if (emailMessage != null)
					{
						if (!String.IsNullOrEmpty(emailMessage.Id))
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
							cmd.Parameters.AddWithValue("@id", emailMessage.Id);
						}
						if (!String.IsNullOrEmpty(emailMessage.Subject))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `subject` = @subject ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `subject` = @subject ");
							}
							cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
						}
						if (!String.IsNullOrEmpty(emailMessage.Header))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `header` = @header ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `header` = @header ");
							}
							cmd.Parameters.AddWithValue("@header", emailMessage.Header);
						}
						if (!String.IsNullOrEmpty(emailMessage.Body))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `body` = @body ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `body` = @body ");
							}
							cmd.Parameters.AddWithValue("@body", emailMessage.Body);
						}
						if (!String.IsNullOrEmpty(emailMessage.Url))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `url` = @url ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `url` = @url ");
							}
							cmd.Parameters.AddWithValue("@url", emailMessage.Url);
						}
						if (!String.IsNullOrEmpty(emailMessage.Footer))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `footer` = @footer ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `footer` = @footer ");
							}
							cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
						}
						if (!String.IsNullOrEmpty(emailMessage.LangCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `lang_code` = @langCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `lang_code` = @langCode ");
							}
							cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessage.Type))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `type` = @type ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `type` = @type ");
							}
							cmd.Parameters.AddWithValue("@type", emailMessage.Type);
						}
						if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
									emailMessage.Id = reader.GetValue(0).ToString();
								else
									emailMessage.Id = null;
								if (!reader.IsDBNull(1))
									emailMessage.Subject = reader.GetValue(1).ToString();
								else
									emailMessage.Subject = null;
								if (!reader.IsDBNull(2))
									emailMessage.Header = reader.GetValue(2).ToString();
								else
									emailMessage.Header = null;
								if (!reader.IsDBNull(3))
									emailMessage.Body = reader.GetValue(3).ToString();
								else
									emailMessage.Body = null;
								if (!reader.IsDBNull(4))
									emailMessage.Url = reader.GetValue(4).ToString();
								else
									emailMessage.Url = null;
								if (!reader.IsDBNull(5))
									emailMessage.Footer = reader.GetValue(5).ToString();
								else
									emailMessage.Footer = null;
								if (!reader.IsDBNull(6))
									emailMessage.LangCode = reader.GetValue(6).ToString();
								else
									emailMessage.LangCode = null;
								if (!reader.IsDBNull(7))
									emailMessage.Type = reader.GetValue(7).ToString();
								else
									emailMessage.Type = null;
								if (!reader.IsDBNull(8))
									emailMessage.ModifiedBy = reader.GetValue(8).ToString();
								else
									emailMessage.ModifiedBy = null;
								if (!reader.IsDBNull(9))
									emailMessage.CreatedAt = reader.GetValue(9).ToString();
								else
									emailMessage.CreatedAt = null;
								if (!reader.IsDBNull(10))
									emailMessage.UpdatedAt = reader.GetValue(10).ToString();
								else
									emailMessage.UpdatedAt = null;
								response.Add(emailMessage.Id, emailMessage);
								emailMessage = new EmailMessageModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, EmailMessageModel>();
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
				response = new Dictionary<string, EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, EmailMessageModel> ReadD(string connectionString, List<EmailMessageModel> emailMessageWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				if (emailMessageWhere != null && emailMessageWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < emailMessageWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Subject))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `subject` = @subject" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Header))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `header` = @header" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@header" + i, emailMessageWhere[i].Header);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Body))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `body` = @body" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Url))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `url` = @url" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@url" + i, emailMessageWhere[i].Url);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Footer))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `footer` = @footer" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@footer" + i, emailMessageWhere[i].Footer);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].LangCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `lang_code` = @langCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@langCode" + i, emailMessageWhere[i].LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Type))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `type` = @type" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@type" + i, emailMessageWhere[i].Type);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != emailMessageWhere.Count - 1)
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
							EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
								emailMessage.Id = reader.GetValue(0).ToString();
							else
								emailMessage.Id = null;
							if (!reader.IsDBNull(1))
								emailMessage.Subject = reader.GetValue(1).ToString();
							else
								emailMessage.Subject = null;
							if (!reader.IsDBNull(2))
								emailMessage.Header = reader.GetValue(2).ToString();
							else
								emailMessage.Header = null;
							if (!reader.IsDBNull(3))
								emailMessage.Body = reader.GetValue(3).ToString();
							else
								emailMessage.Body = null;
							if (!reader.IsDBNull(4))
								emailMessage.Url = reader.GetValue(4).ToString();
							else
								emailMessage.Url = null;
							if (!reader.IsDBNull(5))
								emailMessage.Footer = reader.GetValue(5).ToString();
							else
								emailMessage.Footer = null;
							if (!reader.IsDBNull(6))
								emailMessage.LangCode = reader.GetValue(6).ToString();
							else
								emailMessage.LangCode = null;
							if (!reader.IsDBNull(7))
								emailMessage.Type = reader.GetValue(7).ToString();
							else
								emailMessage.Type = null;
							if (!reader.IsDBNull(8))
								emailMessage.ModifiedBy = reader.GetValue(8).ToString();
							else
								emailMessage.ModifiedBy = null;
							if (!reader.IsDBNull(9))
								emailMessage.CreatedAt = reader.GetValue(9).ToString();
							else
								emailMessage.CreatedAt = null;
							if (!reader.IsDBNull(10))
								emailMessage.UpdatedAt = reader.GetValue(10).ToString();
							else
								emailMessage.UpdatedAt = null;
							response.Add(emailMessage.Id, emailMessage);
							emailMessage = new EmailMessageModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, EmailMessageModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, EmailMessageModel> ReadD(string connectionString, List<EmailMessageModel> emailMessageWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EmailMessageModel> response = new Dictionary<string, EmailMessageModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				if (emailMessageWhere != null && emailMessageWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < emailMessageWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Subject))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `subject` = @subject" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Header))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `header` = @header" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@header" + i, emailMessageWhere[i].Header);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Body))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `body` = @body" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Url))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `url` = @url" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@url" + i, emailMessageWhere[i].Url);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Footer))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `footer` = @footer" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@footer" + i, emailMessageWhere[i].Footer);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].LangCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `lang_code` = @langCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@langCode" + i, emailMessageWhere[i].LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Type))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `type` = @type" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@type" + i, emailMessageWhere[i].Type);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != emailMessageWhere.Count - 1)
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
							EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
								emailMessage.Id = reader.GetValue(0).ToString();
							else
								emailMessage.Id = null;
							if (!reader.IsDBNull(1))
								emailMessage.Subject = reader.GetValue(1).ToString();
							else
								emailMessage.Subject = null;
							if (!reader.IsDBNull(2))
								emailMessage.Header = reader.GetValue(2).ToString();
							else
								emailMessage.Header = null;
							if (!reader.IsDBNull(3))
								emailMessage.Body = reader.GetValue(3).ToString();
							else
								emailMessage.Body = null;
							if (!reader.IsDBNull(4))
								emailMessage.Url = reader.GetValue(4).ToString();
							else
								emailMessage.Url = null;
							if (!reader.IsDBNull(5))
								emailMessage.Footer = reader.GetValue(5).ToString();
							else
								emailMessage.Footer = null;
							if (!reader.IsDBNull(6))
								emailMessage.LangCode = reader.GetValue(6).ToString();
							else
								emailMessage.LangCode = null;
							if (!reader.IsDBNull(7))
								emailMessage.Type = reader.GetValue(7).ToString();
							else
								emailMessage.Type = null;
							if (!reader.IsDBNull(8))
								emailMessage.ModifiedBy = reader.GetValue(8).ToString();
							else
								emailMessage.ModifiedBy = null;
							if (!reader.IsDBNull(9))
								emailMessage.CreatedAt = reader.GetValue(9).ToString();
							else
								emailMessage.CreatedAt = null;
							if (!reader.IsDBNull(10))
								emailMessage.UpdatedAt = reader.GetValue(10).ToString();
							else
								emailMessage.UpdatedAt = null;
							response.Add(emailMessage.Id, emailMessage);
							emailMessage = new EmailMessageModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, EmailMessageModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EmailMessageModel> Read(string connectionString, EmailMessageModel emailMessage, bool firstOne, bool orderByLastDate)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				{
					if (emailMessage != null)
					{
						if (!String.IsNullOrEmpty(emailMessage.Id))
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
							cmd.Parameters.AddWithValue("@id", emailMessage.Id);
						}
						if (!String.IsNullOrEmpty(emailMessage.Subject))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `subject` = @subject ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `subject` = @subject ");
							}
							cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
						}
						if (!String.IsNullOrEmpty(emailMessage.Header))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `header` = @header ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `header` = @header ");
							}
							cmd.Parameters.AddWithValue("@header", emailMessage.Header);
						}
						if (!String.IsNullOrEmpty(emailMessage.Body))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `body` = @body ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `body` = @body ");
							}
							cmd.Parameters.AddWithValue("@body", emailMessage.Body);
						}
						if (!String.IsNullOrEmpty(emailMessage.Url))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `url` = @url ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `url` = @url ");
							}
							cmd.Parameters.AddWithValue("@url", emailMessage.Url);
						}
						if (!String.IsNullOrEmpty(emailMessage.Footer))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `footer` = @footer ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `footer` = @footer ");
							}
							cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
						}
						if (!String.IsNullOrEmpty(emailMessage.LangCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `lang_code` = @langCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `lang_code` = @langCode ");
							}
							cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessage.Type))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `type` = @type ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `type` = @type ");
							}
							cmd.Parameters.AddWithValue("@type", emailMessage.Type);
						}
						if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
									emailMessage.Id = reader.GetValue(0).ToString();
								else
									emailMessage.Id = null;
								if (!reader.IsDBNull(1))
									emailMessage.Subject = reader.GetValue(1).ToString();
								else
									emailMessage.Subject = null;
								if (!reader.IsDBNull(2))
									emailMessage.Header = reader.GetValue(2).ToString();
								else
									emailMessage.Header = null;
								if (!reader.IsDBNull(3))
									emailMessage.Body = reader.GetValue(3).ToString();
								else
									emailMessage.Body = null;
								if (!reader.IsDBNull(4))
									emailMessage.Url = reader.GetValue(4).ToString();
								else
									emailMessage.Url = null;
								if (!reader.IsDBNull(5))
									emailMessage.Footer = reader.GetValue(5).ToString();
								else
									emailMessage.Footer = null;
								if (!reader.IsDBNull(6))
									emailMessage.LangCode = reader.GetValue(6).ToString();
								else
									emailMessage.LangCode = null;
								if (!reader.IsDBNull(7))
									emailMessage.Type = reader.GetValue(7).ToString();
								else
									emailMessage.Type = null;
								if (!reader.IsDBNull(8))
									emailMessage.ModifiedBy = reader.GetValue(8).ToString();
								else
									emailMessage.ModifiedBy = null;
								if (!reader.IsDBNull(9))
									emailMessage.CreatedAt = reader.GetValue(9).ToString();
								else
									emailMessage.CreatedAt = null;
								if (!reader.IsDBNull(10))
									emailMessage.UpdatedAt = reader.GetValue(10).ToString();
								else
									emailMessage.UpdatedAt = null;
								response.Add(emailMessage);
								emailMessage = new EmailMessageModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<EmailMessageModel>();
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
				response = new List<EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EmailMessageModel> Read(string connectionString, EmailMessageModel emailMessage, bool orderByLastDate, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				{
					if (emailMessage != null)
					{
						if (!String.IsNullOrEmpty(emailMessage.Id))
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
							cmd.Parameters.AddWithValue("@id", emailMessage.Id);
						}
						if (!String.IsNullOrEmpty(emailMessage.Subject))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `subject` = @subject ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `subject` = @subject ");
							}
							cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
						}
						if (!String.IsNullOrEmpty(emailMessage.Header))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `header` = @header ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `header` = @header ");
							}
							cmd.Parameters.AddWithValue("@header", emailMessage.Header);
						}
						if (!String.IsNullOrEmpty(emailMessage.Body))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `body` = @body ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `body` = @body ");
							}
							cmd.Parameters.AddWithValue("@body", emailMessage.Body);
						}
						if (!String.IsNullOrEmpty(emailMessage.Url))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `url` = @url ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `url` = @url ");
							}
							cmd.Parameters.AddWithValue("@url", emailMessage.Url);
						}
						if (!String.IsNullOrEmpty(emailMessage.Footer))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `footer` = @footer ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `footer` = @footer ");
							}
							cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
						}
						if (!String.IsNullOrEmpty(emailMessage.LangCode))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `lang_code` = @langCode ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `lang_code` = @langCode ");
							}
							cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessage.Type))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `type` = @type ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `type` = @type ");
							}
							cmd.Parameters.AddWithValue("@type", emailMessage.Type);
						}
						if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
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
									emailMessage.Id = reader.GetValue(0).ToString();
								else
									emailMessage.Id = null;
								if (!reader.IsDBNull(1))
									emailMessage.Subject = reader.GetValue(1).ToString();
								else
									emailMessage.Subject = null;
								if (!reader.IsDBNull(2))
									emailMessage.Header = reader.GetValue(2).ToString();
								else
									emailMessage.Header = null;
								if (!reader.IsDBNull(3))
									emailMessage.Body = reader.GetValue(3).ToString();
								else
									emailMessage.Body = null;
								if (!reader.IsDBNull(4))
									emailMessage.Url = reader.GetValue(4).ToString();
								else
									emailMessage.Url = null;
								if (!reader.IsDBNull(5))
									emailMessage.Footer = reader.GetValue(5).ToString();
								else
									emailMessage.Footer = null;
								if (!reader.IsDBNull(6))
									emailMessage.LangCode = reader.GetValue(6).ToString();
								else
									emailMessage.LangCode = null;
								if (!reader.IsDBNull(7))
									emailMessage.Type = reader.GetValue(7).ToString();
								else
									emailMessage.Type = null;
								if (!reader.IsDBNull(8))
									emailMessage.ModifiedBy = reader.GetValue(8).ToString();
								else
									emailMessage.ModifiedBy = null;
								if (!reader.IsDBNull(9))
									emailMessage.CreatedAt = reader.GetValue(9).ToString();
								else
									emailMessage.CreatedAt = null;
								if (!reader.IsDBNull(10))
									emailMessage.UpdatedAt = reader.GetValue(10).ToString();
								else
									emailMessage.UpdatedAt = null;
								response.Add(emailMessage);
								emailMessage = new EmailMessageModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<EmailMessageModel>();
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
				response = new List<EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EmailMessageModel> Read(string connectionString, List<EmailMessageModel> emailMessageWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				if (emailMessageWhere != null && emailMessageWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < emailMessageWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Subject))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `subject` = @subject" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Header))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `header` = @header" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@header" + i, emailMessageWhere[i].Header);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Body))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `body` = @body" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Url))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `url` = @url" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@url" + i, emailMessageWhere[i].Url);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Footer))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `footer` = @footer" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@footer" + i, emailMessageWhere[i].Footer);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].LangCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `lang_code` = @langCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@langCode" + i, emailMessageWhere[i].LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Type))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `type` = @type" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@type" + i, emailMessageWhere[i].Type);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != emailMessageWhere.Count - 1)
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
							EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
								emailMessage.Id = reader.GetValue(0).ToString();
							else
								emailMessage.Id = null;
							if (!reader.IsDBNull(1))
								emailMessage.Subject = reader.GetValue(1).ToString();
							else
								emailMessage.Subject = null;
							if (!reader.IsDBNull(2))
								emailMessage.Header = reader.GetValue(2).ToString();
							else
								emailMessage.Header = null;
							if (!reader.IsDBNull(3))
								emailMessage.Body = reader.GetValue(3).ToString();
							else
								emailMessage.Body = null;
							if (!reader.IsDBNull(4))
								emailMessage.Url = reader.GetValue(4).ToString();
							else
								emailMessage.Url = null;
							if (!reader.IsDBNull(5))
								emailMessage.Footer = reader.GetValue(5).ToString();
							else
								emailMessage.Footer = null;
							if (!reader.IsDBNull(6))
								emailMessage.LangCode = reader.GetValue(6).ToString();
							else
								emailMessage.LangCode = null;
							if (!reader.IsDBNull(7))
								emailMessage.Type = reader.GetValue(7).ToString();
							else
								emailMessage.Type = null;
							if (!reader.IsDBNull(8))
								emailMessage.ModifiedBy = reader.GetValue(8).ToString();
							else
								emailMessage.ModifiedBy = null;
							if (!reader.IsDBNull(9))
								emailMessage.CreatedAt = reader.GetValue(9).ToString();
							else
								emailMessage.CreatedAt = null;
							if (!reader.IsDBNull(10))
								emailMessage.UpdatedAt = reader.GetValue(10).ToString();
							else
								emailMessage.UpdatedAt = null;
							response.Add(emailMessage);
							emailMessage = new EmailMessageModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<EmailMessageModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EmailMessageModel> Read(string connectionString, List<EmailMessageModel> emailMessageWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `subject`, `header`, `body`, `url`, `footer`, `lang_code`, `type`, `modified_by`, `created_at`, `updated_at`  FROM gus_email_messages ";
				if (emailMessageWhere != null && emailMessageWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < emailMessageWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, emailMessageWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Subject))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `subject` = @subject" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@subject" + i, emailMessageWhere[i].Subject);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Header))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `header` = @header" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@header" + i, emailMessageWhere[i].Header);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Body))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `body` = @body" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@body" + i, emailMessageWhere[i].Body);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Url))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `url` = @url" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@url" + i, emailMessageWhere[i].Url);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Footer))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `footer` = @footer" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@footer" + i, emailMessageWhere[i].Footer);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].LangCode))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `lang_code` = @langCode" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@langCode" + i, emailMessageWhere[i].LangCode);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].Type))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `type` = @type" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@type" + i, emailMessageWhere[i].Type);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(emailMessageWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != emailMessageWhere.Count - 1)
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
							EmailMessageModel emailMessage = new EmailMessageModel(); if (!reader.IsDBNull(0))
								emailMessage.Id = reader.GetValue(0).ToString();
							else
								emailMessage.Id = null;
							if (!reader.IsDBNull(1))
								emailMessage.Subject = reader.GetValue(1).ToString();
							else
								emailMessage.Subject = null;
							if (!reader.IsDBNull(2))
								emailMessage.Header = reader.GetValue(2).ToString();
							else
								emailMessage.Header = null;
							if (!reader.IsDBNull(3))
								emailMessage.Body = reader.GetValue(3).ToString();
							else
								emailMessage.Body = null;
							if (!reader.IsDBNull(4))
								emailMessage.Url = reader.GetValue(4).ToString();
							else
								emailMessage.Url = null;
							if (!reader.IsDBNull(5))
								emailMessage.Footer = reader.GetValue(5).ToString();
							else
								emailMessage.Footer = null;
							if (!reader.IsDBNull(6))
								emailMessage.LangCode = reader.GetValue(6).ToString();
							else
								emailMessage.LangCode = null;
							if (!reader.IsDBNull(7))
								emailMessage.Type = reader.GetValue(7).ToString();
							else
								emailMessage.Type = null;
							if (!reader.IsDBNull(8))
								emailMessage.ModifiedBy = reader.GetValue(8).ToString();
							else
								emailMessage.ModifiedBy = null;
							if (!reader.IsDBNull(9))
								emailMessage.CreatedAt = reader.GetValue(9).ToString();
							else
								emailMessage.CreatedAt = null;
							if (!reader.IsDBNull(10))
								emailMessage.UpdatedAt = reader.GetValue(10).ToString();
							else
								emailMessage.UpdatedAt = null;
							response.Add(emailMessage);
							emailMessage = new EmailMessageModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<EmailMessageModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EmailMessageModel> Create(string connectionString, EmailMessageModel emailMessage)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO gus_email_messages (";
				string values = "(";
				if (!String.IsNullOrEmpty(emailMessage.Id))
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
					cmd.Parameters.AddWithValue("@id", emailMessage.Id);
				}
				if (!String.IsNullOrEmpty(emailMessage.Subject))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `subject` ");
						values = String.Concat(values, ", @subject");
					}
					else
					{
						sql = String.Concat(sql, " `subject` ");
						values = String.Concat(values, " @subject");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
				}
				if (!String.IsNullOrEmpty(emailMessage.Header))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `header` ");
						values = String.Concat(values, ", @header");
					}
					else
					{
						sql = String.Concat(sql, " `header` ");
						values = String.Concat(values, " @header");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@header", emailMessage.Header);
				}
				if (!String.IsNullOrEmpty(emailMessage.Body))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `body` ");
						values = String.Concat(values, ", @body");
					}
					else
					{
						sql = String.Concat(sql, " `body` ");
						values = String.Concat(values, " @body");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@body", emailMessage.Body);
				}
				if (!String.IsNullOrEmpty(emailMessage.Url))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `url` ");
						values = String.Concat(values, ", @url");
					}
					else
					{
						sql = String.Concat(sql, " `url` ");
						values = String.Concat(values, " @url");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@url", emailMessage.Url);
				}
				if (!String.IsNullOrEmpty(emailMessage.Footer))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `footer` ");
						values = String.Concat(values, ", @footer");
					}
					else
					{
						sql = String.Concat(sql, " `footer` ");
						values = String.Concat(values, " @footer");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
				}
				if (!String.IsNullOrEmpty(emailMessage.LangCode))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `lang_code` ");
						values = String.Concat(values, ", @langCode");
					}
					else
					{
						sql = String.Concat(sql, " `lang_code` ");
						values = String.Concat(values, " @langCode");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
				}
				if (!String.IsNullOrEmpty(emailMessage.Type))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `type` ");
						values = String.Concat(values, ", @type");
					}
					else
					{
						sql = String.Concat(sql, " `type` ");
						values = String.Concat(values, " @type");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@type", emailMessage.Type);
				}
				if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
				}
				if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				emailMessage.Id = cmd.LastInsertedId.ToString();
				response.Add(emailMessage);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
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
		public List<EmailMessageModel> Create(string connectionString, List<EmailMessageModel> emailMessageList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
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
						for (int i = 0; i < emailMessageList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO gus_email_messages (";
								string values = "(";
								if (!String.IsNullOrEmpty(emailMessageList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, emailMessageList[i].Id);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Subject))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `subject` ");
										values = String.Concat(values, ", @subject" + i);
									}

									else
									{
										sql = String.Concat(sql, " `subject` ");
										values = String.Concat(values, "@subject" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@subject" + i, emailMessageList[i].Subject);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Header))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `header` ");
										values = String.Concat(values, ", @header" + i);
									}

									else
									{
										sql = String.Concat(sql, " `header` ");
										values = String.Concat(values, "@header" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@header" + i, emailMessageList[i].Header);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Body))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `body` ");
										values = String.Concat(values, ", @body" + i);
									}

									else
									{
										sql = String.Concat(sql, " `body` ");
										values = String.Concat(values, "@body" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@body" + i, emailMessageList[i].Body);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Url))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `url` ");
										values = String.Concat(values, ", @url" + i);
									}

									else
									{
										sql = String.Concat(sql, " `url` ");
										values = String.Concat(values, "@url" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@url" + i, emailMessageList[i].Url);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Footer))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `footer` ");
										values = String.Concat(values, ", @footer" + i);
									}

									else
									{
										sql = String.Concat(sql, " `footer` ");
										values = String.Concat(values, "@footer" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@footer" + i, emailMessageList[i].Footer);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].LangCode))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `lang_code` ");
										values = String.Concat(values, ", @langCode" + i);
									}

									else
									{
										sql = String.Concat(sql, " `lang_code` ");
										values = String.Concat(values, "@langCode" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@langCode" + i, emailMessageList[i].LangCode);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Type))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `type` ");
										values = String.Concat(values, ", @type" + i);
									}

									else
									{
										sql = String.Concat(sql, " `type` ");
										values = String.Concat(values, "@type" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@type" + i, emailMessageList[i].Type);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(emailMessageList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<EmailMessageModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<EmailMessageModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
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

		public List<EmailMessageModel> Update(string connectionString, EmailMessageModel emailMessage, EmailMessageModel emailMessageWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE gus_email_messages SET ";
				if (!String.IsNullOrEmpty(emailMessage.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", emailMessage.Id);
				}
				if (!String.IsNullOrEmpty(emailMessage.Subject))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `subject` = @subject");
					else
						sql = String.Concat(sql, " `subject` = @subject");

					addAnd = true;
					cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
				}
				if (!String.IsNullOrEmpty(emailMessage.Header))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `header` = @header");
					else
						sql = String.Concat(sql, " `header` = @header");

					addAnd = true;
					cmd.Parameters.AddWithValue("@header", emailMessage.Header);
				}
				if (!String.IsNullOrEmpty(emailMessage.Body))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `body` = @body");
					else
						sql = String.Concat(sql, " `body` = @body");

					addAnd = true;
					cmd.Parameters.AddWithValue("@body", emailMessage.Body);
				}
				if (!String.IsNullOrEmpty(emailMessage.Url))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `url` = @url");
					else
						sql = String.Concat(sql, " `url` = @url");

					addAnd = true;
					cmd.Parameters.AddWithValue("@url", emailMessage.Url);
				}
				if (!String.IsNullOrEmpty(emailMessage.Footer))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `footer` = @footer");
					else
						sql = String.Concat(sql, " `footer` = @footer");

					addAnd = true;
					cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
				}
				if (!String.IsNullOrEmpty(emailMessage.LangCode))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `lang_code` = @langCode");
					else
						sql = String.Concat(sql, " `lang_code` = @langCode");

					addAnd = true;
					cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
				}
				if (!String.IsNullOrEmpty(emailMessage.Type))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `type` = @type");
					else
						sql = String.Concat(sql, " `type` = @type");

					addAnd = true;
					cmd.Parameters.AddWithValue("@type", emailMessage.Type);
				}
				if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
				}
				if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
				}
				if (emailMessageWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(emailMessageWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", emailMessageWhere.Id);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.Subject))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `subject` = @subjectWhere");
						else
							sql = String.Concat(sql, " `subject` = @subjectWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@subjectWhere", emailMessageWhere.Subject);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.Header))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `header` = @headerWhere");
						else
							sql = String.Concat(sql, " `header` = @headerWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@headerWhere", emailMessageWhere.Header);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.Body))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `body` = @bodyWhere");
						else
							sql = String.Concat(sql, " `body` = @bodyWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@bodyWhere", emailMessageWhere.Body);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.Url))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `url` = @urlWhere");
						else
							sql = String.Concat(sql, " `url` = @urlWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@urlWhere", emailMessageWhere.Url);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.Footer))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `footer` = @footerWhere");
						else
							sql = String.Concat(sql, " `footer` = @footerWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@footerWhere", emailMessageWhere.Footer);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.LangCode))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere");
						else
							sql = String.Concat(sql, " `lang_code` = @langCodeWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@langCodeWhere", emailMessageWhere.LangCode);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.Type))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `type` = @typeWhere");
						else
							sql = String.Concat(sql, " `type` = @typeWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@typeWhere", emailMessageWhere.Type);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", emailMessageWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", emailMessageWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(emailMessageWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", emailMessageWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(emailMessage);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
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

		public List<EmailMessageModel> Update(string connectionString, List<EmailMessageModel> emailMessageList, List<EmailMessageModel> emailMessageWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
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
						for (int i = 0; i < emailMessageList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE gus_email_messages SET ";
								if (!String.IsNullOrEmpty(emailMessageList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, emailMessageList[i].Id);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Subject))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `subject` = @subject" + i);
									else
										sql = String.Concat(sql, " `subject` = @subject" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@subject" + i, emailMessageList[i].Subject);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Header))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `header` = @header" + i);
									else
										sql = String.Concat(sql, " `header` = @header" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@header" + i, emailMessageList[i].Header);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Body))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `body` = @body" + i);
									else
										sql = String.Concat(sql, " `body` = @body" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@body" + i, emailMessageList[i].Body);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Url))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `url` = @url" + i);
									else
										sql = String.Concat(sql, " `url` = @url" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@url" + i, emailMessageList[i].Url);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Footer))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `footer` = @footer" + i);
									else
										sql = String.Concat(sql, " `footer` = @footer" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@footer" + i, emailMessageList[i].Footer);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].LangCode))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `lang_code` = @langCode" + i);
									else
										sql = String.Concat(sql, " `lang_code` = @langCode" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@langCode" + i, emailMessageList[i].LangCode);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].Type))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `type` = @type" + i);
									else
										sql = String.Concat(sql, " `type` = @type" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@type" + i, emailMessageList[i].Type);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessageList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, emailMessageList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(emailMessageList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessageList[i].UpdatedAt);
								}
								if (emailMessageWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, emailMessageWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Subject))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `subject` = @subjectWhere" + i);
										else
											sql = String.Concat(sql, " `subject` = @subjectWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@subjectWhere" + i, emailMessageWhereList[i].Subject);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Header))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `header` = @headerWhere" + i);
										else
											sql = String.Concat(sql, " `header` = @headerWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@headerWhere" + i, emailMessageWhereList[i].Header);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Body))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `body` = @bodyWhere" + i);
										else
											sql = String.Concat(sql, " `body` = @bodyWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@bodyWhere" + i, emailMessageWhereList[i].Body);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Url))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `url` = @urlWhere" + i);
										else
											sql = String.Concat(sql, " `url` = @urlWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@urlWhere" + i, emailMessageWhereList[i].Url);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Footer))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `footer` = @footerWhere" + i);
										else
											sql = String.Concat(sql, " `footer` = @footerWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@footerWhere" + i, emailMessageWhereList[i].Footer);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].LangCode))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `lang_code` = @langCodeWhere" + i);
										else
											sql = String.Concat(sql, " `lang_code` = @langCodeWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@langCodeWhere" + i, emailMessageWhereList[i].LangCode);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].Type))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `type` = @typeWhere" + i);
										else
											sql = String.Concat(sql, " `type` = @typeWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@typeWhere" + i, emailMessageWhereList[i].Type);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailMessageWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailMessageWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(emailMessageWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailMessageWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(emailMessageList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<EmailMessageModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<EmailMessageModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
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

		public List<EmailMessageModel> Delete(string connectionString, EmailMessageModel emailMessage)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM gus_email_messages  ";
				if (emailMessage != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(emailMessage.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", emailMessage.Id);
					}
					if (!String.IsNullOrEmpty(emailMessage.Subject))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `subject` = @subject");
						else
							sql = String.Concat(sql, " `subject` = @subject");

						addAnd = true;
						cmd.Parameters.AddWithValue("@subject", emailMessage.Subject);
					}
					if (!String.IsNullOrEmpty(emailMessage.Header))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `header` = @header");
						else
							sql = String.Concat(sql, " `header` = @header");

						addAnd = true;
						cmd.Parameters.AddWithValue("@header", emailMessage.Header);
					}
					if (!String.IsNullOrEmpty(emailMessage.Body))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `body` = @body");
						else
							sql = String.Concat(sql, " `body` = @body");

						addAnd = true;
						cmd.Parameters.AddWithValue("@body", emailMessage.Body);
					}
					if (!String.IsNullOrEmpty(emailMessage.Url))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `url` = @url");
						else
							sql = String.Concat(sql, " `url` = @url");

						addAnd = true;
						cmd.Parameters.AddWithValue("@url", emailMessage.Url);
					}
					if (!String.IsNullOrEmpty(emailMessage.Footer))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `footer` = @footer");
						else
							sql = String.Concat(sql, " `footer` = @footer");

						addAnd = true;
						cmd.Parameters.AddWithValue("@footer", emailMessage.Footer);
					}
					if (!String.IsNullOrEmpty(emailMessage.LangCode))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `lang_code` = @langCode");
						else
							sql = String.Concat(sql, " `lang_code` = @langCode");

						addAnd = true;
						cmd.Parameters.AddWithValue("@langCode", emailMessage.LangCode);
					}
					if (!String.IsNullOrEmpty(emailMessage.Type))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `type` = @type");
						else
							sql = String.Concat(sql, " `type` = @type");

						addAnd = true;
						cmd.Parameters.AddWithValue("@type", emailMessage.Type);
					}
					if (!String.IsNullOrEmpty(emailMessage.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", emailMessage.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(emailMessage.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", emailMessage.CreatedAt);
					}
					if (!String.IsNullOrEmpty(emailMessage.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", emailMessage.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(emailMessage);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
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

		public List<EmailMessageModel> Delete(string connectionString, List<EmailMessageModel> emailMessage)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EmailMessageModel> response = new List<EmailMessageModel>();
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
						for (int i = 0; i < emailMessage.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM gus_email_messages  ";
								if (emailMessage[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(emailMessage[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, emailMessage[i].Id);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].Subject))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `subject` = @subject" + i);
										else
											sql = String.Concat(sql, " `subject` = @subject" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@subject" + i, emailMessage[i].Subject);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].Header))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `header` = @header" + i);
										else
											sql = String.Concat(sql, " `header` = @header" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@header" + i, emailMessage[i].Header);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].Body))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `body` = @body" + i);
										else
											sql = String.Concat(sql, " `body` = @body" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@body" + i, emailMessage[i].Body);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].Url))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `url` = @url" + i);
										else
											sql = String.Concat(sql, " `url` = @url" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@url" + i, emailMessage[i].Url);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].Footer))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `footer` = @footer" + i);
										else
											sql = String.Concat(sql, " `footer` = @footer" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@footer" + i, emailMessage[i].Footer);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].LangCode))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `lang_code` = @langCode" + i);
										else
											sql = String.Concat(sql, " `lang_code` = @langCode" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@langCode" + i, emailMessage[i].LangCode);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].Type))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `type` = @type" + i);
										else
											sql = String.Concat(sql, " `type` = @type" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@type" + i, emailMessage[i].Type);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, emailMessage[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, emailMessage[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(emailMessage[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, emailMessage[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(emailMessage[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<EmailMessageModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<EmailMessageModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EmailMessageModel>();
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
