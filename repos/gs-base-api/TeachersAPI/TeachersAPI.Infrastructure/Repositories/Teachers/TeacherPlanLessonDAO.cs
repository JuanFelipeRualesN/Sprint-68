using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherPlanLessonDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherPlanLessonModel> ReadD(string connectionString, TeacherPlanLessonModel teacherPlanLesson, bool firstOne, bool orderByLastDate, TeacherPlanLessonModel teacherPlanLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonModel> response = new Dictionary<string, TeacherPlanLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                {
                    if (teacherPlanLesson != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `tries_quantity` = @triesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `tries_quantity` = @triesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `for_grading` = @forGrading ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `for_grading` = @forGrading ");
                            }
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date` = @dueDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date` = @dueDate ");
                            }
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post_required` = @prePostRequired ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post_required` = @prePostRequired ");
                            }
                            cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanLessonWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + teacherPlanLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.TriesQuantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanLessonWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ForGrading))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `for_grading` LIKE @forGradingWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `for_grading` LIKE @forGradingWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.DueDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date` LIKE @dueDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date` LIKE @dueDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanLessonWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PrePostRequired))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostRequiredWhereLike", "%" + teacherPlanLessonWhereLike.PrePostRequired + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonWhereLike.UpdatedAt + "%");
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
                                teacherPlanLesson = new TeacherPlanLessonModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLesson.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLesson.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLesson.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLesson.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLesson.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLesson.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLesson.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLesson.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLesson.PrePostRequired = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLesson.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLesson.UpdatedAt = null;
                                response.Add(teacherPlanLesson.Id, teacherPlanLesson);
                                teacherPlanLesson = new TeacherPlanLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanLessonModel>();
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
                response = new Dictionary<string, TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanLessonModel> ReadD(string connectionString, TeacherPlanLessonModel teacherPlanLesson, bool orderByLastDate, int fromRow, int limit, TeacherPlanLessonModel teacherPlanLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonModel> response = new Dictionary<string, TeacherPlanLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                {
                    if (teacherPlanLesson != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `tries_quantity` = @triesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `tries_quantity` = @triesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `for_grading` = @forGrading ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `for_grading` = @forGrading ");
                            }
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date` = @dueDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date` = @dueDate ");
                            }
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post_required` = @prePostRequired ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post_required` = @prePostRequired ");
                            }
                            cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanLessonWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + teacherPlanLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.TriesQuantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanLessonWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ForGrading))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `for_grading` LIKE @forGradingWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `for_grading` LIKE @forGradingWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.DueDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date` LIKE @dueDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date` LIKE @dueDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanLessonWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PrePostRequired))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostRequiredWhereLike", "%" + teacherPlanLessonWhereLike.PrePostRequired + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonWhereLike.UpdatedAt + "%");
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
                                teacherPlanLesson = new TeacherPlanLessonModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLesson.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLesson.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLesson.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLesson.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLesson.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLesson.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLesson.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLesson.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLesson.PrePostRequired = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLesson.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLesson.UpdatedAt = null;
                                response.Add(teacherPlanLesson.Id, teacherPlanLesson);
                                teacherPlanLesson = new TeacherPlanLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanLessonModel>();
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
                response = new Dictionary<string, TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanLessonModel> ReadD(string connectionString, List<TeacherPlanLessonModel> teacherPlanLessonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonModel> response = new Dictionary<string, TeacherPlanLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                if (teacherPlanLessonWhere != null && teacherPlanLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLessonWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLessonWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanLessonWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PrePostRequired))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post_required` = @prePostRequired" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLessonWhere[i].PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonWhere.Count - 1)
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
                            TeacherPlanLessonModel teacherPlanLesson = new TeacherPlanLessonModel(); if (!reader.IsDBNull(0))
                                teacherPlanLesson.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLesson.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLesson.LessonId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanLesson.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLesson.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLesson.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLesson.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanLesson.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                            else
                                teacherPlanLesson.PrePostRequired = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLesson.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLesson.UpdatedAt = null;
                            response.Add(teacherPlanLesson.Id, teacherPlanLesson);
                            teacherPlanLesson = new TeacherPlanLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanLessonModel> ReadD(string connectionString, List<TeacherPlanLessonModel> teacherPlanLessonWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonModel> response = new Dictionary<string, TeacherPlanLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                if (teacherPlanLessonWhere != null && teacherPlanLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLessonWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLessonWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanLessonWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PrePostRequired))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post_required` = @prePostRequired" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLessonWhere[i].PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonWhere.Count - 1)
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
                            TeacherPlanLessonModel teacherPlanLesson = new TeacherPlanLessonModel(); if (!reader.IsDBNull(0))
                                teacherPlanLesson.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLesson.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLesson.LessonId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanLesson.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLesson.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLesson.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLesson.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanLesson.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                            else
                                teacherPlanLesson.PrePostRequired = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLesson.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLesson.UpdatedAt = null;
                            response.Add(teacherPlanLesson.Id, teacherPlanLesson);
                            teacherPlanLesson = new TeacherPlanLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonModel> Read(string connectionString, TeacherPlanLessonModel teacherPlanLesson, bool firstOne, bool orderByLastDate, TeacherPlanLessonModel teacherPlanLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                {
                    if (teacherPlanLesson != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `tries_quantity` = @triesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `tries_quantity` = @triesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `for_grading` = @forGrading ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `for_grading` = @forGrading ");
                            }
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date` = @dueDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date` = @dueDate ");
                            }
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post_required` = @prePostRequired ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post_required` = @prePostRequired ");
                            }
                            cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanLessonWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + teacherPlanLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.TriesQuantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanLessonWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ForGrading))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `for_grading` LIKE @forGradingWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `for_grading` LIKE @forGradingWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.DueDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date` LIKE @dueDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date` LIKE @dueDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanLessonWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PrePostRequired))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostRequiredWhereLike", "%" + teacherPlanLessonWhereLike.PrePostRequired + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonWhereLike.UpdatedAt + "%");
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
                                teacherPlanLesson = new TeacherPlanLessonModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLesson.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLesson.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLesson.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLesson.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLesson.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLesson.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLesson.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLesson.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLesson.PrePostRequired = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLesson.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLesson.UpdatedAt = null;
                                response.Add(teacherPlanLesson);
                                teacherPlanLesson = new TeacherPlanLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanLessonModel>();
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
                response = new List<TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonModel> Read(string connectionString, TeacherPlanLessonModel teacherPlanLesson, bool orderByLastDate, int fromRow, int limit, TeacherPlanLessonModel teacherPlanLessonWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                {
                    if (teacherPlanLesson != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `tries_quantity` = @triesQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `tries_quantity` = @triesQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `for_grading` = @forGrading ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `for_grading` = @forGrading ");
                            }
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date` = @dueDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date` = @dueDate ");
                            }
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `pre_post_required` = @prePostRequired ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `pre_post_required` = @prePostRequired ");
                            }
                            cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanLessonWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + teacherPlanLessonWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.TriesQuantity))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `tries_quantity` LIKE @triesQuantityWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanLessonWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ForGrading))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `for_grading` LIKE @forGradingWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `for_grading` LIKE @forGradingWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.DueDate))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date` LIKE @dueDateWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date` LIKE @dueDateWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanLessonWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.PrePostRequired))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `pre_post_required` LIKE @prePostRequiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@prePostRequiredWhereLike", "%" + teacherPlanLessonWhereLike.PrePostRequired + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonWhereLike.UpdatedAt + "%");
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
                                teacherPlanLesson = new TeacherPlanLessonModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLesson.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLesson.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLesson.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLesson.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLesson.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLesson.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLesson.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLesson.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLesson.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLesson.PrePostRequired = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLesson.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLesson.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLesson.UpdatedAt = null;
                                response.Add(teacherPlanLesson);
                                teacherPlanLesson = new TeacherPlanLessonModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanLessonModel>();
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
                response = new List<TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonModel> Read(string connectionString, List<TeacherPlanLessonModel> teacherPlanLessonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                if (teacherPlanLessonWhere != null && teacherPlanLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLessonWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLessonWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanLessonWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PrePostRequired))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post_required` = @prePostRequired" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLessonWhere[i].PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonWhere.Count - 1)
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
                            TeacherPlanLessonModel teacherPlanLesson = new TeacherPlanLessonModel(); if (!reader.IsDBNull(0))
                                teacherPlanLesson.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLesson.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLesson.LessonId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanLesson.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLesson.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLesson.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLesson.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanLesson.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                            else
                                teacherPlanLesson.PrePostRequired = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLesson.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLesson.UpdatedAt = null;
                            response.Add(teacherPlanLesson);
                            teacherPlanLesson = new TeacherPlanLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonModel> Read(string connectionString, List<TeacherPlanLessonModel> teacherPlanLessonWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `pre_post_required`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lessons ";
                if (teacherPlanLessonWhere != null && teacherPlanLessonWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLessonWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLessonWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLessonWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanLessonWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].PrePostRequired))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pre_post_required` = @prePostRequired" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLessonWhere[i].PrePostRequired);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonWhere.Count - 1)
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
                            TeacherPlanLessonModel teacherPlanLesson = new TeacherPlanLessonModel(); if (!reader.IsDBNull(0))
                                teacherPlanLesson.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLesson.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLesson.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLesson.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLesson.LessonId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLesson.LessonId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLesson.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanLesson.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLesson.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLesson.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLesson.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLesson.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLesson.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanLesson.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLesson.PrePostRequired = reader.GetValue(7).ToString();
                            else
                                teacherPlanLesson.PrePostRequired = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLesson.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLesson.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLesson.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLesson.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLesson.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLesson.UpdatedAt = null;
                            response.Add(teacherPlanLesson);
                            teacherPlanLesson = new TeacherPlanLessonModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanLessonModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonModel> Create(string connectionString, TeacherPlanLessonModel teacherPlanLesson)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_plan_lessons (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
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
                    cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
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
                    cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `tries_quantity` ");
                        values = String.Concat(values, ", @triesQuantity");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `tries_quantity` ");
                        values = String.Concat(values, " @triesQuantity");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `for_grading` ");
                        values = String.Concat(values, ", @forGrading");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `for_grading` ");
                        values = String.Concat(values, " @forGrading");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `due_date` ");
                        values = String.Concat(values, ", @dueDate");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `due_date` ");
                        values = String.Concat(values, " @dueDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
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
                    cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `pre_post_required` ");
                        values = String.Concat(values, ", @prePostRequired");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `pre_post_required` ");
                        values = String.Concat(values, " @prePostRequired");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherPlanLesson.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherPlanLesson);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
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
        public List<TeacherPlanLessonModel> Create(string connectionString, List<TeacherPlanLessonModel> teacherPlanLessonList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
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
                        for (int i = 0; i < teacherPlanLessonList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_plan_lessons (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].PlanId))
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
                                    cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLessonList[i].PlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].LessonId))
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
                                    cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLessonList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].TriesQuantity))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `tries_quantity` ");
                                        values = String.Concat(values, ", @triesQuantity" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `tries_quantity` ");
                                        values = String.Concat(values, "@triesQuantity" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLessonList[i].TriesQuantity);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].ForGrading))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `for_grading` ");
                                        values = String.Concat(values, ", @forGrading" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `for_grading` ");
                                        values = String.Concat(values, "@forGrading" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonList[i].ForGrading);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].DueDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `due_date` ");
                                        values = String.Concat(values, ", @dueDate" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `due_date` ");
                                        values = String.Concat(values, "@dueDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonList[i].DueDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, teacherPlanLessonList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].PrePostRequired))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `pre_post_required` ");
                                        values = String.Concat(values, ", @prePostRequired" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `pre_post_required` ");
                                        values = String.Concat(values, "@prePostRequired" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLessonList[i].PrePostRequired);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherPlanLessonList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanLessonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanLessonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
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

        public List<TeacherPlanLessonModel> Update(string connectionString, TeacherPlanLessonModel teacherPlanLesson, TeacherPlanLessonModel teacherPlanLessonWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_plan_lessons SET ";
                if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = String.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lesson_id` = @lessonId");
                    else
                        sql = String.Concat(sql, " `lesson_id` = @lessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `tries_quantity` = @triesQuantity");
                    else
                        sql = String.Concat(sql, " `tries_quantity` = @triesQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `for_grading` = @forGrading");
                    else
                        sql = String.Concat(sql, " `for_grading` = @forGrading");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `due_date` = @dueDate");
                    else
                        sql = String.Concat(sql, " `due_date` = @dueDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `pre_post_required` = @prePostRequired");
                    else
                        sql = String.Concat(sql, " `pre_post_required` = @prePostRequired");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                }
                if (teacherPlanLessonWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherPlanLessonWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.PlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", teacherPlanLessonWhere.PlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonIdWhere", teacherPlanLessonWhere.LessonId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.TriesQuantity))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantityWhere");
                        else
                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@triesQuantityWhere", teacherPlanLessonWhere.TriesQuantity);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.ForGrading))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `for_grading` = @forGradingWhere");
                        else
                            sql = String.Concat(sql, " `for_grading` = @forGradingWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forGradingWhere", teacherPlanLessonWhere.ForGrading);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.DueDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date` = @dueDateWhere");
                        else
                            sql = String.Concat(sql, " `due_date` = @dueDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDateWhere", teacherPlanLessonWhere.DueDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", teacherPlanLessonWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.PrePostRequired))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `pre_post_required` = @prePostRequiredWhere");
                        else
                            sql = String.Concat(sql, " `pre_post_required` = @prePostRequiredWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@prePostRequiredWhere", teacherPlanLessonWhere.PrePostRequired);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherPlanLessonWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherPlanLessonWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherPlanLessonWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherPlanLesson);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
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

        public List<TeacherPlanLessonModel> Update(string connectionString, List<TeacherPlanLessonModel> teacherPlanLessonList, List<TeacherPlanLessonModel> teacherPlanLessonWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
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
                        for (int i = 0; i < teacherPlanLessonList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_plan_lessons SET ";
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLessonList[i].PlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].LessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lesson_id` = @lessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLessonList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].TriesQuantity))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `tries_quantity` = @triesQuantity" + i);
                                    else
                                        sql = String.Concat(sql, " `tries_quantity` = @triesQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLessonList[i].TriesQuantity);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].ForGrading))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `for_grading` = @forGrading" + i);
                                    else
                                        sql = String.Concat(sql, " `for_grading` = @forGrading" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonList[i].ForGrading);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].DueDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `due_date` = @dueDate" + i);
                                    else
                                        sql = String.Concat(sql, " `due_date` = @dueDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonList[i].DueDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, teacherPlanLessonList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].PrePostRequired))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `pre_post_required` = @prePostRequired" + i);
                                    else
                                        sql = String.Concat(sql, " `pre_post_required` = @prePostRequired" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLessonList[i].PrePostRequired);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonList[i].UpdatedAt);
                                }
                                if (teacherPlanLessonWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherPlanLessonWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, teacherPlanLessonWhereList[i].PlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonIdWhere" + i, teacherPlanLessonWhereList[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].TriesQuantity))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantityWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@triesQuantityWhere" + i, teacherPlanLessonWhereList[i].TriesQuantity);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].ForGrading))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `for_grading` = @forGradingWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `for_grading` = @forGradingWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forGradingWhere" + i, teacherPlanLessonWhereList[i].ForGrading);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].DueDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date` = @dueDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date` = @dueDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDateWhere" + i, teacherPlanLessonWhereList[i].DueDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, teacherPlanLessonWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].PrePostRequired))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `pre_post_required` = @prePostRequiredWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `pre_post_required` = @prePostRequiredWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@prePostRequiredWhere" + i, teacherPlanLessonWhereList[i].PrePostRequired);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherPlanLessonWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherPlanLessonWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherPlanLessonWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanLessonList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanLessonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanLessonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
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

        public List<TeacherPlanLessonModel> Delete(string connectionString, TeacherPlanLessonModel teacherPlanLesson)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_plan_lessons  ";
                if (teacherPlanLesson != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherPlanLesson.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherPlanLesson.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.PlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = String.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", teacherPlanLesson.PlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonId", teacherPlanLesson.LessonId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.TriesQuantity))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantity");
                        else
                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanLesson.TriesQuantity);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.ForGrading))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `for_grading` = @forGrading");
                        else
                            sql = String.Concat(sql, " `for_grading` = @forGrading");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forGrading", teacherPlanLesson.ForGrading);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.DueDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date` = @dueDate");
                        else
                            sql = String.Concat(sql, " `due_date` = @dueDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDate", teacherPlanLesson.DueDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", teacherPlanLesson.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.PrePostRequired))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `pre_post_required` = @prePostRequired");
                        else
                            sql = String.Concat(sql, " `pre_post_required` = @prePostRequired");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@prePostRequired", teacherPlanLesson.PrePostRequired);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLesson.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherPlanLesson.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLesson.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLesson.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherPlanLesson);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
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

        public List<TeacherPlanLessonModel> Delete(string connectionString, List<TeacherPlanLessonModel> teacherPlanLesson)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonModel> response = new List<TeacherPlanLessonModel>();
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
                        for (int i = 0; i < teacherPlanLesson.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_plan_lessons  ";
                                if (teacherPlanLesson[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherPlanLesson[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, teacherPlanLesson[i].PlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonId" + i, teacherPlanLesson[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].TriesQuantity))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantity" + i);
                                        else
                                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanLesson[i].TriesQuantity);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].ForGrading))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `for_grading` = @forGrading" + i);
                                        else
                                            sql = String.Concat(sql, " `for_grading` = @forGrading" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLesson[i].ForGrading);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].DueDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date` = @dueDate" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date` = @dueDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLesson[i].DueDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, teacherPlanLesson[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].PrePostRequired))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `pre_post_required` = @prePostRequired" + i);
                                        else
                                            sql = String.Concat(sql, " `pre_post_required` = @prePostRequired" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@prePostRequired" + i, teacherPlanLesson[i].PrePostRequired);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLesson[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLesson[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLesson[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLesson[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanLesson[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanLessonModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanLessonModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonModel>();
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
