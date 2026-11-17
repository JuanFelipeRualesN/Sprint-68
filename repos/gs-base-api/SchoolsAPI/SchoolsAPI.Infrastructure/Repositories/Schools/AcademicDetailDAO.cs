using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class AcademicDetailDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, AcademicDetailModel> ReadD(string connectionString, AcademicDetailModel academicDetail, bool firstOne, bool orderByLastDate, AcademicDetailModel academicDetailWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicDetailModel> response = new Dictionary<string, AcademicDetailModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                {
                    if (academicDetail != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.TermId))
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
                            cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                        }
                    }
                    if (academicDetailWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicDetailWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.TermId))
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
                            cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + academicDetailWhereLike.TermId + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + academicDetailWhereLike.PeriodQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + academicDetailWhereLike.DaysToClosePeriod + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + academicDetailWhereLike.FullTermStartDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + academicDetailWhereLike.FullTermEndDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicDetailWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicDetailWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicDetailWhereLike.UpdatedAt + "%");
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
                                academicDetail = new AcademicDetailModel();
                                if (!reader.IsDBNull(0))
                                    academicDetail.Id = reader.GetValue(0).ToString();
                                else
                                    academicDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicDetail.TermId = reader.GetValue(1).ToString();
                                else
                                    academicDetail.TermId = null;
                                if (!reader.IsDBNull(2))
                                    academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                                else
                                    academicDetail.PeriodQuantity = null;
                                if (!reader.IsDBNull(3))
                                    academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                                else
                                    academicDetail.DaysToClosePeriod = null;
                                if (!reader.IsDBNull(4))
                                    academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                                else
                                    academicDetail.FullTermStartDate = null;
                                if (!reader.IsDBNull(5))
                                    academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                                else
                                    academicDetail.FullTermEndDate = null;
                                if (!reader.IsDBNull(6))
                                    academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    academicDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    academicDetail.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    academicDetail.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    academicDetail.UpdatedAt = null;
                                response.Add(academicDetail.Id, academicDetail);
                                academicDetail = new AcademicDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AcademicDetailModel>();
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
                response = new Dictionary<string, AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AcademicDetailModel> ReadD(string connectionString, AcademicDetailModel academicDetail, bool orderByLastDate, int fromRow, int limit, AcademicDetailModel academicDetailWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicDetailModel> response = new Dictionary<string, AcademicDetailModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                {
                    if (academicDetail != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.TermId))
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
                            cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                        }
                    }
                    if (academicDetailWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicDetailWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.TermId))
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
                            cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + academicDetailWhereLike.TermId + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + academicDetailWhereLike.PeriodQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + academicDetailWhereLike.DaysToClosePeriod + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + academicDetailWhereLike.FullTermStartDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + academicDetailWhereLike.FullTermEndDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicDetailWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicDetailWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicDetailWhereLike.UpdatedAt + "%");
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
                                academicDetail = new AcademicDetailModel();
                                if (!reader.IsDBNull(0))
                                    academicDetail.Id = reader.GetValue(0).ToString();
                                else
                                    academicDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicDetail.TermId = reader.GetValue(1).ToString();
                                else
                                    academicDetail.TermId = null;
                                if (!reader.IsDBNull(2))
                                    academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                                else
                                    academicDetail.PeriodQuantity = null;
                                if (!reader.IsDBNull(3))
                                    academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                                else
                                    academicDetail.DaysToClosePeriod = null;
                                if (!reader.IsDBNull(4))
                                    academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                                else
                                    academicDetail.FullTermStartDate = null;
                                if (!reader.IsDBNull(5))
                                    academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                                else
                                    academicDetail.FullTermEndDate = null;
                                if (!reader.IsDBNull(6))
                                    academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    academicDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    academicDetail.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    academicDetail.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    academicDetail.UpdatedAt = null;
                                response.Add(academicDetail.Id, academicDetail);
                                academicDetail = new AcademicDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AcademicDetailModel>();
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
                response = new Dictionary<string, AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AcademicDetailModel> ReadD(string connectionString, List<AcademicDetailModel> academicDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicDetailModel> response = new Dictionary<string, AcademicDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                if (academicDetailWhere != null && academicDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicDetailWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].TermId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `term_id` = @termId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@termId" + i, academicDetailWhere[i].TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].PeriodQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_quantity` = @periodQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetailWhere[i].PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].DaysToClosePeriod))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetailWhere[i].DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermStartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetailWhere[i].FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermEndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetailWhere[i].FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetailWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicDetailWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicDetailWhere.Count - 1)
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
                            AcademicDetailModel academicDetail = new AcademicDetailModel(); if (!reader.IsDBNull(0))
                                academicDetail.Id = reader.GetValue(0).ToString();
                            else
                                academicDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                academicDetail.TermId = reader.GetValue(1).ToString();
                            else
                                academicDetail.TermId = null;
                            if (!reader.IsDBNull(2))
                                academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                            else
                                academicDetail.PeriodQuantity = null;
                            if (!reader.IsDBNull(3))
                                academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                            else
                                academicDetail.DaysToClosePeriod = null;
                            if (!reader.IsDBNull(4))
                                academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                            else
                                academicDetail.FullTermStartDate = null;
                            if (!reader.IsDBNull(5))
                                academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                            else
                                academicDetail.FullTermEndDate = null;
                            if (!reader.IsDBNull(6))
                                academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                academicDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                academicDetail.CreatedAt = reader.GetValue(7).ToString();
                            else
                                academicDetail.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                academicDetail.UpdatedAt = null;
                            response.Add(academicDetail.Id, academicDetail);
                            academicDetail = new AcademicDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AcademicDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AcademicDetailModel> ReadD(string connectionString, List<AcademicDetailModel> academicDetailWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicDetailModel> response = new Dictionary<string, AcademicDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                if (academicDetailWhere != null && academicDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicDetailWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].TermId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `term_id` = @termId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@termId" + i, academicDetailWhere[i].TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].PeriodQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_quantity` = @periodQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetailWhere[i].PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].DaysToClosePeriod))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetailWhere[i].DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermStartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetailWhere[i].FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermEndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetailWhere[i].FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetailWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicDetailWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicDetailWhere.Count - 1)
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
                            AcademicDetailModel academicDetail = new AcademicDetailModel(); if (!reader.IsDBNull(0))
                                academicDetail.Id = reader.GetValue(0).ToString();
                            else
                                academicDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                academicDetail.TermId = reader.GetValue(1).ToString();
                            else
                                academicDetail.TermId = null;
                            if (!reader.IsDBNull(2))
                                academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                            else
                                academicDetail.PeriodQuantity = null;
                            if (!reader.IsDBNull(3))
                                academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                            else
                                academicDetail.DaysToClosePeriod = null;
                            if (!reader.IsDBNull(4))
                                academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                            else
                                academicDetail.FullTermStartDate = null;
                            if (!reader.IsDBNull(5))
                                academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                            else
                                academicDetail.FullTermEndDate = null;
                            if (!reader.IsDBNull(6))
                                academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                academicDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                academicDetail.CreatedAt = reader.GetValue(7).ToString();
                            else
                                academicDetail.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                academicDetail.UpdatedAt = null;
                            response.Add(academicDetail.Id, academicDetail);
                            academicDetail = new AcademicDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AcademicDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicDetailModel> Read(string connectionString, AcademicDetailModel academicDetail, bool firstOne, bool orderByLastDate, AcademicDetailModel academicDetailWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                {
                    if (academicDetail != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.TermId))
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
                            cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                        }
                    }
                    if (academicDetailWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicDetailWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.TermId))
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
                            cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + academicDetailWhereLike.TermId + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + academicDetailWhereLike.PeriodQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + academicDetailWhereLike.DaysToClosePeriod + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + academicDetailWhereLike.FullTermStartDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + academicDetailWhereLike.FullTermEndDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicDetailWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicDetailWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicDetailWhereLike.UpdatedAt + "%");
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
                                academicDetail = new AcademicDetailModel();
                                if (!reader.IsDBNull(0))
                                    academicDetail.Id = reader.GetValue(0).ToString();
                                else
                                    academicDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicDetail.TermId = reader.GetValue(1).ToString();
                                else
                                    academicDetail.TermId = null;
                                if (!reader.IsDBNull(2))
                                    academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                                else
                                    academicDetail.PeriodQuantity = null;
                                if (!reader.IsDBNull(3))
                                    academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                                else
                                    academicDetail.DaysToClosePeriod = null;
                                if (!reader.IsDBNull(4))
                                    academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                                else
                                    academicDetail.FullTermStartDate = null;
                                if (!reader.IsDBNull(5))
                                    academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                                else
                                    academicDetail.FullTermEndDate = null;
                                if (!reader.IsDBNull(6))
                                    academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    academicDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    academicDetail.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    academicDetail.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    academicDetail.UpdatedAt = null;
                                response.Add(academicDetail);
                                academicDetail = new AcademicDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AcademicDetailModel>();
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
                response = new List<AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicDetailModel> Read(string connectionString, AcademicDetailModel academicDetail, bool orderByLastDate, int fromRow, int limit, AcademicDetailModel academicDetailWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                {
                    if (academicDetail != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetail.Id))
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
                            cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.TermId))
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
                            cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                        }
                    }
                    if (academicDetailWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicDetailWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.TermId))
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
                            cmd.Parameters.AddWithValue("@termIdWhereLike", "%" + academicDetailWhereLike.TermId + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.PeriodQuantity))
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
                            cmd.Parameters.AddWithValue("@periodQuantityWhereLike", "%" + academicDetailWhereLike.PeriodQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.DaysToClosePeriod))
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
                            cmd.Parameters.AddWithValue("@daysToClosePeriodWhereLike", "%" + academicDetailWhereLike.DaysToClosePeriod + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermStartDate))
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
                            cmd.Parameters.AddWithValue("@fullTermStartDateWhereLike", "%" + academicDetailWhereLike.FullTermStartDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.FullTermEndDate))
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
                            cmd.Parameters.AddWithValue("@fullTermEndDateWhereLike", "%" + academicDetailWhereLike.FullTermEndDate + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicDetailWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicDetailWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicDetailWhereLike.UpdatedAt + "%");
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
                                academicDetail = new AcademicDetailModel();
                                if (!reader.IsDBNull(0))
                                    academicDetail.Id = reader.GetValue(0).ToString();
                                else
                                    academicDetail.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicDetail.TermId = reader.GetValue(1).ToString();
                                else
                                    academicDetail.TermId = null;
                                if (!reader.IsDBNull(2))
                                    academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                                else
                                    academicDetail.PeriodQuantity = null;
                                if (!reader.IsDBNull(3))
                                    academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                                else
                                    academicDetail.DaysToClosePeriod = null;
                                if (!reader.IsDBNull(4))
                                    academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                                else
                                    academicDetail.FullTermStartDate = null;
                                if (!reader.IsDBNull(5))
                                    academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                                else
                                    academicDetail.FullTermEndDate = null;
                                if (!reader.IsDBNull(6))
                                    academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                                else
                                    academicDetail.ModifiedBy = null;
                                if (!reader.IsDBNull(7))
                                    academicDetail.CreatedAt = reader.GetValue(7).ToString();
                                else
                                    academicDetail.CreatedAt = null;
                                if (!reader.IsDBNull(8))
                                    academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                                else
                                    academicDetail.UpdatedAt = null;
                                response.Add(academicDetail);
                                academicDetail = new AcademicDetailModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AcademicDetailModel>();
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
                response = new List<AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicDetailModel> Read(string connectionString, List<AcademicDetailModel> academicDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                if (academicDetailWhere != null && academicDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicDetailWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].TermId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `term_id` = @termId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@termId" + i, academicDetailWhere[i].TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].PeriodQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_quantity` = @periodQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetailWhere[i].PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].DaysToClosePeriod))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetailWhere[i].DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermStartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetailWhere[i].FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermEndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetailWhere[i].FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetailWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicDetailWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicDetailWhere.Count - 1)
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
                            AcademicDetailModel academicDetail = new AcademicDetailModel(); if (!reader.IsDBNull(0))
                                academicDetail.Id = reader.GetValue(0).ToString();
                            else
                                academicDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                academicDetail.TermId = reader.GetValue(1).ToString();
                            else
                                academicDetail.TermId = null;
                            if (!reader.IsDBNull(2))
                                academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                            else
                                academicDetail.PeriodQuantity = null;
                            if (!reader.IsDBNull(3))
                                academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                            else
                                academicDetail.DaysToClosePeriod = null;
                            if (!reader.IsDBNull(4))
                                academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                            else
                                academicDetail.FullTermStartDate = null;
                            if (!reader.IsDBNull(5))
                                academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                            else
                                academicDetail.FullTermEndDate = null;
                            if (!reader.IsDBNull(6))
                                academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                academicDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                academicDetail.CreatedAt = reader.GetValue(7).ToString();
                            else
                                academicDetail.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                academicDetail.UpdatedAt = null;
                            response.Add(academicDetail);
                            academicDetail = new AcademicDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AcademicDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicDetailModel> Read(string connectionString, List<AcademicDetailModel> academicDetailWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_details ";
                if (academicDetailWhere != null && academicDetailWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicDetailWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicDetailWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].TermId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `term_id` = @termId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@termId" + i, academicDetailWhere[i].TermId);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].PeriodQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_quantity` = @periodQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetailWhere[i].PeriodQuantity);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].DaysToClosePeriod))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `days_to_close_period` = @daysToClosePeriod" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetailWhere[i].DaysToClosePeriod);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermStartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_start_date` = @fullTermStartDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetailWhere[i].FullTermStartDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].FullTermEndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `full_term_end_date` = @fullTermEndDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetailWhere[i].FullTermEndDate);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetailWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicDetailWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(academicDetailWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetailWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicDetailWhere.Count - 1)
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
                            AcademicDetailModel academicDetail = new AcademicDetailModel(); if (!reader.IsDBNull(0))
                                academicDetail.Id = reader.GetValue(0).ToString();
                            else
                                academicDetail.Id = null;
                            if (!reader.IsDBNull(1))
                                academicDetail.TermId = reader.GetValue(1).ToString();
                            else
                                academicDetail.TermId = null;
                            if (!reader.IsDBNull(2))
                                academicDetail.PeriodQuantity = reader.GetValue(2).ToString();
                            else
                                academicDetail.PeriodQuantity = null;
                            if (!reader.IsDBNull(3))
                                academicDetail.DaysToClosePeriod = reader.GetValue(3).ToString();
                            else
                                academicDetail.DaysToClosePeriod = null;
                            if (!reader.IsDBNull(4))
                                academicDetail.FullTermStartDate = reader.GetValue(4).ToString();
                            else
                                academicDetail.FullTermStartDate = null;
                            if (!reader.IsDBNull(5))
                                academicDetail.FullTermEndDate = reader.GetValue(5).ToString();
                            else
                                academicDetail.FullTermEndDate = null;
                            if (!reader.IsDBNull(6))
                                academicDetail.ModifiedBy = reader.GetValue(6).ToString();
                            else
                                academicDetail.ModifiedBy = null;
                            if (!reader.IsDBNull(7))
                                academicDetail.CreatedAt = reader.GetValue(7).ToString();
                            else
                                academicDetail.CreatedAt = null;
                            if (!reader.IsDBNull(8))
                                academicDetail.UpdatedAt = reader.GetValue(8).ToString();
                            else
                                academicDetail.UpdatedAt = null;
                            response.Add(academicDetail);
                            academicDetail = new AcademicDetailModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AcademicDetailModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicDetailModel> Create(string connectionString, AcademicDetailModel academicDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO academic_details (";
                string values = "(";
                if (!String.IsNullOrEmpty(academicDetail.Id))
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
                    cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                }
                if (!String.IsNullOrEmpty(academicDetail.TermId))
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
                    cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                }
                if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
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
                    cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                }
                if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
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
                    cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                }
                if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
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
                    cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                }
                if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
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
                    cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                }
                if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                }
                if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                academicDetail.Id = cmd.LastInsertedId.ToString();
                response.Add(academicDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
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
        public List<AcademicDetailModel> Create(string connectionString, List<AcademicDetailModel> academicDetailList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
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
                        for (int i = 0; i < academicDetailList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO academic_details (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(academicDetailList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, academicDetailList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].TermId))
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
                                    cmd.Parameters.AddWithValue("@termId" + i, academicDetailList[i].TermId);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].PeriodQuantity))
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
                                    cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetailList[i].PeriodQuantity);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].DaysToClosePeriod))
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
                                    cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetailList[i].DaysToClosePeriod);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].FullTermStartDate))
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
                                    cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetailList[i].FullTermStartDate);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].FullTermEndDate))
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
                                    cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetailList[i].FullTermEndDate);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetailList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, academicDetailList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetailList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(academicDetailList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AcademicDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AcademicDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
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

        public List<AcademicDetailModel> Update(string connectionString, AcademicDetailModel academicDetail, AcademicDetailModel academicDetailWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE academic_details SET ";
                if (!String.IsNullOrEmpty(academicDetail.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                }
                if (!String.IsNullOrEmpty(academicDetail.TermId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `term_id` = @termId");
                    else
                        sql = String.Concat(sql, " `term_id` = @termId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                }
                if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `period_quantity` = @periodQuantity");
                    else
                        sql = String.Concat(sql, " `period_quantity` = @periodQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                }
                if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `days_to_close_period` = @daysToClosePeriod");
                    else
                        sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                }
                if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `full_term_start_date` = @fullTermStartDate");
                    else
                        sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                }
                if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `full_term_end_date` = @fullTermEndDate");
                    else
                        sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                }
                if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                }
                if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                }
                if (academicDetailWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(academicDetailWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", academicDetailWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.TermId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `term_id` = @termIdWhere");
                        else
                            sql = String.Concat(sql, " `term_id` = @termIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@termIdWhere", academicDetailWhere.TermId);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.PeriodQuantity))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `period_quantity` = @periodQuantityWhere");
                        else
                            sql = String.Concat(sql, " `period_quantity` = @periodQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodQuantityWhere", academicDetailWhere.PeriodQuantity);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.DaysToClosePeriod))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriodWhere");
                        else
                            sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriodWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@daysToClosePeriodWhere", academicDetailWhere.DaysToClosePeriod);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.FullTermStartDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDateWhere");
                        else
                            sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fullTermStartDateWhere", academicDetailWhere.FullTermStartDate);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.FullTermEndDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDateWhere");
                        else
                            sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fullTermEndDateWhere", academicDetailWhere.FullTermEndDate);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", academicDetailWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", academicDetailWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(academicDetailWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", academicDetailWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(academicDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
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

        public List<AcademicDetailModel> Update(string connectionString, List<AcademicDetailModel> academicDetailList, List<AcademicDetailModel> academicDetailWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
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
                        for (int i = 0; i < academicDetailList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE academic_details SET ";
                                if (!String.IsNullOrEmpty(academicDetailList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, academicDetailList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].TermId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `term_id` = @termId" + i);
                                    else
                                        sql = String.Concat(sql, " `term_id` = @termId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@termId" + i, academicDetailList[i].TermId);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].PeriodQuantity))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `period_quantity` = @periodQuantity" + i);
                                    else
                                        sql = String.Concat(sql, " `period_quantity` = @periodQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetailList[i].PeriodQuantity);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].DaysToClosePeriod))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `days_to_close_period` = @daysToClosePeriod" + i);
                                    else
                                        sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetailList[i].DaysToClosePeriod);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].FullTermStartDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `full_term_start_date` = @fullTermStartDate" + i);
                                    else
                                        sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetailList[i].FullTermStartDate);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].FullTermEndDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `full_term_end_date` = @fullTermEndDate" + i);
                                    else
                                        sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetailList[i].FullTermEndDate);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetailList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, academicDetailList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(academicDetailList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetailList[i].UpdatedAt);
                                }
                                if (academicDetailWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, academicDetailWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].TermId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `term_id` = @termIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `term_id` = @termIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@termIdWhere" + i, academicDetailWhereList[i].TermId);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].PeriodQuantity))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `period_quantity` = @periodQuantityWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `period_quantity` = @periodQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodQuantityWhere" + i, academicDetailWhereList[i].PeriodQuantity);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].DaysToClosePeriod))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriodWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriodWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@daysToClosePeriodWhere" + i, academicDetailWhereList[i].DaysToClosePeriod);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].FullTermStartDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fullTermStartDateWhere" + i, academicDetailWhereList[i].FullTermStartDate);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].FullTermEndDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fullTermEndDateWhere" + i, academicDetailWhereList[i].FullTermEndDate);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, academicDetailWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, academicDetailWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetailWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, academicDetailWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(academicDetailList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AcademicDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AcademicDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
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

        public List<AcademicDetailModel> Delete(string connectionString, AcademicDetailModel academicDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_academic_details  ";
                if (academicDetail != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(academicDetail.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", academicDetail.Id);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.TermId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `term_id` = @termId");
                        else
                            sql = String.Concat(sql, " `term_id` = @termId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@termId", academicDetail.TermId);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.PeriodQuantity))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `period_quantity` = @periodQuantity");
                        else
                            sql = String.Concat(sql, " `period_quantity` = @periodQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodQuantity", academicDetail.PeriodQuantity);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.DaysToClosePeriod))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriod");
                        else
                            sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@daysToClosePeriod", academicDetail.DaysToClosePeriod);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.FullTermStartDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDate");
                        else
                            sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fullTermStartDate", academicDetail.FullTermStartDate);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.FullTermEndDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDate");
                        else
                            sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fullTermEndDate", academicDetail.FullTermEndDate);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", academicDetail.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", academicDetail.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(academicDetail.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", academicDetail.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(academicDetail);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
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

        public List<AcademicDetailModel> Delete(string connectionString, List<AcademicDetailModel> academicDetail)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicDetailModel> response = new List<AcademicDetailModel>();
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
                        for (int i = 0; i < academicDetail.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_academic_details  ";
                                if (academicDetail[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(academicDetail[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, academicDetail[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].TermId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `term_id` = @termId" + i);
                                        else
                                            sql = String.Concat(sql, " `term_id` = @termId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@termId" + i, academicDetail[i].TermId);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].PeriodQuantity))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `period_quantity` = @periodQuantity" + i);
                                        else
                                            sql = String.Concat(sql, " `period_quantity` = @periodQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodQuantity" + i, academicDetail[i].PeriodQuantity);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].DaysToClosePeriod))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `days_to_close_period` = @daysToClosePeriod" + i);
                                        else
                                            sql = String.Concat(sql, " `days_to_close_period` = @daysToClosePeriod" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@daysToClosePeriod" + i, academicDetail[i].DaysToClosePeriod);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].FullTermStartDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `full_term_start_date` = @fullTermStartDate" + i);
                                        else
                                            sql = String.Concat(sql, " `full_term_start_date` = @fullTermStartDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fullTermStartDate" + i, academicDetail[i].FullTermStartDate);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].FullTermEndDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `full_term_end_date` = @fullTermEndDate" + i);
                                        else
                                            sql = String.Concat(sql, " `full_term_end_date` = @fullTermEndDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fullTermEndDate" + i, academicDetail[i].FullTermEndDate);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, academicDetail[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, academicDetail[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(academicDetail[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, academicDetail[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(academicDetail[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AcademicDetailModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AcademicDetailModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicDetailModel>();
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