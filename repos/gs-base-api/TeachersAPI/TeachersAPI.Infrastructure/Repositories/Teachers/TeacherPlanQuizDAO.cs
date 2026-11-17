using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    class TeacherPlanQuizDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, TeacherPlanQuizModel> ReadD(string connectionString, TeacherPlanQuizModel teacherPlanQuiz, bool firstOne, bool orderByLastDate, TeacherPlanQuizModel teacherPlanQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanQuizModel> response = new Dictionary<string, TeacherPlanQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                {
                    if (teacherPlanQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanQuizWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanQuizWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanQuizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanQuiz.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanQuiz.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanQuiz.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherPlanQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherPlanQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanQuiz.UpdatedAt = null;
                                response.Add(teacherPlanQuiz.Id, teacherPlanQuiz);
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanQuizModel>();
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
                response = new Dictionary<string, TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanQuizModel> ReadD(string connectionString, TeacherPlanQuizModel teacherPlanQuiz, bool orderByLastDate, int fromRow, int limit, TeacherPlanQuizModel teacherPlanQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanQuizModel> response = new Dictionary<string, TeacherPlanQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                {
                    if (teacherPlanQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanQuizWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanQuizWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanQuizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanQuiz.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanQuiz.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanQuiz.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherPlanQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherPlanQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanQuiz.UpdatedAt = null;
                                response.Add(teacherPlanQuiz.Id, teacherPlanQuiz);
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, TeacherPlanQuizModel>();
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
                response = new Dictionary<string, TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanQuizModel> ReadD(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanQuizModel> response = new Dictionary<string, TeacherPlanQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                if (teacherPlanQuizWhere != null && teacherPlanQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuizWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuizWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanQuizWhere.Count - 1)
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
                            TeacherPlanQuizModel teacherPlanQuiz = new TeacherPlanQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanQuiz.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanQuiz.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanQuiz.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherPlanQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherPlanQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanQuiz.UpdatedAt = null;
                            response.Add(teacherPlanQuiz.Id, teacherPlanQuiz);
                            teacherPlanQuiz = new TeacherPlanQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, TeacherPlanQuizModel> ReadD(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuizWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, TeacherPlanQuizModel> response = new Dictionary<string, TeacherPlanQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                if (teacherPlanQuizWhere != null && teacherPlanQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuizWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuizWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanQuizWhere.Count - 1)
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
                            TeacherPlanQuizModel teacherPlanQuiz = new TeacherPlanQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanQuiz.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanQuiz.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanQuiz.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherPlanQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherPlanQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanQuiz.UpdatedAt = null;
                            response.Add(teacherPlanQuiz.Id, teacherPlanQuiz);
                            teacherPlanQuiz = new TeacherPlanQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, TeacherPlanQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanQuizModel> Read(string connectionString, TeacherPlanQuizModel teacherPlanQuiz, bool firstOne, bool orderByLastDate, TeacherPlanQuizModel teacherPlanQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                {
                    if (teacherPlanQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanQuizWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanQuizWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanQuizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanQuiz.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanQuiz.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanQuiz.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherPlanQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherPlanQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanQuiz.UpdatedAt = null;
                                response.Add(teacherPlanQuiz);
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanQuizModel>();
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
                response = new List<TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanQuizModel> Read(string connectionString, TeacherPlanQuizModel teacherPlanQuiz, bool orderByLastDate, int fromRow, int limit, TeacherPlanQuizModel teacherPlanQuizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                {
                    if (teacherPlanQuiz != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
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
                            cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                        }
                    }
                    if (teacherPlanQuizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + teacherPlanQuizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.PlanId))
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
                            cmd.Parameters.AddWithValue("@planIdWhereLike", "%" + teacherPlanQuizWhereLike.PlanId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + teacherPlanQuizWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.TriesQuantity))
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
                            cmd.Parameters.AddWithValue("@triesQuantityWhereLike", "%" + teacherPlanQuizWhereLike.TriesQuantity + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ForGrading))
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
                            cmd.Parameters.AddWithValue("@forGradingWhereLike", "%" + teacherPlanQuizWhereLike.ForGrading + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.DueDate))
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
                            cmd.Parameters.AddWithValue("@dueDateWhereLike", "%" + teacherPlanQuizWhereLike.DueDate + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + teacherPlanQuizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + teacherPlanQuizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + teacherPlanQuizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + teacherPlanQuizWhereLike.UpdatedAt + "%");
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
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                                if (!reader.IsDBNull(0))
                                    teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                                else
                                    teacherPlanQuiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                                else
                                    teacherPlanQuiz.PlanId = null;
                                if (!reader.IsDBNull(2))
                                    teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                                else
                                    teacherPlanQuiz.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                                else
                                    teacherPlanQuiz.TriesQuantity = null;
                                if (!reader.IsDBNull(4))
                                    teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                                else
                                    teacherPlanQuiz.ForGrading = null;
                                if (!reader.IsDBNull(5))
                                    teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                                else
                                    teacherPlanQuiz.DueDate = null;
                                if (!reader.IsDBNull(6))
                                    teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                                else
                                    teacherPlanQuiz.Available = null;
                                if (!reader.IsDBNull(7))
                                    teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                                else
                                    teacherPlanQuiz.ModifiedBy = null;
                                if (!reader.IsDBNull(8))
                                    teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                                else
                                    teacherPlanQuiz.CreatedAt = null;
                                if (!reader.IsDBNull(9))
                                    teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                                else
                                    teacherPlanQuiz.UpdatedAt = null;
                                response.Add(teacherPlanQuiz);
                                teacherPlanQuiz = new TeacherPlanQuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<TeacherPlanQuizModel>();
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
                response = new List<TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanQuizModel> Read(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                if (teacherPlanQuizWhere != null && teacherPlanQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuizWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuizWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanQuizWhere.Count - 1)
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
                            TeacherPlanQuizModel teacherPlanQuiz = new TeacherPlanQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanQuiz.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanQuiz.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanQuiz.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherPlanQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherPlanQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanQuiz.UpdatedAt = null;
                            response.Add(teacherPlanQuiz);
                            teacherPlanQuiz = new TeacherPlanQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanQuizModel> Read(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuizWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `plan_id`, `quiz_id`, `tries_quantity`, `for_grading`, `due_date`, `available`, `modified_by`, `created_at`, `updated_at`  FROM teacher_plan_quizs ";
                if (teacherPlanQuizWhere != null && teacherPlanQuizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < teacherPlanQuizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].PlanId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `plan_id` = @planId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuizWhere[i].PlanId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuizWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].TriesQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `tries_quantity` = @triesQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuizWhere[i].TriesQuantity);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ForGrading))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `for_grading` = @forGrading" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuizWhere[i].ForGrading);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].DueDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `due_date` = @dueDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuizWhere[i].DueDate);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(teacherPlanQuizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != teacherPlanQuizWhere.Count - 1)
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
                            TeacherPlanQuizModel teacherPlanQuiz = new TeacherPlanQuizModel(); if (!reader.IsDBNull(0))
                                teacherPlanQuiz.Id = reader.GetValue(0).ToString();
                            else
                                teacherPlanQuiz.Id = null;
                            if (!reader.IsDBNull(1))
                                teacherPlanQuiz.PlanId = reader.GetValue(1).ToString();
                            else
                                teacherPlanQuiz.PlanId = null;
                            if (!reader.IsDBNull(2))
                                teacherPlanQuiz.QuizId = reader.GetValue(2).ToString();
                            else
                                teacherPlanQuiz.QuizId = null;
                            if (!reader.IsDBNull(3))
                                teacherPlanQuiz.TriesQuantity = reader.GetValue(3).ToString();
                            else
                                teacherPlanQuiz.TriesQuantity = null;
                            if (!reader.IsDBNull(4))
                                teacherPlanQuiz.ForGrading = reader.GetValue(4).ToString();
                            else
                                teacherPlanQuiz.ForGrading = null;
                            if (!reader.IsDBNull(5))
                                teacherPlanQuiz.DueDate = reader.GetValue(5).ToString();
                            else
                                teacherPlanQuiz.DueDate = null;
                            if (!reader.IsDBNull(6))
                                teacherPlanQuiz.Available = reader.GetValue(6).ToString();
                            else
                                teacherPlanQuiz.Available = null;
                            if (!reader.IsDBNull(7))
                                teacherPlanQuiz.ModifiedBy = reader.GetValue(7).ToString();
                            else
                                teacherPlanQuiz.ModifiedBy = null;
                            if (!reader.IsDBNull(8))
                                teacherPlanQuiz.CreatedAt = reader.GetValue(8).ToString();
                            else
                                teacherPlanQuiz.CreatedAt = null;
                            if (!reader.IsDBNull(9))
                                teacherPlanQuiz.UpdatedAt = reader.GetValue(9).ToString();
                            else
                                teacherPlanQuiz.UpdatedAt = null;
                            response.Add(teacherPlanQuiz);
                            teacherPlanQuiz = new TeacherPlanQuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<TeacherPlanQuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<TeacherPlanQuizModel> Create(string connectionString, TeacherPlanQuizModel teacherPlanQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO teacher_plan_quizs (";
                string values = "(";
                if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
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
                    cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
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
                    cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
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
                    cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
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
                    cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
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
                    cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
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
                    cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                teacherPlanQuiz.Id = cmd.LastInsertedId.ToString();
                response.Add(teacherPlanQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
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
        public List<TeacherPlanQuizModel> Create(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuizList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
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
                        for (int i = 0; i < teacherPlanQuizList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_plan_quizs (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuizList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].PlanId))
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
                                    cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuizList[i].PlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuizList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].TriesQuantity))
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
                                    cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuizList[i].TriesQuantity);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].ForGrading))
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
                                    cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuizList[i].ForGrading);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].DueDate))
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
                                    cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuizList[i].DueDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuizList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuizList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuizList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuizList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherPlanQuizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
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

        public List<TeacherPlanQuizModel> Update(string connectionString, TeacherPlanQuizModel teacherPlanQuiz, TeacherPlanQuizModel teacherPlanQuizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE teacher_plan_quizs SET ";
                if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `plan_id` = @planId");
                    else
                        sql = String.Concat(sql, " `plan_id` = @planId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `tries_quantity` = @triesQuantity");
                    else
                        sql = String.Concat(sql, " `tries_quantity` = @triesQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `for_grading` = @forGrading");
                    else
                        sql = String.Concat(sql, " `for_grading` = @forGrading");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `due_date` = @dueDate");
                    else
                        sql = String.Concat(sql, " `due_date` = @dueDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                }
                if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                }
                if (teacherPlanQuizWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", teacherPlanQuizWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.PlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_id` = @planIdWhere");
                        else
                            sql = String.Concat(sql, " `plan_id` = @planIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planIdWhere", teacherPlanQuizWhere.PlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", teacherPlanQuizWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.TriesQuantity))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantityWhere");
                        else
                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@triesQuantityWhere", teacherPlanQuizWhere.TriesQuantity);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.ForGrading))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `for_grading` = @forGradingWhere");
                        else
                            sql = String.Concat(sql, " `for_grading` = @forGradingWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forGradingWhere", teacherPlanQuizWhere.ForGrading);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.DueDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date` = @dueDateWhere");
                        else
                            sql = String.Concat(sql, " `due_date` = @dueDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDateWhere", teacherPlanQuizWhere.DueDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", teacherPlanQuizWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", teacherPlanQuizWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", teacherPlanQuizWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuizWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", teacherPlanQuizWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(teacherPlanQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
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

        public List<TeacherPlanQuizModel> Update(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuizList, List<TeacherPlanQuizModel> teacherPlanQuizWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
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
                        for (int i = 0; i < teacherPlanQuizList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE teacher_plan_quizs SET ";
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuizList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].PlanId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `plan_id` = @planId" + i);
                                    else
                                        sql = String.Concat(sql, " `plan_id` = @planId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuizList[i].PlanId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuizList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].TriesQuantity))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `tries_quantity` = @triesQuantity" + i);
                                    else
                                        sql = String.Concat(sql, " `tries_quantity` = @triesQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuizList[i].TriesQuantity);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].ForGrading))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `for_grading` = @forGrading" + i);
                                    else
                                        sql = String.Concat(sql, " `for_grading` = @forGrading" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuizList[i].ForGrading);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].DueDate))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `due_date` = @dueDate" + i);
                                    else
                                        sql = String.Concat(sql, " `due_date` = @dueDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuizList[i].DueDate);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuizList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuizList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuizList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(teacherPlanQuizList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuizList[i].UpdatedAt);
                                }
                                if (teacherPlanQuizWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, teacherPlanQuizWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_id` = @planIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_id` = @planIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planIdWhere" + i, teacherPlanQuizWhereList[i].PlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, teacherPlanQuizWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].TriesQuantity))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantityWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@triesQuantityWhere" + i, teacherPlanQuizWhereList[i].TriesQuantity);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].ForGrading))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `for_grading` = @forGradingWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `for_grading` = @forGradingWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forGradingWhere" + i, teacherPlanQuizWhereList[i].ForGrading);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].DueDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date` = @dueDateWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date` = @dueDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDateWhere" + i, teacherPlanQuizWhereList[i].DueDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, teacherPlanQuizWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, teacherPlanQuizWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, teacherPlanQuizWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuizWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, teacherPlanQuizWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanQuizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
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

        public List<TeacherPlanQuizModel> Delete(string connectionString, TeacherPlanQuizModel teacherPlanQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM teacher_plan_quizs  ";
                if (teacherPlanQuiz != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", teacherPlanQuiz.Id);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.PlanId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `plan_id` = @planId");
                        else
                            sql = String.Concat(sql, " `plan_id` = @planId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@planId", teacherPlanQuiz.PlanId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", teacherPlanQuiz.QuizId);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.TriesQuantity))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantity");
                        else
                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@triesQuantity", teacherPlanQuiz.TriesQuantity);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.ForGrading))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `for_grading` = @forGrading");
                        else
                            sql = String.Concat(sql, " `for_grading` = @forGrading");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@forGrading", teacherPlanQuiz.ForGrading);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.DueDate))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `due_date` = @dueDate");
                        else
                            sql = String.Concat(sql, " `due_date` = @dueDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@dueDate", teacherPlanQuiz.DueDate);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", teacherPlanQuiz.Available);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", teacherPlanQuiz.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", teacherPlanQuiz.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(teacherPlanQuiz.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", teacherPlanQuiz.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(teacherPlanQuiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
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

        public List<TeacherPlanQuizModel> Delete(string connectionString, List<TeacherPlanQuizModel> teacherPlanQuiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanQuizModel> response = new List<TeacherPlanQuizModel>();
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
                        for (int i = 0; i < teacherPlanQuiz.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM teacher_plan_quizs  ";
                                if (teacherPlanQuiz[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, teacherPlanQuiz[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].PlanId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `plan_id` = @planId" + i);
                                        else
                                            sql = String.Concat(sql, " `plan_id` = @planId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@planId" + i, teacherPlanQuiz[i].PlanId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, teacherPlanQuiz[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].TriesQuantity))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `tries_quantity` = @triesQuantity" + i);
                                        else
                                            sql = String.Concat(sql, " `tries_quantity` = @triesQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@triesQuantity" + i, teacherPlanQuiz[i].TriesQuantity);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].ForGrading))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `for_grading` = @forGrading" + i);
                                        else
                                            sql = String.Concat(sql, " `for_grading` = @forGrading" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@forGrading" + i, teacherPlanQuiz[i].ForGrading);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].DueDate))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `due_date` = @dueDate" + i);
                                        else
                                            sql = String.Concat(sql, " `due_date` = @dueDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@dueDate" + i, teacherPlanQuiz[i].DueDate);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, teacherPlanQuiz[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, teacherPlanQuiz[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, teacherPlanQuiz[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(teacherPlanQuiz[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, teacherPlanQuiz[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(teacherPlanQuiz[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<TeacherPlanQuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<TeacherPlanQuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<TeacherPlanQuizModel>();
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
