using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    namespace TeacherAPI.Infrastructure.Repositories.Lessons
    {
        class QuizRuleDAO
        {
            private MySqlConnection _connection;
            public Dictionary<string, QuizRuleModel> ReadD(string connectionString, QuizRuleModel quizRule, bool firstOne, bool orderByLastDate)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                Dictionary<string, QuizRuleModel> response = new Dictionary<string, QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    {
                        if (quizRule != null)
                        {
                            if (!string.IsNullOrEmpty(quizRule.Id))
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
                                cmd.Parameters.AddWithValue("@id", quizRule.Id);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                                }
                                cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `lesson_addressed_to_id` = @lessonAddressedToId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `lesson_addressed_to_id` = @lessonAddressedToId ");
                                }
                                cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `question_max_length` = @questionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `question_max_length` = @questionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `response_max_length` = @responseMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `response_max_length` = @responseMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `extra_info_max_length` = @extraInfoMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `extra_info_max_length` = @extraInfoMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `instruction_max_length` = @instructionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `instruction_max_length` = @instructionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `max_questions` = @maxQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `max_questions` = @maxQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `min_questions` = @minQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `min_questions` = @minQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `false_response_required` = @falseResponseRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `false_response_required` = @falseResponseRequired ");
                                }
                                cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `title_max_length` = @titleMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `title_max_length` = @titleMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `criterion_required` = @criterionRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `criterion_required` = @criterionRequired ");
                                }
                                cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `reference_required` = @referenceRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `reference_required` = @referenceRequired ");
                                }
                                cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `delete_question_allow` = @deleteQuestionAllow ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `delete_question_allow` = @deleteQuestionAllow ");
                                }
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRule.EditWarning))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `edit_warning` = @editWarning ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `edit_warning` = @editWarning ");
                                }
                                cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
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
                                cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CreatedAt))
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
                                cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
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
                                cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                            }
                            if (orderByLastDate)
                                sql = string.Concat(sql, " ORDER BY updated_at DESC");
                            if (firstOne)
                                sql = string.Concat(sql, " LIMIT 1 ");
                        }
                        cmd.CommandText = sql;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                        quizRule.Id = reader.GetValue(0).ToString();
                                    else
                                        quizRule.Id = null;
                                    if (!reader.IsDBNull(1))
                                        quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                    else
                                        quizRule.QuizTypeId = null;
                                    if (!reader.IsDBNull(2))
                                        quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                    else
                                        quizRule.LessonAddressedToId = null;
                                    if (!reader.IsDBNull(3))
                                        quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                    else
                                        quizRule.QuestionMaxLength = null;
                                    if (!reader.IsDBNull(4))
                                        quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                    else
                                        quizRule.ResponseMaxLength = null;
                                    if (!reader.IsDBNull(5))
                                        quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                    else
                                        quizRule.ExtraInfoMaxLength = null;
                                    if (!reader.IsDBNull(6))
                                        quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                    else
                                        quizRule.InstructionMaxLength = null;
                                    if (!reader.IsDBNull(7))
                                        quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                    else
                                        quizRule.MaxQuestions = null;
                                    if (!reader.IsDBNull(8))
                                        quizRule.MinQuestions = reader.GetValue(8).ToString();
                                    else
                                        quizRule.MinQuestions = null;
                                    if (!reader.IsDBNull(9))
                                        quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                    else
                                        quizRule.FalseResponseRequired = null;
                                    if (!reader.IsDBNull(10))
                                        quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                    else
                                        quizRule.TitleMaxLength = null;
                                    if (!reader.IsDBNull(11))
                                        quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                    else
                                        quizRule.CriterionRequired = null;
                                    if (!reader.IsDBNull(12))
                                        quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                    else
                                        quizRule.ReferenceRequired = null;
                                    if (!reader.IsDBNull(13))
                                        quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                    else
                                        quizRule.DeleteQuestionAllow = null;
                                    if (!reader.IsDBNull(14))
                                        quizRule.EditWarning = reader.GetValue(14).ToString();
                                    else
                                        quizRule.EditWarning = null;
                                    if (!reader.IsDBNull(15))
                                        quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                    else
                                        quizRule.ModifiedBy = null;
                                    if (!reader.IsDBNull(16))
                                        quizRule.CreatedAt = reader.GetValue(16).ToString();
                                    else
                                        quizRule.CreatedAt = null;
                                    if (!reader.IsDBNull(17))
                                        quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                    else
                                        quizRule.UpdatedAt = null;
                                    response.Add(quizRule.Id, quizRule);
                                    quizRule = new QuizRuleModel();
                                }
                        }
                        catch (ArgumentException e)
                        {
                            string MessageString = " Read error occured: " + e.Message;
                            response = new Dictionary<string, QuizRuleModel>();
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
                    response = new Dictionary<string, QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public Dictionary<string, QuizRuleModel> ReadD(string connectionString, QuizRuleModel quizRule, bool orderByLastDate, int fromRow, int limit)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                Dictionary<string, QuizRuleModel> response = new Dictionary<string, QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    {
                        if (quizRule != null)
                        {
                            if (!string.IsNullOrEmpty(quizRule.Id))
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
                                cmd.Parameters.AddWithValue("@id", quizRule.Id);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                                }
                                cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `lesson_addressed_to_id` = @lessonAddressedToId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `lesson_addressed_to_id` = @lessonAddressedToId ");
                                }
                                cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `question_max_length` = @questionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `question_max_length` = @questionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `response_max_length` = @responseMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `response_max_length` = @responseMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `extra_info_max_length` = @extraInfoMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `extra_info_max_length` = @extraInfoMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `instruction_max_length` = @instructionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `instruction_max_length` = @instructionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `max_questions` = @maxQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `max_questions` = @maxQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `min_questions` = @minQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `min_questions` = @minQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `false_response_required` = @falseResponseRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `false_response_required` = @falseResponseRequired ");
                                }
                                cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `title_max_length` = @titleMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `title_max_length` = @titleMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `criterion_required` = @criterionRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `criterion_required` = @criterionRequired ");
                                }
                                cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `reference_required` = @referenceRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `reference_required` = @referenceRequired ");
                                }
                                cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `delete_question_allow` = @deleteQuestionAllow ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `delete_question_allow` = @deleteQuestionAllow ");
                                }
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRule.EditWarning))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `edit_warning` = @editWarning ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `edit_warning` = @editWarning ");
                                }
                                cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
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
                                cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CreatedAt))
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
                                cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
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
                                cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                            }
                            if (orderByLastDate)
                                sql = string.Concat(sql, " ORDER BY updated_at DESC");
                            sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                            cmd.Parameters.AddWithValue("@fromRow", fromRow);
                            cmd.Parameters.AddWithValue("@limit", limit);
                        }
                        cmd.CommandText = sql;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                        quizRule.Id = reader.GetValue(0).ToString();
                                    else
                                        quizRule.Id = null;
                                    if (!reader.IsDBNull(1))
                                        quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                    else
                                        quizRule.QuizTypeId = null;
                                    if (!reader.IsDBNull(2))
                                        quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                    else
                                        quizRule.LessonAddressedToId = null;
                                    if (!reader.IsDBNull(3))
                                        quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                    else
                                        quizRule.QuestionMaxLength = null;
                                    if (!reader.IsDBNull(4))
                                        quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                    else
                                        quizRule.ResponseMaxLength = null;
                                    if (!reader.IsDBNull(5))
                                        quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                    else
                                        quizRule.ExtraInfoMaxLength = null;
                                    if (!reader.IsDBNull(6))
                                        quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                    else
                                        quizRule.InstructionMaxLength = null;
                                    if (!reader.IsDBNull(7))
                                        quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                    else
                                        quizRule.MaxQuestions = null;
                                    if (!reader.IsDBNull(8))
                                        quizRule.MinQuestions = reader.GetValue(8).ToString();
                                    else
                                        quizRule.MinQuestions = null;
                                    if (!reader.IsDBNull(9))
                                        quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                    else
                                        quizRule.FalseResponseRequired = null;
                                    if (!reader.IsDBNull(10))
                                        quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                    else
                                        quizRule.TitleMaxLength = null;
                                    if (!reader.IsDBNull(11))
                                        quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                    else
                                        quizRule.CriterionRequired = null;
                                    if (!reader.IsDBNull(12))
                                        quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                    else
                                        quizRule.ReferenceRequired = null;
                                    if (!reader.IsDBNull(13))
                                        quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                    else
                                        quizRule.DeleteQuestionAllow = null;
                                    if (!reader.IsDBNull(14))
                                        quizRule.EditWarning = reader.GetValue(14).ToString();
                                    else
                                        quizRule.EditWarning = null;
                                    if (!reader.IsDBNull(15))
                                        quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                    else
                                        quizRule.ModifiedBy = null;
                                    if (!reader.IsDBNull(16))
                                        quizRule.CreatedAt = reader.GetValue(16).ToString();
                                    else
                                        quizRule.CreatedAt = null;
                                    if (!reader.IsDBNull(17))
                                        quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                    else
                                        quizRule.UpdatedAt = null;
                                    response.Add(quizRule.Id, quizRule);
                                    quizRule = new QuizRuleModel();
                                }
                        }
                        catch (ArgumentException e)
                        {
                            string MessageString = " Read error occured: " + e.Message;
                            response = new Dictionary<string, QuizRuleModel>();
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
                    response = new Dictionary<string, QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public Dictionary<string, QuizRuleModel> ReadD(string connectionString, List<QuizRuleModel> quizRuleWhere)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                Dictionary<string, QuizRuleModel> response = new Dictionary<string, QuizRuleModel>();
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sqlIn = "";
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    if (quizRuleWhere != null && quizRuleWhere.Count > 0)
                    {
                        sqlIn += " WHERE ";
                        for (int i = 0; i < quizRuleWhere.Count; i++)
                        {
                            sqlIn += "(";
                            bool addAnd = false;
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].Id))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `id` = @id" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@id" + i, quizRuleWhere[i].Id);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuizTypeId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRuleWhere[i].QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].LessonAddressedToId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `lesson_addressed_to_id` = @lessonAddressedToId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRuleWhere[i].LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuestionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `question_max_length` = @questionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRuleWhere[i].QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ResponseMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `response_max_length` = @responseMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRuleWhere[i].ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ExtraInfoMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `extra_info_max_length` = @extraInfoMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRuleWhere[i].ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].InstructionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `instruction_max_length` = @instructionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRuleWhere[i].InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MaxQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `max_questions` = @maxQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRuleWhere[i].MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MinQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `min_questions` = @minQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@minQuestions" + i, quizRuleWhere[i].MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].FalseResponseRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `false_response_required` = @falseResponseRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRuleWhere[i].FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].TitleMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `title_max_length` = @titleMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRuleWhere[i].TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CriterionRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `criterion_required` = @criterionRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRuleWhere[i].CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ReferenceRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `reference_required` = @referenceRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRuleWhere[i].ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].DeleteQuestionAllow))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `delete_question_allow` = @deleteQuestionAllow" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRuleWhere[i].DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].EditWarning))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `edit_warning` = @editWarning" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@editWarning" + i, quizRuleWhere[i].EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ModifiedBy))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `modified_by` = @modifiedBy" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRuleWhere[i].ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CreatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `created_at` = @createdAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@createdAt" + i, quizRuleWhere[i].CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].UpdatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `updated_at` = @updatedAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@updatedAt" + i, quizRuleWhere[i].UpdatedAt);
                            }
                            sqlIn += ")";
                            if (i != quizRuleWhere.Count - 1)
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
                                QuizRuleModel quizRule = new QuizRuleModel(); if (!reader.IsDBNull(0))
                                    quizRule.Id = reader.GetValue(0).ToString();
                                else
                                    quizRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                else
                                    quizRule.QuizTypeId = null;
                                if (!reader.IsDBNull(2))
                                    quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                else
                                    quizRule.LessonAddressedToId = null;
                                if (!reader.IsDBNull(3))
                                    quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                else
                                    quizRule.QuestionMaxLength = null;
                                if (!reader.IsDBNull(4))
                                    quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                else
                                    quizRule.ResponseMaxLength = null;
                                if (!reader.IsDBNull(5))
                                    quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                else
                                    quizRule.ExtraInfoMaxLength = null;
                                if (!reader.IsDBNull(6))
                                    quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                else
                                    quizRule.InstructionMaxLength = null;
                                if (!reader.IsDBNull(7))
                                    quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                else
                                    quizRule.MaxQuestions = null;
                                if (!reader.IsDBNull(8))
                                    quizRule.MinQuestions = reader.GetValue(8).ToString();
                                else
                                    quizRule.MinQuestions = null;
                                if (!reader.IsDBNull(9))
                                    quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                else
                                    quizRule.FalseResponseRequired = null;
                                if (!reader.IsDBNull(10))
                                    quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                else
                                    quizRule.TitleMaxLength = null;
                                if (!reader.IsDBNull(11))
                                    quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                else
                                    quizRule.CriterionRequired = null;
                                if (!reader.IsDBNull(12))
                                    quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                else
                                    quizRule.ReferenceRequired = null;
                                if (!reader.IsDBNull(13))
                                    quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                else
                                    quizRule.DeleteQuestionAllow = null;
                                if (!reader.IsDBNull(14))
                                    quizRule.EditWarning = reader.GetValue(14).ToString();
                                else
                                    quizRule.EditWarning = null;
                                if (!reader.IsDBNull(15))
                                    quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    quizRule.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    quizRule.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    quizRule.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    quizRule.UpdatedAt = null;
                                response.Add(quizRule.Id, quizRule);
                                quizRule = new QuizRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizRuleModel>();
                        reader.Close();
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                    response = new Dictionary<string, QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public Dictionary<string, QuizRuleModel> ReadD(string connectionString, List<QuizRuleModel> quizRuleWhere, int fromRow, int limit)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                Dictionary<string, QuizRuleModel> response = new Dictionary<string, QuizRuleModel>();
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sqlIn = "";
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    if (quizRuleWhere != null && quizRuleWhere.Count > 0)
                    {
                        sqlIn += " WHERE ";
                        for (int i = 0; i < quizRuleWhere.Count; i++)
                        {
                            sqlIn += "(";
                            bool addAnd = false;
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].Id))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `id` = @id" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@id" + i, quizRuleWhere[i].Id);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuizTypeId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRuleWhere[i].QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].LessonAddressedToId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `lesson_addressed_to_id` = @lessonAddressedToId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRuleWhere[i].LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuestionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `question_max_length` = @questionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRuleWhere[i].QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ResponseMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `response_max_length` = @responseMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRuleWhere[i].ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ExtraInfoMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `extra_info_max_length` = @extraInfoMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRuleWhere[i].ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].InstructionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `instruction_max_length` = @instructionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRuleWhere[i].InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MaxQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `max_questions` = @maxQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRuleWhere[i].MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MinQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `min_questions` = @minQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@minQuestions" + i, quizRuleWhere[i].MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].FalseResponseRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `false_response_required` = @falseResponseRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRuleWhere[i].FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].TitleMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `title_max_length` = @titleMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRuleWhere[i].TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CriterionRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `criterion_required` = @criterionRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRuleWhere[i].CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ReferenceRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `reference_required` = @referenceRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRuleWhere[i].ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].DeleteQuestionAllow))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `delete_question_allow` = @deleteQuestionAllow" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRuleWhere[i].DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].EditWarning))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `edit_warning` = @editWarning" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@editWarning" + i, quizRuleWhere[i].EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ModifiedBy))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `modified_by` = @modifiedBy" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRuleWhere[i].ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CreatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `created_at` = @createdAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@createdAt" + i, quizRuleWhere[i].CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].UpdatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `updated_at` = @updatedAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@updatedAt" + i, quizRuleWhere[i].UpdatedAt);
                            }
                            sqlIn += ")";
                            if (i != quizRuleWhere.Count - 1)
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
                                QuizRuleModel quizRule = new QuizRuleModel(); if (!reader.IsDBNull(0))
                                    quizRule.Id = reader.GetValue(0).ToString();
                                else
                                    quizRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                else
                                    quizRule.QuizTypeId = null;
                                if (!reader.IsDBNull(2))
                                    quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                else
                                    quizRule.LessonAddressedToId = null;
                                if (!reader.IsDBNull(3))
                                    quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                else
                                    quizRule.QuestionMaxLength = null;
                                if (!reader.IsDBNull(4))
                                    quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                else
                                    quizRule.ResponseMaxLength = null;
                                if (!reader.IsDBNull(5))
                                    quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                else
                                    quizRule.ExtraInfoMaxLength = null;
                                if (!reader.IsDBNull(6))
                                    quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                else
                                    quizRule.InstructionMaxLength = null;
                                if (!reader.IsDBNull(7))
                                    quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                else
                                    quizRule.MaxQuestions = null;
                                if (!reader.IsDBNull(8))
                                    quizRule.MinQuestions = reader.GetValue(8).ToString();
                                else
                                    quizRule.MinQuestions = null;
                                if (!reader.IsDBNull(9))
                                    quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                else
                                    quizRule.FalseResponseRequired = null;
                                if (!reader.IsDBNull(10))
                                    quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                else
                                    quizRule.TitleMaxLength = null;
                                if (!reader.IsDBNull(11))
                                    quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                else
                                    quizRule.CriterionRequired = null;
                                if (!reader.IsDBNull(12))
                                    quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                else
                                    quizRule.ReferenceRequired = null;
                                if (!reader.IsDBNull(13))
                                    quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                else
                                    quizRule.DeleteQuestionAllow = null;
                                if (!reader.IsDBNull(14))
                                    quizRule.EditWarning = reader.GetValue(14).ToString();
                                else
                                    quizRule.EditWarning = null;
                                if (!reader.IsDBNull(15))
                                    quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    quizRule.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    quizRule.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    quizRule.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    quizRule.UpdatedAt = null;
                                response.Add(quizRule.Id, quizRule);
                                quizRule = new QuizRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizRuleModel>();
                        reader.Close();
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                    response = new Dictionary<string, QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public List<QuizRuleModel> Read(string connectionString, QuizRuleModel quizRule, bool firstOne, bool orderByLastDate)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    {
                        if (quizRule != null)
                        {
                            if (!string.IsNullOrEmpty(quizRule.Id))
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
                                cmd.Parameters.AddWithValue("@id", quizRule.Id);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                                }
                                cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `lesson_addressed_to_id` = @lessonAddressedToId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `lesson_addressed_to_id` = @lessonAddressedToId ");
                                }
                                cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `question_max_length` = @questionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `question_max_length` = @questionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `response_max_length` = @responseMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `response_max_length` = @responseMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `extra_info_max_length` = @extraInfoMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `extra_info_max_length` = @extraInfoMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `instruction_max_length` = @instructionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `instruction_max_length` = @instructionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `max_questions` = @maxQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `max_questions` = @maxQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `min_questions` = @minQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `min_questions` = @minQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `false_response_required` = @falseResponseRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `false_response_required` = @falseResponseRequired ");
                                }
                                cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `title_max_length` = @titleMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `title_max_length` = @titleMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `criterion_required` = @criterionRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `criterion_required` = @criterionRequired ");
                                }
                                cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `reference_required` = @referenceRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `reference_required` = @referenceRequired ");
                                }
                                cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `delete_question_allow` = @deleteQuestionAllow ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `delete_question_allow` = @deleteQuestionAllow ");
                                }
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRule.EditWarning))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `edit_warning` = @editWarning ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `edit_warning` = @editWarning ");
                                }
                                cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
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
                                cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CreatedAt))
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
                                cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
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
                                cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                            }
                            if (orderByLastDate)
                                sql = string.Concat(sql, " ORDER BY updated_at DESC");
                            if (firstOne)
                                sql = string.Concat(sql, " LIMIT 1 ");
                        }
                        cmd.CommandText = sql;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                        quizRule.Id = reader.GetValue(0).ToString();
                                    else
                                        quizRule.Id = null;
                                    if (!reader.IsDBNull(1))
                                        quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                    else
                                        quizRule.QuizTypeId = null;
                                    if (!reader.IsDBNull(2))
                                        quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                    else
                                        quizRule.LessonAddressedToId = null;
                                    if (!reader.IsDBNull(3))
                                        quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                    else
                                        quizRule.QuestionMaxLength = null;
                                    if (!reader.IsDBNull(4))
                                        quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                    else
                                        quizRule.ResponseMaxLength = null;
                                    if (!reader.IsDBNull(5))
                                        quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                    else
                                        quizRule.ExtraInfoMaxLength = null;
                                    if (!reader.IsDBNull(6))
                                        quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                    else
                                        quizRule.InstructionMaxLength = null;
                                    if (!reader.IsDBNull(7))
                                        quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                    else
                                        quizRule.MaxQuestions = null;
                                    if (!reader.IsDBNull(8))
                                        quizRule.MinQuestions = reader.GetValue(8).ToString();
                                    else
                                        quizRule.MinQuestions = null;
                                    if (!reader.IsDBNull(9))
                                        quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                    else
                                        quizRule.FalseResponseRequired = null;
                                    if (!reader.IsDBNull(10))
                                        quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                    else
                                        quizRule.TitleMaxLength = null;
                                    if (!reader.IsDBNull(11))
                                        quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                    else
                                        quizRule.CriterionRequired = null;
                                    if (!reader.IsDBNull(12))
                                        quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                    else
                                        quizRule.ReferenceRequired = null;
                                    if (!reader.IsDBNull(13))
                                        quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                    else
                                        quizRule.DeleteQuestionAllow = null;
                                    if (!reader.IsDBNull(14))
                                        quizRule.EditWarning = reader.GetValue(14).ToString();
                                    else
                                        quizRule.EditWarning = null;
                                    if (!reader.IsDBNull(15))
                                        quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                    else
                                        quizRule.ModifiedBy = null;
                                    if (!reader.IsDBNull(16))
                                        quizRule.CreatedAt = reader.GetValue(16).ToString();
                                    else
                                        quizRule.CreatedAt = null;
                                    if (!reader.IsDBNull(17))
                                        quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                    else
                                        quizRule.UpdatedAt = null;
                                    response.Add(quizRule);
                                    quizRule = new QuizRuleModel();
                                }
                        }
                        catch (ArgumentException e)
                        {
                            string MessageString = " Read error occured: " + e.Message;
                            response = new List<QuizRuleModel>();
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
                    response = new List<QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public List<QuizRuleModel> Read(string connectionString, QuizRuleModel quizRule, bool orderByLastDate, int fromRow, int limit)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    {
                        if (quizRule != null)
                        {
                            if (!string.IsNullOrEmpty(quizRule.Id))
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
                                cmd.Parameters.AddWithValue("@id", quizRule.Id);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `quiz_type_id` = @quizTypeId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `quiz_type_id` = @quizTypeId ");
                                }
                                cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `lesson_addressed_to_id` = @lessonAddressedToId ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `lesson_addressed_to_id` = @lessonAddressedToId ");
                                }
                                cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `question_max_length` = @questionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `question_max_length` = @questionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `response_max_length` = @responseMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `response_max_length` = @responseMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `extra_info_max_length` = @extraInfoMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `extra_info_max_length` = @extraInfoMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `instruction_max_length` = @instructionMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `instruction_max_length` = @instructionMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `max_questions` = @maxQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `max_questions` = @maxQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `min_questions` = @minQuestions ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `min_questions` = @minQuestions ");
                                }
                                cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `false_response_required` = @falseResponseRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `false_response_required` = @falseResponseRequired ");
                                }
                                cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `title_max_length` = @titleMaxLength ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `title_max_length` = @titleMaxLength ");
                                }
                                cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `criterion_required` = @criterionRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `criterion_required` = @criterionRequired ");
                                }
                                cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `reference_required` = @referenceRequired ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `reference_required` = @referenceRequired ");
                                }
                                cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `delete_question_allow` = @deleteQuestionAllow ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `delete_question_allow` = @deleteQuestionAllow ");
                                }
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRule.EditWarning))
                            {
                                if (addAnd != true)
                                {
                                    sql = string.Concat(sql, " where `edit_warning` = @editWarning ");
                                    addAnd = true;
                                }
                                else
                                {
                                    sql = string.Concat(sql, " and `edit_warning` = @editWarning ");
                                }
                                cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
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
                                cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRule.CreatedAt))
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
                                cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
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
                                cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                            }
                            if (orderByLastDate)
                                sql = string.Concat(sql, " ORDER BY updated_at DESC");
                            sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                            cmd.Parameters.AddWithValue("@fromRow", fromRow);
                            cmd.Parameters.AddWithValue("@limit", limit);
                        }
                        cmd.CommandText = sql;
                        MySqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                        quizRule.Id = reader.GetValue(0).ToString();
                                    else
                                        quizRule.Id = null;
                                    if (!reader.IsDBNull(1))
                                        quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                    else
                                        quizRule.QuizTypeId = null;
                                    if (!reader.IsDBNull(2))
                                        quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                    else
                                        quizRule.LessonAddressedToId = null;
                                    if (!reader.IsDBNull(3))
                                        quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                    else
                                        quizRule.QuestionMaxLength = null;
                                    if (!reader.IsDBNull(4))
                                        quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                    else
                                        quizRule.ResponseMaxLength = null;
                                    if (!reader.IsDBNull(5))
                                        quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                    else
                                        quizRule.ExtraInfoMaxLength = null;
                                    if (!reader.IsDBNull(6))
                                        quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                    else
                                        quizRule.InstructionMaxLength = null;
                                    if (!reader.IsDBNull(7))
                                        quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                    else
                                        quizRule.MaxQuestions = null;
                                    if (!reader.IsDBNull(8))
                                        quizRule.MinQuestions = reader.GetValue(8).ToString();
                                    else
                                        quizRule.MinQuestions = null;
                                    if (!reader.IsDBNull(9))
                                        quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                    else
                                        quizRule.FalseResponseRequired = null;
                                    if (!reader.IsDBNull(10))
                                        quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                    else
                                        quizRule.TitleMaxLength = null;
                                    if (!reader.IsDBNull(11))
                                        quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                    else
                                        quizRule.CriterionRequired = null;
                                    if (!reader.IsDBNull(12))
                                        quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                    else
                                        quizRule.ReferenceRequired = null;
                                    if (!reader.IsDBNull(13))
                                        quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                    else
                                        quizRule.DeleteQuestionAllow = null;
                                    if (!reader.IsDBNull(14))
                                        quizRule.EditWarning = reader.GetValue(14).ToString();
                                    else
                                        quizRule.EditWarning = null;
                                    if (!reader.IsDBNull(15))
                                        quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                    else
                                        quizRule.ModifiedBy = null;
                                    if (!reader.IsDBNull(16))
                                        quizRule.CreatedAt = reader.GetValue(16).ToString();
                                    else
                                        quizRule.CreatedAt = null;
                                    if (!reader.IsDBNull(17))
                                        quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                    else
                                        quizRule.UpdatedAt = null;
                                    response.Add(quizRule);
                                    quizRule = new QuizRuleModel();
                                }
                        }
                        catch (ArgumentException e)
                        {
                            string MessageString = " Read error occured: " + e.Message;
                            response = new List<QuizRuleModel>();
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
                    response = new List<QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public List<QuizRuleModel> Read(string connectionString, List<QuizRuleModel> quizRuleWhere)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sqlIn = "";
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    if (quizRuleWhere != null && quizRuleWhere.Count > 0)
                    {
                        sqlIn += " WHERE ";
                        for (int i = 0; i < quizRuleWhere.Count; i++)
                        {
                            sqlIn += "(";
                            bool addAnd = false;
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].Id))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `id` = @id" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@id" + i, quizRuleWhere[i].Id);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuizTypeId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRuleWhere[i].QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].LessonAddressedToId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `lesson_addressed_to_id` = @lessonAddressedToId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRuleWhere[i].LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuestionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `question_max_length` = @questionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRuleWhere[i].QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ResponseMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `response_max_length` = @responseMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRuleWhere[i].ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ExtraInfoMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `extra_info_max_length` = @extraInfoMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRuleWhere[i].ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].InstructionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `instruction_max_length` = @instructionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRuleWhere[i].InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MaxQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `max_questions` = @maxQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRuleWhere[i].MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MinQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `min_questions` = @minQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@minQuestions" + i, quizRuleWhere[i].MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].FalseResponseRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `false_response_required` = @falseResponseRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRuleWhere[i].FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].TitleMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `title_max_length` = @titleMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRuleWhere[i].TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CriterionRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `criterion_required` = @criterionRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRuleWhere[i].CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ReferenceRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `reference_required` = @referenceRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRuleWhere[i].ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].DeleteQuestionAllow))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `delete_question_allow` = @deleteQuestionAllow" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRuleWhere[i].DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].EditWarning))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `edit_warning` = @editWarning" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@editWarning" + i, quizRuleWhere[i].EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ModifiedBy))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `modified_by` = @modifiedBy" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRuleWhere[i].ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CreatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `created_at` = @createdAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@createdAt" + i, quizRuleWhere[i].CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].UpdatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `updated_at` = @updatedAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@updatedAt" + i, quizRuleWhere[i].UpdatedAt);
                            }
                            sqlIn += ")";
                            if (i != quizRuleWhere.Count - 1)
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
                                QuizRuleModel quizRule = new QuizRuleModel(); if (!reader.IsDBNull(0))
                                    quizRule.Id = reader.GetValue(0).ToString();
                                else
                                    quizRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                else
                                    quizRule.QuizTypeId = null;
                                if (!reader.IsDBNull(2))
                                    quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                else
                                    quizRule.LessonAddressedToId = null;
                                if (!reader.IsDBNull(3))
                                    quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                else
                                    quizRule.QuestionMaxLength = null;
                                if (!reader.IsDBNull(4))
                                    quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                else
                                    quizRule.ResponseMaxLength = null;
                                if (!reader.IsDBNull(5))
                                    quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                else
                                    quizRule.ExtraInfoMaxLength = null;
                                if (!reader.IsDBNull(6))
                                    quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                else
                                    quizRule.InstructionMaxLength = null;
                                if (!reader.IsDBNull(7))
                                    quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                else
                                    quizRule.MaxQuestions = null;
                                if (!reader.IsDBNull(8))
                                    quizRule.MinQuestions = reader.GetValue(8).ToString();
                                else
                                    quizRule.MinQuestions = null;
                                if (!reader.IsDBNull(9))
                                    quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                else
                                    quizRule.FalseResponseRequired = null;
                                if (!reader.IsDBNull(10))
                                    quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                else
                                    quizRule.TitleMaxLength = null;
                                if (!reader.IsDBNull(11))
                                    quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                else
                                    quizRule.CriterionRequired = null;
                                if (!reader.IsDBNull(12))
                                    quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                else
                                    quizRule.ReferenceRequired = null;
                                if (!reader.IsDBNull(13))
                                    quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                else
                                    quizRule.DeleteQuestionAllow = null;
                                if (!reader.IsDBNull(14))
                                    quizRule.EditWarning = reader.GetValue(14).ToString();
                                else
                                    quizRule.EditWarning = null;
                                if (!reader.IsDBNull(15))
                                    quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    quizRule.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    quizRule.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    quizRule.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    quizRule.UpdatedAt = null;
                                response.Add(quizRule);
                                quizRule = new QuizRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizRuleModel>();
                        reader.Close();
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public List<QuizRuleModel> Read(string connectionString, List<QuizRuleModel> quizRuleWhere, int fromRow, int limit)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sqlIn = "";
                    string sql = "SELECT `id`, `quiz_type_id`, `lesson_addressed_to_id`, `question_max_length`, `response_max_length`, `extra_info_max_length`, `instruction_max_length`, `max_questions`, `min_questions`, `false_response_required`, `title_max_length`, `criterion_required`, `reference_required`, `delete_question_allow`, `edit_warning`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rules ";
                    if (quizRuleWhere != null && quizRuleWhere.Count > 0)
                    {
                        sqlIn += " WHERE ";
                        for (int i = 0; i < quizRuleWhere.Count; i++)
                        {
                            sqlIn += "(";
                            bool addAnd = false;
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].Id))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `id` = @id" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@id" + i, quizRuleWhere[i].Id);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuizTypeId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `quiz_type_id` = @quizTypeId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRuleWhere[i].QuizTypeId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].LessonAddressedToId))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `lesson_addressed_to_id` = @lessonAddressedToId" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRuleWhere[i].LessonAddressedToId);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].QuestionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `question_max_length` = @questionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRuleWhere[i].QuestionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ResponseMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `response_max_length` = @responseMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRuleWhere[i].ResponseMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ExtraInfoMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `extra_info_max_length` = @extraInfoMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRuleWhere[i].ExtraInfoMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].InstructionMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `instruction_max_length` = @instructionMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRuleWhere[i].InstructionMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MaxQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `max_questions` = @maxQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRuleWhere[i].MaxQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].MinQuestions))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `min_questions` = @minQuestions" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@minQuestions" + i, quizRuleWhere[i].MinQuestions);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].FalseResponseRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `false_response_required` = @falseResponseRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRuleWhere[i].FalseResponseRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].TitleMaxLength))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `title_max_length` = @titleMaxLength" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRuleWhere[i].TitleMaxLength);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CriterionRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `criterion_required` = @criterionRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRuleWhere[i].CriterionRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ReferenceRequired))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `reference_required` = @referenceRequired" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRuleWhere[i].ReferenceRequired);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].DeleteQuestionAllow))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `delete_question_allow` = @deleteQuestionAllow" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRuleWhere[i].DeleteQuestionAllow);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].EditWarning))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `edit_warning` = @editWarning" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@editWarning" + i, quizRuleWhere[i].EditWarning);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].ModifiedBy))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `modified_by` = @modifiedBy" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRuleWhere[i].ModifiedBy);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].CreatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `created_at` = @createdAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@createdAt" + i, quizRuleWhere[i].CreatedAt);
                            }
                            if (!string.IsNullOrEmpty(quizRuleWhere[i].UpdatedAt))
                            {
                                if (addAnd)
                                    sqlIn += " AND ";

                                sqlIn += " `updated_at` = @updatedAt" + i;
                                addAnd = true;
                                cmd.Parameters.AddWithValue("@updatedAt" + i, quizRuleWhere[i].UpdatedAt);
                            }
                            sqlIn += ")";
                            if (i != quizRuleWhere.Count - 1)
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
                                QuizRuleModel quizRule = new QuizRuleModel(); if (!reader.IsDBNull(0))
                                    quizRule.Id = reader.GetValue(0).ToString();
                                else
                                    quizRule.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRule.QuizTypeId = reader.GetValue(1).ToString();
                                else
                                    quizRule.QuizTypeId = null;
                                if (!reader.IsDBNull(2))
                                    quizRule.LessonAddressedToId = reader.GetValue(2).ToString();
                                else
                                    quizRule.LessonAddressedToId = null;
                                if (!reader.IsDBNull(3))
                                    quizRule.QuestionMaxLength = reader.GetValue(3).ToString();
                                else
                                    quizRule.QuestionMaxLength = null;
                                if (!reader.IsDBNull(4))
                                    quizRule.ResponseMaxLength = reader.GetValue(4).ToString();
                                else
                                    quizRule.ResponseMaxLength = null;
                                if (!reader.IsDBNull(5))
                                    quizRule.ExtraInfoMaxLength = reader.GetValue(5).ToString();
                                else
                                    quizRule.ExtraInfoMaxLength = null;
                                if (!reader.IsDBNull(6))
                                    quizRule.InstructionMaxLength = reader.GetValue(6).ToString();
                                else
                                    quizRule.InstructionMaxLength = null;
                                if (!reader.IsDBNull(7))
                                    quizRule.MaxQuestions = reader.GetValue(7).ToString();
                                else
                                    quizRule.MaxQuestions = null;
                                if (!reader.IsDBNull(8))
                                    quizRule.MinQuestions = reader.GetValue(8).ToString();
                                else
                                    quizRule.MinQuestions = null;
                                if (!reader.IsDBNull(9))
                                    quizRule.FalseResponseRequired = reader.GetValue(9).ToString();
                                else
                                    quizRule.FalseResponseRequired = null;
                                if (!reader.IsDBNull(10))
                                    quizRule.TitleMaxLength = reader.GetValue(10).ToString();
                                else
                                    quizRule.TitleMaxLength = null;
                                if (!reader.IsDBNull(11))
                                    quizRule.CriterionRequired = reader.GetValue(11).ToString();
                                else
                                    quizRule.CriterionRequired = null;
                                if (!reader.IsDBNull(12))
                                    quizRule.ReferenceRequired = reader.GetValue(12).ToString();
                                else
                                    quizRule.ReferenceRequired = null;
                                if (!reader.IsDBNull(13))
                                    quizRule.DeleteQuestionAllow = reader.GetValue(13).ToString();
                                else
                                    quizRule.DeleteQuestionAllow = null;
                                if (!reader.IsDBNull(14))
                                    quizRule.EditWarning = reader.GetValue(14).ToString();
                                else
                                    quizRule.EditWarning = null;
                                if (!reader.IsDBNull(15))
                                    quizRule.ModifiedBy = reader.GetValue(15).ToString();
                                else
                                    quizRule.ModifiedBy = null;
                                if (!reader.IsDBNull(16))
                                    quizRule.CreatedAt = reader.GetValue(16).ToString();
                                else
                                    quizRule.CreatedAt = null;
                                if (!reader.IsDBNull(17))
                                    quizRule.UpdatedAt = reader.GetValue(17).ToString();
                                else
                                    quizRule.UpdatedAt = null;
                                response.Add(quizRule);
                                quizRule = new QuizRuleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizRuleModel>();
                        reader.Close();
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
                }
                finally
                {
                    if (_connection != null)
                        dbConn.CloseConnection(_connection);
                }
                return response;
            }
            public List<QuizRuleModel> Create(string connectionString, QuizRuleModel quizRule)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "INSERT INTO quiz_rules (";
                    string values = "(";
                    if (!string.IsNullOrEmpty(quizRule.Id))
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
                        cmd.Parameters.AddWithValue("@id", quizRule.Id);
                    }
                    if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `quiz_type_id` ");
                            values = string.Concat(values, ", @quizTypeId");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `quiz_type_id` ");
                            values = string.Concat(values, " @quizTypeId");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                    }
                    if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `lesson_addressed_to_id` ");
                            values = string.Concat(values, ", @lessonAddressedToId");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `lesson_addressed_to_id` ");
                            values = string.Concat(values, " @lessonAddressedToId");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                    }
                    if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `question_max_length` ");
                            values = string.Concat(values, ", @questionMaxLength");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `question_max_length` ");
                            values = string.Concat(values, " @questionMaxLength");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `response_max_length` ");
                            values = string.Concat(values, ", @responseMaxLength");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `response_max_length` ");
                            values = string.Concat(values, " @responseMaxLength");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `extra_info_max_length` ");
                            values = string.Concat(values, ", @extraInfoMaxLength");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `extra_info_max_length` ");
                            values = string.Concat(values, " @extraInfoMaxLength");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `instruction_max_length` ");
                            values = string.Concat(values, ", @instructionMaxLength");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `instruction_max_length` ");
                            values = string.Concat(values, " @instructionMaxLength");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `max_questions` ");
                            values = string.Concat(values, ", @maxQuestions");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `max_questions` ");
                            values = string.Concat(values, " @maxQuestions");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                    }
                    if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `min_questions` ");
                            values = string.Concat(values, ", @minQuestions");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `min_questions` ");
                            values = string.Concat(values, " @minQuestions");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                    }
                    if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `false_response_required` ");
                            values = string.Concat(values, ", @falseResponseRequired");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `false_response_required` ");
                            values = string.Concat(values, " @falseResponseRequired");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                    }
                    if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `title_max_length` ");
                            values = string.Concat(values, ", @titleMaxLength");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `title_max_length` ");
                            values = string.Concat(values, " @titleMaxLength");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `criterion_required` ");
                            values = string.Concat(values, ", @criterionRequired");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `criterion_required` ");
                            values = string.Concat(values, " @criterionRequired");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `reference_required` ");
                            values = string.Concat(values, ", @referenceRequired");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `reference_required` ");
                            values = string.Concat(values, " @referenceRequired");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                    }
                    if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `delete_question_allow` ");
                            values = string.Concat(values, ", @deleteQuestionAllow");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `delete_question_allow` ");
                            values = string.Concat(values, " @deleteQuestionAllow");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                    }
                    if (!string.IsNullOrEmpty(quizRule.EditWarning))
                    {
                        if (addAnd)
                        {
                            sql = string.Concat(sql, ", `edit_warning` ");
                            values = string.Concat(values, ", @editWarning");
                        }
                        else
                        {
                            sql = string.Concat(sql, " `edit_warning` ");
                            values = string.Concat(values, " @editWarning");
                        }
                        addAnd = true;
                        cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
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
                        cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizRule.CreatedAt))
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
                        cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
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
                        cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                    }
                    sql = string.Concat(sql, ")");
                    values = string.Concat(values, ")");
                    cmd.CommandText = string.Concat(sql, " VALUES ", values);
                    cmd.ExecuteNonQuery();
                    quizRule.Id = cmd.LastInsertedId.ToString();
                    response.Add(quizRule);
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
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
            public List<QuizRuleModel> Create(string connectionString, List<QuizRuleModel> quizRuleList)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
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
                            for (int i = 0; i < quizRuleList.Count; i++)
                            {
                                bool addAnd = false;
                                try
                                {
                                    string sql = "INSERT INTO quiz_rules (";
                                    string values = "(";
                                    if (!string.IsNullOrEmpty(quizRuleList[i].Id))
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
                                        cmd.Parameters.AddWithValue("@id" + i, quizRuleList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].QuizTypeId))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `quiz_type_id` ");
                                            values = string.Concat(values, ", @quizTypeId" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `quiz_type_id` ");
                                            values = string.Concat(values, "@quizTypeId" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRuleList[i].QuizTypeId);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].LessonAddressedToId))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `lesson_addressed_to_id` ");
                                            values = string.Concat(values, ", @lessonAddressedToId" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `lesson_addressed_to_id` ");
                                            values = string.Concat(values, "@lessonAddressedToId" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRuleList[i].LessonAddressedToId);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].QuestionMaxLength))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `question_max_length` ");
                                            values = string.Concat(values, ", @questionMaxLength" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `question_max_length` ");
                                            values = string.Concat(values, "@questionMaxLength" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRuleList[i].QuestionMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ResponseMaxLength))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `response_max_length` ");
                                            values = string.Concat(values, ", @responseMaxLength" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `response_max_length` ");
                                            values = string.Concat(values, "@responseMaxLength" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRuleList[i].ResponseMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ExtraInfoMaxLength))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `extra_info_max_length` ");
                                            values = string.Concat(values, ", @extraInfoMaxLength" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `extra_info_max_length` ");
                                            values = string.Concat(values, "@extraInfoMaxLength" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRuleList[i].ExtraInfoMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].InstructionMaxLength))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `instruction_max_length` ");
                                            values = string.Concat(values, ", @instructionMaxLength" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `instruction_max_length` ");
                                            values = string.Concat(values, "@instructionMaxLength" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRuleList[i].InstructionMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].MaxQuestions))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `max_questions` ");
                                            values = string.Concat(values, ", @maxQuestions" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `max_questions` ");
                                            values = string.Concat(values, "@maxQuestions" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRuleList[i].MaxQuestions);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].MinQuestions))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `min_questions` ");
                                            values = string.Concat(values, ", @minQuestions" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `min_questions` ");
                                            values = string.Concat(values, "@minQuestions" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@minQuestions" + i, quizRuleList[i].MinQuestions);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].FalseResponseRequired))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `false_response_required` ");
                                            values = string.Concat(values, ", @falseResponseRequired" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `false_response_required` ");
                                            values = string.Concat(values, "@falseResponseRequired" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRuleList[i].FalseResponseRequired);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].TitleMaxLength))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `title_max_length` ");
                                            values = string.Concat(values, ", @titleMaxLength" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `title_max_length` ");
                                            values = string.Concat(values, "@titleMaxLength" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRuleList[i].TitleMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].CriterionRequired))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `criterion_required` ");
                                            values = string.Concat(values, ", @criterionRequired" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `criterion_required` ");
                                            values = string.Concat(values, "@criterionRequired" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRuleList[i].CriterionRequired);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ReferenceRequired))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `reference_required` ");
                                            values = string.Concat(values, ", @referenceRequired" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `reference_required` ");
                                            values = string.Concat(values, "@referenceRequired" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRuleList[i].ReferenceRequired);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].DeleteQuestionAllow))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `delete_question_allow` ");
                                            values = string.Concat(values, ", @deleteQuestionAllow" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `delete_question_allow` ");
                                            values = string.Concat(values, "@deleteQuestionAllow" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRuleList[i].DeleteQuestionAllow);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].EditWarning))
                                    {
                                        if (addAnd)
                                        {
                                            sql = string.Concat(sql, ", `edit_warning` ");
                                            values = string.Concat(values, ", @editWarning" + i);
                                        }

                                        else
                                        {
                                            sql = string.Concat(sql, " `edit_warning` ");
                                            values = string.Concat(values, "@editWarning" + i);
                                        }
                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@editWarning" + i, quizRuleList[i].EditWarning);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ModifiedBy))
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
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRuleList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].CreatedAt))
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
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizRuleList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].UpdatedAt))
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
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizRuleList[i].UpdatedAt);
                                    }
                                    sql = string.Concat(sql, ")");
                                    values = string.Concat(values, ")");
                                    cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                    cmd.ExecuteNonQuery();
                                    response.Add(quizRuleList[i]);
                                }
                                catch (MySqlException e)
                                {
                                    string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                    response = new List<QuizRuleModel>();
                                }
                            }
                            trans.Commit();
                        }
                        catch (MySqlException e)
                        {
                            string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                            trans.Rollback();
                            response = new List<QuizRuleModel>();
                        }
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
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

            public List<QuizRuleModel> Update(string connectionString, QuizRuleModel quizRule, QuizRuleModel quizRuleWhere)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "UPDATE quiz_rules SET ";
                    if (!string.IsNullOrEmpty(quizRule.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizRule.Id);
                    }
                    if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `quiz_type_id` = @quizTypeId");
                        else
                            sql = string.Concat(sql, " `quiz_type_id` = @quizTypeId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                    }
                    if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `lesson_addressed_to_id` = @lessonAddressedToId");
                        else
                            sql = string.Concat(sql, " `lesson_addressed_to_id` = @lessonAddressedToId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                    }
                    if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `question_max_length` = @questionMaxLength");
                        else
                            sql = string.Concat(sql, " `question_max_length` = @questionMaxLength");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `response_max_length` = @responseMaxLength");
                        else
                            sql = string.Concat(sql, " `response_max_length` = @responseMaxLength");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `extra_info_max_length` = @extraInfoMaxLength");
                        else
                            sql = string.Concat(sql, " `extra_info_max_length` = @extraInfoMaxLength");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `instruction_max_length` = @instructionMaxLength");
                        else
                            sql = string.Concat(sql, " `instruction_max_length` = @instructionMaxLength");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `max_questions` = @maxQuestions");
                        else
                            sql = string.Concat(sql, " `max_questions` = @maxQuestions");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                    }
                    if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `min_questions` = @minQuestions");
                        else
                            sql = string.Concat(sql, " `min_questions` = @minQuestions");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                    }
                    if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `false_response_required` = @falseResponseRequired");
                        else
                            sql = string.Concat(sql, " `false_response_required` = @falseResponseRequired");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                    }
                    if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `title_max_length` = @titleMaxLength");
                        else
                            sql = string.Concat(sql, " `title_max_length` = @titleMaxLength");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                    }
                    if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `criterion_required` = @criterionRequired");
                        else
                            sql = string.Concat(sql, " `criterion_required` = @criterionRequired");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `reference_required` = @referenceRequired");
                        else
                            sql = string.Concat(sql, " `reference_required` = @referenceRequired");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                    }
                    if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `delete_question_allow` = @deleteQuestionAllow");
                        else
                            sql = string.Concat(sql, " `delete_question_allow` = @deleteQuestionAllow");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                    }
                    if (!string.IsNullOrEmpty(quizRule.EditWarning))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `edit_warning` = @editWarning");
                        else
                            sql = string.Concat(sql, " `edit_warning` = @editWarning");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                    }
                    if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizRule.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                    }
                    if (quizRuleWhere != null)
                    {
                        sql = string.Concat(sql, " WHERE ");
                        addAnd = false; //Reset the addAnd for the WHERES
                        if (!string.IsNullOrEmpty(quizRuleWhere.Id))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `id` = @idWhere");
                            else
                                sql = string.Concat(sql, " `id` = @idWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@idWhere", quizRuleWhere.Id);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.QuizTypeId))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `quiz_type_id` = @quizTypeIdWhere");
                            else
                                sql = string.Concat(sql, " `quiz_type_id` = @quizTypeIdWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizTypeIdWhere", quizRuleWhere.QuizTypeId);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.LessonAddressedToId))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `lesson_addressed_to_id` = @lessonAddressedToIdWhere");
                            else
                                sql = string.Concat(sql, " `lesson_addressed_to_id` = @lessonAddressedToIdWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonAddressedToIdWhere", quizRuleWhere.LessonAddressedToId);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.QuestionMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `question_max_length` = @questionMaxLengthWhere");
                            else
                                sql = string.Concat(sql, " `question_max_length` = @questionMaxLengthWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionMaxLengthWhere", quizRuleWhere.QuestionMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.ResponseMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `response_max_length` = @responseMaxLengthWhere");
                            else
                                sql = string.Concat(sql, " `response_max_length` = @responseMaxLengthWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseMaxLengthWhere", quizRuleWhere.ResponseMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.ExtraInfoMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `extra_info_max_length` = @extraInfoMaxLengthWhere");
                            else
                                sql = string.Concat(sql, " `extra_info_max_length` = @extraInfoMaxLengthWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@extraInfoMaxLengthWhere", quizRuleWhere.ExtraInfoMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.InstructionMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `instruction_max_length` = @instructionMaxLengthWhere");
                            else
                                sql = string.Concat(sql, " `instruction_max_length` = @instructionMaxLengthWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instructionMaxLengthWhere", quizRuleWhere.InstructionMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.MaxQuestions))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `max_questions` = @maxQuestionsWhere");
                            else
                                sql = string.Concat(sql, " `max_questions` = @maxQuestionsWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxQuestionsWhere", quizRuleWhere.MaxQuestions);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.MinQuestions))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `min_questions` = @minQuestionsWhere");
                            else
                                sql = string.Concat(sql, " `min_questions` = @minQuestionsWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@minQuestionsWhere", quizRuleWhere.MinQuestions);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.FalseResponseRequired))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `false_response_required` = @falseResponseRequiredWhere");
                            else
                                sql = string.Concat(sql, " `false_response_required` = @falseResponseRequiredWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@falseResponseRequiredWhere", quizRuleWhere.FalseResponseRequired);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.TitleMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `title_max_length` = @titleMaxLengthWhere");
                            else
                                sql = string.Concat(sql, " `title_max_length` = @titleMaxLengthWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@titleMaxLengthWhere", quizRuleWhere.TitleMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.CriterionRequired))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `criterion_required` = @criterionRequiredWhere");
                            else
                                sql = string.Concat(sql, " `criterion_required` = @criterionRequiredWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@criterionRequiredWhere", quizRuleWhere.CriterionRequired);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.ReferenceRequired))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `reference_required` = @referenceRequiredWhere");
                            else
                                sql = string.Concat(sql, " `reference_required` = @referenceRequiredWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@referenceRequiredWhere", quizRuleWhere.ReferenceRequired);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.DeleteQuestionAllow))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `delete_question_allow` = @deleteQuestionAllowWhere");
                            else
                                sql = string.Concat(sql, " `delete_question_allow` = @deleteQuestionAllowWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@deleteQuestionAllowWhere", quizRuleWhere.DeleteQuestionAllow);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.EditWarning))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `edit_warning` = @editWarningWhere");
                            else
                                sql = string.Concat(sql, " `edit_warning` = @editWarningWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@editWarningWhere", quizRuleWhere.EditWarning);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.ModifiedBy))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                            else
                                sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhere", quizRuleWhere.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.CreatedAt))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                            else
                                sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAtWhere", quizRuleWhere.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizRuleWhere.UpdatedAt))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                            else
                                sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhere", quizRuleWhere.UpdatedAt);
                        }
                    }
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    response.Add(quizRule);
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
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

            public List<QuizRuleModel> Update(string connectionString, List<QuizRuleModel> quizRuleList, List<QuizRuleModel> quizRuleWhereList)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
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
                            for (int i = 0; i < quizRuleList.Count; i++)
                            {
                                try
                                {
                                    bool addAnd = false;

                                    string sql = "UPDATE quiz_rules SET ";
                                    if (!string.IsNullOrEmpty(quizRuleList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizRuleList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].QuizTypeId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `quiz_type_id` = @quizTypeId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_type_id` = @quizTypeId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRuleList[i].QuizTypeId);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].LessonAddressedToId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `lesson_addressed_to_id` = @lessonAddressedToId" + i);
                                        else
                                            sql = string.Concat(sql, " `lesson_addressed_to_id` = @lessonAddressedToId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRuleList[i].LessonAddressedToId);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].QuestionMaxLength))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `question_max_length` = @questionMaxLength" + i);
                                        else
                                            sql = string.Concat(sql, " `question_max_length` = @questionMaxLength" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRuleList[i].QuestionMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ResponseMaxLength))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `response_max_length` = @responseMaxLength" + i);
                                        else
                                            sql = string.Concat(sql, " `response_max_length` = @responseMaxLength" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRuleList[i].ResponseMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ExtraInfoMaxLength))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `extra_info_max_length` = @extraInfoMaxLength" + i);
                                        else
                                            sql = string.Concat(sql, " `extra_info_max_length` = @extraInfoMaxLength" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRuleList[i].ExtraInfoMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].InstructionMaxLength))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `instruction_max_length` = @instructionMaxLength" + i);
                                        else
                                            sql = string.Concat(sql, " `instruction_max_length` = @instructionMaxLength" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRuleList[i].InstructionMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].MaxQuestions))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `max_questions` = @maxQuestions" + i);
                                        else
                                            sql = string.Concat(sql, " `max_questions` = @maxQuestions" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRuleList[i].MaxQuestions);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].MinQuestions))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `min_questions` = @minQuestions" + i);
                                        else
                                            sql = string.Concat(sql, " `min_questions` = @minQuestions" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@minQuestions" + i, quizRuleList[i].MinQuestions);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].FalseResponseRequired))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `false_response_required` = @falseResponseRequired" + i);
                                        else
                                            sql = string.Concat(sql, " `false_response_required` = @falseResponseRequired" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRuleList[i].FalseResponseRequired);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].TitleMaxLength))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `title_max_length` = @titleMaxLength" + i);
                                        else
                                            sql = string.Concat(sql, " `title_max_length` = @titleMaxLength" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRuleList[i].TitleMaxLength);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].CriterionRequired))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `criterion_required` = @criterionRequired" + i);
                                        else
                                            sql = string.Concat(sql, " `criterion_required` = @criterionRequired" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRuleList[i].CriterionRequired);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ReferenceRequired))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `reference_required` = @referenceRequired" + i);
                                        else
                                            sql = string.Concat(sql, " `reference_required` = @referenceRequired" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRuleList[i].ReferenceRequired);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].DeleteQuestionAllow))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `delete_question_allow` = @deleteQuestionAllow" + i);
                                        else
                                            sql = string.Concat(sql, " `delete_question_allow` = @deleteQuestionAllow" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRuleList[i].DeleteQuestionAllow);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].EditWarning))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `edit_warning` = @editWarning" + i);
                                        else
                                            sql = string.Concat(sql, " `edit_warning` = @editWarning" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@editWarning" + i, quizRuleList[i].EditWarning);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRuleList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizRuleList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizRuleList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizRuleList[i].UpdatedAt);
                                    }
                                    if (quizRuleWhereList[i] != null)
                                    {
                                        sql = string.Concat(sql, " WHERE ");
                                        addAnd = false; // Reset addAnd for the Where
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].Id))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `id` = @idWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@idWhere" + i, quizRuleWhereList[i].Id);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].QuizTypeId))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `quiz_type_id` = @quizTypeIdWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `quiz_type_id` = @quizTypeIdWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@quizTypeIdWhere" + i, quizRuleWhereList[i].QuizTypeId);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].LessonAddressedToId))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `lesson_addressed_to_id` = @lessonAddressedToIdWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `lesson_addressed_to_id` = @lessonAddressedToIdWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@lessonAddressedToIdWhere" + i, quizRuleWhereList[i].LessonAddressedToId);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].QuestionMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `question_max_length` = @questionMaxLengthWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `question_max_length` = @questionMaxLengthWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@questionMaxLengthWhere" + i, quizRuleWhereList[i].QuestionMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].ResponseMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `response_max_length` = @responseMaxLengthWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `response_max_length` = @responseMaxLengthWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@responseMaxLengthWhere" + i, quizRuleWhereList[i].ResponseMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].ExtraInfoMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `extra_info_max_length` = @extraInfoMaxLengthWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `extra_info_max_length` = @extraInfoMaxLengthWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@extraInfoMaxLengthWhere" + i, quizRuleWhereList[i].ExtraInfoMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].InstructionMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `instruction_max_length` = @instructionMaxLengthWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `instruction_max_length` = @instructionMaxLengthWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@instructionMaxLengthWhere" + i, quizRuleWhereList[i].InstructionMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].MaxQuestions))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `max_questions` = @maxQuestionsWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `max_questions` = @maxQuestionsWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@maxQuestionsWhere" + i, quizRuleWhereList[i].MaxQuestions);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].MinQuestions))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `min_questions` = @minQuestionsWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `min_questions` = @minQuestionsWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@minQuestionsWhere" + i, quizRuleWhereList[i].MinQuestions);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].FalseResponseRequired))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `false_response_required` = @falseResponseRequiredWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `false_response_required` = @falseResponseRequiredWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@falseResponseRequiredWhere" + i, quizRuleWhereList[i].FalseResponseRequired);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].TitleMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `title_max_length` = @titleMaxLengthWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `title_max_length` = @titleMaxLengthWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@titleMaxLengthWhere" + i, quizRuleWhereList[i].TitleMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].CriterionRequired))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `criterion_required` = @criterionRequiredWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `criterion_required` = @criterionRequiredWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@criterionRequiredWhere" + i, quizRuleWhereList[i].CriterionRequired);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].ReferenceRequired))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `reference_required` = @referenceRequiredWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `reference_required` = @referenceRequiredWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@referenceRequiredWhere" + i, quizRuleWhereList[i].ReferenceRequired);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].DeleteQuestionAllow))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `delete_question_allow` = @deleteQuestionAllowWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `delete_question_allow` = @deleteQuestionAllowWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@deleteQuestionAllowWhere" + i, quizRuleWhereList[i].DeleteQuestionAllow);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].EditWarning))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `edit_warning` = @editWarningWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `edit_warning` = @editWarningWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@editWarningWhere" + i, quizRuleWhereList[i].EditWarning);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].ModifiedBy))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizRuleWhereList[i].ModifiedBy);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].CreatedAt))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizRuleWhereList[i].CreatedAt);
                                        }
                                        if (!string.IsNullOrEmpty(quizRuleWhereList[i].UpdatedAt))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                            else
                                                sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizRuleWhereList[i].UpdatedAt);
                                        }
                                    }
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                    response.Add(quizRuleList[i]);
                                }
                                catch (MySqlException e)
                                {
                                    string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                    response = new List<QuizRuleModel>();
                                }
                            }
                            trans.Commit();
                        }
                        catch (MySqlException e)
                        {
                            string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                            trans.Rollback();
                            response = new List<QuizRuleModel>();
                        }
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
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

            public List<QuizRuleModel> Delete(string connectionString, QuizRuleModel quizRule)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
                bool addAnd = false;
                try
                {
                    _connection = dbConn.GetConnection();
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = _connection
                    };
                    string sql = "DELETE FROM quiz_rules  ";
                    if (quizRule != null)
                    {
                        sql += " WHERE ";
                        if (!string.IsNullOrEmpty(quizRule.Id))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `id` = @id");
                            else
                                sql = string.Concat(sql, " `id` = @id");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id", quizRule.Id);
                        }
                        if (!string.IsNullOrEmpty(quizRule.QuizTypeId))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `quiz_type_id` = @quizTypeId");
                            else
                                sql = string.Concat(sql, " `quiz_type_id` = @quizTypeId");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizTypeId", quizRule.QuizTypeId);
                        }
                        if (!string.IsNullOrEmpty(quizRule.LessonAddressedToId))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `lesson_addressed_to_id` = @lessonAddressedToId");
                            else
                                sql = string.Concat(sql, " `lesson_addressed_to_id` = @lessonAddressedToId");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonAddressedToId", quizRule.LessonAddressedToId);
                        }
                        if (!string.IsNullOrEmpty(quizRule.QuestionMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `question_max_length` = @questionMaxLength");
                            else
                                sql = string.Concat(sql, " `question_max_length` = @questionMaxLength");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionMaxLength", quizRule.QuestionMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRule.ResponseMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `response_max_length` = @responseMaxLength");
                            else
                                sql = string.Concat(sql, " `response_max_length` = @responseMaxLength");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseMaxLength", quizRule.ResponseMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRule.ExtraInfoMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `extra_info_max_length` = @extraInfoMaxLength");
                            else
                                sql = string.Concat(sql, " `extra_info_max_length` = @extraInfoMaxLength");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@extraInfoMaxLength", quizRule.ExtraInfoMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRule.InstructionMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `instruction_max_length` = @instructionMaxLength");
                            else
                                sql = string.Concat(sql, " `instruction_max_length` = @instructionMaxLength");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@instructionMaxLength", quizRule.InstructionMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRule.MaxQuestions))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `max_questions` = @maxQuestions");
                            else
                                sql = string.Concat(sql, " `max_questions` = @maxQuestions");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@maxQuestions", quizRule.MaxQuestions);
                        }
                        if (!string.IsNullOrEmpty(quizRule.MinQuestions))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `min_questions` = @minQuestions");
                            else
                                sql = string.Concat(sql, " `min_questions` = @minQuestions");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@minQuestions", quizRule.MinQuestions);
                        }
                        if (!string.IsNullOrEmpty(quizRule.FalseResponseRequired))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `false_response_required` = @falseResponseRequired");
                            else
                                sql = string.Concat(sql, " `false_response_required` = @falseResponseRequired");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@falseResponseRequired", quizRule.FalseResponseRequired);
                        }
                        if (!string.IsNullOrEmpty(quizRule.TitleMaxLength))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `title_max_length` = @titleMaxLength");
                            else
                                sql = string.Concat(sql, " `title_max_length` = @titleMaxLength");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@titleMaxLength", quizRule.TitleMaxLength);
                        }
                        if (!string.IsNullOrEmpty(quizRule.CriterionRequired))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `criterion_required` = @criterionRequired");
                            else
                                sql = string.Concat(sql, " `criterion_required` = @criterionRequired");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@criterionRequired", quizRule.CriterionRequired);
                        }
                        if (!string.IsNullOrEmpty(quizRule.ReferenceRequired))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `reference_required` = @referenceRequired");
                            else
                                sql = string.Concat(sql, " `reference_required` = @referenceRequired");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@referenceRequired", quizRule.ReferenceRequired);
                        }
                        if (!string.IsNullOrEmpty(quizRule.DeleteQuestionAllow))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `delete_question_allow` = @deleteQuestionAllow");
                            else
                                sql = string.Concat(sql, " `delete_question_allow` = @deleteQuestionAllow");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@deleteQuestionAllow", quizRule.DeleteQuestionAllow);
                        }
                        if (!string.IsNullOrEmpty(quizRule.EditWarning))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `edit_warning` = @editWarning");
                            else
                                sql = string.Concat(sql, " `edit_warning` = @editWarning");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@editWarning", quizRule.EditWarning);
                        }
                        if (!string.IsNullOrEmpty(quizRule.ModifiedBy))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                            else
                                sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRule.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizRule.CreatedAt))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `created_at` = @createdAt");
                            else
                                sql = string.Concat(sql, " `created_at` = @createdAt");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt", quizRule.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizRule.UpdatedAt))
                        {
                            if (addAnd)
                                sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                            else
                                sql = string.Concat(sql, " `updated_at` = @updatedAt");

                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt", quizRule.UpdatedAt);
                        }
                    }
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    response.Add(quizRule);
                }
                catch (MySqlException e)
                {
                    string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
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

            public List<QuizRuleModel> Delete(string connectionString, List<QuizRuleModel> quizRule)
            {
                DBConnection dbConn = new DBConnection(connectionString);
                List<QuizRuleModel> response = new List<QuizRuleModel>();
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
                            for (int i = 0; i < quizRule.Count; i++)
                            {
                                try
                                {
                                    bool addAnd = false;
                                    string sql = "DELETE FROM quiz_rules  ";
                                    if (quizRule[i] != null)
                                    {
                                        sql = string.Concat(sql, " WHERE ");
                                        addAnd = false;
                                        if (!string.IsNullOrEmpty(quizRule[i].Id))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `id` = @id" + i);
                                            else
                                                sql = string.Concat(sql, " `id` = @id" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@id" + i, quizRule[i].Id);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].QuizTypeId))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `quiz_type_id` = @quizTypeId" + i);
                                            else
                                                sql = string.Concat(sql, " `quiz_type_id` = @quizTypeId" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@quizTypeId" + i, quizRule[i].QuizTypeId);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].LessonAddressedToId))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `lesson_addressed_to_id` = @lessonAddressedToId" + i);
                                            else
                                                sql = string.Concat(sql, " `lesson_addressed_to_id` = @lessonAddressedToId" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@lessonAddressedToId" + i, quizRule[i].LessonAddressedToId);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].QuestionMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `question_max_length` = @questionMaxLength" + i);
                                            else
                                                sql = string.Concat(sql, " `question_max_length` = @questionMaxLength" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@questionMaxLength" + i, quizRule[i].QuestionMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].ResponseMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `response_max_length` = @responseMaxLength" + i);
                                            else
                                                sql = string.Concat(sql, " `response_max_length` = @responseMaxLength" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@responseMaxLength" + i, quizRule[i].ResponseMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].ExtraInfoMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `extra_info_max_length` = @extraInfoMaxLength" + i);
                                            else
                                                sql = string.Concat(sql, " `extra_info_max_length` = @extraInfoMaxLength" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@extraInfoMaxLength" + i, quizRule[i].ExtraInfoMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].InstructionMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `instruction_max_length` = @instructionMaxLength" + i);
                                            else
                                                sql = string.Concat(sql, " `instruction_max_length` = @instructionMaxLength" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@instructionMaxLength" + i, quizRule[i].InstructionMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].MaxQuestions))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `max_questions` = @maxQuestions" + i);
                                            else
                                                sql = string.Concat(sql, " `max_questions` = @maxQuestions" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@maxQuestions" + i, quizRule[i].MaxQuestions);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].MinQuestions))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `min_questions` = @minQuestions" + i);
                                            else
                                                sql = string.Concat(sql, " `min_questions` = @minQuestions" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@minQuestions" + i, quizRule[i].MinQuestions);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].FalseResponseRequired))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `false_response_required` = @falseResponseRequired" + i);
                                            else
                                                sql = string.Concat(sql, " `false_response_required` = @falseResponseRequired" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@falseResponseRequired" + i, quizRule[i].FalseResponseRequired);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].TitleMaxLength))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `title_max_length` = @titleMaxLength" + i);
                                            else
                                                sql = string.Concat(sql, " `title_max_length` = @titleMaxLength" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@titleMaxLength" + i, quizRule[i].TitleMaxLength);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].CriterionRequired))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `criterion_required` = @criterionRequired" + i);
                                            else
                                                sql = string.Concat(sql, " `criterion_required` = @criterionRequired" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@criterionRequired" + i, quizRule[i].CriterionRequired);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].ReferenceRequired))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `reference_required` = @referenceRequired" + i);
                                            else
                                                sql = string.Concat(sql, " `reference_required` = @referenceRequired" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@referenceRequired" + i, quizRule[i].ReferenceRequired);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].DeleteQuestionAllow))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `delete_question_allow` = @deleteQuestionAllow" + i);
                                            else
                                                sql = string.Concat(sql, " `delete_question_allow` = @deleteQuestionAllow" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@deleteQuestionAllow" + i, quizRule[i].DeleteQuestionAllow);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].EditWarning))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `edit_warning` = @editWarning" + i);
                                            else
                                                sql = string.Concat(sql, " `edit_warning` = @editWarning" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@editWarning" + i, quizRule[i].EditWarning);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].ModifiedBy))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                            else
                                                sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRule[i].ModifiedBy);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].CreatedAt))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                            else
                                                sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@createdAt" + i, quizRule[i].CreatedAt);
                                        }
                                        if (!string.IsNullOrEmpty(quizRule[i].UpdatedAt))
                                        {
                                            if (addAnd)
                                                sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                            else
                                                sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                            addAnd = true;
                                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizRule[i].UpdatedAt);
                                        }
                                    }
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                    response.Add(quizRule[i]);
                                }
                                catch (MySqlException e)
                                {
                                    string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                    response = new List<QuizRuleModel>();
                                }
                            }
                            trans.Commit();
                        }
                        catch (MySqlException e)
                        {
                            string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                            trans.Rollback();
                            response = new List<QuizRuleModel>();
                        }
                    }
                }
                catch (MySqlException e)
                {
                    string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                    response = new List<QuizRuleModel>();
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

}
