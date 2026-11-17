using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizModel> ReadD(string connectionString, QuizModel quiz, bool firstOne, bool orderByLastDate, QuizModel quizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizModel> response = new Dictionary<string, QuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                {
                    if (quiz != null)
                    {
                        if (!String.IsNullOrEmpty(quiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", quiz.Id);
                        }
                        if (!String.IsNullOrEmpty(quiz.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                        }
                        if (!String.IsNullOrEmpty(quiz.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", quiz.Title);
                        }
                        if (!String.IsNullOrEmpty(quiz.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }
                            cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                        }
                        if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` = @levelOfKnowledge ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` = @levelOfKnowledge ");
                            }
                            cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quiz.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                        }
                        if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quiz.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }
                            cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` = @readyToPublish ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` = @readyToPublish ");
                            }
                            cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", quiz.Available);
                        }
                        if (!String.IsNullOrEmpty(quiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                        }
                    }
                    if (quizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + quizWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + quizWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike", "%" + quizWhereLike.Instruction + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LevelOfKnowledge))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledgeWhereLike", "%" + quizWhereLike.LevelOfKnowledge + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + quizWhereLike.IsExam + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.QuizTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizTypeIdWhereLike", "%" + quizWhereLike.QuizTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike", "%" + quizWhereLike.CustomValue + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ReadyToPublish))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@readyToPublishWhereLike", "%" + quizWhereLike.ReadyToPublish + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + quizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizWhereLike.UpdatedAt + "%");
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
                                quiz = new QuizModel();
                                if (!reader.IsDBNull(0))
                                    quiz.Id = reader.GetValue(0).ToString();
                                else
                                    quiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    quiz.LessonId = reader.GetValue(1).ToString();
                                else
                                    quiz.LessonId = null;
                                if (!reader.IsDBNull(2))
                                    quiz.Title = reader.GetValue(2).ToString();
                                else
                                    quiz.Title = null;
                                if (!reader.IsDBNull(3))
                                    quiz.Instruction = reader.GetValue(3).ToString();
                                else
                                    quiz.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                                else
                                    quiz.LevelOfKnowledge = null;
                                if (!reader.IsDBNull(5))
                                    quiz.IsExam = reader.GetValue(5).ToString();
                                else
                                    quiz.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    quiz.QuizTypeId = reader.GetValue(6).ToString();
                                else
                                    quiz.QuizTypeId = null;
                                if (!reader.IsDBNull(7))
                                    quiz.CustomValue = reader.GetValue(7).ToString();
                                else
                                    quiz.CustomValue = null;
                                if (!reader.IsDBNull(8))
                                    quiz.ReadyToPublish = reader.GetValue(8).ToString();
                                else
                                    quiz.ReadyToPublish = null;
                                if (!reader.IsDBNull(9))
                                    quiz.Available = reader.GetValue(9).ToString();
                                else
                                    quiz.Available = null;
                                if (!reader.IsDBNull(10))
                                    quiz.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    quiz.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    quiz.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    quiz.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    quiz.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    quiz.UpdatedAt = null;
                                response.Add(quiz.Id, quiz);
                                quiz = new QuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizModel>();
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
                response = new Dictionary<string, QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizModel> ReadD(string connectionString, QuizModel quiz, bool orderByLastDate, int fromRow, int limit, QuizModel quizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizModel> response = new Dictionary<string, QuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                {
                    if (quiz != null)
                    {
                        if (!String.IsNullOrEmpty(quiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", quiz.Id);
                        }
                        if (!String.IsNullOrEmpty(quiz.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                        }
                        if (!String.IsNullOrEmpty(quiz.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", quiz.Title);
                        }
                        if (!String.IsNullOrEmpty(quiz.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }
                            cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                        }
                        if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` = @levelOfKnowledge ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` = @levelOfKnowledge ");
                            }
                            cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quiz.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                        }
                        if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quiz.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }
                            cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` = @readyToPublish ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` = @readyToPublish ");
                            }
                            cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", quiz.Available);
                        }
                        if (!String.IsNullOrEmpty(quiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                        }
                    }
                    if (quizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + quizWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + quizWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike", "%" + quizWhereLike.Instruction + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LevelOfKnowledge))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledgeWhereLike", "%" + quizWhereLike.LevelOfKnowledge + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + quizWhereLike.IsExam + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.QuizTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizTypeIdWhereLike", "%" + quizWhereLike.QuizTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike", "%" + quizWhereLike.CustomValue + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ReadyToPublish))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@readyToPublishWhereLike", "%" + quizWhereLike.ReadyToPublish + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + quizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizWhereLike.UpdatedAt + "%");
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
                                quiz = new QuizModel();
                                if (!reader.IsDBNull(0))
                                    quiz.Id = reader.GetValue(0).ToString();
                                else
                                    quiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    quiz.LessonId = reader.GetValue(1).ToString();
                                else
                                    quiz.LessonId = null;
                                if (!reader.IsDBNull(2))
                                    quiz.Title = reader.GetValue(2).ToString();
                                else
                                    quiz.Title = null;
                                if (!reader.IsDBNull(3))
                                    quiz.Instruction = reader.GetValue(3).ToString();
                                else
                                    quiz.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                                else
                                    quiz.LevelOfKnowledge = null;
                                if (!reader.IsDBNull(5))
                                    quiz.IsExam = reader.GetValue(5).ToString();
                                else
                                    quiz.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    quiz.QuizTypeId = reader.GetValue(6).ToString();
                                else
                                    quiz.QuizTypeId = null;
                                if (!reader.IsDBNull(7))
                                    quiz.CustomValue = reader.GetValue(7).ToString();
                                else
                                    quiz.CustomValue = null;
                                if (!reader.IsDBNull(8))
                                    quiz.ReadyToPublish = reader.GetValue(8).ToString();
                                else
                                    quiz.ReadyToPublish = null;
                                if (!reader.IsDBNull(9))
                                    quiz.Available = reader.GetValue(9).ToString();
                                else
                                    quiz.Available = null;
                                if (!reader.IsDBNull(10))
                                    quiz.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    quiz.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    quiz.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    quiz.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    quiz.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    quiz.UpdatedAt = null;
                                response.Add(quiz.Id, quiz);
                                quiz = new QuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizModel>();
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
                response = new Dictionary<string, QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizModel> ReadD(string connectionString, List<QuizModel> quizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizModel> response = new Dictionary<string, QuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                if (quizWhere != null && quizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, quizWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, quizWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizWhere[i].Instruction);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LevelOfKnowledge))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_of_knowledge` = @levelOfKnowledge" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quizWhere[i].LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, quizWhere[i].IsExam);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].QuizTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizTypeId" + i, quizWhere[i].QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizWhere[i].CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ReadyToPublish))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ready_to_publish` = @readyToPublish" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@readyToPublish" + i, quizWhere[i].ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, quizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizWhere.Count - 1)
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
                            QuizModel quiz = new QuizModel(); if (!reader.IsDBNull(0))
                                quiz.Id = reader.GetValue(0).ToString();
                            else
                                quiz.Id = null;
                            if (!reader.IsDBNull(1))
                                quiz.LessonId = reader.GetValue(1).ToString();
                            else
                                quiz.LessonId = null;
                            if (!reader.IsDBNull(2))
                                quiz.Title = reader.GetValue(2).ToString();
                            else
                                quiz.Title = null;
                            if (!reader.IsDBNull(3))
                                quiz.Instruction = reader.GetValue(3).ToString();
                            else
                                quiz.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                            else
                                quiz.LevelOfKnowledge = null;
                            if (!reader.IsDBNull(5))
                                quiz.IsExam = reader.GetValue(5).ToString();
                            else
                                quiz.IsExam = null;
                            if (!reader.IsDBNull(6))
                                quiz.QuizTypeId = reader.GetValue(6).ToString();
                            else
                                quiz.QuizTypeId = null;
                            if (!reader.IsDBNull(7))
                                quiz.CustomValue = reader.GetValue(7).ToString();
                            else
                                quiz.CustomValue = null;
                            if (!reader.IsDBNull(8))
                                quiz.ReadyToPublish = reader.GetValue(8).ToString();
                            else
                                quiz.ReadyToPublish = null;
                            if (!reader.IsDBNull(9))
                                quiz.Available = reader.GetValue(9).ToString();
                            else
                                quiz.Available = null;
                            if (!reader.IsDBNull(10))
                                quiz.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                quiz.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                quiz.CreatedAt = reader.GetValue(11).ToString();
                            else
                                quiz.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                quiz.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                quiz.UpdatedAt = null;
                            response.Add(quiz.Id, quiz);
                            quiz = new QuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizModel> ReadD(string connectionString, List<QuizModel> quizWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizModel> response = new Dictionary<string, QuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                if (quizWhere != null && quizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, quizWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, quizWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizWhere[i].Instruction);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LevelOfKnowledge))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_of_knowledge` = @levelOfKnowledge" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quizWhere[i].LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, quizWhere[i].IsExam);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].QuizTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizTypeId" + i, quizWhere[i].QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizWhere[i].CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ReadyToPublish))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ready_to_publish` = @readyToPublish" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@readyToPublish" + i, quizWhere[i].ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, quizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizWhere.Count - 1)
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
                            QuizModel quiz = new QuizModel(); if (!reader.IsDBNull(0))
                                quiz.Id = reader.GetValue(0).ToString();
                            else
                                quiz.Id = null;
                            if (!reader.IsDBNull(1))
                                quiz.LessonId = reader.GetValue(1).ToString();
                            else
                                quiz.LessonId = null;
                            if (!reader.IsDBNull(2))
                                quiz.Title = reader.GetValue(2).ToString();
                            else
                                quiz.Title = null;
                            if (!reader.IsDBNull(3))
                                quiz.Instruction = reader.GetValue(3).ToString();
                            else
                                quiz.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                            else
                                quiz.LevelOfKnowledge = null;
                            if (!reader.IsDBNull(5))
                                quiz.IsExam = reader.GetValue(5).ToString();
                            else
                                quiz.IsExam = null;
                            if (!reader.IsDBNull(6))
                                quiz.QuizTypeId = reader.GetValue(6).ToString();
                            else
                                quiz.QuizTypeId = null;
                            if (!reader.IsDBNull(7))
                                quiz.CustomValue = reader.GetValue(7).ToString();
                            else
                                quiz.CustomValue = null;
                            if (!reader.IsDBNull(8))
                                quiz.ReadyToPublish = reader.GetValue(8).ToString();
                            else
                                quiz.ReadyToPublish = null;
                            if (!reader.IsDBNull(9))
                                quiz.Available = reader.GetValue(9).ToString();
                            else
                                quiz.Available = null;
                            if (!reader.IsDBNull(10))
                                quiz.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                quiz.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                quiz.CreatedAt = reader.GetValue(11).ToString();
                            else
                                quiz.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                quiz.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                quiz.UpdatedAt = null;
                            response.Add(quiz.Id, quiz);
                            quiz = new QuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizModel> Read(string connectionString, QuizModel quiz, bool firstOne, bool orderByLastDate, QuizModel quizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                {
                    if (quiz != null)
                    {
                        if (!String.IsNullOrEmpty(quiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", quiz.Id);
                        }
                        if (!String.IsNullOrEmpty(quiz.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                        }
                        if (!String.IsNullOrEmpty(quiz.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", quiz.Title);
                        }
                        if (!String.IsNullOrEmpty(quiz.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }
                            cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                        }
                        if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` = @levelOfKnowledge ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` = @levelOfKnowledge ");
                            }
                            cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quiz.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                        }
                        if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quiz.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }
                            cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` = @readyToPublish ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` = @readyToPublish ");
                            }
                            cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", quiz.Available);
                        }
                        if (!String.IsNullOrEmpty(quiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                        }
                    }
                    if (quizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + quizWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + quizWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike", "%" + quizWhereLike.Instruction + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LevelOfKnowledge))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledgeWhereLike", "%" + quizWhereLike.LevelOfKnowledge + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + quizWhereLike.IsExam + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.QuizTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizTypeIdWhereLike", "%" + quizWhereLike.QuizTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike", "%" + quizWhereLike.CustomValue + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ReadyToPublish))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@readyToPublishWhereLike", "%" + quizWhereLike.ReadyToPublish + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + quizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizWhereLike.UpdatedAt + "%");
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
                                quiz = new QuizModel();
                                if (!reader.IsDBNull(0))
                                    quiz.Id = reader.GetValue(0).ToString();
                                else
                                    quiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    quiz.LessonId = reader.GetValue(1).ToString();
                                else
                                    quiz.LessonId = null;
                                if (!reader.IsDBNull(2))
                                    quiz.Title = reader.GetValue(2).ToString();
                                else
                                    quiz.Title = null;
                                if (!reader.IsDBNull(3))
                                    quiz.Instruction = reader.GetValue(3).ToString();
                                else
                                    quiz.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                                else
                                    quiz.LevelOfKnowledge = null;
                                if (!reader.IsDBNull(5))
                                    quiz.IsExam = reader.GetValue(5).ToString();
                                else
                                    quiz.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    quiz.QuizTypeId = reader.GetValue(6).ToString();
                                else
                                    quiz.QuizTypeId = null;
                                if (!reader.IsDBNull(7))
                                    quiz.CustomValue = reader.GetValue(7).ToString();
                                else
                                    quiz.CustomValue = null;
                                if (!reader.IsDBNull(8))
                                    quiz.ReadyToPublish = reader.GetValue(8).ToString();
                                else
                                    quiz.ReadyToPublish = null;
                                if (!reader.IsDBNull(9))
                                    quiz.Available = reader.GetValue(9).ToString();
                                else
                                    quiz.Available = null;
                                if (!reader.IsDBNull(10))
                                    quiz.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    quiz.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    quiz.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    quiz.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    quiz.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    quiz.UpdatedAt = null;
                                response.Add(quiz);
                                quiz = new QuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizModel>();
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
                response = new List<QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizModel> Read(string connectionString, QuizModel quiz, bool orderByLastDate, int fromRow, int limit, QuizModel quizWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                {
                    if (quiz != null)
                    {
                        if (!String.IsNullOrEmpty(quiz.Id))
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
                            cmd.Parameters.AddWithValue("@id", quiz.Id);
                        }
                        if (!String.IsNullOrEmpty(quiz.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                        }
                        if (!String.IsNullOrEmpty(quiz.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", quiz.Title);
                        }
                        if (!String.IsNullOrEmpty(quiz.Instruction))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `instruction` = @instruction ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `instruction` = @instruction ");
                            }
                            cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                        }
                        if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` = @levelOfKnowledge ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` = @levelOfKnowledge ");
                            }
                            cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quiz.IsExam))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `is_exam` = @isExam ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `is_exam` = @isExam ");
                            }
                            cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                        }
                        if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                            }
                            cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quiz.CustomValue))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `custom_value` = @customValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `custom_value` = @customValue ");
                            }
                            cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` = @readyToPublish ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` = @readyToPublish ");
                            }
                            cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quiz.Available))
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
                            cmd.Parameters.AddWithValue("@available", quiz.Available);
                        }
                        if (!String.IsNullOrEmpty(quiz.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quiz.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quiz.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                        }
                    }
                    if (quizWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LessonId))
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
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + quizWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Title))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `title` LIKE @titleWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `title` LIKE @titleWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@titleWhereLike", "%" + quizWhereLike.Title + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Instruction))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `instruction` LIKE @instructionWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `instruction` LIKE @instructionWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@instructionWhereLike", "%" + quizWhereLike.Instruction + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.LevelOfKnowledge))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `level_of_knowledge` LIKE @levelOfKnowledgeWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledgeWhereLike", "%" + quizWhereLike.LevelOfKnowledge + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.IsExam))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `is_exam` LIKE @isExamWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `is_exam` LIKE @isExamWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@isExamWhereLike", "%" + quizWhereLike.IsExam + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.QuizTypeId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_type_id` LIKE @quizTypeIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizTypeIdWhereLike", "%" + quizWhereLike.QuizTypeId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CustomValue))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `custom_value` LIKE @customValueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `custom_value` LIKE @customValueWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@customValueWhereLike", "%" + quizWhereLike.CustomValue + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ReadyToPublish))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `ready_to_publish` LIKE @readyToPublishWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@readyToPublishWhereLike", "%" + quizWhereLike.ReadyToPublish + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.Available))
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
                            cmd.Parameters.AddWithValue("@availableWhereLike", "%" + quizWhereLike.Available + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizWhereLike.UpdatedAt + "%");
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
                                quiz = new QuizModel();
                                if (!reader.IsDBNull(0))
                                    quiz.Id = reader.GetValue(0).ToString();
                                else
                                    quiz.Id = null;
                                if (!reader.IsDBNull(1))
                                    quiz.LessonId = reader.GetValue(1).ToString();
                                else
                                    quiz.LessonId = null;
                                if (!reader.IsDBNull(2))
                                    quiz.Title = reader.GetValue(2).ToString();
                                else
                                    quiz.Title = null;
                                if (!reader.IsDBNull(3))
                                    quiz.Instruction = reader.GetValue(3).ToString();
                                else
                                    quiz.Instruction = null;
                                if (!reader.IsDBNull(4))
                                    quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                                else
                                    quiz.LevelOfKnowledge = null;
                                if (!reader.IsDBNull(5))
                                    quiz.IsExam = reader.GetValue(5).ToString();
                                else
                                    quiz.IsExam = null;
                                if (!reader.IsDBNull(6))
                                    quiz.QuizTypeId = reader.GetValue(6).ToString();
                                else
                                    quiz.QuizTypeId = null;
                                if (!reader.IsDBNull(7))
                                    quiz.CustomValue = reader.GetValue(7).ToString();
                                else
                                    quiz.CustomValue = null;
                                if (!reader.IsDBNull(8))
                                    quiz.ReadyToPublish = reader.GetValue(8).ToString();
                                else
                                    quiz.ReadyToPublish = null;
                                if (!reader.IsDBNull(9))
                                    quiz.Available = reader.GetValue(9).ToString();
                                else
                                    quiz.Available = null;
                                if (!reader.IsDBNull(10))
                                    quiz.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    quiz.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    quiz.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    quiz.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    quiz.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    quiz.UpdatedAt = null;
                                response.Add(quiz);
                                quiz = new QuizModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizModel>();
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
                response = new List<QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizModel> Read(string connectionString, List<QuizModel> quizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                if (quizWhere != null && quizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, quizWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, quizWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizWhere[i].Instruction);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LevelOfKnowledge))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_of_knowledge` = @levelOfKnowledge" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quizWhere[i].LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, quizWhere[i].IsExam);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].QuizTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizTypeId" + i, quizWhere[i].QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizWhere[i].CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ReadyToPublish))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ready_to_publish` = @readyToPublish" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@readyToPublish" + i, quizWhere[i].ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, quizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizWhere.Count - 1)
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
                            QuizModel quiz = new QuizModel(); if (!reader.IsDBNull(0))
                                quiz.Id = reader.GetValue(0).ToString();
                            else
                                quiz.Id = null;
                            if (!reader.IsDBNull(1))
                                quiz.LessonId = reader.GetValue(1).ToString();
                            else
                                quiz.LessonId = null;
                            if (!reader.IsDBNull(2))
                                quiz.Title = reader.GetValue(2).ToString();
                            else
                                quiz.Title = null;
                            if (!reader.IsDBNull(3))
                                quiz.Instruction = reader.GetValue(3).ToString();
                            else
                                quiz.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                            else
                                quiz.LevelOfKnowledge = null;
                            if (!reader.IsDBNull(5))
                                quiz.IsExam = reader.GetValue(5).ToString();
                            else
                                quiz.IsExam = null;
                            if (!reader.IsDBNull(6))
                                quiz.QuizTypeId = reader.GetValue(6).ToString();
                            else
                                quiz.QuizTypeId = null;
                            if (!reader.IsDBNull(7))
                                quiz.CustomValue = reader.GetValue(7).ToString();
                            else
                                quiz.CustomValue = null;
                            if (!reader.IsDBNull(8))
                                quiz.ReadyToPublish = reader.GetValue(8).ToString();
                            else
                                quiz.ReadyToPublish = null;
                            if (!reader.IsDBNull(9))
                                quiz.Available = reader.GetValue(9).ToString();
                            else
                                quiz.Available = null;
                            if (!reader.IsDBNull(10))
                                quiz.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                quiz.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                quiz.CreatedAt = reader.GetValue(11).ToString();
                            else
                                quiz.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                quiz.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                quiz.UpdatedAt = null;
                            response.Add(quiz);
                            quiz = new QuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizModel> Read(string connectionString, List<QuizModel> quizWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `lesson_id`, `title`, `instruction`, `level_of_knowledge`, `is_exam`, `quiz_type_id`, `custom_value`, `ready_to_publish`, `available`, `modified_by`, `created_at`, `updated_at`  FROM quizs ";
                if (quizWhere != null && quizWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, quizWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, quizWhere[i].Title);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Instruction))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `instruction` = @instruction" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instruction" + i, quizWhere[i].Instruction);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].LevelOfKnowledge))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_of_knowledge` = @levelOfKnowledge" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quizWhere[i].LevelOfKnowledge);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].IsExam))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_exam` = @isExam" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isExam" + i, quizWhere[i].IsExam);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].QuizTypeId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizTypeId" + i, quizWhere[i].QuizTypeId);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CustomValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `custom_value` = @customValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@customValue" + i, quizWhere[i].CustomValue);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ReadyToPublish))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `ready_to_publish` = @readyToPublish" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@readyToPublish" + i, quizWhere[i].ReadyToPublish);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, quizWhere[i].Available);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizWhere.Count - 1)
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
                            QuizModel quiz = new QuizModel(); if (!reader.IsDBNull(0))
                                quiz.Id = reader.GetValue(0).ToString();
                            else
                                quiz.Id = null;
                            if (!reader.IsDBNull(1))
                                quiz.LessonId = reader.GetValue(1).ToString();
                            else
                                quiz.LessonId = null;
                            if (!reader.IsDBNull(2))
                                quiz.Title = reader.GetValue(2).ToString();
                            else
                                quiz.Title = null;
                            if (!reader.IsDBNull(3))
                                quiz.Instruction = reader.GetValue(3).ToString();
                            else
                                quiz.Instruction = null;
                            if (!reader.IsDBNull(4))
                                quiz.LevelOfKnowledge = reader.GetValue(4).ToString();
                            else
                                quiz.LevelOfKnowledge = null;
                            if (!reader.IsDBNull(5))
                                quiz.IsExam = reader.GetValue(5).ToString();
                            else
                                quiz.IsExam = null;
                            if (!reader.IsDBNull(6))
                                quiz.QuizTypeId = reader.GetValue(6).ToString();
                            else
                                quiz.QuizTypeId = null;
                            if (!reader.IsDBNull(7))
                                quiz.CustomValue = reader.GetValue(7).ToString();
                            else
                                quiz.CustomValue = null;
                            if (!reader.IsDBNull(8))
                                quiz.ReadyToPublish = reader.GetValue(8).ToString();
                            else
                                quiz.ReadyToPublish = null;
                            if (!reader.IsDBNull(9))
                                quiz.Available = reader.GetValue(9).ToString();
                            else
                                quiz.Available = null;
                            if (!reader.IsDBNull(10))
                                quiz.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                quiz.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                quiz.CreatedAt = reader.GetValue(11).ToString();
                            else
                                quiz.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                quiz.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                quiz.UpdatedAt = null;
                            response.Add(quiz);
                            quiz = new QuizModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizModel> Create(string connectionString, QuizModel quiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quizs (";
                string values = "(";
                if (!String.IsNullOrEmpty(quiz.Id))
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
                    cmd.Parameters.AddWithValue("@id", quiz.Id);
                }
                if (!String.IsNullOrEmpty(quiz.LessonId))
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
                    cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                }
                if (!String.IsNullOrEmpty(quiz.Title))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `title` ");
                        values = String.Concat(values, ", @title");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `title` ");
                        values = String.Concat(values, " @title");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", quiz.Title);
                }
                if (!String.IsNullOrEmpty(quiz.Instruction))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `instruction` ");
                        values = String.Concat(values, ", @instruction");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `instruction` ");
                        values = String.Concat(values, " @instruction");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                }
                if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `level_of_knowledge` ");
                        values = String.Concat(values, ", @levelOfKnowledge");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `level_of_knowledge` ");
                        values = String.Concat(values, " @levelOfKnowledge");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                }
                if (!String.IsNullOrEmpty(quiz.IsExam))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `is_exam` ");
                        values = String.Concat(values, ", @isExam");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `is_exam` ");
                        values = String.Concat(values, " @isExam");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                }
                if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `quiz_type_id` ");
                        values = String.Concat(values, ", @quizTypeId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `quiz_type_id` ");
                        values = String.Concat(values, " @quizTypeId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                }
                if (!String.IsNullOrEmpty(quiz.CustomValue))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `custom_value` ");
                        values = String.Concat(values, ", @customValue");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `custom_value` ");
                        values = String.Concat(values, " @customValue");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                }
                if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `ready_to_publish` ");
                        values = String.Concat(values, ", @readyToPublish");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `ready_to_publish` ");
                        values = String.Concat(values, " @readyToPublish");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                }
                if (!String.IsNullOrEmpty(quiz.Available))
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
                    cmd.Parameters.AddWithValue("@available", quiz.Available);
                }
                if (!String.IsNullOrEmpty(quiz.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quiz.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quiz.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quiz.Id = cmd.LastInsertedId.ToString();
                response.Add(quiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
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
        public List<QuizModel> Create(string connectionString, List<QuizModel> quizList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
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
                        for (int i = 0; i < quizList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quizs (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].LessonId))
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
                                    cmd.Parameters.AddWithValue("@lessonId" + i, quizList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].Title))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `title` ");
                                        values = String.Concat(values, ", @title" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `title` ");
                                        values = String.Concat(values, "@title" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, quizList[i].Title);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].Instruction))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `instruction` ");
                                        values = String.Concat(values, ", @instruction" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `instruction` ");
                                        values = String.Concat(values, "@instruction" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@instruction" + i, quizList[i].Instruction);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].LevelOfKnowledge))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `level_of_knowledge` ");
                                        values = String.Concat(values, ", @levelOfKnowledge" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `level_of_knowledge` ");
                                        values = String.Concat(values, "@levelOfKnowledge" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quizList[i].LevelOfKnowledge);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].IsExam))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `is_exam` ");
                                        values = String.Concat(values, ", @isExam" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `is_exam` ");
                                        values = String.Concat(values, "@isExam" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isExam" + i, quizList[i].IsExam);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].QuizTypeId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `quiz_type_id` ");
                                        values = String.Concat(values, ", @quizTypeId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `quiz_type_id` ");
                                        values = String.Concat(values, "@quizTypeId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizTypeId" + i, quizList[i].QuizTypeId);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].CustomValue))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `custom_value` ");
                                        values = String.Concat(values, ", @customValue" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `custom_value` ");
                                        values = String.Concat(values, "@customValue" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customValue" + i, quizList[i].CustomValue);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].ReadyToPublish))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `ready_to_publish` ");
                                        values = String.Concat(values, ", @readyToPublish" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `ready_to_publish` ");
                                        values = String.Concat(values, "@readyToPublish" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@readyToPublish" + i, quizList[i].ReadyToPublish);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].Available))
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
                                    cmd.Parameters.AddWithValue("@available" + i, quizList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
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

        public List<QuizModel> Update(string connectionString, QuizModel quiz, QuizModel quizWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quizs SET ";
                if (!String.IsNullOrEmpty(quiz.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quiz.Id);
                }
                if (!String.IsNullOrEmpty(quiz.LessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lesson_id` = @lessonId");
                    else
                        sql = String.Concat(sql, " `lesson_id` = @lessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                }
                if (!String.IsNullOrEmpty(quiz.Title))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `title` = @title");
                    else
                        sql = String.Concat(sql, " `title` = @title");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", quiz.Title);
                }
                if (!String.IsNullOrEmpty(quiz.Instruction))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `instruction` = @instruction");
                    else
                        sql = String.Concat(sql, " `instruction` = @instruction");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                }
                if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `level_of_knowledge` = @levelOfKnowledge");
                    else
                        sql = String.Concat(sql, " `level_of_knowledge` = @levelOfKnowledge");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                }
                if (!String.IsNullOrEmpty(quiz.IsExam))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `is_exam` = @isExam");
                    else
                        sql = String.Concat(sql, " `is_exam` = @isExam");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                }
                if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_type_id` = @quizTypeId");
                    else
                        sql = String.Concat(sql, " `quiz_type_id` = @quizTypeId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                }
                if (!String.IsNullOrEmpty(quiz.CustomValue))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `custom_value` = @customValue");
                    else
                        sql = String.Concat(sql, " `custom_value` = @customValue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                }
                if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `ready_to_publish` = @readyToPublish");
                    else
                        sql = String.Concat(sql, " `ready_to_publish` = @readyToPublish");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                }
                if (!String.IsNullOrEmpty(quiz.Available))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `available` = @available");
                    else
                        sql = String.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", quiz.Available);
                }
                if (!String.IsNullOrEmpty(quiz.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quiz.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quiz.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                }
                if (quizWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonIdWhere", quizWhere.LessonId);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.Title))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `title` = @titleWhere");
                        else
                            sql = String.Concat(sql, " `title` = @titleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleWhere", quizWhere.Title);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.Instruction))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `instruction` = @instructionWhere");
                        else
                            sql = String.Concat(sql, " `instruction` = @instructionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@instructionWhere", quizWhere.Instruction);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.LevelOfKnowledge))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_of_knowledge` = @levelOfKnowledgeWhere");
                        else
                            sql = String.Concat(sql, " `level_of_knowledge` = @levelOfKnowledgeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelOfKnowledgeWhere", quizWhere.LevelOfKnowledge);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.IsExam))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `is_exam` = @isExamWhere");
                        else
                            sql = String.Concat(sql, " `is_exam` = @isExamWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isExamWhere", quizWhere.IsExam);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.QuizTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_type_id` = @quizTypeIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_type_id` = @quizTypeIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizTypeIdWhere", quizWhere.QuizTypeId);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.CustomValue))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `custom_value` = @customValueWhere");
                        else
                            sql = String.Concat(sql, " `custom_value` = @customValueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customValueWhere", quizWhere.CustomValue);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.ReadyToPublish))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `ready_to_publish` = @readyToPublishWhere");
                        else
                            sql = String.Concat(sql, " `ready_to_publish` = @readyToPublishWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@readyToPublishWhere", quizWhere.ReadyToPublish);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = String.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", quizWhere.Available);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
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

        public List<QuizModel> Update(string connectionString, List<QuizModel> quizList, List<QuizModel> quizWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
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
                        for (int i = 0; i < quizList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quizs SET ";
                                if (!String.IsNullOrEmpty(quizList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].LessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lesson_id` = @lessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, quizList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].Title))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `title` = @title" + i);
                                    else
                                        sql = String.Concat(sql, " `title` = @title" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, quizList[i].Title);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].Instruction))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `instruction` = @instruction" + i);
                                    else
                                        sql = String.Concat(sql, " `instruction` = @instruction" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@instruction" + i, quizList[i].Instruction);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].LevelOfKnowledge))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `level_of_knowledge` = @levelOfKnowledge" + i);
                                    else
                                        sql = String.Concat(sql, " `level_of_knowledge` = @levelOfKnowledge" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quizList[i].LevelOfKnowledge);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].IsExam))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `is_exam` = @isExam" + i);
                                    else
                                        sql = String.Concat(sql, " `is_exam` = @isExam" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isExam" + i, quizList[i].IsExam);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].QuizTypeId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_type_id` = @quizTypeId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_type_id` = @quizTypeId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizTypeId" + i, quizList[i].QuizTypeId);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].CustomValue))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `custom_value` = @customValue" + i);
                                    else
                                        sql = String.Concat(sql, " `custom_value` = @customValue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@customValue" + i, quizList[i].CustomValue);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].ReadyToPublish))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `ready_to_publish` = @readyToPublish" + i);
                                    else
                                        sql = String.Concat(sql, " `ready_to_publish` = @readyToPublish" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@readyToPublish" + i, quizList[i].ReadyToPublish);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].Available))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = String.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, quizList[i].Available);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizList[i].UpdatedAt);
                                }
                                if (quizWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonIdWhere" + i, quizWhereList[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].Title))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `title` = @titleWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `title` = @titleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleWhere" + i, quizWhereList[i].Title);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].Instruction))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `instruction` = @instructionWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `instruction` = @instructionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@instructionWhere" + i, quizWhereList[i].Instruction);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].LevelOfKnowledge))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_of_knowledge` = @levelOfKnowledgeWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `level_of_knowledge` = @levelOfKnowledgeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelOfKnowledgeWhere" + i, quizWhereList[i].LevelOfKnowledge);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].IsExam))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `is_exam` = @isExamWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `is_exam` = @isExamWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isExamWhere" + i, quizWhereList[i].IsExam);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].QuizTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_type_id` = @quizTypeIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_type_id` = @quizTypeIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizTypeIdWhere" + i, quizWhereList[i].QuizTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].CustomValue))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `custom_value` = @customValueWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `custom_value` = @customValueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customValueWhere" + i, quizWhereList[i].CustomValue);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].ReadyToPublish))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `ready_to_publish` = @readyToPublishWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `ready_to_publish` = @readyToPublishWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@readyToPublishWhere" + i, quizWhereList[i].ReadyToPublish);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, quizWhereList[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
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

        public List<QuizModel> Delete(string connectionString, QuizModel quiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quizs  ";
                if (quiz != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quiz.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quiz.Id);
                    }
                    if (!String.IsNullOrEmpty(quiz.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonId", quiz.LessonId);
                    }
                    if (!String.IsNullOrEmpty(quiz.Title))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `title` = @title");
                        else
                            sql = String.Concat(sql, " `title` = @title");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@title", quiz.Title);
                    }
                    if (!String.IsNullOrEmpty(quiz.Instruction))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `instruction` = @instruction");
                        else
                            sql = String.Concat(sql, " `instruction` = @instruction");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@instruction", quiz.Instruction);
                    }
                    if (!String.IsNullOrEmpty(quiz.LevelOfKnowledge))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `level_of_knowledge` = @levelOfKnowledge");
                        else
                            sql = String.Concat(sql, " `level_of_knowledge` = @levelOfKnowledge");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelOfKnowledge", quiz.LevelOfKnowledge);
                    }
                    if (!String.IsNullOrEmpty(quiz.IsExam))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `is_exam` = @isExam");
                        else
                            sql = String.Concat(sql, " `is_exam` = @isExam");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isExam", quiz.IsExam);
                    }
                    if (!String.IsNullOrEmpty(quiz.QuizTypeId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_type_id` = @quizTypeId");
                        else
                            sql = String.Concat(sql, " `quiz_type_id` = @quizTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizTypeId", quiz.QuizTypeId);
                    }
                    if (!String.IsNullOrEmpty(quiz.CustomValue))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `custom_value` = @customValue");
                        else
                            sql = String.Concat(sql, " `custom_value` = @customValue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@customValue", quiz.CustomValue);
                    }
                    if (!String.IsNullOrEmpty(quiz.ReadyToPublish))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `ready_to_publish` = @readyToPublish");
                        else
                            sql = String.Concat(sql, " `ready_to_publish` = @readyToPublish");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@readyToPublish", quiz.ReadyToPublish);
                    }
                    if (!String.IsNullOrEmpty(quiz.Available))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `available` = @available");
                        else
                            sql = String.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", quiz.Available);
                    }
                    if (!String.IsNullOrEmpty(quiz.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quiz.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quiz.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quiz.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quiz.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quiz.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quiz);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
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

        public List<QuizModel> Delete(string connectionString, List<QuizModel> quiz)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizModel> response = new List<QuizModel>();
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
                        for (int i = 0; i < quiz.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quizs  ";
                                if (quiz[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quiz[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quiz[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonId" + i, quiz[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].Title))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `title` = @title" + i);
                                        else
                                            sql = String.Concat(sql, " `title` = @title" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@title" + i, quiz[i].Title);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].Instruction))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `instruction` = @instruction" + i);
                                        else
                                            sql = String.Concat(sql, " `instruction` = @instruction" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@instruction" + i, quiz[i].Instruction);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].LevelOfKnowledge))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `level_of_knowledge` = @levelOfKnowledge" + i);
                                        else
                                            sql = String.Concat(sql, " `level_of_knowledge` = @levelOfKnowledge" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelOfKnowledge" + i, quiz[i].LevelOfKnowledge);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].IsExam))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `is_exam` = @isExam" + i);
                                        else
                                            sql = String.Concat(sql, " `is_exam` = @isExam" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isExam" + i, quiz[i].IsExam);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].QuizTypeId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_type_id` = @quizTypeId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_type_id` = @quizTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizTypeId" + i, quiz[i].QuizTypeId);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].CustomValue))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `custom_value` = @customValue" + i);
                                        else
                                            sql = String.Concat(sql, " `custom_value` = @customValue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@customValue" + i, quiz[i].CustomValue);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].ReadyToPublish))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `ready_to_publish` = @readyToPublish" + i);
                                        else
                                            sql = String.Concat(sql, " `ready_to_publish` = @readyToPublish" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@readyToPublish" + i, quiz[i].ReadyToPublish);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].Available))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = String.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, quiz[i].Available);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quiz[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quiz[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quiz[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quiz[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quiz[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizModel>();
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
