using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherPlanLessonQuizDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherPlanLessonQuizModel> ReadD(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz, bool firstOne, bool orderByLastDate, TeacherPlanLessonQuizModel teacherPlanLessonQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonQuizModel> response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                {
                    if (teacherPlanLessonQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `required` = @required ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `required` = @required ");
                            }
                            cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` = @dueDateTimezone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` = @dueDateTimezone ");
                            }
                            cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Required))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `required` LIKE @requiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `required` LIKE @requiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@requiredWhereLike", "%" + teacherPlanLessonQuizWhereLike.Required + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDateTimezone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezoneWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDateTimezone + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Duration))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `duration` LIKE @durationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `duration` LIKE @durationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@durationWhereLike", "%" + teacherPlanLessonQuizWhereLike.Duration + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLessonQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLessonQuiz.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLessonQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLessonQuiz.Required = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLessonQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDateTimezone = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLessonQuiz.Duration = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLessonQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLessonQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLessonQuiz.UpdatedAt = null;
                                response.Add(teacherPlanLessonQuiz.Id, teacherPlanLessonQuiz);
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanLessonQuizModel>();
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
                response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanLessonQuizModel> ReadD(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz, bool orderByLastDate, int fromRow, int limit, TeacherPlanLessonQuizModel teacherPlanLessonQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonQuizModel> response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                {
                    if (teacherPlanLessonQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `required` = @required ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `required` = @required ");
                            }
                            cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` = @dueDateTimezone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` = @dueDateTimezone ");
                            }
                            cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Required))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `required` LIKE @requiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `required` LIKE @requiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@requiredWhereLike", "%" + teacherPlanLessonQuizWhereLike.Required + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDateTimezone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezoneWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDateTimezone + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Duration))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `duration` LIKE @durationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `duration` LIKE @durationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@durationWhereLike", "%" + teacherPlanLessonQuizWhereLike.Duration + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLessonQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLessonQuiz.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLessonQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLessonQuiz.Required = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLessonQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDateTimezone = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLessonQuiz.Duration = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLessonQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLessonQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLessonQuiz.UpdatedAt = null;
                                response.Add(teacherPlanLessonQuiz.Id, teacherPlanLessonQuiz);
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanLessonQuizModel>();
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
                response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanLessonQuizModel> ReadD(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonQuizModel> response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                if (teacherPlanLessonQuizWhere != null && teacherPlanLessonQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuizWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Required))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `required` = @required" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuizWhere[i].Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDateTimezone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date_timezone` = @dueDateTimezone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuizWhere[i].DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuizWhere[i].Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonQuizWhere.Count - 1)
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
                            TeacherPlanLessonQuizModel teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLessonQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLessonQuiz.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLessonQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                            else
                                teacherPlanLessonQuiz.Required = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLessonQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLessonQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                            else
                                teacherPlanLessonQuiz.DueDateTimezone = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                            else
                                teacherPlanLessonQuiz.Duration = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLessonQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLessonQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLessonQuiz.UpdatedAt = null;
                            response.Add(teacherPlanLessonQuiz.Id, teacherPlanLessonQuiz);
                            teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanLessonQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanLessonQuizModel> ReadD(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanLessonQuizModel> response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                if (teacherPlanLessonQuizWhere != null && teacherPlanLessonQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuizWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Required))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `required` = @required" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuizWhere[i].Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDateTimezone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date_timezone` = @dueDateTimezone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuizWhere[i].DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuizWhere[i].Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonQuizWhere.Count - 1)
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
                            TeacherPlanLessonQuizModel teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLessonQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLessonQuiz.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLessonQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                            else
                                teacherPlanLessonQuiz.Required = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLessonQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLessonQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                            else
                                teacherPlanLessonQuiz.DueDateTimezone = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                            else
                                teacherPlanLessonQuiz.Duration = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLessonQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLessonQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLessonQuiz.UpdatedAt = null;
                            response.Add(teacherPlanLessonQuiz.Id, teacherPlanLessonQuiz);
                            teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanLessonQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonQuizModel> Read(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz, bool firstOne, bool orderByLastDate, TeacherPlanLessonQuizModel teacherPlanLessonQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                {
                    if (teacherPlanLessonQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `required` = @required ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `required` = @required ");
                            }
                            cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` = @dueDateTimezone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` = @dueDateTimezone ");
                            }
                            cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Required))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `required` LIKE @requiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `required` LIKE @requiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@requiredWhereLike", "%" + teacherPlanLessonQuizWhereLike.Required + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDateTimezone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezoneWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDateTimezone + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Duration))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `duration` LIKE @durationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `duration` LIKE @durationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@durationWhereLike", "%" + teacherPlanLessonQuizWhereLike.Duration + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLessonQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLessonQuiz.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLessonQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLessonQuiz.Required = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLessonQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDateTimezone = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLessonQuiz.Duration = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLessonQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLessonQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLessonQuiz.UpdatedAt = null;
                                response.Add(teacherPlanLessonQuiz);
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanLessonQuizModel>();
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
                response = new List<TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonQuizModel> Read(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz, bool orderByLastDate, int fromRow, int limit, TeacherPlanLessonQuizModel teacherPlanLessonQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                {
                    if (teacherPlanLessonQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `required` = @required ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `required` = @required ");
                            }
                            cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` = @dueDateTimezone ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` = @dueDateTimezone ");
                            }
                            cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `duration` = @duration ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `duration` = @duration ");
                            }
                            cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanLessonQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanLessonQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.PlanLessonId))
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
                            cmd.Parameters.AddWithValue("@planLessonIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.PlanLessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanLessonQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Required))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `required` LIKE @requiredWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `required` LIKE @requiredWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@requiredWhereLike", "%" + teacherPlanLessonQuizWhereLike.Required + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanLessonQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.DueDateTimezone))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `due_date_timezone` LIKE @dueDateTimezoneWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezoneWhereLike", "%" + teacherPlanLessonQuizWhereLike.DueDateTimezone + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.Duration))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `duration` LIKE @durationWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `duration` LIKE @durationWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@durationWhereLike", "%" + teacherPlanLessonQuizWhereLike.Duration + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanLessonQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanLessonQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanLessonQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanLessonQuiz.PlanLessonId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanLessonQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                                else
                                    teacherPlanLessonQuiz.Required = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanLessonQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                                else
                                    teacherPlanLessonQuiz.DueDateTimezone = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                                else
                                    teacherPlanLessonQuiz.Duration = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    teacherPlanLessonQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanLessonQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    teacherPlanLessonQuiz.UpdatedAt = null;
                                response.Add(teacherPlanLessonQuiz);
                                teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanLessonQuizModel>();
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
                response = new List<TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonQuizModel> Read(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                if (teacherPlanLessonQuizWhere != null && teacherPlanLessonQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuizWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Required))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `required` = @required" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuizWhere[i].Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDateTimezone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date_timezone` = @dueDateTimezone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuizWhere[i].DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuizWhere[i].Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonQuizWhere.Count - 1)
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
                            TeacherPlanLessonQuizModel teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLessonQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLessonQuiz.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLessonQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                            else
                                teacherPlanLessonQuiz.Required = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLessonQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLessonQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                            else
                                teacherPlanLessonQuiz.DueDateTimezone = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                            else
                                teacherPlanLessonQuiz.Duration = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLessonQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLessonQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLessonQuiz.UpdatedAt = null;
                            response.Add(teacherPlanLessonQuiz);
                            teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanLessonQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonQuizModel> Read(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_lesson_id`, `quiz_id`, `required`, `for_grading`, `due_date`, `due_date_timezone`, `duration`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_lesson_quizs ";
                if (teacherPlanLessonQuizWhere != null && teacherPlanLessonQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanLessonQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].PlanLessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_lesson_id` = @planLessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuizWhere[i].PlanLessonId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Required))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `required` = @required" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuizWhere[i].Required);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].DueDateTimezone))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date_timezone` = @dueDateTimezone" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuizWhere[i].DueDateTimezone);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].Duration))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `duration` = @duration" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuizWhere[i].Duration);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanLessonQuizWhere.Count - 1)
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
                            TeacherPlanLessonQuizModel teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanLessonQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanLessonQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanLessonQuiz.PlanLessonId = reader.GetValue(1).ToString();
                            else
                                teacherPlanLessonQuiz.PlanLessonId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanLessonQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanLessonQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanLessonQuiz.Required = reader.GetValue(3).ToString();
                            else
                                teacherPlanLessonQuiz.Required = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanLessonQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanLessonQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanLessonQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanLessonQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanLessonQuiz.DueDateTimezone = reader.GetValue(6).ToString();
                            else
                                teacherPlanLessonQuiz.DueDateTimezone = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanLessonQuiz.Duration = reader.GetValue(7).ToString();
                            else
                                teacherPlanLessonQuiz.Duration = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanLessonQuiz.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                teacherPlanLessonQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanLessonQuiz.CreatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanLessonQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                teacherPlanLessonQuiz.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                teacherPlanLessonQuiz.UpdatedAt = null;
                            response.Add(teacherPlanLessonQuiz);
                            teacherPlanLessonQuiz = new TeacherPlanLessonQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanLessonQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanLessonQuizModel> Create(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_plan_lesson_quizs (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
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
                    cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `quiz_id` ");
                        values = String.Concat(values, ", @quizId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `quiz_id` ");
                        values = String.Concat(values, " @quizId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `required` ");
                        values = String.Concat(values, ", @required");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `required` ");
                        values = String.Concat(values, " @required");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
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
                    cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
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
                    cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `due_date_timezone` ");
                        values = String.Concat(values, ", @dueDateTimezone");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `due_date_timezone` ");
                        values = String.Concat(values, " @dueDateTimezone");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `duration` ");
                        values = String.Concat(values, ", @duration");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `duration` ");
                        values = String.Concat(values, " @duration");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherPlanLessonQuiz.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherPlanLessonQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
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
        public List<TeacherPlanLessonQuizModel> Create(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
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
                        for (int i = 0; i < teacherPlanLessonQuizList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_plan_lesson_quizs (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuizList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].PlanLessonId))
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
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuizList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].QuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `quiz_id` ");
                                        values = String.Concat(values, ", @quizId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `quiz_id` ");
                                        values = String.Concat(values, "@quizId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuizList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].Required))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `required` ");
                                        values = String.Concat(values, ", @required" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `required` ");
                                        values = String.Concat(values, "@required" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuizList[i].Required);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].ForGrading))
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
                                    cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuizList[i].ForGrading);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].DueDate))
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
                                    cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuizList[i].DueDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].DueDateTimezone))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `due_date_timezone` ");
                                        values = String.Concat(values, ", @dueDateTimezone" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `due_date_timezone` ");
                                        values = String.Concat(values, "@dueDateTimezone" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuizList[i].DueDateTimezone);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].Duration))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `duration` ");
                                        values = String.Concat(values, ", @duration" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `duration` ");
                                        values = String.Concat(values, "@duration" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuizList[i].Duration);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuizList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuizList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuizList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherPlanLessonQuizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanLessonQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanLessonQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
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

        public List<TeacherPlanLessonQuizModel> Update(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz, TeacherPlanLessonQuizModel teacherPlanLessonQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_plan_lesson_quizs SET ";
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId");
                    else
                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `required` = @required");
                    else
                        sql = String.Concat(sql, " `required` = @required");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `for_grading` = @forGrading");
                    else
                        sql = String.Concat(sql, " `for_grading` = @forGrading");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `due_date` = @dueDate");
                    else
                        sql = String.Concat(sql, " `due_date` = @dueDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `due_date_timezone` = @dueDateTimezone");
                    else
                        sql = String.Concat(sql, " `due_date_timezone` = @dueDateTimezone");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `duration` = @duration");
                    else
                        sql = String.Concat(sql, " `duration` = @duration");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                }
                if (teacherPlanLessonQuizWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherPlanLessonQuizWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonIdWhere", teacherPlanLessonQuizWhere.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", teacherPlanLessonQuizWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.Required))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `required` = @requiredWhere");
                        else
                            sql = String.Concat(sql, " `required` = @requiredWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@requiredWhere", teacherPlanLessonQuizWhere.Required);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.ForGrading))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `for_grading` = @forGradingWhere");
                        else
                            sql = String.Concat(sql, " `for_grading` = @forGradingWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forGradingWhere", teacherPlanLessonQuizWhere.ForGrading);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.DueDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date` = @dueDateWhere");
                        else
                            sql = String.Concat(sql, " `due_date` = @dueDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDateWhere", teacherPlanLessonQuizWhere.DueDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.DueDateTimezone))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date_timezone` = @dueDateTimezoneWhere");
                        else
                            sql = String.Concat(sql, " `due_date_timezone` = @dueDateTimezoneWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDateTimezoneWhere", teacherPlanLessonQuizWhere.DueDateTimezone);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.Duration))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `duration` = @durationWhere");
                        else
                            sql = String.Concat(sql, " `duration` = @durationWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@durationWhere", teacherPlanLessonQuizWhere.Duration);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherPlanLessonQuizWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherPlanLessonQuizWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherPlanLessonQuizWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherPlanLessonQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
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

        public List<TeacherPlanLessonQuizModel> Update(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizList, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuizWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
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
                        for (int i = 0; i < teacherPlanLessonQuizList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_plan_lesson_quizs SET ";
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuizList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].PlanLessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_lesson_id` = @planLessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuizList[i].PlanLessonId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuizList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].Required))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `required` = @required" + i);
                                    else
                                        sql = String.Concat(sql, " `required` = @required" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuizList[i].Required);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].ForGrading))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `for_grading` = @forGrading" + i);
                                    else
                                        sql = String.Concat(sql, " `for_grading` = @forGrading" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuizList[i].ForGrading);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].DueDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `due_date` = @dueDate" + i);
                                    else
                                        sql = String.Concat(sql, " `due_date` = @dueDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuizList[i].DueDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].DueDateTimezone))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `due_date_timezone` = @dueDateTimezone" + i);
                                    else
                                        sql = String.Concat(sql, " `due_date_timezone` = @dueDateTimezone" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuizList[i].DueDateTimezone);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].Duration))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `duration` = @duration" + i);
                                    else
                                        sql = String.Concat(sql, " `duration` = @duration" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuizList[i].Duration);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuizList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuizList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanLessonQuizList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuizList[i].UpdatedAt);
                                }
                                if (teacherPlanLessonQuizWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherPlanLessonQuizWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonIdWhere" + i, teacherPlanLessonQuizWhereList[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, teacherPlanLessonQuizWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].Required))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `required` = @requiredWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `required` = @requiredWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@requiredWhere" + i, teacherPlanLessonQuizWhereList[i].Required);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].ForGrading))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `for_grading` = @forGradingWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `for_grading` = @forGradingWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forGradingWhere" + i, teacherPlanLessonQuizWhereList[i].ForGrading);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].DueDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date` = @dueDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date` = @dueDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDateWhere" + i, teacherPlanLessonQuizWhereList[i].DueDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].DueDateTimezone))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date_timezone` = @dueDateTimezoneWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date_timezone` = @dueDateTimezoneWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDateTimezoneWhere" + i, teacherPlanLessonQuizWhereList[i].DueDateTimezone);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].Duration))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `duration` = @durationWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `duration` = @durationWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@durationWhere" + i, teacherPlanLessonQuizWhereList[i].Duration);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherPlanLessonQuizWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherPlanLessonQuizWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuizWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherPlanLessonQuizWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanLessonQuizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanLessonQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanLessonQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
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

        public List<TeacherPlanLessonQuizModel> Delete(string connectionString, TeacherPlanLessonQuizModel teacherPlanLessonQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_plan_lesson_quizs  ";
                if (teacherPlanLessonQuiz != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherPlanLessonQuiz.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.PlanLessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId");
                        else
                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planLessonId", teacherPlanLessonQuiz.PlanLessonId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", teacherPlanLessonQuiz.QuizId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Required))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `required` = @required");
                        else
                            sql = String.Concat(sql, " `required` = @required");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@required", teacherPlanLessonQuiz.Required);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ForGrading))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `for_grading` = @forGrading");
                        else
                            sql = String.Concat(sql, " `for_grading` = @forGrading");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forGrading", teacherPlanLessonQuiz.ForGrading);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date` = @dueDate");
                        else
                            sql = String.Concat(sql, " `due_date` = @dueDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDate", teacherPlanLessonQuiz.DueDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.DueDateTimezone))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date_timezone` = @dueDateTimezone");
                        else
                            sql = String.Concat(sql, " `due_date_timezone` = @dueDateTimezone");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDateTimezone", teacherPlanLessonQuiz.DueDateTimezone);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.Duration))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `duration` = @duration");
                        else
                            sql = String.Concat(sql, " `duration` = @duration");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@duration", teacherPlanLessonQuiz.Duration);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanLessonQuiz.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherPlanLessonQuiz.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherPlanLessonQuiz.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherPlanLessonQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
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

        public List<TeacherPlanLessonQuizModel> Delete(string connectionString, List<TeacherPlanLessonQuizModel> teacherPlanLessonQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonQuizModel> response = new List<TeacherPlanLessonQuizModel>();
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
                        for (int i = 0; i < teacherPlanLessonQuiz.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_plan_lesson_quizs  ";
                                if (teacherPlanLessonQuiz[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherPlanLessonQuiz[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].PlanLessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_lesson_id` = @planLessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_lesson_id` = @planLessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planLessonId" + i, teacherPlanLessonQuiz[i].PlanLessonId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanLessonQuiz[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].Required))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `required` = @required" + i);
                                        else
                                            sql = String.Concat(sql, " `required` = @required" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@required" + i, teacherPlanLessonQuiz[i].Required);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].ForGrading))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `for_grading` = @forGrading" + i);
                                        else
                                            sql = String.Concat(sql, " `for_grading` = @forGrading" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanLessonQuiz[i].ForGrading);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].DueDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date` = @dueDate" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date` = @dueDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanLessonQuiz[i].DueDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].DueDateTimezone))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date_timezone` = @dueDateTimezone" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date_timezone` = @dueDateTimezone" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDateTimezone" + i, teacherPlanLessonQuiz[i].DueDateTimezone);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].Duration))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `duration` = @duration" + i);
                                        else
                                            sql = String.Concat(sql, " `duration` = @duration" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@duration" + i, teacherPlanLessonQuiz[i].Duration);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanLessonQuiz[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanLessonQuiz[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanLessonQuiz[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanLessonQuiz[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanLessonQuiz[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanLessonQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanLessonQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanLessonQuizModel>();
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
