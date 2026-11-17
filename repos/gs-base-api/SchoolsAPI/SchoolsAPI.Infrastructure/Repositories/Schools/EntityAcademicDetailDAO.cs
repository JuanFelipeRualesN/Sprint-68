using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
	class EntityAcademicDetailDAO
	{
		private MySqlConnection _connection;
		public Dictionary<string, EntityAcademicDetailModel> ReadD(string connectionString, EntityAcademicDetailModel entityAcademicDetail, bool firstOne, bool orderByLastDate, EntityAcademicDetailModel entityAcademicDetailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EntityAcademicDetailModel> response = new Dictionary<string, EntityAcademicDetailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				{
					if (entityAcademicDetail != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
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
							cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `entity_id` = @entityId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `entity_id` = @entityId ");
							}
							cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `term_id` = @termId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `term_id` = @termId ");
							}
							cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `period_quantity` = @periodQuantity ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `period_quantity` = @periodQuantity ");
							}
							cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `days_to_close_period` = @daysToClosePeriod ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `days_to_close_period` = @daysToClosePeriod ");
							}
							cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_start_date` = @fullTermStartDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_start_date` = @fullTermStartDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_end_date` = @fullTermEndDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_end_date` = @fullTermEndDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
						}
					}
					if (entityAcademicDetailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityAcademicDetailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.EntityId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entityAcademicDetailWhereLike.EntityId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.TermId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `term_id` LIKE @termIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `term_id` LIKE @termIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + entityAcademicDetailWhereLike.TermId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.PeriodQuantity))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + entityAcademicDetailWhereLike.PeriodQuantity + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.DaysToClosePeriod))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + entityAcademicDetailWhereLike.DaysToClosePeriod + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermStartDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermStartDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermEndDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermEndDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityAcademicDetailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityAcademicDetailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityAcademicDetailWhereLike.UpdatedAt + "%");
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
								entityAcademicDetail = new EntityAcademicDetailModel();
								if (!reader.IsDBNull(0))
									entityAcademicDetail.Id = reader.GetValue(0).ToString();
								else
									entityAcademicDetail.Id = null;
								if (!reader.IsDBNull(1))
									entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
								else
									entityAcademicDetail.EntityId = null;
								if (!reader.IsDBNull(2))
									entityAcademicDetail.TermId = reader.GetValue(2).ToString();
								else
									entityAcademicDetail.TermId = null;
								if (!reader.IsDBNull(3))
									entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
								else
									entityAcademicDetail.PeriodQuantity = null;
								if (!reader.IsDBNull(4))
									entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
								else
									entityAcademicDetail.DaysToClosePeriod = null;
								if (!reader.IsDBNull(5))
									entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
								else
									entityAcademicDetail.FullTermStartDate = null;
								if (!reader.IsDBNull(6))
									entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
								else
									entityAcademicDetail.FullTermEndDate = null;
								if (!reader.IsDBNull(7))
									entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
								else
									entityAcademicDetail.ModifiedBy = null;
								if (!reader.IsDBNull(8))
									entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
								else
									entityAcademicDetail.CreatedAt = null;
								if (!reader.IsDBNull(9))
									entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
								else
									entityAcademicDetail.UpdatedAt = null;
								response.Add(entityAcademicDetail.Id, entityAcademicDetail);
								entityAcademicDetail = new EntityAcademicDetailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, EntityAcademicDetailModel>();
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
				response = new Dictionary<string, EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, EntityAcademicDetailModel> ReadD(string connectionString, EntityAcademicDetailModel entityAcademicDetail, bool orderByLastDate, int fromRow, int limit, EntityAcademicDetailModel entityAcademicDetailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EntityAcademicDetailModel> response = new Dictionary<string, EntityAcademicDetailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				{
					if (entityAcademicDetail != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
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
							cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `entity_id` = @entityId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `entity_id` = @entityId ");
							}
							cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `term_id` = @termId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `term_id` = @termId ");
							}
							cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `period_quantity` = @periodQuantity ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `period_quantity` = @periodQuantity ");
							}
							cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `days_to_close_period` = @daysToClosePeriod ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `days_to_close_period` = @daysToClosePeriod ");
							}
							cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_start_date` = @fullTermStartDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_start_date` = @fullTermStartDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_end_date` = @fullTermEndDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_end_date` = @fullTermEndDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
						}
					}
					if (entityAcademicDetailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityAcademicDetailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.EntityId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entityAcademicDetailWhereLike.EntityId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.TermId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `term_id` LIKE @termIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `term_id` LIKE @termIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + entityAcademicDetailWhereLike.TermId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.PeriodQuantity))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + entityAcademicDetailWhereLike.PeriodQuantity + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.DaysToClosePeriod))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + entityAcademicDetailWhereLike.DaysToClosePeriod + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermStartDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermStartDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermEndDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermEndDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityAcademicDetailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityAcademicDetailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityAcademicDetailWhereLike.UpdatedAt + "%");
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
								entityAcademicDetail = new EntityAcademicDetailModel();
								if (!reader.IsDBNull(0))
									entityAcademicDetail.Id = reader.GetValue(0).ToString();
								else
									entityAcademicDetail.Id = null;
								if (!reader.IsDBNull(1))
									entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
								else
									entityAcademicDetail.EntityId = null;
								if (!reader.IsDBNull(2))
									entityAcademicDetail.TermId = reader.GetValue(2).ToString();
								else
									entityAcademicDetail.TermId = null;
								if (!reader.IsDBNull(3))
									entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
								else
									entityAcademicDetail.PeriodQuantity = null;
								if (!reader.IsDBNull(4))
									entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
								else
									entityAcademicDetail.DaysToClosePeriod = null;
								if (!reader.IsDBNull(5))
									entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
								else
									entityAcademicDetail.FullTermStartDate = null;
								if (!reader.IsDBNull(6))
									entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
								else
									entityAcademicDetail.FullTermEndDate = null;
								if (!reader.IsDBNull(7))
									entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
								else
									entityAcademicDetail.ModifiedBy = null;
								if (!reader.IsDBNull(8))
									entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
								else
									entityAcademicDetail.CreatedAt = null;
								if (!reader.IsDBNull(9))
									entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
								else
									entityAcademicDetail.UpdatedAt = null;
								response.Add(entityAcademicDetail.Id, entityAcademicDetail);
								entityAcademicDetail = new EntityAcademicDetailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new Dictionary<string, EntityAcademicDetailModel>();
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
				response = new Dictionary<string, EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, EntityAcademicDetailModel> ReadD(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EntityAcademicDetailModel> response = new Dictionary<string, EntityAcademicDetailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				if (entityAcademicDetailWhere != null && entityAcademicDetailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < entityAcademicDetailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].EntityId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `entity_id` = @entityId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetailWhere[i].EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].TermId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `term_id` = @termId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetailWhere[i].TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].PeriodQuantity))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `period_quantity` = @periodQuantity" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetailWhere[i].PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].DaysToClosePeriod))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetailWhere[i].DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermStartDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetailWhere[i].FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermEndDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetailWhere[i].FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != entityAcademicDetailWhere.Count - 1)
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
							EntityAcademicDetailModel entityAcademicDetail = new EntityAcademicDetailModel(); if (!reader.IsDBNull(0))
								entityAcademicDetail.Id = reader.GetValue(0).ToString();
							else
								entityAcademicDetail.Id = null;
							if (!reader.IsDBNull(1))
								entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
							else
								entityAcademicDetail.EntityId = null;
							if (!reader.IsDBNull(2))
								entityAcademicDetail.TermId = reader.GetValue(2).ToString();
							else
								entityAcademicDetail.TermId = null;
							if (!reader.IsDBNull(3))
								entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
							else
								entityAcademicDetail.PeriodQuantity = null;
							if (!reader.IsDBNull(4))
								entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
							else
								entityAcademicDetail.DaysToClosePeriod = null;
							if (!reader.IsDBNull(5))
								entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
							else
								entityAcademicDetail.FullTermStartDate = null;
							if (!reader.IsDBNull(6))
								entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
							else
								entityAcademicDetail.FullTermEndDate = null;
							if (!reader.IsDBNull(7))
								entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
							else
								entityAcademicDetail.ModifiedBy = null;
							if (!reader.IsDBNull(8))
								entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
							else
								entityAcademicDetail.CreatedAt = null;
							if (!reader.IsDBNull(9))
								entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
							else
								entityAcademicDetail.UpdatedAt = null;
							response.Add(entityAcademicDetail.Id, entityAcademicDetail);
							entityAcademicDetail = new EntityAcademicDetailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, EntityAcademicDetailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public Dictionary<string, EntityAcademicDetailModel> ReadD(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetailWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			Dictionary<string, EntityAcademicDetailModel> response = new Dictionary<string, EntityAcademicDetailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				if (entityAcademicDetailWhere != null && entityAcademicDetailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < entityAcademicDetailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].EntityId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `entity_id` = @entityId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetailWhere[i].EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].TermId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `term_id` = @termId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetailWhere[i].TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].PeriodQuantity))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `period_quantity` = @periodQuantity" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetailWhere[i].PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].DaysToClosePeriod))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetailWhere[i].DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermStartDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetailWhere[i].FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermEndDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetailWhere[i].FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != entityAcademicDetailWhere.Count - 1)
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
							EntityAcademicDetailModel entityAcademicDetail = new EntityAcademicDetailModel(); if (!reader.IsDBNull(0))
								entityAcademicDetail.Id = reader.GetValue(0).ToString();
							else
								entityAcademicDetail.Id = null;
							if (!reader.IsDBNull(1))
								entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
							else
								entityAcademicDetail.EntityId = null;
							if (!reader.IsDBNull(2))
								entityAcademicDetail.TermId = reader.GetValue(2).ToString();
							else
								entityAcademicDetail.TermId = null;
							if (!reader.IsDBNull(3))
								entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
							else
								entityAcademicDetail.PeriodQuantity = null;
							if (!reader.IsDBNull(4))
								entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
							else
								entityAcademicDetail.DaysToClosePeriod = null;
							if (!reader.IsDBNull(5))
								entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
							else
								entityAcademicDetail.FullTermStartDate = null;
							if (!reader.IsDBNull(6))
								entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
							else
								entityAcademicDetail.FullTermEndDate = null;
							if (!reader.IsDBNull(7))
								entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
							else
								entityAcademicDetail.ModifiedBy = null;
							if (!reader.IsDBNull(8))
								entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
							else
								entityAcademicDetail.CreatedAt = null;
							if (!reader.IsDBNull(9))
								entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
							else
								entityAcademicDetail.UpdatedAt = null;
							response.Add(entityAcademicDetail.Id, entityAcademicDetail);
							entityAcademicDetail = new EntityAcademicDetailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new Dictionary<string, EntityAcademicDetailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new Dictionary<string, EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EntityAcademicDetailModel> Read(string connectionString, EntityAcademicDetailModel entityAcademicDetail, bool firstOne, bool orderByLastDate, EntityAcademicDetailModel entityAcademicDetailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				{
					if (entityAcademicDetail != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
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
							cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `entity_id` = @entityId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `entity_id` = @entityId ");
							}
							cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `term_id` = @termId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `term_id` = @termId ");
							}
							cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `period_quantity` = @periodQuantity ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `period_quantity` = @periodQuantity ");
							}
							cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `days_to_close_period` = @daysToClosePeriod ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `days_to_close_period` = @daysToClosePeriod ");
							}
							cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_start_date` = @fullTermStartDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_start_date` = @fullTermStartDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_end_date` = @fullTermEndDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_end_date` = @fullTermEndDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
						}
					}
					if (entityAcademicDetailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityAcademicDetailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.EntityId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entityAcademicDetailWhereLike.EntityId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.TermId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `term_id` LIKE @termIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `term_id` LIKE @termIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + entityAcademicDetailWhereLike.TermId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.PeriodQuantity))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + entityAcademicDetailWhereLike.PeriodQuantity + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.DaysToClosePeriod))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + entityAcademicDetailWhereLike.DaysToClosePeriod + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermStartDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermStartDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermEndDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermEndDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityAcademicDetailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityAcademicDetailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityAcademicDetailWhereLike.UpdatedAt + "%");
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
								entityAcademicDetail = new EntityAcademicDetailModel();
								if (!reader.IsDBNull(0))
									entityAcademicDetail.Id = reader.GetValue(0).ToString();
								else
									entityAcademicDetail.Id = null;
								if (!reader.IsDBNull(1))
									entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
								else
									entityAcademicDetail.EntityId = null;
								if (!reader.IsDBNull(2))
									entityAcademicDetail.TermId = reader.GetValue(2).ToString();
								else
									entityAcademicDetail.TermId = null;
								if (!reader.IsDBNull(3))
									entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
								else
									entityAcademicDetail.PeriodQuantity = null;
								if (!reader.IsDBNull(4))
									entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
								else
									entityAcademicDetail.DaysToClosePeriod = null;
								if (!reader.IsDBNull(5))
									entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
								else
									entityAcademicDetail.FullTermStartDate = null;
								if (!reader.IsDBNull(6))
									entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
								else
									entityAcademicDetail.FullTermEndDate = null;
								if (!reader.IsDBNull(7))
									entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
								else
									entityAcademicDetail.ModifiedBy = null;
								if (!reader.IsDBNull(8))
									entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
								else
									entityAcademicDetail.CreatedAt = null;
								if (!reader.IsDBNull(9))
									entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
								else
									entityAcademicDetail.UpdatedAt = null;
								response.Add(entityAcademicDetail);
								entityAcademicDetail = new EntityAcademicDetailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<EntityAcademicDetailModel>();
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
				response = new List<EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EntityAcademicDetailModel> Read(string connectionString, EntityAcademicDetailModel entityAcademicDetail, bool orderByLastDate, int fromRow, int limit, EntityAcademicDetailModel entityAcademicDetailWhereLike = null)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			bool addAnd = false;
			bool addAndWhereLike = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				{
					if (entityAcademicDetail != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
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
							cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `entity_id` = @entityId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `entity_id` = @entityId ");
							}
							cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `term_id` = @termId ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `term_id` = @termId ");
							}
							cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `period_quantity` = @periodQuantity ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `period_quantity` = @periodQuantity ");
							}
							cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `days_to_close_period` = @daysToClosePeriod ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `days_to_close_period` = @daysToClosePeriod ");
							}
							cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_start_date` = @fullTermStartDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_start_date` = @fullTermStartDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
						{
							if (addAnd != true)
							{
								sql = String.Concat(sql, " where `full_term_end_date` = @fullTermEndDate ");
								addAnd = true;
							}
							else
							{
								sql = String.Concat(sql, " and `full_term_end_date` = @fullTermEndDate ");
							}
							cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
						}
					}
					if (entityAcademicDetailWhereLike != null)
					{
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.Id))
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
							cmd.Parameters.AddWithValue("@idWhereLike", "%" + entityAcademicDetailWhereLike.Id + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.EntityId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `entity_id` LIKE @entityIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `entity_id` LIKE @entityIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@entityIdWhereLike", "%" + entityAcademicDetailWhereLike.EntityId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.TermId))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `term_id` LIKE @termIdWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `term_id` LIKE @termIdWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + entityAcademicDetailWhereLike.TermId + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.PeriodQuantity))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `period_quantity` LIKE @periodQuantityWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + entityAcademicDetailWhereLike.PeriodQuantity + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.DaysToClosePeriod))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `days_to_close_period` LIKE @daysToClosePeriodWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + entityAcademicDetailWhereLike.DaysToClosePeriod + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermStartDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_start_date` LIKE @fullTermStartDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermStartDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.FullTermEndDate))
						{
							if (addAnd || addAndWhereLike)
							{
								sql = String.Concat(sql, " and `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							else
							{
								sql = String.Concat(sql, " where `full_term_end_date` LIKE @fullTermEndDateWhereLike ");
							}
							addAndWhereLike = true;
							cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + entityAcademicDetailWhereLike.FullTermEndDate + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.ModifiedBy))
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
							cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + entityAcademicDetailWhereLike.ModifiedBy + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.CreatedAt))
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
							cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + entityAcademicDetailWhereLike.CreatedAt + "%");
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhereLike.UpdatedAt))
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
							cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + entityAcademicDetailWhereLike.UpdatedAt + "%");
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
								entityAcademicDetail = new EntityAcademicDetailModel();
								if (!reader.IsDBNull(0))
									entityAcademicDetail.Id = reader.GetValue(0).ToString();
								else
									entityAcademicDetail.Id = null;
								if (!reader.IsDBNull(1))
									entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
								else
									entityAcademicDetail.EntityId = null;
								if (!reader.IsDBNull(2))
									entityAcademicDetail.TermId = reader.GetValue(2).ToString();
								else
									entityAcademicDetail.TermId = null;
								if (!reader.IsDBNull(3))
									entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
								else
									entityAcademicDetail.PeriodQuantity = null;
								if (!reader.IsDBNull(4))
									entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
								else
									entityAcademicDetail.DaysToClosePeriod = null;
								if (!reader.IsDBNull(5))
									entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
								else
									entityAcademicDetail.FullTermStartDate = null;
								if (!reader.IsDBNull(6))
									entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
								else
									entityAcademicDetail.FullTermEndDate = null;
								if (!reader.IsDBNull(7))
									entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
								else
									entityAcademicDetail.ModifiedBy = null;
								if (!reader.IsDBNull(8))
									entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
								else
									entityAcademicDetail.CreatedAt = null;
								if (!reader.IsDBNull(9))
									entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
								else
									entityAcademicDetail.UpdatedAt = null;
								response.Add(entityAcademicDetail);
								entityAcademicDetail = new EntityAcademicDetailModel();
							}
					}
					catch (ArgumentException e)
					{
						string MessageString = " Read error occured: " + e.Message;
						response = new List<EntityAcademicDetailModel>();
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
				response = new List<EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EntityAcademicDetailModel> Read(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				if (entityAcademicDetailWhere != null && entityAcademicDetailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < entityAcademicDetailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].EntityId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `entity_id` = @entityId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetailWhere[i].EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].TermId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `term_id` = @termId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetailWhere[i].TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].PeriodQuantity))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `period_quantity` = @periodQuantity" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetailWhere[i].PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].DaysToClosePeriod))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetailWhere[i].DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermStartDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetailWhere[i].FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermEndDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetailWhere[i].FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != entityAcademicDetailWhere.Count - 1)
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
							EntityAcademicDetailModel entityAcademicDetail = new EntityAcademicDetailModel(); if (!reader.IsDBNull(0))
								entityAcademicDetail.Id = reader.GetValue(0).ToString();
							else
								entityAcademicDetail.Id = null;
							if (!reader.IsDBNull(1))
								entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
							else
								entityAcademicDetail.EntityId = null;
							if (!reader.IsDBNull(2))
								entityAcademicDetail.TermId = reader.GetValue(2).ToString();
							else
								entityAcademicDetail.TermId = null;
							if (!reader.IsDBNull(3))
								entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
							else
								entityAcademicDetail.PeriodQuantity = null;
							if (!reader.IsDBNull(4))
								entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
							else
								entityAcademicDetail.DaysToClosePeriod = null;
							if (!reader.IsDBNull(5))
								entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
							else
								entityAcademicDetail.FullTermStartDate = null;
							if (!reader.IsDBNull(6))
								entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
							else
								entityAcademicDetail.FullTermEndDate = null;
							if (!reader.IsDBNull(7))
								entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
							else
								entityAcademicDetail.ModifiedBy = null;
							if (!reader.IsDBNull(8))
								entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
							else
								entityAcademicDetail.CreatedAt = null;
							if (!reader.IsDBNull(9))
								entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
							else
								entityAcademicDetail.UpdatedAt = null;
							response.Add(entityAcademicDetail);
							entityAcademicDetail = new EntityAcademicDetailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<EntityAcademicDetailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EntityAcademicDetailModel> Read(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetailWhere, int fromRow, int limit)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sqlIn = "";
				string sql = "SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_entity_academic_details ";
				if (entityAcademicDetailWhere != null && entityAcademicDetailWhere.Count > 0)
				{
					sqlIn += " WHERE ";
					for (int i = 0; i < entityAcademicDetailWhere.Count; i++)
					{
						sqlIn += "(";
						bool addAnd = false;
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].Id))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `id` = @id" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetailWhere[i].Id);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].EntityId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `entity_id` = @entityId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetailWhere[i].EntityId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].TermId))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `term_id` = @termId" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetailWhere[i].TermId);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].PeriodQuantity))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `period_quantity` = @periodQuantity" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetailWhere[i].PeriodQuantity);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].DaysToClosePeriod))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetailWhere[i].DaysToClosePeriod);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermStartDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetailWhere[i].FullTermStartDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].FullTermEndDate))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetailWhere[i].FullTermEndDate);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].ModifiedBy))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `modified_by` = @modifiedBy" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetailWhere[i].ModifiedBy);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].CreatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `created_at` = @createdAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetailWhere[i].CreatedAt);
						}
						if (!String.IsNullOrEmpty(entityAcademicDetailWhere[i].UpdatedAt))
						{
							if (addAnd)
								sqlIn += " AND ";

							sqlIn += " `updated_at` = @updatedAt" + i;
							addAnd = true;
							cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetailWhere[i].UpdatedAt);
						}
						sqlIn += ")";
						if (i != entityAcademicDetailWhere.Count - 1)
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
							EntityAcademicDetailModel entityAcademicDetail = new EntityAcademicDetailModel(); if (!reader.IsDBNull(0))
								entityAcademicDetail.Id = reader.GetValue(0).ToString();
							else
								entityAcademicDetail.Id = null;
							if (!reader.IsDBNull(1))
								entityAcademicDetail.EntityId = reader.GetValue(1).ToString();
							else
								entityAcademicDetail.EntityId = null;
							if (!reader.IsDBNull(2))
								entityAcademicDetail.TermId = reader.GetValue(2).ToString();
							else
								entityAcademicDetail.TermId = null;
							if (!reader.IsDBNull(3))
								entityAcademicDetail.PeriodQuantity = reader.GetValue(3).ToString();
							else
								entityAcademicDetail.PeriodQuantity = null;
							if (!reader.IsDBNull(4))
								entityAcademicDetail.DaysToClosePeriod = reader.GetValue(4).ToString();
							else
								entityAcademicDetail.DaysToClosePeriod = null;
							if (!reader.IsDBNull(5))
								entityAcademicDetail.FullTermStartDate = reader.GetValue(5).ToString();
							else
								entityAcademicDetail.FullTermStartDate = null;
							if (!reader.IsDBNull(6))
								entityAcademicDetail.FullTermEndDate = reader.GetValue(6).ToString();
							else
								entityAcademicDetail.FullTermEndDate = null;
							if (!reader.IsDBNull(7))
								entityAcademicDetail.ModifiedBy = reader.GetValue(7).ToString();
							else
								entityAcademicDetail.ModifiedBy = null;
							if (!reader.IsDBNull(8))
								entityAcademicDetail.CreatedAt = reader.GetValue(8).ToString();
							else
								entityAcademicDetail.CreatedAt = null;
							if (!reader.IsDBNull(9))
								entityAcademicDetail.UpdatedAt = reader.GetValue(9).ToString();
							else
								entityAcademicDetail.UpdatedAt = null;
							response.Add(entityAcademicDetail);
							entityAcademicDetail = new EntityAcademicDetailModel();
						}
				}
				catch (ArgumentException e)
				{
					string MessageString = " Read error occured: " + e.Message;
					response = new List<EntityAcademicDetailModel>();
					reader.Close();
				}
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
			}
			finally
			{
				if (_connection != null)
					dbConn.CloseConnection(_connection);
			}
			return response;
		}
		public List<EntityAcademicDetailModel> Create(string connectionString, EntityAcademicDetailModel entityAcademicDetail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "INSERT INTO sch_entity_academic_details (";
				string values = "(";
				if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
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
					cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `entity_id` ");
						values = String.Concat(values, ", @entityId");
					}
					else
					{
						sql = String.Concat(sql, " `entity_id` ");
						values = String.Concat(values, " @entityId");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `term_id` ");
						values = String.Concat(values, ", @termId");
					}
					else
					{
						sql = String.Concat(sql, " `term_id` ");
						values = String.Concat(values, " @termId");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `period_quantity` ");
						values = String.Concat(values, ", @periodQuantity");
					}
					else
					{
						sql = String.Concat(sql, " `period_quantity` ");
						values = String.Concat(values, " @periodQuantity");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `days_to_close_period` ");
						values = String.Concat(values, ", @daysToClosePeriod");
					}
					else
					{
						sql = String.Concat(sql, " `days_to_close_period` ");
						values = String.Concat(values, " @daysToClosePeriod");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `full_term_start_date` ");
						values = String.Concat(values, ", @fullTermStartDate");
					}
					else
					{
						sql = String.Concat(sql, " `full_term_start_date` ");
						values = String.Concat(values, " @fullTermStartDate");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
				{
					if (addAnd)
					{
						sql = String.Concat(sql, ", `full_term_end_date` ");
						values = String.Concat(values, ", @fullTermEndDate");
					}
					else
					{
						sql = String.Concat(sql, " `full_term_end_date` ");
						values = String.Concat(values, " @fullTermEndDate");
					}
					addAnd = true;
					cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
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
					cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
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
					cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
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
					cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
				}
				sql = String.Concat(sql, ")");
				values = String.Concat(values, ")");
				cmd.CommandText = String.Concat(sql, " VALUES ", values);
				cmd.ExecuteNonQuery();
				entityAcademicDetail.Id = cmd.LastInsertedId.ToString();
				response.Add(entityAcademicDetail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
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
		public List<EntityAcademicDetailModel> Create(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetailList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
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
						for (int i = 0; i < entityAcademicDetailList.Count; i++)
						{
							bool addAnd = false;
							try
							{
								string sql = "INSERT INTO sch_entity_academic_details (";
								string values = "(";
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].Id))
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
									cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetailList[i].Id);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].EntityId))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `entity_id` ");
										values = String.Concat(values, ", @entityId" + i);
									}

									else
									{
										sql = String.Concat(sql, " `entity_id` ");
										values = String.Concat(values, "@entityId" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetailList[i].EntityId);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].TermId))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `term_id` ");
										values = String.Concat(values, ", @termId" + i);
									}

									else
									{
										sql = String.Concat(sql, " `term_id` ");
										values = String.Concat(values, "@termId" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetailList[i].TermId);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].PeriodQuantity))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `period_quantity` ");
										values = String.Concat(values, ", @periodQuantity" + i);
									}

									else
									{
										sql = String.Concat(sql, " `period_quantity` ");
										values = String.Concat(values, "@periodQuantity" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetailList[i].PeriodQuantity);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].DaysToClosePeriod))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `days_to_close_period` ");
										values = String.Concat(values, ", @daysToClosePeriod" + i);
									}

									else
									{
										sql = String.Concat(sql, " `days_to_close_period` ");
										values = String.Concat(values, "@daysToClosePeriod" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetailList[i].DaysToClosePeriod);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].FullTermStartDate))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `full_term_start_date` ");
										values = String.Concat(values, ", @fullTermStartDate" + i);
									}

									else
									{
										sql = String.Concat(sql, " `full_term_start_date` ");
										values = String.Concat(values, "@fullTermStartDate" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetailList[i].FullTermStartDate);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].FullTermEndDate))
								{
									if (addAnd)
									{
										sql = String.Concat(sql, ", `full_term_end_date` ");
										values = String.Concat(values, ", @fullTermEndDate" + i);
									}

									else
									{
										sql = String.Concat(sql, " `full_term_end_date` ");
										values = String.Concat(values, "@fullTermEndDate" + i);
									}
									addAnd = true;
									cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetailList[i].FullTermEndDate);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].ModifiedBy))
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
									cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetailList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].CreatedAt))
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
									cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetailList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].UpdatedAt))
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
									cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetailList[i].UpdatedAt);
								}
								sql = String.Concat(sql, ")");
								values = String.Concat(values, ")");
								cmd.CommandText = String.Concat(sql, " VALUES ", values);
								cmd.ExecuteNonQuery();
								response.Add(entityAcademicDetailList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
								response = new List<EntityAcademicDetailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<EntityAcademicDetailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
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

		public List<EntityAcademicDetailModel> Update(string connectionString, EntityAcademicDetailModel entityAcademicDetail, EntityAcademicDetailModel entityAcademicDetailWhere)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "UPDATE sch_entity_academic_details SET ";
				if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `id` = @id");
					else
						sql = String.Concat(sql, " `id` = @id");

					addAnd = true;
					cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `entity_id` = @entityId");
					else
						sql = String.Concat(sql, " `entity_id` = @entityId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `term_id` = @termId");
					else
						sql = String.Concat(sql, " `term_id` = @termId");

					addAnd = true;
					cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `period_quantity` = @periodQuantity");
					else
						sql = String.Concat(sql, " `period_quantity` = @periodQuantity");

					addAnd = true;
					cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `days_to_close_period` = @daysToClosePeriod");
					else
						sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod");

					addAnd = true;
					cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `full_term_start_date` = @fullTermStartDate");
					else
						sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate");

					addAnd = true;
					cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `full_term_end_date` = @fullTermEndDate");
					else
						sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate");

					addAnd = true;
					cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
					else
						sql = String.Concat(sql, " `modified_by` = @modifiedBy");

					addAnd = true;
					cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `created_at` = @createdAt");
					else
						sql = String.Concat(sql, " `created_at` = @createdAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
				}
				if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
				{
					if (addAnd)
						sql = String.Concat(sql, ", `updated_at` = @updatedAt");
					else
						sql = String.Concat(sql, " `updated_at` = @updatedAt");

					addAnd = true;
					cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
				}
				if (entityAcademicDetailWhere != null)
				{
					sql = String.Concat(sql, " WHERE ");
					addAnd = false; //Reset the addAnd for the WHERES
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @idWhere");
						else
							sql = String.Concat(sql, " `id` = @idWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@idWhere", entityAcademicDetailWhere.Id);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.EntityId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `entity_id` = @entityIdWhere");
						else
							sql = String.Concat(sql, " `entity_id` = @entityIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@entityIdWhere", entityAcademicDetailWhere.EntityId);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.TermId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `term_id` = @termIdWhere");
						else
							sql = String.Concat(sql, " `term_id` = @termIdWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@termIdWhere", entityAcademicDetailWhere.TermId);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.PeriodQuantity))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `period_quantity` = @periodQuantityWhere");
						else
							sql = String.Concat(sql, " `period_quantity` = @periodQuantityWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@periodQuantityWhere", entityAcademicDetailWhere.PeriodQuantity);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.DaysToClosePeriod))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriodWhere");
						else
							sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriodWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@daysToClosePeriodWhere", entityAcademicDetailWhere.DaysToClosePeriod);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.FullTermStartDate))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDateWhere");
						else
							sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDateWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@fullTermStartDateWhere", entityAcademicDetailWhere.FullTermStartDate);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.FullTermEndDate))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDateWhere");
						else
							sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDateWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@fullTermEndDateWhere", entityAcademicDetailWhere.FullTermEndDate);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedByWhere", entityAcademicDetailWhere.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
						else
							sql = String.Concat(sql, " `created_at` = @createdAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAtWhere", entityAcademicDetailWhere.CreatedAt);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetailWhere.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAtWhere", entityAcademicDetailWhere.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				response.Add(entityAcademicDetail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
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

		public List<EntityAcademicDetailModel> Update(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetailList, List<EntityAcademicDetailModel> entityAcademicDetailWhereList)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
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
						for (int i = 0; i < entityAcademicDetailList.Count; i++)
						{
							try
							{
								bool addAnd = false;

								string sql = "UPDATE sch_entity_academic_details SET ";
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].Id))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `id` = @id" + i);
									else
										sql = String.Concat(sql, " `id` = @id" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetailList[i].Id);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].EntityId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `entity_id` = @entityId" + i);
									else
										sql = String.Concat(sql, " `entity_id` = @entityId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetailList[i].EntityId);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].TermId))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `term_id` = @termId" + i);
									else
										sql = String.Concat(sql, " `term_id` = @termId" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetailList[i].TermId);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].PeriodQuantity))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `period_quantity` = @periodQuantity" + i);
									else
										sql = String.Concat(sql, " `period_quantity` = @periodQuantity" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetailList[i].PeriodQuantity);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].DaysToClosePeriod))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `days_to_close_period` = @daysToClosePeriod" + i);
									else
										sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetailList[i].DaysToClosePeriod);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].FullTermStartDate))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `full_term_start_date` = @fullTermStartDate" + i);
									else
										sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetailList[i].FullTermStartDate);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].FullTermEndDate))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `full_term_end_date` = @fullTermEndDate" + i);
									else
										sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetailList[i].FullTermEndDate);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].ModifiedBy))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
									else
										sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetailList[i].ModifiedBy);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].CreatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
									else
										sql = String.Concat(sql, " `created_at` = @createdAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetailList[i].CreatedAt);
								}
								if (!String.IsNullOrEmpty(entityAcademicDetailList[i].UpdatedAt))
								{
									if (addAnd)
										sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
									else
										sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

									addAnd = true;
									cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetailList[i].UpdatedAt);
								}
								if (entityAcademicDetailWhereList[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false; // Reset addAnd for the Where
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @idWhere" + i);
										else
											sql = String.Concat(sql, " `id` = @idWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@idWhere" + i, entityAcademicDetailWhereList[i].Id);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].EntityId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `entity_id` = @entityIdWhere" + i);
										else
											sql = String.Concat(sql, " `entity_id` = @entityIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@entityIdWhere" + i, entityAcademicDetailWhereList[i].EntityId);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].TermId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `term_id` = @termIdWhere" + i);
										else
											sql = String.Concat(sql, " `term_id` = @termIdWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@termIdWhere" + i, entityAcademicDetailWhereList[i].TermId);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].PeriodQuantity))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `period_quantity` = @periodQuantityWhere" + i);
										else
											sql = String.Concat(sql, " `period_quantity` = @periodQuantityWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@periodQuantityWhere" + i, entityAcademicDetailWhereList[i].PeriodQuantity);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].DaysToClosePeriod))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriodWhere" + i);
										else
											sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriodWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@daysToClosePeriodWhere" + i, entityAcademicDetailWhereList[i].DaysToClosePeriod);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].FullTermStartDate))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDateWhere" + i);
										else
											sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDateWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@fullTermStartDateWhere" + i, entityAcademicDetailWhereList[i].FullTermStartDate);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].FullTermEndDate))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDateWhere" + i);
										else
											sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDateWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@fullTermEndDateWhere" + i, entityAcademicDetailWhereList[i].FullTermEndDate);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedByWhere" + i, entityAcademicDetailWhereList[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAtWhere" + i, entityAcademicDetailWhereList[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetailWhereList[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAtWhere" + i, entityAcademicDetailWhereList[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(entityAcademicDetailList[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<EntityAcademicDetailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<EntityAcademicDetailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
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

		public List<EntityAcademicDetailModel> Delete(string connectionString, EntityAcademicDetailModel entityAcademicDetail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
			bool addAnd = false;
			try
			{
				_connection = dbConn.GetConnection();
				MySqlCommand cmd = new MySqlCommand
				{
					Connection = _connection
				};
				string sql = "DELETE FROM sch_entity_academic_details  ";
				if (entityAcademicDetail != null)
				{
					sql += " WHERE ";
					if (!String.IsNullOrEmpty(entityAcademicDetail.Id))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `id` = @id");
						else
							sql = String.Concat(sql, " `id` = @id");

						addAnd = true;
						cmd.Parameters.AddWithValue("@id", entityAcademicDetail.Id);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.EntityId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `entity_id` = @entityId");
						else
							sql = String.Concat(sql, " `entity_id` = @entityId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@entityId", entityAcademicDetail.EntityId);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.TermId))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `term_id` = @termId");
						else
							sql = String.Concat(sql, " `term_id` = @termId");

						addAnd = true;
						cmd.Parameters.AddWithValue("@termId", entityAcademicDetail.TermId);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.PeriodQuantity))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `period_quantity` = @periodQuantity");
						else
							sql = String.Concat(sql, " `period_quantity` = @periodQuantity");

						addAnd = true;
						cmd.Parameters.AddWithValue("@periodQuantity", entityAcademicDetail.PeriodQuantity);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.DaysToClosePeriod))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriod");
						else
							sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod");

						addAnd = true;
						cmd.Parameters.AddWithValue("@daysToClosePeriod", entityAcademicDetail.DaysToClosePeriod);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermStartDate))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDate");
						else
							sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate");

						addAnd = true;
						cmd.Parameters.AddWithValue("@fullTermStartDate", entityAcademicDetail.FullTermStartDate);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.FullTermEndDate))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDate");
						else
							sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate");

						addAnd = true;
						cmd.Parameters.AddWithValue("@fullTermEndDate", entityAcademicDetail.FullTermEndDate);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.ModifiedBy))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
						else
							sql = String.Concat(sql, " `modified_by` = @modifiedBy");

						addAnd = true;
						cmd.Parameters.AddWithValue("@modifiedBy", entityAcademicDetail.ModifiedBy);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.CreatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `created_at` = @createdAt");
						else
							sql = String.Concat(sql, " `created_at` = @createdAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@createdAt", entityAcademicDetail.CreatedAt);
					}
					if (!String.IsNullOrEmpty(entityAcademicDetail.UpdatedAt))
					{
						if (addAnd)
							sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
						else
							sql = String.Concat(sql, " `updated_at` = @updatedAt");

						addAnd = true;
						cmd.Parameters.AddWithValue("@updatedAt", entityAcademicDetail.UpdatedAt);
					}
				}
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				response.Add(entityAcademicDetail);
			}
			catch (MySqlException e)
			{
				string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
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

		public List<EntityAcademicDetailModel> Delete(string connectionString, List<EntityAcademicDetailModel> entityAcademicDetail)
		{
			DBConnection dbConn = new DBConnection(connectionString);
			List<EntityAcademicDetailModel> response = new List<EntityAcademicDetailModel>();
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
						for (int i = 0; i < entityAcademicDetail.Count; i++)
						{
							try
							{
								bool addAnd = false;
								string sql = "DELETE FROM sch_entity_academic_details  ";
								if (entityAcademicDetail[i] != null)
								{
									sql = String.Concat(sql, " WHERE ");
									addAnd = false;
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].Id))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `id` = @id" + i);
										else
											sql = String.Concat(sql, " `id` = @id" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@id" + i, entityAcademicDetail[i].Id);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].EntityId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `entity_id` = @entityId" + i);
										else
											sql = String.Concat(sql, " `entity_id` = @entityId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@entityId" + i, entityAcademicDetail[i].EntityId);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].TermId))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `term_id` = @termId" + i);
										else
											sql = String.Concat(sql, " `term_id` = @termId" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@termId" + i, entityAcademicDetail[i].TermId);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].PeriodQuantity))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `period_quantity` = @periodQuantity" + i);
										else
											sql = String.Concat(sql, " `period_quantity` = @periodQuantity" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@periodQuantity" + i, entityAcademicDetail[i].PeriodQuantity);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].DaysToClosePeriod))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriod" + i);
										else
											sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, entityAcademicDetail[i].DaysToClosePeriod);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].FullTermStartDate))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDate" + i);
										else
											sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@fullTermStartDate" + i, entityAcademicDetail[i].FullTermStartDate);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].FullTermEndDate))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDate" + i);
										else
											sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@fullTermEndDate" + i, entityAcademicDetail[i].FullTermEndDate);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].ModifiedBy))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
										else
											sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@modifiedBy" + i, entityAcademicDetail[i].ModifiedBy);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].CreatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
										else
											sql = String.Concat(sql, " `created_at` = @createdAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@createdAt" + i, entityAcademicDetail[i].CreatedAt);
									}
									if (!String.IsNullOrEmpty(entityAcademicDetail[i].UpdatedAt))
									{
										if (addAnd)
											sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
										else
											sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

										addAnd = true;
										cmd.Parameters.AddWithValue("@updatedAt" + i, entityAcademicDetail[i].UpdatedAt);
									}
								}
								cmd.CommandText = sql;
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								response.Add(entityAcademicDetail[i]);
							}
							catch (MySqlException e)
							{
								string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
								response = new List<EntityAcademicDetailModel>();
							}
						}
						trans.Commit();
					}
					catch (MySqlException e)
					{
						string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
						trans.Rollback();
						response = new List<EntityAcademicDetailModel>();
					}
				}
			}
			catch (MySqlException e)
			{
				string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
				response = new List<EntityAcademicDetailModel>();
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
