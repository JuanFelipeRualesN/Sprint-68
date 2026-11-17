using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class PlanAssignmentUserResultDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlanAssignmentUserResultModel> ReadD(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult, bool firstOne, bool orderByLastDate, PlanAssignmentUserResultModel planAssignmentUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanAssignmentUserResultModel> response = new Dictionary<string, PlanAssignmentUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                {
                    if (planAssignmentUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` = @planQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` = @planQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                        }
                    }
                    if (planAssignmentUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planAssignmentUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.PlanQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planQuizIdWhereLike", "%" + planAssignmentUserResultWhereLike.PlanQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planAssignmentUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planAssignmentUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + planAssignmentUserResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planAssignmentUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planAssignmentUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planAssignmentUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planAssignmentUserResultWhereLike.UpdatedAt + "%");
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
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planAssignmentUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                                else
                                    planAssignmentUserResult.PlanQuizId = null;
                                if (!reader.IsDBNull(2))
                                    planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planAssignmentUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                                else
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(4))
                                    planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                                else
                                    planAssignmentUserResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                                else
                                    planAssignmentUserResult.TotalPercent = null;
                                if (!reader.IsDBNull(6))
                                    planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planAssignmentUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planAssignmentUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planAssignmentUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planAssignmentUserResult.UpdatedAt = null;
                                response.Add(planAssignmentUserResult.Id, planAssignmentUserResult);
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanAssignmentUserResultModel>();
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
                response = new Dictionary<string, PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanAssignmentUserResultModel> ReadD(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult, bool orderByLastDate, int fromRow, int limit, PlanAssignmentUserResultModel planAssignmentUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanAssignmentUserResultModel> response = new Dictionary<string, PlanAssignmentUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                {
                    if (planAssignmentUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` = @planQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` = @planQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                        }
                    }
                    if (planAssignmentUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planAssignmentUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.PlanQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planQuizIdWhereLike", "%" + planAssignmentUserResultWhereLike.PlanQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planAssignmentUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planAssignmentUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + planAssignmentUserResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planAssignmentUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planAssignmentUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planAssignmentUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planAssignmentUserResultWhereLike.UpdatedAt + "%");
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
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planAssignmentUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                                else
                                    planAssignmentUserResult.PlanQuizId = null;
                                if (!reader.IsDBNull(2))
                                    planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planAssignmentUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                                else
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(4))
                                    planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                                else
                                    planAssignmentUserResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                                else
                                    planAssignmentUserResult.TotalPercent = null;
                                if (!reader.IsDBNull(6))
                                    planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planAssignmentUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planAssignmentUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planAssignmentUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planAssignmentUserResult.UpdatedAt = null;
                                response.Add(planAssignmentUserResult.Id, planAssignmentUserResult);
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanAssignmentUserResultModel>();
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
                response = new Dictionary<string, PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanAssignmentUserResultModel> ReadD(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanAssignmentUserResultModel> response = new Dictionary<string, PlanAssignmentUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                if (planAssignmentUserResultWhere != null && planAssignmentUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planAssignmentUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].PlanQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_quiz_id` = @planQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResultWhere[i].PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planAssignmentUserResultWhere.Count - 1)
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
                            PlanAssignmentUserResultModel planAssignmentUserResult = new PlanAssignmentUserResultModel(); if (!reader.IsDBNull(0))
                                planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planAssignmentUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                            else
                                planAssignmentUserResult.PlanQuizId = null;
                            if (!reader.IsDBNull(2))
                                planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planAssignmentUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                            else
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(4))
                                planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                            else
                                planAssignmentUserResult.Score = null;
                            if (!reader.IsDBNull(5))
                                planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                            else
                                planAssignmentUserResult.TotalPercent = null;
                            if (!reader.IsDBNull(6))
                                planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planAssignmentUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planAssignmentUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planAssignmentUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planAssignmentUserResult.UpdatedAt = null;
                            response.Add(planAssignmentUserResult.Id, planAssignmentUserResult);
                            planAssignmentUserResult = new PlanAssignmentUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanAssignmentUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanAssignmentUserResultModel> ReadD(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanAssignmentUserResultModel> response = new Dictionary<string, PlanAssignmentUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                if (planAssignmentUserResultWhere != null && planAssignmentUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planAssignmentUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].PlanQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_quiz_id` = @planQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResultWhere[i].PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planAssignmentUserResultWhere.Count - 1)
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
                            PlanAssignmentUserResultModel planAssignmentUserResult = new PlanAssignmentUserResultModel(); if (!reader.IsDBNull(0))
                                planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planAssignmentUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                            else
                                planAssignmentUserResult.PlanQuizId = null;
                            if (!reader.IsDBNull(2))
                                planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planAssignmentUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                            else
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(4))
                                planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                            else
                                planAssignmentUserResult.Score = null;
                            if (!reader.IsDBNull(5))
                                planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                            else
                                planAssignmentUserResult.TotalPercent = null;
                            if (!reader.IsDBNull(6))
                                planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planAssignmentUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planAssignmentUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planAssignmentUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planAssignmentUserResult.UpdatedAt = null;
                            response.Add(planAssignmentUserResult.Id, planAssignmentUserResult);
                            planAssignmentUserResult = new PlanAssignmentUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanAssignmentUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAssignmentUserResultModel> Read(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult, bool firstOne, bool orderByLastDate, PlanAssignmentUserResultModel planAssignmentUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                {
                    if (planAssignmentUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` = @planQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` = @planQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                        }
                    }
                    if (planAssignmentUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planAssignmentUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.PlanQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planQuizIdWhereLike", "%" + planAssignmentUserResultWhereLike.PlanQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planAssignmentUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planAssignmentUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + planAssignmentUserResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planAssignmentUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planAssignmentUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planAssignmentUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planAssignmentUserResultWhereLike.UpdatedAt + "%");
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
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planAssignmentUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                                else
                                    planAssignmentUserResult.PlanQuizId = null;
                                if (!reader.IsDBNull(2))
                                    planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planAssignmentUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                                else
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(4))
                                    planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                                else
                                    planAssignmentUserResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                                else
                                    planAssignmentUserResult.TotalPercent = null;
                                if (!reader.IsDBNull(6))
                                    planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planAssignmentUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planAssignmentUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planAssignmentUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planAssignmentUserResult.UpdatedAt = null;
                                response.Add(planAssignmentUserResult);
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanAssignmentUserResultModel>();
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
                response = new List<PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAssignmentUserResultModel> Read(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult, bool orderByLastDate, int fromRow, int limit, PlanAssignmentUserResultModel planAssignmentUserResultWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                {
                    if (planAssignmentUserResult != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
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
                            cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` = @planQuizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` = @planQuizId ");
                            }
                            cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher ");
                            }
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `score` = @score ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `score` = @score ");
                            }
                            cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `total_percent` = @totalPercent ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `total_percent` = @totalPercent ");
                            }
                            cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` = @usquiUserResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` = @usquiUserResultId ");
                            }
                            cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                        }
                    }
                    if (planAssignmentUserResultWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planAssignmentUserResultWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.PlanQuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_quiz_id` LIKE @planQuizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planQuizIdWhereLike", "%" + planAssignmentUserResultWhereLike.PlanQuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planAssignmentUserResultWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `manually_corrected_by_teacher` LIKE @manuallyCorrectedByTeacherWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhereLike", "%" + planAssignmentUserResultWhereLike.ManuallyCorrectedByTeacher + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.Score))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `score` LIKE @scoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `score` LIKE @scoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@scoreWhereLike", "%" + planAssignmentUserResultWhereLike.Score + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.TotalPercent))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `total_percent` LIKE @totalPercentWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@totalPercentWhereLike", "%" + planAssignmentUserResultWhereLike.TotalPercent + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UsquiUserResultId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `usqui_user_result_id` LIKE @usquiUserResultIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultIdWhereLike", "%" + planAssignmentUserResultWhereLike.UsquiUserResultId + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planAssignmentUserResultWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planAssignmentUserResultWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planAssignmentUserResultWhereLike.UpdatedAt + "%");
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
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                                if (!reader.IsDBNull(0))
                                    planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                                else
                                    planAssignmentUserResult.Id = null;
                                if (!reader.IsDBNull(1))
                                    planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                                else
                                    planAssignmentUserResult.PlanQuizId = null;
                                if (!reader.IsDBNull(2))
                                    planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                                else
                                    planAssignmentUserResult.UserId = null;
                                if (!reader.IsDBNull(3))
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                                else
                                    planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                                if (!reader.IsDBNull(4))
                                    planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                                else
                                    planAssignmentUserResult.Score = null;
                                if (!reader.IsDBNull(5))
                                    planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                                else
                                    planAssignmentUserResult.TotalPercent = null;
                                if (!reader.IsDBNull(6))
                                    planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                                else
                                    planAssignmentUserResult.UsquiUserResultId = null;
                                if (!reader.IsDBNull(7))
                                    planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planAssignmentUserResult.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planAssignmentUserResult.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planAssignmentUserResult.UpdatedAt = null;
                                response.Add(planAssignmentUserResult);
                                planAssignmentUserResult = new PlanAssignmentUserResultModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanAssignmentUserResultModel>();
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
                response = new List<PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAssignmentUserResultModel> Read(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                if (planAssignmentUserResultWhere != null && planAssignmentUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planAssignmentUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].PlanQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_quiz_id` = @planQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResultWhere[i].PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planAssignmentUserResultWhere.Count - 1)
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
                            PlanAssignmentUserResultModel planAssignmentUserResult = new PlanAssignmentUserResultModel(); if (!reader.IsDBNull(0))
                                planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planAssignmentUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                            else
                                planAssignmentUserResult.PlanQuizId = null;
                            if (!reader.IsDBNull(2))
                                planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planAssignmentUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                            else
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(4))
                                planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                            else
                                planAssignmentUserResult.Score = null;
                            if (!reader.IsDBNull(5))
                                planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                            else
                                planAssignmentUserResult.TotalPercent = null;
                            if (!reader.IsDBNull(6))
                                planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planAssignmentUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planAssignmentUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planAssignmentUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planAssignmentUserResult.UpdatedAt = null;
                            response.Add(planAssignmentUserResult);
                            planAssignmentUserResult = new PlanAssignmentUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanAssignmentUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAssignmentUserResultModel> Read(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResultWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_quiz_id`, `user_id`, `manually_corrected_by_teacher`, `score`, `total_percent`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at`  FROM plan_assignment_user_results ";
                if (planAssignmentUserResultWhere != null && planAssignmentUserResultWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planAssignmentUserResultWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResultWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].PlanQuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_quiz_id` = @planQuizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResultWhere[i].PlanQuizId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResultWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResultWhere[i].ManuallyCorrectedByTeacher);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].Score))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `score` = @score" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResultWhere[i].Score);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].TotalPercent))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `total_percent` = @totalPercent" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResultWhere[i].TotalPercent);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UsquiUserResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `usqui_user_result_id` = @usquiUserResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResultWhere[i].UsquiUserResultId);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResultWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResultWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planAssignmentUserResultWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResultWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planAssignmentUserResultWhere.Count - 1)
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
                            PlanAssignmentUserResultModel planAssignmentUserResult = new PlanAssignmentUserResultModel(); if (!reader.IsDBNull(0))
                                planAssignmentUserResult.Id = reader.GetValue(0).ToString();
                            else
                                planAssignmentUserResult.Id = null;
                            if (!reader.IsDBNull(1))
                                planAssignmentUserResult.PlanQuizId = reader.GetValue(1).ToString();
                            else
                                planAssignmentUserResult.PlanQuizId = null;
                            if (!reader.IsDBNull(2))
                                planAssignmentUserResult.UserId = reader.GetValue(2).ToString();
                            else
                                planAssignmentUserResult.UserId = null;
                            if (!reader.IsDBNull(3))
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = reader.GetValue(3).ToString();
                            else
                                planAssignmentUserResult.ManuallyCorrectedByTeacher = null;
                            if (!reader.IsDBNull(4))
                                planAssignmentUserResult.Score = reader.GetValue(4).ToString();
                            else
                                planAssignmentUserResult.Score = null;
                            if (!reader.IsDBNull(5))
                                planAssignmentUserResult.TotalPercent = reader.GetValue(5).ToString();
                            else
                                planAssignmentUserResult.TotalPercent = null;
                            if (!reader.IsDBNull(6))
                                planAssignmentUserResult.UsquiUserResultId = reader.GetValue(6).ToString();
                            else
                                planAssignmentUserResult.UsquiUserResultId = null;
                            if (!reader.IsDBNull(7))
                                planAssignmentUserResult.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planAssignmentUserResult.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planAssignmentUserResult.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planAssignmentUserResult.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planAssignmentUserResult.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planAssignmentUserResult.UpdatedAt = null;
                            response.Add(planAssignmentUserResult);
                            planAssignmentUserResult = new PlanAssignmentUserResultModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanAssignmentUserResultModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanAssignmentUserResultModel> Create(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO plan_assignment_user_results (";
                string values = "(";
                if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
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
                    cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `plan_quiz_id` ");
                        values = String.Concat(values, ", @planQuizId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `plan_quiz_id` ");
                        values = String.Concat(values, " @planQuizId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` ");
                        values = String.Concat(values, ", @manuallyCorrectedByTeacher");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `manually_corrected_by_teacher` ");
                        values = String.Concat(values, " @manuallyCorrectedByTeacher");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `score` ");
                        values = String.Concat(values, ", @score");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `score` ");
                        values = String.Concat(values, " @score");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `total_percent` ");
                        values = String.Concat(values, ", @totalPercent");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `total_percent` ");
                        values = String.Concat(values, " @totalPercent");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `usqui_user_result_id` ");
                        values = String.Concat(values, ", @usquiUserResultId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `usqui_user_result_id` ");
                        values = String.Concat(values, " @usquiUserResultId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planAssignmentUserResult.Id = cmd.LastInsertedId.ToString();
                response.Add(planAssignmentUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
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
        public List<PlanAssignmentUserResultModel> Create(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResultList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
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
                        for (int i = 0; i < planAssignmentUserResultList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO plan_assignment_user_results (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].PlanQuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `plan_quiz_id` ");
                                        values = String.Concat(values, ", @planQuizId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `plan_quiz_id` ");
                                        values = String.Concat(values, "@planQuizId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResultList[i].PlanQuizId);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResultList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].ManuallyCorrectedByTeacher))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` ");
                                        values = String.Concat(values, ", @manuallyCorrectedByTeacher" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `manually_corrected_by_teacher` ");
                                        values = String.Concat(values, "@manuallyCorrectedByTeacher" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResultList[i].ManuallyCorrectedByTeacher);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].Score))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `score` ");
                                        values = String.Concat(values, ", @score" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `score` ");
                                        values = String.Concat(values, "@score" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].TotalPercent))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `total_percent` ");
                                        values = String.Concat(values, ", @totalPercent" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `total_percent` ");
                                        values = String.Concat(values, "@totalPercent" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResultList[i].TotalPercent);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].UsquiUserResultId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `usqui_user_result_id` ");
                                        values = String.Concat(values, ", @usquiUserResultId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `usqui_user_result_id` ");
                                        values = String.Concat(values, "@usquiUserResultId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResultList[i].UsquiUserResultId);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResultList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planAssignmentUserResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanAssignmentUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanAssignmentUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
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

        public List<PlanAssignmentUserResultModel> Update(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult, PlanAssignmentUserResultModel planAssignmentUserResultWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE plan_assignment_user_results SET ";
                if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_quiz_id` = @planQuizId");
                    else
                        sql = String.Concat(sql, " `plan_quiz_id` = @planQuizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");
                    else
                        sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `score` = @score");
                    else
                        sql = String.Concat(sql, " `score` = @score");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `total_percent` = @totalPercent");
                    else
                        sql = String.Concat(sql, " `total_percent` = @totalPercent");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `usqui_user_result_id` = @usquiUserResultId");
                    else
                        sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                }
                if (planAssignmentUserResultWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planAssignmentUserResultWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.PlanQuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_quiz_id` = @planQuizIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_quiz_id` = @planQuizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planQuizIdWhere", planAssignmentUserResultWhere.PlanQuizId);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", planAssignmentUserResultWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.ManuallyCorrectedByTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere");
                        else
                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhere", planAssignmentUserResultWhere.ManuallyCorrectedByTeacher);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @scoreWhere");
                        else
                            sql = String.Concat(sql, " `score` = @scoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@scoreWhere", planAssignmentUserResultWhere.Score);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.TotalPercent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_percent` = @totalPercentWhere");
                        else
                            sql = String.Concat(sql, " `total_percent` = @totalPercentWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalPercentWhere", planAssignmentUserResultWhere.TotalPercent);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.UsquiUserResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultIdWhere");
                        else
                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usquiUserResultIdWhere", planAssignmentUserResultWhere.UsquiUserResultId);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planAssignmentUserResultWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planAssignmentUserResultWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planAssignmentUserResultWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planAssignmentUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
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

        public List<PlanAssignmentUserResultModel> Update(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResultList, List<PlanAssignmentUserResultModel> planAssignmentUserResultWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
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
                        for (int i = 0; i < planAssignmentUserResultList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE plan_assignment_user_results SET ";
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResultList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].PlanQuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_quiz_id` = @planQuizId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_quiz_id` = @planQuizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResultList[i].PlanQuizId);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResultList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].ManuallyCorrectedByTeacher))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);
                                    else
                                        sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResultList[i].ManuallyCorrectedByTeacher);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].Score))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `score` = @score" + i);
                                    else
                                        sql = String.Concat(sql, " `score` = @score" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResultList[i].Score);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].TotalPercent))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `total_percent` = @totalPercent" + i);
                                    else
                                        sql = String.Concat(sql, " `total_percent` = @totalPercent" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResultList[i].TotalPercent);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].UsquiUserResultId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `usqui_user_result_id` = @usquiUserResultId" + i);
                                    else
                                        sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResultList[i].UsquiUserResultId);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResultList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResultList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planAssignmentUserResultList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResultList[i].UpdatedAt);
                                }
                                if (planAssignmentUserResultWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planAssignmentUserResultWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].PlanQuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_quiz_id` = @planQuizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_quiz_id` = @planQuizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planQuizIdWhere" + i, planAssignmentUserResultWhereList[i].PlanQuizId);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, planAssignmentUserResultWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].ManuallyCorrectedByTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacherWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacherWhere" + i, planAssignmentUserResultWhereList[i].ManuallyCorrectedByTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @scoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @scoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@scoreWhere" + i, planAssignmentUserResultWhereList[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].TotalPercent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_percent` = @totalPercentWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `total_percent` = @totalPercentWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalPercentWhere" + i, planAssignmentUserResultWhereList[i].TotalPercent);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].UsquiUserResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usquiUserResultIdWhere" + i, planAssignmentUserResultWhereList[i].UsquiUserResultId);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planAssignmentUserResultWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planAssignmentUserResultWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResultWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planAssignmentUserResultWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planAssignmentUserResultList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanAssignmentUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanAssignmentUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
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

        public List<PlanAssignmentUserResultModel> Delete(string connectionString, PlanAssignmentUserResultModel planAssignmentUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM plan_assignment_user_results  ";
                if (planAssignmentUserResult != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planAssignmentUserResult.Id);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.PlanQuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_quiz_id` = @planQuizId");
                        else
                            sql = String.Concat(sql, " `plan_quiz_id` = @planQuizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planQuizId", planAssignmentUserResult.PlanQuizId);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", planAssignmentUserResult.UserId);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.ManuallyCorrectedByTeacher))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");
                        else
                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher", planAssignmentUserResult.ManuallyCorrectedByTeacher);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.Score))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `score` = @score");
                        else
                            sql = String.Concat(sql, " `score` = @score");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@score", planAssignmentUserResult.Score);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.TotalPercent))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `total_percent` = @totalPercent");
                        else
                            sql = String.Concat(sql, " `total_percent` = @totalPercent");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@totalPercent", planAssignmentUserResult.TotalPercent);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.UsquiUserResultId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultId");
                        else
                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@usquiUserResultId", planAssignmentUserResult.UsquiUserResultId);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planAssignmentUserResult.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planAssignmentUserResult.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planAssignmentUserResult.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planAssignmentUserResult.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planAssignmentUserResult);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
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

        public List<PlanAssignmentUserResultModel> Delete(string connectionString, List<PlanAssignmentUserResultModel> planAssignmentUserResult)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanAssignmentUserResultModel> response = new List<PlanAssignmentUserResultModel>();
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
                        for (int i = 0; i < planAssignmentUserResult.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM plan_assignment_user_results  ";
                                if (planAssignmentUserResult[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planAssignmentUserResult[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].PlanQuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_quiz_id` = @planQuizId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_quiz_id` = @planQuizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planQuizId" + i, planAssignmentUserResult[i].PlanQuizId);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, planAssignmentUserResult[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].ManuallyCorrectedByTeacher))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);
                                        else
                                            sql = String.Concat(sql, " `manually_corrected_by_teacher` = @manuallyCorrectedByTeacher" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@manuallyCorrectedByTeacher" + i, planAssignmentUserResult[i].ManuallyCorrectedByTeacher);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].Score))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `score` = @score" + i);
                                        else
                                            sql = String.Concat(sql, " `score` = @score" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@score" + i, planAssignmentUserResult[i].Score);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].TotalPercent))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `total_percent` = @totalPercent" + i);
                                        else
                                            sql = String.Concat(sql, " `total_percent` = @totalPercent" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@totalPercent" + i, planAssignmentUserResult[i].TotalPercent);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].UsquiUserResultId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `usqui_user_result_id` = @usquiUserResultId" + i);
                                        else
                                            sql = String.Concat(sql, " `usqui_user_result_id` = @usquiUserResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@usquiUserResultId" + i, planAssignmentUserResult[i].UsquiUserResultId);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planAssignmentUserResult[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planAssignmentUserResult[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planAssignmentUserResult[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planAssignmentUserResult[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planAssignmentUserResult[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanAssignmentUserResultModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanAssignmentUserResultModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanAssignmentUserResultModel>();
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
