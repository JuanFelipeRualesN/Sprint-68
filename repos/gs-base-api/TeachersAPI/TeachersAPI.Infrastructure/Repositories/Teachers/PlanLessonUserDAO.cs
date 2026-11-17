using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class PlanLessonUserDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, PlanLessonUserModel> ReadD(string connectionString, PlanLessonUserModel planLessonUser, bool firstOne, bool orderByLastDate, PlanLessonUserModel planLessonUserWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonUserModel> response = new Dictionary<string, PlanLessonUserModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                {
                    if (planLessonUser != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_id` = @planId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_id` = @planId ");
                            }
                            cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `overall_score` = @overallScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `overall_score` = @overallScore ");
                            }
                            cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                        }
                    }
                    if (planLessonUserWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonUserWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_id` LIKE @planIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_id` LIKE @planIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + planLessonUserWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonUserWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + planLessonUserWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonUserWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.OverallScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@overallScoreWhereLike", "%" + planLessonUserWhereLike.OverallScore + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + planLessonUserWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonUserWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonUserWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonUserWhereLike.UpdatedAt + "%");
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
                                planLessonUser = new PlanLessonUserModel();
                                if (!reader.IsDBNull(0))
                                    planLessonUser.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonUser.PlanId = reader.GetValue(1).ToString();
                                else
                                    planLessonUser.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                                else
                                    planLessonUser.PlanLessonId = null;
                                if (!reader.IsDBNull(3))
                                    planLessonUser.LessonId = reader.GetValue(3).ToString();
                                else
                                    planLessonUser.LessonId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonUser.UserId = reader.GetValue(4).ToString();
                                else
                                    planLessonUser.UserId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonUser.OverallScore = reader.GetValue(5).ToString();
                                else
                                    planLessonUser.OverallScore = null;
                                if (!reader.IsDBNull(6))
                                    planLessonUser.Completed = reader.GetValue(6).ToString();
                                else
                                    planLessonUser.Completed = null;
                                if (!reader.IsDBNull(7))
                                    planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonUser.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonUser.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonUser.UpdatedAt = null;
                                response.Add(planLessonUser.Id, planLessonUser);
                                planLessonUser = new PlanLessonUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanLessonUserModel>();
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
                response = new Dictionary<string, PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanLessonUserModel> ReadD(string connectionString, PlanLessonUserModel planLessonUser, bool orderByLastDate, int fromRow, int limit, PlanLessonUserModel planLessonUserWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonUserModel> response = new Dictionary<string, PlanLessonUserModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                {
                    if (planLessonUser != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_id` = @planId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_id` = @planId ");
                            }
                            cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `overall_score` = @overallScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `overall_score` = @overallScore ");
                            }
                            cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                        }
                    }
                    if (planLessonUserWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonUserWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_id` LIKE @planIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_id` LIKE @planIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + planLessonUserWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonUserWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + planLessonUserWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonUserWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.OverallScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@overallScoreWhereLike", "%" + planLessonUserWhereLike.OverallScore + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + planLessonUserWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonUserWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonUserWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonUserWhereLike.UpdatedAt + "%");
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
                                planLessonUser = new PlanLessonUserModel();
                                if (!reader.IsDBNull(0))
                                    planLessonUser.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonUser.PlanId = reader.GetValue(1).ToString();
                                else
                                    planLessonUser.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                                else
                                    planLessonUser.PlanLessonId = null;
                                if (!reader.IsDBNull(3))
                                    planLessonUser.LessonId = reader.GetValue(3).ToString();
                                else
                                    planLessonUser.LessonId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonUser.UserId = reader.GetValue(4).ToString();
                                else
                                    planLessonUser.UserId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonUser.OverallScore = reader.GetValue(5).ToString();
                                else
                                    planLessonUser.OverallScore = null;
                                if (!reader.IsDBNull(6))
                                    planLessonUser.Completed = reader.GetValue(6).ToString();
                                else
                                    planLessonUser.Completed = null;
                                if (!reader.IsDBNull(7))
                                    planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonUser.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonUser.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonUser.UpdatedAt = null;
                                response.Add(planLessonUser.Id, planLessonUser);
                                planLessonUser = new PlanLessonUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, PlanLessonUserModel>();
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
                response = new Dictionary<string, PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanLessonUserModel> ReadD(string connectionString, List<PlanLessonUserModel> planLessonUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonUserModel> response = new Dictionary<string, PlanLessonUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                if (planLessonUserWhere != null && planLessonUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planLessonUserWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUserWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUserWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].OverallScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `overall_score` = @overallScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUserWhere[i].OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, planLessonUserWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonUserWhere.Count - 1)
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
                            PlanLessonUserModel planLessonUser = new PlanLessonUserModel(); if (!reader.IsDBNull(0))
                                planLessonUser.Id = reader.GetValue(0).ToString();
                            else
                                planLessonUser.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonUser.PlanId = reader.GetValue(1).ToString();
                            else
                                planLessonUser.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                            else
                                planLessonUser.PlanLessonId = null;
                            if (!reader.IsDBNull(3))
                                planLessonUser.LessonId = reader.GetValue(3).ToString();
                            else
                                planLessonUser.LessonId = null;
                            if (!reader.IsDBNull(4))
                                planLessonUser.UserId = reader.GetValue(4).ToString();
                            else
                                planLessonUser.UserId = null;
                            if (!reader.IsDBNull(5))
                                planLessonUser.OverallScore = reader.GetValue(5).ToString();
                            else
                                planLessonUser.OverallScore = null;
                            if (!reader.IsDBNull(6))
                                planLessonUser.Completed = reader.GetValue(6).ToString();
                            else
                                planLessonUser.Completed = null;
                            if (!reader.IsDBNull(7))
                                planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonUser.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonUser.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonUser.UpdatedAt = null;
                            response.Add(planLessonUser.Id, planLessonUser);
                            planLessonUser = new PlanLessonUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanLessonUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, PlanLessonUserModel> ReadD(string connectionString, List<PlanLessonUserModel> planLessonUserWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, PlanLessonUserModel> response = new Dictionary<string, PlanLessonUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                if (planLessonUserWhere != null && planLessonUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planLessonUserWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUserWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUserWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].OverallScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `overall_score` = @overallScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUserWhere[i].OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, planLessonUserWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonUserWhere.Count - 1)
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
                            PlanLessonUserModel planLessonUser = new PlanLessonUserModel(); if (!reader.IsDBNull(0))
                                planLessonUser.Id = reader.GetValue(0).ToString();
                            else
                                planLessonUser.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonUser.PlanId = reader.GetValue(1).ToString();
                            else
                                planLessonUser.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                            else
                                planLessonUser.PlanLessonId = null;
                            if (!reader.IsDBNull(3))
                                planLessonUser.LessonId = reader.GetValue(3).ToString();
                            else
                                planLessonUser.LessonId = null;
                            if (!reader.IsDBNull(4))
                                planLessonUser.UserId = reader.GetValue(4).ToString();
                            else
                                planLessonUser.UserId = null;
                            if (!reader.IsDBNull(5))
                                planLessonUser.OverallScore = reader.GetValue(5).ToString();
                            else
                                planLessonUser.OverallScore = null;
                            if (!reader.IsDBNull(6))
                                planLessonUser.Completed = reader.GetValue(6).ToString();
                            else
                                planLessonUser.Completed = null;
                            if (!reader.IsDBNull(7))
                                planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonUser.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonUser.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonUser.UpdatedAt = null;
                            response.Add(planLessonUser.Id, planLessonUser);
                            planLessonUser = new PlanLessonUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, PlanLessonUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonUserModel> Read(string connectionString, PlanLessonUserModel planLessonUser, bool firstOne, bool orderByLastDate, PlanLessonUserModel planLessonUserWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                {
                    if (planLessonUser != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_id` = @planId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_id` = @planId ");
                            }
                            cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `overall_score` = @overallScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `overall_score` = @overallScore ");
                            }
                            cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                        }
                    }
                    if (planLessonUserWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonUserWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_id` LIKE @planIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_id` LIKE @planIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + planLessonUserWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonUserWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + planLessonUserWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonUserWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.OverallScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@overallScoreWhereLike", "%" + planLessonUserWhereLike.OverallScore + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + planLessonUserWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonUserWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonUserWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonUserWhereLike.UpdatedAt + "%");
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
                                planLessonUser = new PlanLessonUserModel();
                                if (!reader.IsDBNull(0))
                                    planLessonUser.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonUser.PlanId = reader.GetValue(1).ToString();
                                else
                                    planLessonUser.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                                else
                                    planLessonUser.PlanLessonId = null;
                                if (!reader.IsDBNull(3))
                                    planLessonUser.LessonId = reader.GetValue(3).ToString();
                                else
                                    planLessonUser.LessonId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonUser.UserId = reader.GetValue(4).ToString();
                                else
                                    planLessonUser.UserId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonUser.OverallScore = reader.GetValue(5).ToString();
                                else
                                    planLessonUser.OverallScore = null;
                                if (!reader.IsDBNull(6))
                                    planLessonUser.Completed = reader.GetValue(6).ToString();
                                else
                                    planLessonUser.Completed = null;
                                if (!reader.IsDBNull(7))
                                    planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonUser.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonUser.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonUser.UpdatedAt = null;
                                response.Add(planLessonUser);
                                planLessonUser = new PlanLessonUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanLessonUserModel>();
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
                response = new List<PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonUserModel> Read(string connectionString, PlanLessonUserModel planLessonUser, bool orderByLastDate, int fromRow, int limit, PlanLessonUserModel planLessonUserWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                {
                    if (planLessonUser != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUser.Id))
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
                            cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_id` = @planId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_id` = @planId ");
                            }
                            cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` = @planLessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` = @planLessonId ");
                            }
                            cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `overall_score` = @overallScore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `overall_score` = @overallScore ");
                            }
                            cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                        }
                    }
                    if (planLessonUserWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + planLessonUserWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_id` LIKE @planIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_id` LIKE @planIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + planLessonUserWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.PlanLessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `plan_lesson_id` LIKE @planLessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + planLessonUserWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + planLessonUserWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UserId))
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
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + planLessonUserWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.OverallScore))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `overall_score` LIKE @overallScoreWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@overallScoreWhereLike", "%" + planLessonUserWhereLike.OverallScore + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + planLessonUserWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + planLessonUserWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + planLessonUserWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + planLessonUserWhereLike.UpdatedAt + "%");
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
                                planLessonUser = new PlanLessonUserModel();
                                if (!reader.IsDBNull(0))
                                    planLessonUser.Id = reader.GetValue(0).ToString();
                                else
                                    planLessonUser.Id = null;
                                if (!reader.IsDBNull(1))
                                    planLessonUser.PlanId = reader.GetValue(1).ToString();
                                else
                                    planLessonUser.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                                else
                                    planLessonUser.PlanLessonId = null;
                                if (!reader.IsDBNull(3))
                                    planLessonUser.LessonId = reader.GetValue(3).ToString();
                                else
                                    planLessonUser.LessonId = null;
                                if (!reader.IsDBNull(4))
                                    planLessonUser.UserId = reader.GetValue(4).ToString();
                                else
                                    planLessonUser.UserId = null;
                                if (!reader.IsDBNull(5))
                                    planLessonUser.OverallScore = reader.GetValue(5).ToString();
                                else
                                    planLessonUser.OverallScore = null;
                                if (!reader.IsDBNull(6))
                                    planLessonUser.Completed = reader.GetValue(6).ToString();
                                else
                                    planLessonUser.Completed = null;
                                if (!reader.IsDBNull(7))
                                    planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    planLessonUser.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    planLessonUser.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    planLessonUser.UpdatedAt = null;
                                response.Add(planLessonUser);
                                planLessonUser = new PlanLessonUserModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<PlanLessonUserModel>();
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
                response = new List<PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonUserModel> Read(string connectionString, List<PlanLessonUserModel> planLessonUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                if (planLessonUserWhere != null && planLessonUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planLessonUserWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUserWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUserWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].OverallScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `overall_score` = @overallScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUserWhere[i].OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, planLessonUserWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonUserWhere.Count - 1)
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
                            PlanLessonUserModel planLessonUser = new PlanLessonUserModel(); if (!reader.IsDBNull(0))
                                planLessonUser.Id = reader.GetValue(0).ToString();
                            else
                                planLessonUser.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonUser.PlanId = reader.GetValue(1).ToString();
                            else
                                planLessonUser.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                            else
                                planLessonUser.PlanLessonId = null;
                            if (!reader.IsDBNull(3))
                                planLessonUser.LessonId = reader.GetValue(3).ToString();
                            else
                                planLessonUser.LessonId = null;
                            if (!reader.IsDBNull(4))
                                planLessonUser.UserId = reader.GetValue(4).ToString();
                            else
                                planLessonUser.UserId = null;
                            if (!reader.IsDBNull(5))
                                planLessonUser.OverallScore = reader.GetValue(5).ToString();
                            else
                                planLessonUser.OverallScore = null;
                            if (!reader.IsDBNull(6))
                                planLessonUser.Completed = reader.GetValue(6).ToString();
                            else
                                planLessonUser.Completed = null;
                            if (!reader.IsDBNull(7))
                                planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonUser.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonUser.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonUser.UpdatedAt = null;
                            response.Add(planLessonUser);
                            planLessonUser = new PlanLessonUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanLessonUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonUserModel> Read(string connectionString, List<PlanLessonUserModel> planLessonUserWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `plan_lesson_id`, `lesson_id`, `user_id`, `overall_score`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM plan_lesson_users ";
                if (planLessonUserWhere != null && planLessonUserWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < planLessonUserWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, planLessonUserWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, planLessonUserWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUserWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUserWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, planLessonUserWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].OverallScore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `overall_score` = @overallScore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUserWhere[i].OverallScore);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, planLessonUserWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUserWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUserWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(planLessonUserWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUserWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != planLessonUserWhere.Count - 1)
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
                            PlanLessonUserModel planLessonUser = new PlanLessonUserModel(); if (!reader.IsDBNull(0))
                                planLessonUser.Id = reader.GetValue(0).ToString();
                            else
                                planLessonUser.Id = null;
                            if (!reader.IsDBNull(1))
                                planLessonUser.PlanId = reader.GetValue(1).ToString();
                            else
                                planLessonUser.PlanId = null;
                            if (!reader.IsDBNull(2))
                                planLessonUser.PlanLessonId = reader.GetValue(2).ToString();
                            else
                                planLessonUser.PlanLessonId = null;
                            if (!reader.IsDBNull(3))
                                planLessonUser.LessonId = reader.GetValue(3).ToString();
                            else
                                planLessonUser.LessonId = null;
                            if (!reader.IsDBNull(4))
                                planLessonUser.UserId = reader.GetValue(4).ToString();
                            else
                                planLessonUser.UserId = null;
                            if (!reader.IsDBNull(5))
                                planLessonUser.OverallScore = reader.GetValue(5).ToString();
                            else
                                planLessonUser.OverallScore = null;
                            if (!reader.IsDBNull(6))
                                planLessonUser.Completed = reader.GetValue(6).ToString();
                            else
                                planLessonUser.Completed = null;
                            if (!reader.IsDBNull(7))
                                planLessonUser.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                planLessonUser.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                planLessonUser.CreatedAt = reader.GetValue(8).ToString();
                            else
                                planLessonUser.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                planLessonUser.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                planLessonUser.UpdatedAt = null;
                            response.Add(planLessonUser);
                            planLessonUser = new PlanLessonUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<PlanLessonUserModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<PlanLessonUserModel> Create(string connectionString, PlanLessonUserModel planLessonUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO plan_lesson_users (";
                string values = "(";
                if (!String.IsNullOrEmpty(planLessonUser.Id))
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
                    cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                }
                if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `plan_id` ");
                        values = String.Concat(values, ", @planId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `plan_id` ");
                        values = String.Concat(values, " @planId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `plan_lesson_id` ");
                        values = String.Concat(values, ", @planLessonId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `plan_lesson_id` ");
                        values = String.Concat(values, " @planLessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `lesson_id` ");
                        values = String.Concat(values, ", @lessonId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `lesson_id` ");
                        values = String.Concat(values, " @lessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `overall_score` ");
                        values = String.Concat(values, ", @overallScore");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `overall_score` ");
                        values = String.Concat(values, " @overallScore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                }
                if (!String.IsNullOrEmpty(planLessonUser.Completed))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `completed` ");
                        values = String.Concat(values, ", @completed");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `completed` ");
                        values = String.Concat(values, " @completed");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                }
                if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                planLessonUser.Id = cmd.LastInsertedId.ToString();
                response.Add(planLessonUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
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
        public List<PlanLessonUserModel> Create(string connectionString, List<PlanLessonUserModel> planLessonUserList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
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
                        for (int i = 0; i < planLessonUserList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO plan_lesson_users (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(planLessonUserList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, planLessonUserList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].PlanId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `plan_id` ");
                                        values = String.Concat(values, ", @planId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `plan_id` ");
                                        values = String.Concat(values, "@planId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, planLessonUserList[i].PlanId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].PlanLessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `plan_lesson_id` ");
                                        values = String.Concat(values, ", @planLessonId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `plan_lesson_id` ");
                                        values = String.Concat(values, "@planLessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUserList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].LessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `lesson_id` ");
                                        values = String.Concat(values, ", @lessonId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `lesson_id` ");
                                        values = String.Concat(values, "@lessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUserList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, planLessonUserList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].OverallScore))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `overall_score` ");
                                        values = String.Concat(values, ", @overallScore" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `overall_score` ");
                                        values = String.Concat(values, "@overallScore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUserList[i].OverallScore);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].Completed))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `completed` ");
                                        values = String.Concat(values, ", @completed" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `completed` ");
                                        values = String.Concat(values, "@completed" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@completed" + i, planLessonUserList[i].Completed);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUserList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUserList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUserList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(planLessonUserList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLessonUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLessonUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
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

        public List<PlanLessonUserModel> Update(string connectionString, PlanLessonUserModel planLessonUser, PlanLessonUserModel planLessonUserWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE plan_lesson_users SET ";
                if (!String.IsNullOrEmpty(planLessonUser.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                }
                if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = String.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId");
                    else
                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lesson_id` = @lessonId");
                    else
                        sql = String.Concat(sql, " `lesson_id` = @lessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                }
                if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `overall_score` = @overallScore");
                    else
                        sql = String.Concat(sql, " `overall_score` = @overallScore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                }
                if (!String.IsNullOrEmpty(planLessonUser.Completed))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `completed` = @completed");
                    else
                        sql = String.Concat(sql, " `completed` = @completed");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                }
                if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                }
                if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                }
                if (planLessonUserWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(planLessonUserWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", planLessonUserWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.PlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", planLessonUserWhere.PlanId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonIdWhere", planLessonUserWhere.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonIdWhere", planLessonUserWhere.LessonId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", planLessonUserWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.OverallScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `overall_score` = @overallScoreWhere");
                        else
                            sql = String.Concat(sql, " `overall_score` = @overallScoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@overallScoreWhere", planLessonUserWhere.OverallScore);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.Completed))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `completed` = @completedWhere");
                        else
                            sql = String.Concat(sql, " `completed` = @completedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@completedWhere", planLessonUserWhere.Completed);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", planLessonUserWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", planLessonUserWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planLessonUserWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", planLessonUserWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(planLessonUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
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

        public List<PlanLessonUserModel> Update(string connectionString, List<PlanLessonUserModel> planLessonUserList, List<PlanLessonUserModel> planLessonUserWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
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
                        for (int i = 0; i < planLessonUserList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE plan_lesson_users SET ";
                                if (!String.IsNullOrEmpty(planLessonUserList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, planLessonUserList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, planLessonUserList[i].PlanId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].PlanLessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUserList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].LessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lesson_id` = @lessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUserList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, planLessonUserList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].OverallScore))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `overall_score` = @overallScore" + i);
                                    else
                                        sql = String.Concat(sql, " `overall_score` = @overallScore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUserList[i].OverallScore);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].Completed))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `completed` = @completed" + i);
                                    else
                                        sql = String.Concat(sql, " `completed` = @completed" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@completed" + i, planLessonUserList[i].Completed);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUserList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUserList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(planLessonUserList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUserList[i].UpdatedAt);
                                }
                                if (planLessonUserWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, planLessonUserWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, planLessonUserWhereList[i].PlanId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonIdWhere" + i, planLessonUserWhereList[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonIdWhere" + i, planLessonUserWhereList[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, planLessonUserWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].OverallScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `overall_score` = @overallScoreWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `overall_score` = @overallScoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@overallScoreWhere" + i, planLessonUserWhereList[i].OverallScore);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].Completed))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `completed` = @completedWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `completed` = @completedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@completedWhere" + i, planLessonUserWhereList[i].Completed);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, planLessonUserWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, planLessonUserWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUserWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, planLessonUserWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planLessonUserList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLessonUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLessonUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
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

        public List<PlanLessonUserModel> Delete(string connectionString, PlanLessonUserModel planLessonUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM plan_lesson_users  ";
                if (planLessonUser != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(planLessonUser.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", planLessonUser.Id);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.PlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = String.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", planLessonUser.PlanId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonId", planLessonUser.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonId", planLessonUser.LessonId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", planLessonUser.UserId);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.OverallScore))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `overall_score` = @overallScore");
                        else
                            sql = String.Concat(sql, " `overall_score` = @overallScore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@overallScore", planLessonUser.OverallScore);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.Completed))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `completed` = @completed");
                        else
                            sql = String.Concat(sql, " `completed` = @completed");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@completed", planLessonUser.Completed);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", planLessonUser.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", planLessonUser.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(planLessonUser.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", planLessonUser.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(planLessonUser);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
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

        public List<PlanLessonUserModel> Delete(string connectionString, List<PlanLessonUserModel> planLessonUser)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<PlanLessonUserModel> response = new List<PlanLessonUserModel>();
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
                        for (int i = 0; i < planLessonUser.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM plan_lesson_users  ";
                                if (planLessonUser[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(planLessonUser[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, planLessonUser[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, planLessonUser[i].PlanId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonId" + i, planLessonUser[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonId" + i, planLessonUser[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, planLessonUser[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].OverallScore))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `overall_score` = @overallScore" + i);
                                        else
                                            sql = String.Concat(sql, " `overall_score` = @overallScore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@overallScore" + i, planLessonUser[i].OverallScore);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].Completed))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `completed` = @completed" + i);
                                        else
                                            sql = String.Concat(sql, " `completed` = @completed" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@completed" + i, planLessonUser[i].Completed);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, planLessonUser[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, planLessonUser[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(planLessonUser[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, planLessonUser[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(planLessonUser[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<PlanLessonUserModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<PlanLessonUserModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<PlanLessonUserModel>();
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
