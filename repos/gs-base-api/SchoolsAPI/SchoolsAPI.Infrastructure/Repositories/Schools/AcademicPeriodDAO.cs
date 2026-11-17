using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class AcademicPeriodDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, AcademicPeriodModel> ReadD(string connectionString, AcademicPeriodModel academicPeriod, bool firstOne, bool orderByLastDate, AcademicPeriodModel academicPeriodWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicPeriodModel> response = new Dictionary<string, AcademicPeriodModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                {
                    if (academicPeriod != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriod.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` = @entityAcademicDetailId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` = @entityAcademicDetailId ");
                            }
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_number` = @periodNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_number` = @periodNumber ");
                            }
                            cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_from` = @periodFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_from` = @periodFrom ");
                            }
                            cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_to` = @periodTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_to` = @periodTo ");
                            }
                            cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                        }
                    }
                    if (academicPeriodWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicPeriodWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.EntityAcademicDetailId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailIdWhereLike", "%" + academicPeriodWhereLike.EntityAcademicDetailId + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_number` LIKE @periodNumberWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_number` LIKE @periodNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodNumberWhereLike", "%" + academicPeriodWhereLike.PeriodNumber + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_from` LIKE @periodFromWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_from` LIKE @periodFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodFromWhereLike", "%" + academicPeriodWhereLike.PeriodFrom + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_to` LIKE @periodToWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_to` LIKE @periodToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodToWhereLike", "%" + academicPeriodWhereLike.PeriodTo + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicPeriodWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicPeriodWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicPeriodWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                academicPeriod = new AcademicPeriodModel();
                                if (!reader.IsDBNull(0))
                                    academicPeriod.Id = reader.GetValue(0).ToString();
                                else
                                    academicPeriod.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                                else
                                    academicPeriod.EntityAcademicDetailId = null;
                                if (!reader.IsDBNull(2))
                                    academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                                else
                                    academicPeriod.PeriodNumber = null;
                                if (!reader.IsDBNull(3))
                                    academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                                else
                                    academicPeriod.PeriodFrom = null;
                                if (!reader.IsDBNull(4))
                                    academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                                else
                                    academicPeriod.PeriodTo = null;
                                if (!reader.IsDBNull(5))
                                    academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    academicPeriod.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    academicPeriod.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    academicPeriod.UpdatedAt = null;
                                response.Add(academicPeriod.Id, academicPeriod);
                                academicPeriod = new AcademicPeriodModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AcademicPeriodModel>();
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
                response = new Dictionary<string, AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AcademicPeriodModel> ReadD(string connectionString, AcademicPeriodModel academicPeriod, bool orderByLastDate, int fromRow, int limit, AcademicPeriodModel academicPeriodWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicPeriodModel> response = new Dictionary<string, AcademicPeriodModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                {
                    if (academicPeriod != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriod.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` = @entityAcademicDetailId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` = @entityAcademicDetailId ");
                            }
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_number` = @periodNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_number` = @periodNumber ");
                            }
                            cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_from` = @periodFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_from` = @periodFrom ");
                            }
                            cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_to` = @periodTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_to` = @periodTo ");
                            }
                            cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                        }
                    }
                    if (academicPeriodWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicPeriodWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.EntityAcademicDetailId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailIdWhereLike", "%" + academicPeriodWhereLike.EntityAcademicDetailId + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_number` LIKE @periodNumberWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_number` LIKE @periodNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodNumberWhereLike", "%" + academicPeriodWhereLike.PeriodNumber + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_from` LIKE @periodFromWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_from` LIKE @periodFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodFromWhereLike", "%" + academicPeriodWhereLike.PeriodFrom + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_to` LIKE @periodToWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_to` LIKE @periodToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodToWhereLike", "%" + academicPeriodWhereLike.PeriodTo + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicPeriodWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicPeriodWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicPeriodWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                academicPeriod = new AcademicPeriodModel();
                                if (!reader.IsDBNull(0))
                                    academicPeriod.Id = reader.GetValue(0).ToString();
                                else
                                    academicPeriod.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                                else
                                    academicPeriod.EntityAcademicDetailId = null;
                                if (!reader.IsDBNull(2))
                                    academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                                else
                                    academicPeriod.PeriodNumber = null;
                                if (!reader.IsDBNull(3))
                                    academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                                else
                                    academicPeriod.PeriodFrom = null;
                                if (!reader.IsDBNull(4))
                                    academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                                else
                                    academicPeriod.PeriodTo = null;
                                if (!reader.IsDBNull(5))
                                    academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    academicPeriod.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    academicPeriod.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    academicPeriod.UpdatedAt = null;
                                response.Add(academicPeriod.Id, academicPeriod);
                                academicPeriod = new AcademicPeriodModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, AcademicPeriodModel>();
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
                response = new Dictionary<string, AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AcademicPeriodModel> ReadD(string connectionString, List<AcademicPeriodModel> academicPeriodWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicPeriodModel> response = new Dictionary<string, AcademicPeriodModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                if (academicPeriodWhere != null && academicPeriodWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicPeriodWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicPeriodWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].EntityAcademicDetailId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_academic_detail_id` = @entityAcademicDetailId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriodWhere[i].EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_number` = @periodNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriodWhere[i].PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_from` = @periodFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriodWhere[i].PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_to` = @periodTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriodWhere[i].PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriodWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriodWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriodWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicPeriodWhere.Count - 1)
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
                            AcademicPeriodModel academicPeriod = new AcademicPeriodModel(); if (!reader.IsDBNull(0))
                                academicPeriod.Id = reader.GetValue(0).ToString();
                            else
                                academicPeriod.Id = null;
                            if (!reader.IsDBNull(1))
                                academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                            else
                                academicPeriod.EntityAcademicDetailId = null;
                            if (!reader.IsDBNull(2))
                                academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                            else
                                academicPeriod.PeriodNumber = null;
                            if (!reader.IsDBNull(3))
                                academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                            else
                                academicPeriod.PeriodFrom = null;
                            if (!reader.IsDBNull(4))
                                academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                            else
                                academicPeriod.PeriodTo = null;
                            if (!reader.IsDBNull(5))
                                academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                academicPeriod.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                            else
                                academicPeriod.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                academicPeriod.UpdatedAt = null;
                            response.Add(academicPeriod.Id, academicPeriod);
                            academicPeriod = new AcademicPeriodModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AcademicPeriodModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, AcademicPeriodModel> ReadD(string connectionString, List<AcademicPeriodModel> academicPeriodWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, AcademicPeriodModel> response = new Dictionary<string, AcademicPeriodModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                if (academicPeriodWhere != null && academicPeriodWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicPeriodWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicPeriodWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].EntityAcademicDetailId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_academic_detail_id` = @entityAcademicDetailId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriodWhere[i].EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_number` = @periodNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriodWhere[i].PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_from` = @periodFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriodWhere[i].PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_to` = @periodTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriodWhere[i].PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriodWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriodWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriodWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicPeriodWhere.Count - 1)
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
                            AcademicPeriodModel academicPeriod = new AcademicPeriodModel(); if (!reader.IsDBNull(0))
                                academicPeriod.Id = reader.GetValue(0).ToString();
                            else
                                academicPeriod.Id = null;
                            if (!reader.IsDBNull(1))
                                academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                            else
                                academicPeriod.EntityAcademicDetailId = null;
                            if (!reader.IsDBNull(2))
                                academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                            else
                                academicPeriod.PeriodNumber = null;
                            if (!reader.IsDBNull(3))
                                academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                            else
                                academicPeriod.PeriodFrom = null;
                            if (!reader.IsDBNull(4))
                                academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                            else
                                academicPeriod.PeriodTo = null;
                            if (!reader.IsDBNull(5))
                                academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                academicPeriod.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                            else
                                academicPeriod.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                academicPeriod.UpdatedAt = null;
                            response.Add(academicPeriod.Id, academicPeriod);
                            academicPeriod = new AcademicPeriodModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, AcademicPeriodModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicPeriodModel> Read(string connectionString, AcademicPeriodModel academicPeriod, bool firstOne, bool orderByLastDate, AcademicPeriodModel academicPeriodWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                {
                    if (academicPeriod != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriod.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` = @entityAcademicDetailId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` = @entityAcademicDetailId ");
                            }
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_number` = @periodNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_number` = @periodNumber ");
                            }
                            cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_from` = @periodFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_from` = @periodFrom ");
                            }
                            cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_to` = @periodTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_to` = @periodTo ");
                            }
                            cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                        }
                    }
                    if (academicPeriodWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicPeriodWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.EntityAcademicDetailId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailIdWhereLike", "%" + academicPeriodWhereLike.EntityAcademicDetailId + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_number` LIKE @periodNumberWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_number` LIKE @periodNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodNumberWhereLike", "%" + academicPeriodWhereLike.PeriodNumber + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_from` LIKE @periodFromWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_from` LIKE @periodFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodFromWhereLike", "%" + academicPeriodWhereLike.PeriodFrom + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_to` LIKE @periodToWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_to` LIKE @periodToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodToWhereLike", "%" + academicPeriodWhereLike.PeriodTo + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicPeriodWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicPeriodWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicPeriodWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = string.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                academicPeriod = new AcademicPeriodModel();
                                if (!reader.IsDBNull(0))
                                    academicPeriod.Id = reader.GetValue(0).ToString();
                                else
                                    academicPeriod.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                                else
                                    academicPeriod.EntityAcademicDetailId = null;
                                if (!reader.IsDBNull(2))
                                    academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                                else
                                    academicPeriod.PeriodNumber = null;
                                if (!reader.IsDBNull(3))
                                    academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                                else
                                    academicPeriod.PeriodFrom = null;
                                if (!reader.IsDBNull(4))
                                    academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                                else
                                    academicPeriod.PeriodTo = null;
                                if (!reader.IsDBNull(5))
                                    academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    academicPeriod.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    academicPeriod.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    academicPeriod.UpdatedAt = null;
                                response.Add(academicPeriod);
                                academicPeriod = new AcademicPeriodModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AcademicPeriodModel>();
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
                response = new List<AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicPeriodModel> Read(string connectionString, AcademicPeriodModel academicPeriod, bool orderByLastDate, int fromRow, int limit, AcademicPeriodModel academicPeriodWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                {
                    if (academicPeriod != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriod.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` = @entityAcademicDetailId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` = @entityAcademicDetailId ");
                            }
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_number` = @periodNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_number` = @periodNumber ");
                            }
                            cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_from` = @periodFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_from` = @periodFrom ");
                            }
                            cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `period_to` = @periodTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `period_to` = @periodTo ");
                            }
                            cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                        }
                    }
                    if (academicPeriodWhereLike != null)
                    {
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + academicPeriodWhereLike.Id + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.EntityAcademicDetailId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `entity_academic_detail_id` LIKE @entityAcademicDetailIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailIdWhereLike", "%" + academicPeriodWhereLike.EntityAcademicDetailId + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_number` LIKE @periodNumberWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_number` LIKE @periodNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodNumberWhereLike", "%" + academicPeriodWhereLike.PeriodNumber + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_from` LIKE @periodFromWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_from` LIKE @periodFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodFromWhereLike", "%" + academicPeriodWhereLike.PeriodFrom + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.PeriodTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `period_to` LIKE @periodToWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `period_to` LIKE @periodToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodToWhereLike", "%" + academicPeriodWhereLike.PeriodTo + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + academicPeriodWhereLike.ModifiedBy + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + academicPeriodWhereLike.CreatedAt + "%");
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = string.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = string.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + academicPeriodWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = string.Concat(sql, " ORDER BY updated_at DESC");
                    sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                academicPeriod = new AcademicPeriodModel();
                                if (!reader.IsDBNull(0))
                                    academicPeriod.Id = reader.GetValue(0).ToString();
                                else
                                    academicPeriod.Id = null;
                                if (!reader.IsDBNull(1))
                                    academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                                else
                                    academicPeriod.EntityAcademicDetailId = null;
                                if (!reader.IsDBNull(2))
                                    academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                                else
                                    academicPeriod.PeriodNumber = null;
                                if (!reader.IsDBNull(3))
                                    academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                                else
                                    academicPeriod.PeriodFrom = null;
                                if (!reader.IsDBNull(4))
                                    academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                                else
                                    academicPeriod.PeriodTo = null;
                                if (!reader.IsDBNull(5))
                                    academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    academicPeriod.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    academicPeriod.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    academicPeriod.UpdatedAt = null;
                                response.Add(academicPeriod);
                                academicPeriod = new AcademicPeriodModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<AcademicPeriodModel>();
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
                response = new List<AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicPeriodModel> Read(string connectionString, List<AcademicPeriodModel> academicPeriodWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                if (academicPeriodWhere != null && academicPeriodWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicPeriodWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicPeriodWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].EntityAcademicDetailId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_academic_detail_id` = @entityAcademicDetailId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriodWhere[i].EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_number` = @periodNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriodWhere[i].PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_from` = @periodFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriodWhere[i].PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_to` = @periodTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriodWhere[i].PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriodWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriodWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriodWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicPeriodWhere.Count - 1)
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
                            AcademicPeriodModel academicPeriod = new AcademicPeriodModel(); if (!reader.IsDBNull(0))
                                academicPeriod.Id = reader.GetValue(0).ToString();
                            else
                                academicPeriod.Id = null;
                            if (!reader.IsDBNull(1))
                                academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                            else
                                academicPeriod.EntityAcademicDetailId = null;
                            if (!reader.IsDBNull(2))
                                academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                            else
                                academicPeriod.PeriodNumber = null;
                            if (!reader.IsDBNull(3))
                                academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                            else
                                academicPeriod.PeriodFrom = null;
                            if (!reader.IsDBNull(4))
                                academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                            else
                                academicPeriod.PeriodTo = null;
                            if (!reader.IsDBNull(5))
                                academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                academicPeriod.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                            else
                                academicPeriod.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                academicPeriod.UpdatedAt = null;
                            response.Add(academicPeriod);
                            academicPeriod = new AcademicPeriodModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AcademicPeriodModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicPeriodModel> Read(string connectionString, List<AcademicPeriodModel> academicPeriodWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at`  FROM sch_academic_periods ";
                if (academicPeriodWhere != null && academicPeriodWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < academicPeriodWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, academicPeriodWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].EntityAcademicDetailId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `entity_academic_detail_id` = @entityAcademicDetailId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriodWhere[i].EntityAcademicDetailId);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_number` = @periodNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriodWhere[i].PeriodNumber);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_from` = @periodFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriodWhere[i].PeriodFrom);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].PeriodTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_to` = @periodTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriodWhere[i].PeriodTo);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriodWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriodWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(academicPeriodWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriodWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != academicPeriodWhere.Count - 1)
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
                            AcademicPeriodModel academicPeriod = new AcademicPeriodModel(); if (!reader.IsDBNull(0))
                                academicPeriod.Id = reader.GetValue(0).ToString();
                            else
                                academicPeriod.Id = null;
                            if (!reader.IsDBNull(1))
                                academicPeriod.EntityAcademicDetailId = reader.GetValue(1).ToString();
                            else
                                academicPeriod.EntityAcademicDetailId = null;
                            if (!reader.IsDBNull(2))
                                academicPeriod.PeriodNumber = reader.GetValue(2).ToString();
                            else
                                academicPeriod.PeriodNumber = null;
                            if (!reader.IsDBNull(3))
                                academicPeriod.PeriodFrom = reader.GetValue(3).ToString();
                            else
                                academicPeriod.PeriodFrom = null;
                            if (!reader.IsDBNull(4))
                                academicPeriod.PeriodTo = reader.GetValue(4).ToString();
                            else
                                academicPeriod.PeriodTo = null;
                            if (!reader.IsDBNull(5))
                                academicPeriod.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                academicPeriod.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                academicPeriod.CreatedAt = reader.GetValue(6).ToString();
                            else
                                academicPeriod.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                academicPeriod.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                academicPeriod.UpdatedAt = null;
                            response.Add(academicPeriod);
                            academicPeriod = new AcademicPeriodModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<AcademicPeriodModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<AcademicPeriodModel> Create(string connectionString, AcademicPeriodModel academicPeriod)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_academic_periods (";
                string values = "(";
                if (!string.IsNullOrEmpty(academicPeriod.Id))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `id` ");
                        values = string.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `id` ");
                        values = string.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                }
                if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `entity_academic_detail_id` ");
                        values = string.Concat(values, ", @entityAcademicDetailId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `entity_academic_detail_id` ");
                        values = string.Concat(values, " @entityAcademicDetailId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                }
                if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `period_number` ");
                        values = string.Concat(values, ", @periodNumber");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `period_number` ");
                        values = string.Concat(values, " @periodNumber");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                }
                if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `period_from` ");
                        values = string.Concat(values, ", @periodFrom");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `period_from` ");
                        values = string.Concat(values, " @periodFrom");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                }
                if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `period_to` ");
                        values = string.Concat(values, ", @periodTo");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `period_to` ");
                        values = string.Concat(values, " @periodTo");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                }
                if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `modified_by` ");
                        values = string.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `modified_by` ");
                        values = string.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `created_at` ");
                        values = string.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `created_at` ");
                        values = string.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                }
                if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `updated_at` ");
                        values = string.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `updated_at` ");
                        values = string.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                academicPeriod.Id = cmd.LastInsertedId.ToString();
                response.Add(academicPeriod);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
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
        public List<AcademicPeriodModel> Create(string connectionString, List<AcademicPeriodModel> academicPeriodList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
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
                        for (int i = 0; i < academicPeriodList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_academic_periods (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(academicPeriodList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `id` ");
                                        values = string.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `id` ");
                                        values = string.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, academicPeriodList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].EntityAcademicDetailId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `entity_academic_detail_id` ");
                                        values = string.Concat(values, ", @entityAcademicDetailId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `entity_academic_detail_id` ");
                                        values = string.Concat(values, "@entityAcademicDetailId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriodList[i].EntityAcademicDetailId);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].PeriodNumber))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `period_number` ");
                                        values = string.Concat(values, ", @periodNumber" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `period_number` ");
                                        values = string.Concat(values, "@periodNumber" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriodList[i].PeriodNumber);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].PeriodFrom))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `period_from` ");
                                        values = string.Concat(values, ", @periodFrom" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `period_from` ");
                                        values = string.Concat(values, "@periodFrom" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriodList[i].PeriodFrom);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].PeriodTo))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `period_to` ");
                                        values = string.Concat(values, ", @periodTo" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `period_to` ");
                                        values = string.Concat(values, "@periodTo" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriodList[i].PeriodTo);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriodList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `created_at` ");
                                        values = string.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `created_at` ");
                                        values = string.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriodList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `updated_at` ");
                                        values = string.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `updated_at` ");
                                        values = string.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriodList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(academicPeriodList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<AcademicPeriodModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AcademicPeriodModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
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

        public List<AcademicPeriodModel> Update(string connectionString, AcademicPeriodModel academicPeriod, AcademicPeriodModel academicPeriodWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_academic_periods SET ";
                if (!string.IsNullOrEmpty(academicPeriod.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                }
                if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `entity_academic_detail_id` = @entityAcademicDetailId");
                    else
                        sql = string.Concat(sql, " `entity_academic_detail_id` = @entityAcademicDetailId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                }
                if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `period_number` = @periodNumber");
                    else
                        sql = string.Concat(sql, " `period_number` = @periodNumber");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                }
                if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `period_from` = @periodFrom");
                    else
                        sql = string.Concat(sql, " `period_from` = @periodFrom");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                }
                if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `period_to` = @periodTo");
                    else
                        sql = string.Concat(sql, " `period_to` = @periodTo");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                }
                if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                }
                if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                }
                if (academicPeriodWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(academicPeriodWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", academicPeriodWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.EntityAcademicDetailId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_academic_detail_id` = @entityAcademicDetailIdWhere");
                        else
                            sql = string.Concat(sql, " `entity_academic_detail_id` = @entityAcademicDetailIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityAcademicDetailIdWhere", academicPeriodWhere.EntityAcademicDetailId);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.PeriodNumber))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `period_number` = @periodNumberWhere");
                        else
                            sql = string.Concat(sql, " `period_number` = @periodNumberWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodNumberWhere", academicPeriodWhere.PeriodNumber);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.PeriodFrom))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `period_from` = @periodFromWhere");
                        else
                            sql = string.Concat(sql, " `period_from` = @periodFromWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodFromWhere", academicPeriodWhere.PeriodFrom);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.PeriodTo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `period_to` = @periodToWhere");
                        else
                            sql = string.Concat(sql, " `period_to` = @periodToWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodToWhere", academicPeriodWhere.PeriodTo);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", academicPeriodWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", academicPeriodWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(academicPeriodWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", academicPeriodWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(academicPeriod);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
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

        public List<AcademicPeriodModel> Update(string connectionString, List<AcademicPeriodModel> academicPeriodList, List<AcademicPeriodModel> academicPeriodWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
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
                        for (int i = 0; i < academicPeriodList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_academic_periods SET ";
                                if (!string.IsNullOrEmpty(academicPeriodList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, academicPeriodList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].EntityAcademicDetailId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `entity_academic_detail_id` = @entityAcademicDetailId" + i);
                                    else
                                        sql = string.Concat(sql, " `entity_academic_detail_id` = @entityAcademicDetailId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriodList[i].EntityAcademicDetailId);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].PeriodNumber))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `period_number` = @periodNumber" + i);
                                    else
                                        sql = string.Concat(sql, " `period_number` = @periodNumber" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriodList[i].PeriodNumber);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].PeriodFrom))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `period_from` = @periodFrom" + i);
                                    else
                                        sql = string.Concat(sql, " `period_from` = @periodFrom" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriodList[i].PeriodFrom);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].PeriodTo))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `period_to` = @periodTo" + i);
                                    else
                                        sql = string.Concat(sql, " `period_to` = @periodTo" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriodList[i].PeriodTo);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriodList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriodList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(academicPeriodList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriodList[i].UpdatedAt);
                                }
                                if (academicPeriodWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, academicPeriodWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].EntityAcademicDetailId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_academic_detail_id` = @entityAcademicDetailIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_academic_detail_id` = @entityAcademicDetailIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityAcademicDetailIdWhere" + i, academicPeriodWhereList[i].EntityAcademicDetailId);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].PeriodNumber))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `period_number` = @periodNumberWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `period_number` = @periodNumberWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodNumberWhere" + i, academicPeriodWhereList[i].PeriodNumber);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].PeriodFrom))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `period_from` = @periodFromWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `period_from` = @periodFromWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodFromWhere" + i, academicPeriodWhereList[i].PeriodFrom);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].PeriodTo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `period_to` = @periodToWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `period_to` = @periodToWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodToWhere" + i, academicPeriodWhereList[i].PeriodTo);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, academicPeriodWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, academicPeriodWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriodWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, academicPeriodWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(academicPeriodList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AcademicPeriodModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AcademicPeriodModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
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

        public List<AcademicPeriodModel> Delete(string connectionString, AcademicPeriodModel academicPeriod)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_academic_periods  ";
                if (academicPeriod != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(academicPeriod.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", academicPeriod.Id);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.EntityAcademicDetailId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `entity_academic_detail_id` = @entityAcademicDetailId");
                        else
                            sql = string.Concat(sql, " `entity_academic_detail_id` = @entityAcademicDetailId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@entityAcademicDetailId", academicPeriod.EntityAcademicDetailId);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.PeriodNumber))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `period_number` = @periodNumber");
                        else
                            sql = string.Concat(sql, " `period_number` = @periodNumber");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodNumber", academicPeriod.PeriodNumber);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.PeriodFrom))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `period_from` = @periodFrom");
                        else
                            sql = string.Concat(sql, " `period_from` = @periodFrom");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodFrom", academicPeriod.PeriodFrom);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.PeriodTo))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `period_to` = @periodTo");
                        else
                            sql = string.Concat(sql, " `period_to` = @periodTo");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodTo", academicPeriod.PeriodTo);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", academicPeriod.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", academicPeriod.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(academicPeriod.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", academicPeriod.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(academicPeriod);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
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

        public List<AcademicPeriodModel> Delete(string connectionString, List<AcademicPeriodModel> academicPeriod)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<AcademicPeriodModel> response = new List<AcademicPeriodModel>();
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
                        for (int i = 0; i < academicPeriod.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_academic_periods  ";
                                if (academicPeriod[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(academicPeriod[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, academicPeriod[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].EntityAcademicDetailId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `entity_academic_detail_id` = @entityAcademicDetailId" + i);
                                        else
                                            sql = string.Concat(sql, " `entity_academic_detail_id` = @entityAcademicDetailId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@entityAcademicDetailId" + i, academicPeriod[i].EntityAcademicDetailId);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].PeriodNumber))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `period_number` = @periodNumber" + i);
                                        else
                                            sql = string.Concat(sql, " `period_number` = @periodNumber" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodNumber" + i, academicPeriod[i].PeriodNumber);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].PeriodFrom))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `period_from` = @periodFrom" + i);
                                        else
                                            sql = string.Concat(sql, " `period_from` = @periodFrom" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodFrom" + i, academicPeriod[i].PeriodFrom);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].PeriodTo))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `period_to` = @periodTo" + i);
                                        else
                                            sql = string.Concat(sql, " `period_to` = @periodTo" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodTo" + i, academicPeriod[i].PeriodTo);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, academicPeriod[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, academicPeriod[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(academicPeriod[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, academicPeriod[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(academicPeriod[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<AcademicPeriodModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<AcademicPeriodModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<AcademicPeriodModel>();
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
