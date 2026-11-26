using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherPlanDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherPlanModel> ReadD(string connectionString, TeacherPlanModel teacherPlan, bool firstOne, bool orderByLastDate, TeacherPlanModel teacherPlanWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanModel> response = new Dictionary<string, TeacherPlanModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                {
                    if (teacherPlan != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlan.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Name))
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
                            cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_instructional` = @isInstructional ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_instructional` = @isInstructional ");
                            }
                            cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_type_id` = @planTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_type_id` = @planTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_number` = @planNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_number` = @planNumber ");
                            }
                            cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_from` = @dateFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_from` = @dateFrom ");
                            }
                            cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_to` = @dateTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_to` = @dateTo ");
                            }
                            cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `period_id` = @periodId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `period_id` = @periodId ");
                            }
                            cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Opened))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `opened` = @opened ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `opened` = @opened ");
                            }
                            cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `open_date` = @openDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `open_date` = @openDate ");
                            }
                            cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `close_date` = @closeDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `close_date` = @closeDate ");
                            }
                            cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `week_number` = @weekNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `week_number` = @weekNumber ");
                            }
                            cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `timeframe` = @timeframe ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `timeframe` = @timeframe ");
                            }
                            cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                        }
                    }
                    if (teacherPlanWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherPlanWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.IsInstructional))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isInstructionalWhereLike", "%" + teacherPlanWhereLike.IsInstructional + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planTypeIdWhereLike", "%" + teacherPlanWhereLike.PlanTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_number` LIKE @planNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_number` LIKE @planNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planNumberWhereLike", "%" + teacherPlanWhereLike.PlanNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_from` LIKE @dateFromWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_from` LIKE @dateFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateFromWhereLike", "%" + teacherPlanWhereLike.DateFrom + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_to` LIKE @dateToWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_to` LIKE @dateToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateToWhereLike", "%" + teacherPlanWhereLike.DateTo + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + teacherPlanWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherPlanWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherPlanWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PeriodId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `period_id` LIKE @periodIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `period_id` LIKE @periodIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodIdWhereLike", "%" + teacherPlanWhereLike.PeriodId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Opened))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `opened` LIKE @openedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `opened` LIKE @openedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openedWhereLike", "%" + teacherPlanWhereLike.Opened + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanWhereLike.UpdatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.OpenDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `open_date` LIKE @openDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `open_date` LIKE @openDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openDateWhereLike", "%" + teacherPlanWhereLike.OpenDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CloseDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `close_date` LIKE @closeDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `close_date` LIKE @closeDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@closeDateWhereLike", "%" + teacherPlanWhereLike.CloseDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.WeekNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `week_number` LIKE @weekNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `week_number` LIKE @weekNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@weekNumberWhereLike", "%" + teacherPlanWhereLike.WeekNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Timeframe))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `timeframe` LIKE @timeframeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `timeframe` LIKE @timeframeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@timeframeWhereLike", "%" + teacherPlanWhereLike.Timeframe + "%");
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
                                teacherPlan = new TeacherPlanModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlan.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlan.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlan.Name = reader.GetValue(1).ToString();
                                else
                                    teacherPlan.Name = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                                else
                                    teacherPlan.IsInstructional = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                                else
                                    teacherPlan.PlanTypeId = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                                else
                                    teacherPlan.PlanNumber = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlan.DateFrom = reader.GetValue(5).ToString();
                                else
                                    teacherPlan.DateFrom = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlan.DateTo = reader.GetValue(6).ToString();
                                else
                                    teacherPlan.DateTo = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlan.UserId = reader.GetValue(7).ToString();
                                else
                                    teacherPlan.UserId = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlan.Available = reader.GetValue(8).ToString();
                                else
                                    teacherPlan.Available = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                                else
                                    teacherPlan.SubjectCode = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlan.LevelCode = reader.GetValue(10).ToString();
                                else
                                    teacherPlan.LevelCode = null;
                                if (!reader.IsDBNull(11))
                                    teacherPlan.PeriodId = reader.GetValue(11).ToString();
                                else
                                    teacherPlan.PeriodId = null;
                                if (!reader.IsDBNull(12))
                                    teacherPlan.Opened = reader.GetValue(12).ToString();
                                else
                                    teacherPlan.Opened = null;
                                if (!reader.IsDBNull(13))
                                    teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                                else
                                    teacherPlan.ModifiedBy = null;
                                if (!reader.IsDBNull(14))
                                    teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                                else
                                    teacherPlan.CreatedAt = null;
                                if (!reader.IsDBNull(15))
                                    teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                                else
                                    teacherPlan.UpdatedAt = null;
                                if (!reader.IsDBNull(16))
                                    teacherPlan.OpenDate = reader.GetValue(16).ToString();
                                else
                                    teacherPlan.OpenDate = null;
                                if (!reader.IsDBNull(17))
                                    teacherPlan.CloseDate = reader.GetValue(17).ToString();
                                else
                                    teacherPlan.CloseDate = null;
                                if (!reader.IsDBNull(18))
                                    teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                                else
                                    teacherPlan.WeekNumber = null;
                                if (!reader.IsDBNull(19))
                                    teacherPlan.Timeframe = reader.GetValue(19).ToString();
                                else
                                    teacherPlan.Timeframe = null;
                                response.Add(teacherPlan.Id, teacherPlan);
                                teacherPlan = new TeacherPlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanModel>();
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
                response = new Dictionary<string, TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanModel> ReadD(string connectionString, TeacherPlanModel teacherPlan, bool orderByLastDate, int fromRow, int limit, TeacherPlanModel teacherPlanWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanModel> response = new Dictionary<string, TeacherPlanModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                {
                    if (teacherPlan != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlan.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Name))
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
                            cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_instructional` = @isInstructional ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_instructional` = @isInstructional ");
                            }
                            cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_type_id` = @planTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_type_id` = @planTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_number` = @planNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_number` = @planNumber ");
                            }
                            cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_from` = @dateFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_from` = @dateFrom ");
                            }
                            cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_to` = @dateTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_to` = @dateTo ");
                            }
                            cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `period_id` = @periodId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `period_id` = @periodId ");
                            }
                            cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Opened))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `opened` = @opened ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `opened` = @opened ");
                            }
                            cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `open_date` = @openDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `open_date` = @openDate ");
                            }
                            cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `close_date` = @closeDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `close_date` = @closeDate ");
                            }
                            cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `week_number` = @weekNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `week_number` = @weekNumber ");
                            }
                            cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `timeframe` = @timeframe ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `timeframe` = @timeframe ");
                            }
                            cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                        }
                    }
                    if (teacherPlanWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherPlanWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.IsInstructional))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isInstructionalWhereLike", "%" + teacherPlanWhereLike.IsInstructional + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planTypeIdWhereLike", "%" + teacherPlanWhereLike.PlanTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_number` LIKE @planNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_number` LIKE @planNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planNumberWhereLike", "%" + teacherPlanWhereLike.PlanNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_from` LIKE @dateFromWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_from` LIKE @dateFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateFromWhereLike", "%" + teacherPlanWhereLike.DateFrom + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_to` LIKE @dateToWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_to` LIKE @dateToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateToWhereLike", "%" + teacherPlanWhereLike.DateTo + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + teacherPlanWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherPlanWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherPlanWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PeriodId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `period_id` LIKE @periodIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `period_id` LIKE @periodIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodIdWhereLike", "%" + teacherPlanWhereLike.PeriodId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Opened))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `opened` LIKE @openedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `opened` LIKE @openedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openedWhereLike", "%" + teacherPlanWhereLike.Opened + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanWhereLike.UpdatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.OpenDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `open_date` LIKE @openDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `open_date` LIKE @openDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openDateWhereLike", "%" + teacherPlanWhereLike.OpenDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CloseDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `close_date` LIKE @closeDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `close_date` LIKE @closeDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@closeDateWhereLike", "%" + teacherPlanWhereLike.CloseDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.WeekNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `week_number` LIKE @weekNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `week_number` LIKE @weekNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@weekNumberWhereLike", "%" + teacherPlanWhereLike.WeekNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Timeframe))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `timeframe` LIKE @timeframeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `timeframe` LIKE @timeframeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@timeframeWhereLike", "%" + teacherPlanWhereLike.Timeframe + "%");
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
                                teacherPlan = new TeacherPlanModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlan.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlan.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlan.Name = reader.GetValue(1).ToString();
                                else
                                    teacherPlan.Name = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                                else
                                    teacherPlan.IsInstructional = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                                else
                                    teacherPlan.PlanTypeId = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                                else
                                    teacherPlan.PlanNumber = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlan.DateFrom = reader.GetValue(5).ToString();
                                else
                                    teacherPlan.DateFrom = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlan.DateTo = reader.GetValue(6).ToString();
                                else
                                    teacherPlan.DateTo = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlan.UserId = reader.GetValue(7).ToString();
                                else
                                    teacherPlan.UserId = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlan.Available = reader.GetValue(8).ToString();
                                else
                                    teacherPlan.Available = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                                else
                                    teacherPlan.SubjectCode = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlan.LevelCode = reader.GetValue(10).ToString();
                                else
                                    teacherPlan.LevelCode = null;
                                if (!reader.IsDBNull(11))
                                    teacherPlan.PeriodId = reader.GetValue(11).ToString();
                                else
                                    teacherPlan.PeriodId = null;
                                if (!reader.IsDBNull(12))
                                    teacherPlan.Opened = reader.GetValue(12).ToString();
                                else
                                    teacherPlan.Opened = null;
                                if (!reader.IsDBNull(13))
                                    teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                                else
                                    teacherPlan.ModifiedBy = null;
                                if (!reader.IsDBNull(14))
                                    teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                                else
                                    teacherPlan.CreatedAt = null;
                                if (!reader.IsDBNull(15))
                                    teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                                else
                                    teacherPlan.UpdatedAt = null;
                                if (!reader.IsDBNull(16))
                                    teacherPlan.OpenDate = reader.GetValue(16).ToString();
                                else
                                    teacherPlan.OpenDate = null;
                                if (!reader.IsDBNull(17))
                                    teacherPlan.CloseDate = reader.GetValue(17).ToString();
                                else
                                    teacherPlan.CloseDate = null;
                                if (!reader.IsDBNull(18))
                                    teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                                else
                                    teacherPlan.WeekNumber = null;
                                if (!reader.IsDBNull(19))
                                    teacherPlan.Timeframe = reader.GetValue(19).ToString();
                                else
                                    teacherPlan.Timeframe = null;
                                response.Add(teacherPlan.Id, teacherPlan);
                                teacherPlan = new TeacherPlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanModel>();
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
                response = new Dictionary<string, TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanModel> ReadD(string connectionString, List<TeacherPlanModel> teacherPlanWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanModel> response = new Dictionary<string, TeacherPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                if (teacherPlanWhere != null && teacherPlanWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherPlanWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].IsInstructional))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_instructional` = @isInstructional" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlanWhere[i].IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_type_id` = @planTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlanWhere[i].PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_number` = @planNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlanWhere[i].PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_from` = @dateFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlanWhere[i].DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_to` = @dateTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlanWhere[i].DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, teacherPlanWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlanWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlanWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PeriodId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_id` = @periodId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodId" + i, teacherPlanWhere[i].PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Opened))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `opened` = @opened" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@opened" + i, teacherPlanWhere[i].Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanWhere[i].UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].OpenDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `open_date` = @openDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@openDate" + i, teacherPlanWhere[i].OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CloseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `close_date` = @closeDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlanWhere[i].CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].WeekNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `week_number` = @weekNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlanWhere[i].WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Timeframe))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `timeframe` = @timeframe" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlanWhere[i].Timeframe);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanWhere.Count - 1)
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
                            TeacherPlanModel teacherPlan = new TeacherPlanModel(); if (!reader.IsDBNull(0))
                                teacherPlan.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlan.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlan.Name = reader.GetValue(1).ToString();
                            else
                                teacherPlan.Name = null;
                            if (!reader.IsDBNull(2))
                                teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                            else
                                teacherPlan.IsInstructional = null;
                            if (!reader.IsDBNull(3))
                                teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                            else
                                teacherPlan.PlanTypeId = null;
                            if (!reader.IsDBNull(4))
                                teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                            else
                                teacherPlan.PlanNumber = null;
                            if (!reader.IsDBNull(5))
                                teacherPlan.DateFrom = reader.GetValue(5).ToString();
                            else
                                teacherPlan.DateFrom = null;
                            if (!reader.IsDBNull(6))
                                teacherPlan.DateTo = reader.GetValue(6).ToString();
                            else
                                teacherPlan.DateTo = null;
                            if (!reader.IsDBNull(7))
                                teacherPlan.UserId = reader.GetValue(7).ToString();
                            else
                                teacherPlan.UserId = null;
                            if (!reader.IsDBNull(8))
                                teacherPlan.Available = reader.GetValue(8).ToString();
                            else
                                teacherPlan.Available = null;
                            if (!reader.IsDBNull(9))
                                teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                            else
                                teacherPlan.SubjectCode = null;
                            if (!reader.IsDBNull(10))
                                teacherPlan.LevelCode = reader.GetValue(10).ToString();
                            else
                                teacherPlan.LevelCode = null;
                            if (!reader.IsDBNull(11))
                                teacherPlan.PeriodId = reader.GetValue(11).ToString();
                            else
                                teacherPlan.PeriodId = null;
                            if (!reader.IsDBNull(12))
                                teacherPlan.Opened = reader.GetValue(12).ToString();
                            else
                                teacherPlan.Opened = null;
                            if (!reader.IsDBNull(13))
                                teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                            else
                                teacherPlan.ModifiedBy = null;
                            if (!reader.IsDBNull(14))
                                teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                            else
                                teacherPlan.CreatedAt = null;
                            if (!reader.IsDBNull(15))
                                teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                            else
                                teacherPlan.UpdatedAt = null;
                            if (!reader.IsDBNull(16))
                                teacherPlan.OpenDate = reader.GetValue(16).ToString();
                            else
                                teacherPlan.OpenDate = null;
                            if (!reader.IsDBNull(17))
                                teacherPlan.CloseDate = reader.GetValue(17).ToString();
                            else
                                teacherPlan.CloseDate = null;
                            if (!reader.IsDBNull(18))
                                teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                            else
                                teacherPlan.WeekNumber = null;
                            if (!reader.IsDBNull(19))
                                teacherPlan.Timeframe = reader.GetValue(19).ToString();
                            else
                                teacherPlan.Timeframe = null;
                            response.Add(teacherPlan.Id, teacherPlan);
                            teacherPlan = new TeacherPlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanModel> ReadD(string connectionString, List<TeacherPlanModel> teacherPlanWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanModel> response = new Dictionary<string, TeacherPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                if (teacherPlanWhere != null && teacherPlanWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherPlanWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].IsInstructional))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_instructional` = @isInstructional" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlanWhere[i].IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_type_id` = @planTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlanWhere[i].PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_number` = @planNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlanWhere[i].PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_from` = @dateFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlanWhere[i].DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_to` = @dateTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlanWhere[i].DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, teacherPlanWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlanWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlanWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PeriodId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_id` = @periodId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodId" + i, teacherPlanWhere[i].PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Opened))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `opened` = @opened" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@opened" + i, teacherPlanWhere[i].Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanWhere[i].UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].OpenDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `open_date` = @openDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@openDate" + i, teacherPlanWhere[i].OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CloseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `close_date` = @closeDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlanWhere[i].CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].WeekNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `week_number` = @weekNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlanWhere[i].WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Timeframe))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `timeframe` = @timeframe" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlanWhere[i].Timeframe);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanWhere.Count - 1)
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
                            TeacherPlanModel teacherPlan = new TeacherPlanModel(); if (!reader.IsDBNull(0))
                                teacherPlan.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlan.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlan.Name = reader.GetValue(1).ToString();
                            else
                                teacherPlan.Name = null;
                            if (!reader.IsDBNull(2))
                                teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                            else
                                teacherPlan.IsInstructional = null;
                            if (!reader.IsDBNull(3))
                                teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                            else
                                teacherPlan.PlanTypeId = null;
                            if (!reader.IsDBNull(4))
                                teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                            else
                                teacherPlan.PlanNumber = null;
                            if (!reader.IsDBNull(5))
                                teacherPlan.DateFrom = reader.GetValue(5).ToString();
                            else
                                teacherPlan.DateFrom = null;
                            if (!reader.IsDBNull(6))
                                teacherPlan.DateTo = reader.GetValue(6).ToString();
                            else
                                teacherPlan.DateTo = null;
                            if (!reader.IsDBNull(7))
                                teacherPlan.UserId = reader.GetValue(7).ToString();
                            else
                                teacherPlan.UserId = null;
                            if (!reader.IsDBNull(8))
                                teacherPlan.Available = reader.GetValue(8).ToString();
                            else
                                teacherPlan.Available = null;
                            if (!reader.IsDBNull(9))
                                teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                            else
                                teacherPlan.SubjectCode = null;
                            if (!reader.IsDBNull(10))
                                teacherPlan.LevelCode = reader.GetValue(10).ToString();
                            else
                                teacherPlan.LevelCode = null;
                            if (!reader.IsDBNull(11))
                                teacherPlan.PeriodId = reader.GetValue(11).ToString();
                            else
                                teacherPlan.PeriodId = null;
                            if (!reader.IsDBNull(12))
                                teacherPlan.Opened = reader.GetValue(12).ToString();
                            else
                                teacherPlan.Opened = null;
                            if (!reader.IsDBNull(13))
                                teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                            else
                                teacherPlan.ModifiedBy = null;
                            if (!reader.IsDBNull(14))
                                teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                            else
                                teacherPlan.CreatedAt = null;
                            if (!reader.IsDBNull(15))
                                teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                            else
                                teacherPlan.UpdatedAt = null;
                            if (!reader.IsDBNull(16))
                                teacherPlan.OpenDate = reader.GetValue(16).ToString();
                            else
                                teacherPlan.OpenDate = null;
                            if (!reader.IsDBNull(17))
                                teacherPlan.CloseDate = reader.GetValue(17).ToString();
                            else
                                teacherPlan.CloseDate = null;
                            if (!reader.IsDBNull(18))
                                teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                            else
                                teacherPlan.WeekNumber = null;
                            if (!reader.IsDBNull(19))
                                teacherPlan.Timeframe = reader.GetValue(19).ToString();
                            else
                                teacherPlan.Timeframe = null;
                            response.Add(teacherPlan.Id, teacherPlan);
                            teacherPlan = new TeacherPlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanModel> Read(string connectionString, TeacherPlanModel teacherPlan, bool firstOne, bool orderByLastDate, TeacherPlanModel teacherPlanWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                {
                    if (teacherPlan != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlan.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Name))
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
                            cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_instructional` = @isInstructional ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_instructional` = @isInstructional ");
                            }
                            cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_type_id` = @planTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_type_id` = @planTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_number` = @planNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_number` = @planNumber ");
                            }
                            cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_from` = @dateFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_from` = @dateFrom ");
                            }
                            cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_to` = @dateTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_to` = @dateTo ");
                            }
                            cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `period_id` = @periodId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `period_id` = @periodId ");
                            }
                            cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Opened))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `opened` = @opened ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `opened` = @opened ");
                            }
                            cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `open_date` = @openDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `open_date` = @openDate ");
                            }
                            cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `close_date` = @closeDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `close_date` = @closeDate ");
                            }
                            cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `week_number` = @weekNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `week_number` = @weekNumber ");
                            }
                            cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `timeframe` = @timeframe ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `timeframe` = @timeframe ");
                            }
                            cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                        }
                    }
                    if (teacherPlanWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherPlanWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.IsInstructional))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isInstructionalWhereLike", "%" + teacherPlanWhereLike.IsInstructional + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planTypeIdWhereLike", "%" + teacherPlanWhereLike.PlanTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_number` LIKE @planNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_number` LIKE @planNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planNumberWhereLike", "%" + teacherPlanWhereLike.PlanNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_from` LIKE @dateFromWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_from` LIKE @dateFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateFromWhereLike", "%" + teacherPlanWhereLike.DateFrom + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_to` LIKE @dateToWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_to` LIKE @dateToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateToWhereLike", "%" + teacherPlanWhereLike.DateTo + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + teacherPlanWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherPlanWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherPlanWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PeriodId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `period_id` LIKE @periodIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `period_id` LIKE @periodIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodIdWhereLike", "%" + teacherPlanWhereLike.PeriodId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Opened))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `opened` LIKE @openedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `opened` LIKE @openedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openedWhereLike", "%" + teacherPlanWhereLike.Opened + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanWhereLike.UpdatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.OpenDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `open_date` LIKE @openDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `open_date` LIKE @openDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openDateWhereLike", "%" + teacherPlanWhereLike.OpenDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CloseDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `close_date` LIKE @closeDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `close_date` LIKE @closeDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@closeDateWhereLike", "%" + teacherPlanWhereLike.CloseDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.WeekNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `week_number` LIKE @weekNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `week_number` LIKE @weekNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@weekNumberWhereLike", "%" + teacherPlanWhereLike.WeekNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Timeframe))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `timeframe` LIKE @timeframeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `timeframe` LIKE @timeframeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@timeframeWhereLike", "%" + teacherPlanWhereLike.Timeframe + "%");
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
                                teacherPlan = new TeacherPlanModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlan.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlan.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlan.Name = reader.GetValue(1).ToString();
                                else
                                    teacherPlan.Name = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                                else
                                    teacherPlan.IsInstructional = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                                else
                                    teacherPlan.PlanTypeId = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                                else
                                    teacherPlan.PlanNumber = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlan.DateFrom = reader.GetValue(5).ToString();
                                else
                                    teacherPlan.DateFrom = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlan.DateTo = reader.GetValue(6).ToString();
                                else
                                    teacherPlan.DateTo = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlan.UserId = reader.GetValue(7).ToString();
                                else
                                    teacherPlan.UserId = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlan.Available = reader.GetValue(8).ToString();
                                else
                                    teacherPlan.Available = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                                else
                                    teacherPlan.SubjectCode = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlan.LevelCode = reader.GetValue(10).ToString();
                                else
                                    teacherPlan.LevelCode = null;
                                if (!reader.IsDBNull(11))
                                    teacherPlan.PeriodId = reader.GetValue(11).ToString();
                                else
                                    teacherPlan.PeriodId = null;
                                if (!reader.IsDBNull(12))
                                    teacherPlan.Opened = reader.GetValue(12).ToString();
                                else
                                    teacherPlan.Opened = null;
                                if (!reader.IsDBNull(13))
                                    teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                                else
                                    teacherPlan.ModifiedBy = null;
                                if (!reader.IsDBNull(14))
                                    teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                                else
                                    teacherPlan.CreatedAt = null;
                                if (!reader.IsDBNull(15))
                                    teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                                else
                                    teacherPlan.UpdatedAt = null;
                                if (!reader.IsDBNull(16))
                                    teacherPlan.OpenDate = reader.GetValue(16).ToString();
                                else
                                    teacherPlan.OpenDate = null;
                                if (!reader.IsDBNull(17))
                                    teacherPlan.CloseDate = reader.GetValue(17).ToString();
                                else
                                    teacherPlan.CloseDate = null;
                                if (!reader.IsDBNull(18))
                                    teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                                else
                                    teacherPlan.WeekNumber = null;
                                if (!reader.IsDBNull(19))
                                    teacherPlan.Timeframe = reader.GetValue(19).ToString();
                                else
                                    teacherPlan.Timeframe = null;
                                response.Add(teacherPlan);
                                teacherPlan = new TeacherPlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanModel>();
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
                response = new List<TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanModel> Read(string connectionString, TeacherPlanModel teacherPlan, bool orderByLastDate, int fromRow, int limit, TeacherPlanModel teacherPlanWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                {
                    if (teacherPlan != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlan.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Name))
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
                            cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_instructional` = @isInstructional ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_instructional` = @isInstructional ");
                            }
                            cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_type_id` = @planTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_type_id` = @planTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_number` = @planNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_number` = @planNumber ");
                            }
                            cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_from` = @dateFrom ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_from` = @dateFrom ");
                            }
                            cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `date_to` = @dateTo ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `date_to` = @dateTo ");
                            }
                            cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `period_id` = @periodId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `period_id` = @periodId ");
                            }
                            cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Opened))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `opened` = @opened ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `opened` = @opened ");
                            }
                            cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `open_date` = @openDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `open_date` = @openDate ");
                            }
                            cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `close_date` = @closeDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `close_date` = @closeDate ");
                            }
                            cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `week_number` = @weekNumber ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `week_number` = @weekNumber ");
                            }
                            cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `timeframe` = @timeframe ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `timeframe` = @timeframe ");
                            }
                            cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                        }
                    }
                    if (teacherPlanWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + teacherPlanWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.IsInstructional))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_instructional` LIKE @isInstructionalWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isInstructionalWhereLike", "%" + teacherPlanWhereLike.IsInstructional + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_type_id` LIKE @planTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planTypeIdWhereLike", "%" + teacherPlanWhereLike.PlanTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PlanNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_number` LIKE @planNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_number` LIKE @planNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planNumberWhereLike", "%" + teacherPlanWhereLike.PlanNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateFrom))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_from` LIKE @dateFromWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_from` LIKE @dateFromWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateFromWhereLike", "%" + teacherPlanWhereLike.DateFrom + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.DateTo))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `date_to` LIKE @dateToWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `date_to` LIKE @dateToWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dateToWhereLike", "%" + teacherPlanWhereLike.DateTo + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + teacherPlanWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Available))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `available` LIKE @availableWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `available` LIKE @availableWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.SubjectCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `subject_code` LIKE @subjectCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@subjectCodeWhereLike", "%" + teacherPlanWhereLike.SubjectCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.LevelCode))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_code` LIKE @levelCodeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_code` LIKE @levelCodeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelCodeWhereLike", "%" + teacherPlanWhereLike.LevelCode + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.PeriodId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `period_id` LIKE @periodIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `period_id` LIKE @periodIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@periodIdWhereLike", "%" + teacherPlanWhereLike.PeriodId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Opened))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `opened` LIKE @openedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `opened` LIKE @openedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openedWhereLike", "%" + teacherPlanWhereLike.Opened + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanWhereLike.UpdatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.OpenDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `open_date` LIKE @openDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `open_date` LIKE @openDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@openDateWhereLike", "%" + teacherPlanWhereLike.OpenDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.CloseDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `close_date` LIKE @closeDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `close_date` LIKE @closeDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@closeDateWhereLike", "%" + teacherPlanWhereLike.CloseDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.WeekNumber))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `week_number` LIKE @weekNumberWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `week_number` LIKE @weekNumberWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@weekNumberWhereLike", "%" + teacherPlanWhereLike.WeekNumber + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhereLike.Timeframe))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `timeframe` LIKE @timeframeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `timeframe` LIKE @timeframeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@timeframeWhereLike", "%" + teacherPlanWhereLike.Timeframe + "%");
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
                                teacherPlan = new TeacherPlanModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlan.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlan.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlan.Name = reader.GetValue(1).ToString();
                                else
                                    teacherPlan.Name = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                                else
                                    teacherPlan.IsInstructional = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                                else
                                    teacherPlan.PlanTypeId = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                                else
                                    teacherPlan.PlanNumber = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlan.DateFrom = reader.GetValue(5).ToString();
                                else
                                    teacherPlan.DateFrom = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlan.DateTo = reader.GetValue(6).ToString();
                                else
                                    teacherPlan.DateTo = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlan.UserId = reader.GetValue(7).ToString();
                                else
                                    teacherPlan.UserId = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlan.Available = reader.GetValue(8).ToString();
                                else
                                    teacherPlan.Available = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                                else
                                    teacherPlan.SubjectCode = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlan.LevelCode = reader.GetValue(10).ToString();
                                else
                                    teacherPlan.LevelCode = null;
                                if (!reader.IsDBNull(11))
                                    teacherPlan.PeriodId = reader.GetValue(11).ToString();
                                else
                                    teacherPlan.PeriodId = null;
                                if (!reader.IsDBNull(12))
                                    teacherPlan.Opened = reader.GetValue(12).ToString();
                                else
                                    teacherPlan.Opened = null;
                                if (!reader.IsDBNull(13))
                                    teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                                else
                                    teacherPlan.ModifiedBy = null;
                                if (!reader.IsDBNull(14))
                                    teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                                else
                                    teacherPlan.CreatedAt = null;
                                if (!reader.IsDBNull(15))
                                    teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                                else
                                    teacherPlan.UpdatedAt = null;
                                if (!reader.IsDBNull(16))
                                    teacherPlan.OpenDate = reader.GetValue(16).ToString();
                                else
                                    teacherPlan.OpenDate = null;
                                if (!reader.IsDBNull(17))
                                    teacherPlan.CloseDate = reader.GetValue(17).ToString();
                                else
                                    teacherPlan.CloseDate = null;
                                if (!reader.IsDBNull(18))
                                    teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                                else
                                    teacherPlan.WeekNumber = null;
                                if (!reader.IsDBNull(19))
                                    teacherPlan.Timeframe = reader.GetValue(19).ToString();
                                else
                                    teacherPlan.Timeframe = null;
                                response.Add(teacherPlan);
                                teacherPlan = new TeacherPlanModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanModel>();
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
                response = new List<TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanModel> Read(string connectionString, List<TeacherPlanModel> teacherPlanWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                if (teacherPlanWhere != null && teacherPlanWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherPlanWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].IsInstructional))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_instructional` = @isInstructional" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlanWhere[i].IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_type_id` = @planTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlanWhere[i].PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_number` = @planNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlanWhere[i].PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_from` = @dateFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlanWhere[i].DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_to` = @dateTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlanWhere[i].DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, teacherPlanWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlanWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlanWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PeriodId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_id` = @periodId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodId" + i, teacherPlanWhere[i].PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Opened))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `opened` = @opened" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@opened" + i, teacherPlanWhere[i].Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanWhere[i].UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].OpenDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `open_date` = @openDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@openDate" + i, teacherPlanWhere[i].OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CloseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `close_date` = @closeDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlanWhere[i].CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].WeekNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `week_number` = @weekNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlanWhere[i].WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Timeframe))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `timeframe` = @timeframe" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlanWhere[i].Timeframe);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanWhere.Count - 1)
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
                            TeacherPlanModel teacherPlan = new TeacherPlanModel(); if (!reader.IsDBNull(0))
                                teacherPlan.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlan.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlan.Name = reader.GetValue(1).ToString();
                            else
                                teacherPlan.Name = null;
                            if (!reader.IsDBNull(2))
                                teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                            else
                                teacherPlan.IsInstructional = null;
                            if (!reader.IsDBNull(3))
                                teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                            else
                                teacherPlan.PlanTypeId = null;
                            if (!reader.IsDBNull(4))
                                teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                            else
                                teacherPlan.PlanNumber = null;
                            if (!reader.IsDBNull(5))
                                teacherPlan.DateFrom = reader.GetValue(5).ToString();
                            else
                                teacherPlan.DateFrom = null;
                            if (!reader.IsDBNull(6))
                                teacherPlan.DateTo = reader.GetValue(6).ToString();
                            else
                                teacherPlan.DateTo = null;
                            if (!reader.IsDBNull(7))
                                teacherPlan.UserId = reader.GetValue(7).ToString();
                            else
                                teacherPlan.UserId = null;
                            if (!reader.IsDBNull(8))
                                teacherPlan.Available = reader.GetValue(8).ToString();
                            else
                                teacherPlan.Available = null;
                            if (!reader.IsDBNull(9))
                                teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                            else
                                teacherPlan.SubjectCode = null;
                            if (!reader.IsDBNull(10))
                                teacherPlan.LevelCode = reader.GetValue(10).ToString();
                            else
                                teacherPlan.LevelCode = null;
                            if (!reader.IsDBNull(11))
                                teacherPlan.PeriodId = reader.GetValue(11).ToString();
                            else
                                teacherPlan.PeriodId = null;
                            if (!reader.IsDBNull(12))
                                teacherPlan.Opened = reader.GetValue(12).ToString();
                            else
                                teacherPlan.Opened = null;
                            if (!reader.IsDBNull(13))
                                teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                            else
                                teacherPlan.ModifiedBy = null;
                            if (!reader.IsDBNull(14))
                                teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                            else
                                teacherPlan.CreatedAt = null;
                            if (!reader.IsDBNull(15))
                                teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                            else
                                teacherPlan.UpdatedAt = null;
                            if (!reader.IsDBNull(16))
                                teacherPlan.OpenDate = reader.GetValue(16).ToString();
                            else
                                teacherPlan.OpenDate = null;
                            if (!reader.IsDBNull(17))
                                teacherPlan.CloseDate = reader.GetValue(17).ToString();
                            else
                                teacherPlan.CloseDate = null;
                            if (!reader.IsDBNull(18))
                                teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                            else
                                teacherPlan.WeekNumber = null;
                            if (!reader.IsDBNull(19))
                                teacherPlan.Timeframe = reader.GetValue(19).ToString();
                            else
                                teacherPlan.Timeframe = null;
                            response.Add(teacherPlan);
                            teacherPlan = new TeacherPlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanModel> Read(string connectionString, List<TeacherPlanModel> teacherPlanWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `is_instructional`, `plan_type_id`, `plan_number`, `date_from`, `date_to`, `user_id`, `available`, `subject_code`, `level_code`, `period_id`, `opened`, `modified_by`, `created_at`, `updated_at`, `open_date`, `close_date`, `week_number`, `timeframe`  FROM teacher_plans ";
                if (teacherPlanWhere != null && teacherPlanWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, teacherPlanWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].IsInstructional))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_instructional` = @isInstructional" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlanWhere[i].IsInstructional);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_type_id` = @planTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlanWhere[i].PlanTypeId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PlanNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_number` = @planNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlanWhere[i].PlanNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateFrom))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_from` = @dateFrom" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlanWhere[i].DateFrom);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].DateTo))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `date_to` = @dateTo" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlanWhere[i].DateTo);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, teacherPlanWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlanWhere[i].SubjectCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlanWhere[i].LevelCode);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].PeriodId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `period_id` = @periodId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@periodId" + i, teacherPlanWhere[i].PeriodId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Opened))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `opened` = @opened" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@opened" + i, teacherPlanWhere[i].Opened);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanWhere[i].UpdatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].OpenDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `open_date` = @openDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@openDate" + i, teacherPlanWhere[i].OpenDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].CloseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `close_date` = @closeDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlanWhere[i].CloseDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].WeekNumber))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `week_number` = @weekNumber" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlanWhere[i].WeekNumber);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanWhere[i].Timeframe))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `timeframe` = @timeframe" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlanWhere[i].Timeframe);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanWhere.Count - 1)
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
                            TeacherPlanModel teacherPlan = new TeacherPlanModel(); if (!reader.IsDBNull(0))
                                teacherPlan.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlan.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlan.Name = reader.GetValue(1).ToString();
                            else
                                teacherPlan.Name = null;
                            if (!reader.IsDBNull(2))
                                teacherPlan.IsInstructional = reader.GetValue(2).ToString();
                            else
                                teacherPlan.IsInstructional = null;
                            if (!reader.IsDBNull(3))
                                teacherPlan.PlanTypeId = reader.GetValue(3).ToString();
                            else
                                teacherPlan.PlanTypeId = null;
                            if (!reader.IsDBNull(4))
                                teacherPlan.PlanNumber = reader.GetValue(4).ToString();
                            else
                                teacherPlan.PlanNumber = null;
                            if (!reader.IsDBNull(5))
                                teacherPlan.DateFrom = reader.GetValue(5).ToString();
                            else
                                teacherPlan.DateFrom = null;
                            if (!reader.IsDBNull(6))
                                teacherPlan.DateTo = reader.GetValue(6).ToString();
                            else
                                teacherPlan.DateTo = null;
                            if (!reader.IsDBNull(7))
                                teacherPlan.UserId = reader.GetValue(7).ToString();
                            else
                                teacherPlan.UserId = null;
                            if (!reader.IsDBNull(8))
                                teacherPlan.Available = reader.GetValue(8).ToString();
                            else
                                teacherPlan.Available = null;
                            if (!reader.IsDBNull(9))
                                teacherPlan.SubjectCode = reader.GetValue(9).ToString();
                            else
                                teacherPlan.SubjectCode = null;
                            if (!reader.IsDBNull(10))
                                teacherPlan.LevelCode = reader.GetValue(10).ToString();
                            else
                                teacherPlan.LevelCode = null;
                            if (!reader.IsDBNull(11))
                                teacherPlan.PeriodId = reader.GetValue(11).ToString();
                            else
                                teacherPlan.PeriodId = null;
                            if (!reader.IsDBNull(12))
                                teacherPlan.Opened = reader.GetValue(12).ToString();
                            else
                                teacherPlan.Opened = null;
                            if (!reader.IsDBNull(13))
                                teacherPlan.ModifiedBy = reader.GetValue(13).ToString();
                            else
                                teacherPlan.ModifiedBy = null;
                            if (!reader.IsDBNull(14))
                                teacherPlan.CreatedAt = reader.GetValue(14).ToString();
                            else
                                teacherPlan.CreatedAt = null;
                            if (!reader.IsDBNull(15))
                                teacherPlan.UpdatedAt = reader.GetValue(15).ToString();
                            else
                                teacherPlan.UpdatedAt = null;
                            if (!reader.IsDBNull(16))
                                teacherPlan.OpenDate = reader.GetValue(16).ToString();
                            else
                                teacherPlan.OpenDate = null;
                            if (!reader.IsDBNull(17))
                                teacherPlan.CloseDate = reader.GetValue(17).ToString();
                            else
                                teacherPlan.CloseDate = null;
                            if (!reader.IsDBNull(18))
                                teacherPlan.WeekNumber = reader.GetValue(18).ToString();
                            else
                                teacherPlan.WeekNumber = null;
                            if (!reader.IsDBNull(19))
                                teacherPlan.Timeframe = reader.GetValue(19).ToString();
                            else
                                teacherPlan.Timeframe = null;
                            response.Add(teacherPlan);
                            teacherPlan = new TeacherPlanModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanModel> Create(string connectionString, TeacherPlanModel teacherPlan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_plans (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherPlan.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Name))
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
                    cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                }
                if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `is_instructional` ");
                        values = String.Concat(values, ", @isInstructional");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `is_instructional` ");
                        values = String.Concat(values, " @isInstructional");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                }
                if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `plan_type_id` ");
                        values = String.Concat(values, ", @planTypeId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `plan_type_id` ");
                        values = String.Concat(values, " @planTypeId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                }
                if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `plan_number` ");
                        values = String.Concat(values, ", @planNumber");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `plan_number` ");
                        values = String.Concat(values, " @planNumber");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                }
                if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `date_from` ");
                        values = String.Concat(values, ", @dateFrom");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `date_from` ");
                        values = String.Concat(values, " @dateFrom");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                }
                if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `date_to` ");
                        values = String.Concat(values, ", @dateTo");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `date_to` ");
                        values = String.Concat(values, " @dateTo");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                }
                if (!String.IsNullOrEmpty(teacherPlan.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Available))
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
                    cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                }
                if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `subject_code` ");
                        values = String.Concat(values, ", @subjectCode");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `subject_code` ");
                        values = String.Concat(values, " @subjectCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                }
                if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `level_code` ");
                        values = String.Concat(values, ", @levelCode");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `level_code` ");
                        values = String.Concat(values, " @levelCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                }
                if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `period_id` ");
                        values = String.Concat(values, ", @periodId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `period_id` ");
                        values = String.Concat(values, " @periodId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Opened))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `opened` ");
                        values = String.Concat(values, ", @opened");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `opened` ");
                        values = String.Concat(values, " @opened");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                }
                if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `open_date` ");
                        values = String.Concat(values, ", @openDate");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `open_date` ");
                        values = String.Concat(values, " @openDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                }
                if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `close_date` ");
                        values = String.Concat(values, ", @closeDate");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `close_date` ");
                        values = String.Concat(values, " @closeDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                }
                if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `week_number` ");
                        values = String.Concat(values, ", @weekNumber");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `week_number` ");
                        values = String.Concat(values, " @weekNumber");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `timeframe` ");
                        values = String.Concat(values, ", @timeframe");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `timeframe` ");
                        values = String.Concat(values, " @timeframe");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherPlan.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherPlan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
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
        public List<TeacherPlanModel> Create(string connectionString, List<TeacherPlanModel> teacherPlanList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
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
                        for (int i = 0; i < teacherPlanList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_plans (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, teacherPlanList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].IsInstructional))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `is_instructional` ");
                                        values = String.Concat(values, ", @isInstructional" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `is_instructional` ");
                                        values = String.Concat(values, "@isInstructional" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlanList[i].IsInstructional);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].PlanTypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `plan_type_id` ");
                                        values = String.Concat(values, ", @planTypeId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `plan_type_id` ");
                                        values = String.Concat(values, "@planTypeId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlanList[i].PlanTypeId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].PlanNumber))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `plan_number` ");
                                        values = String.Concat(values, ", @planNumber" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `plan_number` ");
                                        values = String.Concat(values, "@planNumber" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlanList[i].PlanNumber);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].DateFrom))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `date_from` ");
                                        values = String.Concat(values, ", @dateFrom" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `date_from` ");
                                        values = String.Concat(values, "@dateFrom" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlanList[i].DateFrom);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].DateTo))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `date_to` ");
                                        values = String.Concat(values, ", @dateTo" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `date_to` ");
                                        values = String.Concat(values, "@dateTo" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlanList[i].DateTo);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, teacherPlanList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, teacherPlanList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].SubjectCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `subject_code` ");
                                        values = String.Concat(values, ", @subjectCode" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `subject_code` ");
                                        values = String.Concat(values, "@subjectCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlanList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].LevelCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `level_code` ");
                                        values = String.Concat(values, ", @levelCode" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `level_code` ");
                                        values = String.Concat(values, "@levelCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlanList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].PeriodId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `period_id` ");
                                        values = String.Concat(values, ", @periodId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `period_id` ");
                                        values = String.Concat(values, "@periodId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodId" + i, teacherPlanList[i].PeriodId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Opened))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `opened` ");
                                        values = String.Concat(values, ", @opened" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `opened` ");
                                        values = String.Concat(values, "@opened" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@opened" + i, teacherPlanList[i].Opened);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanList[i].UpdatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].OpenDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `open_date` ");
                                        values = String.Concat(values, ", @openDate" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `open_date` ");
                                        values = String.Concat(values, "@openDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@openDate" + i, teacherPlanList[i].OpenDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].CloseDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `close_date` ");
                                        values = String.Concat(values, ", @closeDate" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `close_date` ");
                                        values = String.Concat(values, "@closeDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlanList[i].CloseDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].WeekNumber))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `week_number` ");
                                        values = String.Concat(values, ", @weekNumber" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `week_number` ");
                                        values = String.Concat(values, "@weekNumber" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlanList[i].WeekNumber);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Timeframe))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `timeframe` ");
                                        values = String.Concat(values, ", @timeframe" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `timeframe` ");
                                        values = String.Concat(values, "@timeframe" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlanList[i].Timeframe);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherPlanList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
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

        public List<TeacherPlanModel> Update(string connectionString, TeacherPlanModel teacherPlan, TeacherPlanModel teacherPlanWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_plans SET ";
                if (!String.IsNullOrEmpty(teacherPlan.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Name))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `name` = @name");
                    else
                        sql = String.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                }
                if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `is_instructional` = @isInstructional");
                    else
                        sql = String.Concat(sql, " `is_instructional` = @isInstructional");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                }
                if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_type_id` = @planTypeId");
                    else
                        sql = String.Concat(sql, " `plan_type_id` = @planTypeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                }
                if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_number` = @planNumber");
                    else
                        sql = String.Concat(sql, " `plan_number` = @planNumber");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                }
                if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `date_from` = @dateFrom");
                    else
                        sql = String.Concat(sql, " `date_from` = @dateFrom");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                }
                if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `date_to` = @dateTo");
                    else
                        sql = String.Concat(sql, " `date_to` = @dateTo");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                }
                if (!String.IsNullOrEmpty(teacherPlan.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                }
                if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = String.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                }
                if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = String.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                }
                if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `period_id` = @periodId");
                    else
                        sql = String.Concat(sql, " `period_id` = @periodId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Opened))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `opened` = @opened");
                    else
                        sql = String.Concat(sql, " `opened` = @opened");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                }
                if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `open_date` = @openDate");
                    else
                        sql = String.Concat(sql, " `open_date` = @openDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                }
                if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `close_date` = @closeDate");
                    else
                        sql = String.Concat(sql, " `close_date` = @closeDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                }
                if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `week_number` = @weekNumber");
                    else
                        sql = String.Concat(sql, " `week_number` = @weekNumber");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                }
                if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `timeframe` = @timeframe");
                    else
                        sql = String.Concat(sql, " `timeframe` = @timeframe");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                }
                if (teacherPlanWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherPlanWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherPlanWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = String.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", teacherPlanWhere.Name);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.IsInstructional))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `is_instructional` = @isInstructionalWhere");
                        else
                            sql = String.Concat(sql, " `is_instructional` = @isInstructionalWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isInstructionalWhere", teacherPlanWhere.IsInstructional);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.PlanTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_type_id` = @planTypeIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_type_id` = @planTypeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planTypeIdWhere", teacherPlanWhere.PlanTypeId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.PlanNumber))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_number` = @planNumberWhere");
                        else
                            sql = String.Concat(sql, " `plan_number` = @planNumberWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planNumberWhere", teacherPlanWhere.PlanNumber);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.DateFrom))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `date_from` = @dateFromWhere");
                        else
                            sql = String.Concat(sql, " `date_from` = @dateFromWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateFromWhere", teacherPlanWhere.DateFrom);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.DateTo))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `date_to` = @dateToWhere");
                        else
                            sql = String.Concat(sql, " `date_to` = @dateToWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateToWhere", teacherPlanWhere.DateTo);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", teacherPlanWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", teacherPlanWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", teacherPlanWhere.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", teacherPlanWhere.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.PeriodId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `period_id` = @periodIdWhere");
                        else
                            sql = String.Concat(sql, " `period_id` = @periodIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodIdWhere", teacherPlanWhere.PeriodId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.Opened))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `opened` = @openedWhere");
                        else
                            sql = String.Concat(sql, " `opened` = @openedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@openedWhere", teacherPlanWhere.Opened);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherPlanWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherPlanWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherPlanWhere.UpdatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.OpenDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `open_date` = @openDateWhere");
                        else
                            sql = String.Concat(sql, " `open_date` = @openDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@openDateWhere", teacherPlanWhere.OpenDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.CloseDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `close_date` = @closeDateWhere");
                        else
                            sql = String.Concat(sql, " `close_date` = @closeDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@closeDateWhere", teacherPlanWhere.CloseDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.WeekNumber))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `week_number` = @weekNumberWhere");
                        else
                            sql = String.Concat(sql, " `week_number` = @weekNumberWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@weekNumberWhere", teacherPlanWhere.WeekNumber);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanWhere.Timeframe))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `timeframe` = @timeframeWhere");
                        else
                            sql = String.Concat(sql, " `timeframe` = @timeframeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@timeframeWhere", teacherPlanWhere.Timeframe);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                teacherPlan.Valid = true;
                response.Add(teacherPlan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>()
                {
                    new TeacherPlanModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            e.Message
                        }
                    }
                };
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

        public List<TeacherPlanModel> Update(string connectionString, List<TeacherPlanModel> teacherPlanList, List<TeacherPlanModel> teacherPlanWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
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
                        for (int i = 0; i < teacherPlanList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_plans SET ";
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Name))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = String.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, teacherPlanList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].IsInstructional))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `is_instructional` = @isInstructional" + i);
                                    else
                                        sql = String.Concat(sql, " `is_instructional` = @isInstructional" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlanList[i].IsInstructional);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].PlanTypeId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_type_id` = @planTypeId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_type_id` = @planTypeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlanList[i].PlanTypeId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].PlanNumber))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_number` = @planNumber" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_number` = @planNumber" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlanList[i].PlanNumber);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].DateFrom))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `date_from` = @dateFrom" + i);
                                    else
                                        sql = String.Concat(sql, " `date_from` = @dateFrom" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlanList[i].DateFrom);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].DateTo))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `date_to` = @dateTo" + i);
                                    else
                                        sql = String.Concat(sql, " `date_to` = @dateTo" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlanList[i].DateTo);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, teacherPlanList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, teacherPlanList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlanList[i].SubjectCode);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlanList[i].LevelCode);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].PeriodId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `period_id` = @periodId" + i);
                                    else
                                        sql = String.Concat(sql, " `period_id` = @periodId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@periodId" + i, teacherPlanList[i].PeriodId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Opened))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `opened` = @opened" + i);
                                    else
                                        sql = String.Concat(sql, " `opened` = @opened" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@opened" + i, teacherPlanList[i].Opened);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanList[i].UpdatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].OpenDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `open_date` = @openDate" + i);
                                    else
                                        sql = String.Concat(sql, " `open_date` = @openDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@openDate" + i, teacherPlanList[i].OpenDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].CloseDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `close_date` = @closeDate" + i);
                                    else
                                        sql = String.Concat(sql, " `close_date` = @closeDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlanList[i].CloseDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].WeekNumber))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `week_number` = @weekNumber" + i);
                                    else
                                        sql = String.Concat(sql, " `week_number` = @weekNumber" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlanList[i].WeekNumber);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanList[i].Timeframe))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `timeframe` = @timeframe" + i);
                                    else
                                        sql = String.Concat(sql, " `timeframe` = @timeframe" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlanList[i].Timeframe);
                                }
                                if (teacherPlanWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherPlanWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, teacherPlanWhereList[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].IsInstructional))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `is_instructional` = @isInstructionalWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `is_instructional` = @isInstructionalWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isInstructionalWhere" + i, teacherPlanWhereList[i].IsInstructional);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].PlanTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_type_id` = @planTypeIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_type_id` = @planTypeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planTypeIdWhere" + i, teacherPlanWhereList[i].PlanTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].PlanNumber))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_number` = @planNumberWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_number` = @planNumberWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planNumberWhere" + i, teacherPlanWhereList[i].PlanNumber);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].DateFrom))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `date_from` = @dateFromWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `date_from` = @dateFromWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateFromWhere" + i, teacherPlanWhereList[i].DateFrom);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].DateTo))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `date_to` = @dateToWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `date_to` = @dateToWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateToWhere" + i, teacherPlanWhereList[i].DateTo);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, teacherPlanWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, teacherPlanWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, teacherPlanWhereList[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, teacherPlanWhereList[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].PeriodId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `period_id` = @periodIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `period_id` = @periodIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodIdWhere" + i, teacherPlanWhereList[i].PeriodId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].Opened))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `opened` = @openedWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `opened` = @openedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@openedWhere" + i, teacherPlanWhereList[i].Opened);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherPlanWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherPlanWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherPlanWhereList[i].UpdatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].OpenDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `open_date` = @openDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `open_date` = @openDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@openDateWhere" + i, teacherPlanWhereList[i].OpenDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].CloseDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `close_date` = @closeDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `close_date` = @closeDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@closeDateWhere" + i, teacherPlanWhereList[i].CloseDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].WeekNumber))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `week_number` = @weekNumberWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `week_number` = @weekNumberWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@weekNumberWhere" + i, teacherPlanWhereList[i].WeekNumber);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanWhereList[i].Timeframe))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `timeframe` = @timeframeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `timeframe` = @timeframeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@timeframeWhere" + i, teacherPlanWhereList[i].Timeframe);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
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

        public List<TeacherPlanModel> Delete(string connectionString, TeacherPlanModel teacherPlan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_plans  ";
                if (teacherPlan != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherPlan.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherPlan.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @name");
                        else
                            sql = String.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", teacherPlan.Name);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.IsInstructional))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `is_instructional` = @isInstructional");
                        else
                            sql = String.Concat(sql, " `is_instructional` = @isInstructional");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isInstructional", teacherPlan.IsInstructional);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.PlanTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_type_id` = @planTypeId");
                        else
                            sql = String.Concat(sql, " `plan_type_id` = @planTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planTypeId", teacherPlan.PlanTypeId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.PlanNumber))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_number` = @planNumber");
                        else
                            sql = String.Concat(sql, " `plan_number` = @planNumber");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planNumber", teacherPlan.PlanNumber);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.DateFrom))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `date_from` = @dateFrom");
                        else
                            sql = String.Concat(sql, " `date_from` = @dateFrom");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateFrom", teacherPlan.DateFrom);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.DateTo))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `date_to` = @dateTo");
                        else
                            sql = String.Concat(sql, " `date_to` = @dateTo");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dateTo", teacherPlan.DateTo);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", teacherPlan.UserId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", teacherPlan.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.SubjectCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = String.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", teacherPlan.SubjectCode);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.LevelCode))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = String.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", teacherPlan.LevelCode);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.PeriodId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `period_id` = @periodId");
                        else
                            sql = String.Concat(sql, " `period_id` = @periodId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@periodId", teacherPlan.PeriodId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.Opened))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `opened` = @opened");
                        else
                            sql = String.Concat(sql, " `opened` = @opened");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@opened", teacherPlan.Opened);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherPlan.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherPlan.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherPlan.UpdatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.OpenDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `open_date` = @openDate");
                        else
                            sql = String.Concat(sql, " `open_date` = @openDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@openDate", teacherPlan.OpenDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.CloseDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `close_date` = @closeDate");
                        else
                            sql = String.Concat(sql, " `close_date` = @closeDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@closeDate", teacherPlan.CloseDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.WeekNumber))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `week_number` = @weekNumber");
                        else
                            sql = String.Concat(sql, " `week_number` = @weekNumber");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@weekNumber", teacherPlan.WeekNumber);
                    }
                    if (!String.IsNullOrEmpty(teacherPlan.Timeframe))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `timeframe` = @timeframe");
                        else
                            sql = String.Concat(sql, " `timeframe` = @timeframe");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@timeframe", teacherPlan.Timeframe);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherPlan);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
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

        public List<TeacherPlanModel> Delete(string connectionString, List<TeacherPlanModel> teacherPlan)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanModel> response = new List<TeacherPlanModel>();
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
                        for (int i = 0; i < teacherPlan.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_plans  ";
                                if (teacherPlan[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherPlan[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherPlan[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, teacherPlan[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].IsInstructional))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `is_instructional` = @isInstructional" + i);
                                        else
                                            sql = String.Concat(sql, " `is_instructional` = @isInstructional" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isInstructional" + i, teacherPlan[i].IsInstructional);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].PlanTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_type_id` = @planTypeId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_type_id` = @planTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planTypeId" + i, teacherPlan[i].PlanTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].PlanNumber))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_number` = @planNumber" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_number` = @planNumber" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planNumber" + i, teacherPlan[i].PlanNumber);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].DateFrom))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `date_from` = @dateFrom" + i);
                                        else
                                            sql = String.Concat(sql, " `date_from` = @dateFrom" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateFrom" + i, teacherPlan[i].DateFrom);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].DateTo))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `date_to` = @dateTo" + i);
                                        else
                                            sql = String.Concat(sql, " `date_to` = @dateTo" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dateTo" + i, teacherPlan[i].DateTo);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, teacherPlan[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, teacherPlan[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = String.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, teacherPlan[i].SubjectCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = String.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, teacherPlan[i].LevelCode);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].PeriodId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `period_id` = @periodId" + i);
                                        else
                                            sql = String.Concat(sql, " `period_id` = @periodId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@periodId" + i, teacherPlan[i].PeriodId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].Opened))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `opened` = @opened" + i);
                                        else
                                            sql = String.Concat(sql, " `opened` = @opened" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@opened" + i, teacherPlan[i].Opened);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlan[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlan[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlan[i].UpdatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].OpenDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `open_date` = @openDate" + i);
                                        else
                                            sql = String.Concat(sql, " `open_date` = @openDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@openDate" + i, teacherPlan[i].OpenDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].CloseDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `close_date` = @closeDate" + i);
                                        else
                                            sql = String.Concat(sql, " `close_date` = @closeDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@closeDate" + i, teacherPlan[i].CloseDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].WeekNumber))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `week_number` = @weekNumber" + i);
                                        else
                                            sql = String.Concat(sql, " `week_number` = @weekNumber" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@weekNumber" + i, teacherPlan[i].WeekNumber);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlan[i].Timeframe))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `timeframe` = @timeframe" + i);
                                        else
                                            sql = String.Concat(sql, " `timeframe` = @timeframe" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@timeframe" + i, teacherPlan[i].Timeframe);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlan[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanModel>();
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
