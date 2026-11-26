using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle the user results with Dapper.
    /// </summary>
    public static class UserResults
    {
        /// <summary>
        /// To get the user plan lesson results by PlanLessonId and UserId.
        /// </summary>
        /// <param name="planUserLessons">Plan User Lesson list.</param>
        /// <returns>Returns a PlanLessonUsermodel list.</returns>
        public static List<PlanLessonUserModel> GetUserPlanLessonModels(List<PlanLessonUserModel> planUserLessons)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT id, plan_id, plan_lesson_id, lesson_id, user_id, overall_score, completed, modified_by, created_at, updated_at 
                FROM gs_teachers_web_db_prod.plan_lesson_users 
                WHERE ";

            for (int i = 0; i < planUserLessons.Count; i++)
            {
                query += " (plan_lesson_id = @planLessonId" + i.ToString() + " AND user_id = @userId" + i.ToString() + ") ";
                parameters.Add(string.Concat("@planLessonId", i.ToString()), planUserLessons[i].PlanLessonId);
                parameters.Add(string.Concat("@userId", i.ToString()), planUserLessons[i].UserId);
                if (i < planUserLessons.Count - 1)
                {
                    query += " OR ";
                }
            }
            query += "; ";

            List<PlanLessonUserModel> planLessonUserModels = new List<PlanLessonUserModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();

            try
            {
                planLessonUserModels = connection.Query<PlanLessonUserModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return planLessonUserModels;
        }

        /// <summary>
        /// To call the plan lesson quiz user results by plan lesson ids and student.
        /// </summary>
        /// <param name="planLessonIds">Plan Lesson Ids.</param>
        /// <param name="studentId">Student's id.</param>
        /// <returns>Returns a list of plan lesson quiz user results.</returns>
        public static List<PlanLessonQuizUserResultModel> GetUserLessonQuizPlanResultsSimpleByStudent(List<string> planLessonIds, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at` FROM `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` ";
            parameters.Add("@studentId", studentId);
            for (int i = 0; i < planLessonIds.Count; i++)
            {
                query += string.Concat("@planLessonId", i.ToString());
                parameters.Add(string.Concat("@planLessonId", i.ToString()), planLessonIds[i]);
                if (i < planLessonIds.Count - 1)
                {
                    query += ",";
                }
            }
            query += "); ";

            List<PlanLessonQuizUserResultModel> planLessonQuizUserResultModels = new List<PlanLessonQuizUserResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();

            try
            {
                planLessonQuizUserResultModels = connection.Query<PlanLessonQuizUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return planLessonQuizUserResultModels;
        }

        /// <summary>
        /// To get the homework plan results.
        /// </summary>
        /// <param name="planQuizIds">The plan quiz ids list.</param>
        /// <param name="studentIds">The students' ids to call.</param>
        /// <returns>Returns the teacher plan homework results.</returns>
        public static List<TeacherPlanHomeworkResultModel> GetUserHomeworkPlanResultsSimpleByStudent(List<string> planQuizIds, List<string> studentIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT DISTINCT pre.`id`, pre.`plan_quiz_id`, pre.`user_id`, pre.`usqui_user_result_id`, qr.total_percent, qr.`status`, pre.created_at
            FROM `gs_teachers_web_db_prod`.`plan_assignment_user_results` pre
            INNER JOIN `gs_teachers_web_db_prod`.`user_quiz_results` qr ON qr.id = pre.usqui_user_result_id
            ";

            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;

            if (studentIds != null && studentIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE pre.user_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND pre.user_id IN (";
                }

                for (int i = 0; i < studentIds.Count; i++)
                {
                    query += string.Concat(" @userIds", i.ToString());
                    parameters.Add("@userIds" + i.ToString(), studentIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < studentIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }


            if (planQuizIds != null && planQuizIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE pre.`plan_quiz_id` IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND pre.`plan_quiz_id` IN (";
                }

                for (int i = 0; i < planQuizIds.Count; i++)
                {
                    query += string.Concat(" @planQuizId", i.ToString());
                    parameters.Add("@planQuizId" + i.ToString(), planQuizIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < planQuizIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            List<TeacherPlanHomeworkResultModel> userQuizResultModels = new List<TeacherPlanHomeworkResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();


            try
            {
                userQuizResultModels = connection.Query<TeacherPlanHomeworkResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userQuizResultModels;
        }

        /// <summary>
        /// To get the user quiz results.
        /// </summary>
        /// <param name="quizIds">The quizzes' ids list.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns a plan lesson quiz user result models.</returns>
        public static List<PlanLessonQuizUserResultModel> GetUserLessonQuizPlanResults(List<string> quizIds, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT re.id, re.plan_lesson_id, re.user_id, re.quiz_id, re.score, re.usqui_user_result_id,
                re.modified_by, re.created_at, re.updated_at
                FROM gs_teachers_web_db_prod.plan_lesson_quiz_user_results re 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls ON ls.id = re.plan_lesson_id 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plans` pl ON pl.id = ls.plan_id
                WHERE 
                ls.available = 1 
                AND 
                pl.opened = 1 
                AND pl.available = 1
                AND re.user_id = @studentId AND re.quiz_id IN (";
            parameters.Add("@studentId", studentId);
            for (int i = 0; i < quizIds.Count; i++)
            {
                query += string.Concat("@quizId", i.ToString());
                parameters.Add(string.Concat("@quizId", i.ToString()), quizIds[i]);
                if (i < quizIds.Count - 1)
                {
                    query += ",";
                }
            }
            query += "); ";

            List<PlanLessonQuizUserResultModel> planLessonQuizUserResultModels = new List<PlanLessonQuizUserResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();

            try
            {
                planLessonQuizUserResultModels = connection.Query<PlanLessonQuizUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return planLessonQuizUserResultModels;
        }

        /// <summary>
        /// To get the user quiz results.
        /// </summary>
        /// <param name="quizIds">The quizzes' ids list.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns a plan lesson quiz user result models.</returns>
        public static List<PlanLessonQuizUserResultModel> GetUserLessonQuizPlanResults(List<string> quizIds, string studentId, string planLessonId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query =
                @"SELECT DISTINCT re.id, re.plan_lesson_id,  re.manually_corrected_by_teacher, 
                re.user_id, re.quiz_id, re.score, re.usqui_user_result_id,
                re.modified_by, re.created_at, re.updated_at
                FROM gs_teachers_web_db_prod.plan_lesson_quiz_user_results re 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls ON ls.id = re.plan_lesson_id 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plans` pl ON pl.id = ls.plan_id
                WHERE 
                ls.available = 1 
                AND 
                pl.opened = 1 
                AND pl.available = 1
                AND re.user_id = @studentId ";

            if (!string.IsNullOrWhiteSpace(planLessonId))
            {
                query += " AND re.plan_lesson_id = @planLessonId ";
                parameters.Add("@planLessonId", planLessonId);
            }

            query += " AND re.quiz_id IN (";
            parameters.Add("@studentId", studentId);
            for (int i = 0; i < quizIds.Count; i++)
            {
                query += string.Concat("@quizId", i.ToString());
                parameters.Add(string.Concat("@quizId", i.ToString()), quizIds[i]);
                if (i < quizIds.Count - 1)
                {
                    query += ",";
                }
            }
            query += "); ";

            List<PlanLessonQuizUserResultModel> planLessonQuizUserResultModels = new List<PlanLessonQuizUserResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();

            try
            {
                planLessonQuizUserResultModels = connection.Query<PlanLessonQuizUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return planLessonQuizUserResultModels;
        }

        /// <summary>
        /// To get the user quiz results.
        /// </summary>
        /// <param name="quizIds">The quizzes' ids list.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns a plan lesson quiz user result models.</returns>
        public static List<Domain.Models.Generals.Dapper.UserQuizResultModel> GetUserQuizHomeworkPlanResults(List<string> quizIds, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT
	                `usqui`.`id`,
                    `usqui`.`user_id`,
                    `usqui`.`quiz_id`,
                    `usqui`.`total_correct`,
                    `usqui`.`total_questions`,
                    `usqui`.`total_percent`,
                    `usqui`.`feedback`,
                    `usqui`.`status`,
                    `usqui`.`modified_by`,
                    `usqui`.`created_at`,
                    `usqui`.`updated_at`,
                    `re`.`plan_quiz_id`
                FROM gs_teachers_web_db_prod.plan_assignment_user_results re 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_quizs` qui ON qui.id = re.plan_quiz_id 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plans` pl ON pl.id = qui.plan_id
                INNER JOIN  gs_teachers_web_db_prod.user_quiz_results usqui ON usqui.id = re.usqui_user_result_id
                WHERE 
                qui.available = 1 
                AND 
                pl.opened = 1 
                AND pl.available = 1
                AND re.user_id = @studentId  AND usqui.quiz_id IN (";
            parameters.Add("@studentId", studentId);
            for (int i = 0; i < quizIds.Count; i++)
            {
                query += string.Concat("@quizId", i.ToString());
                parameters.Add(string.Concat("@quizId", i.ToString()), quizIds[i]);
                if (i < quizIds.Count - 1)
                {
                    query += ",";
                }
            }
            query += "); ";

            List<Domain.Models.Generals.Dapper.UserQuizResultModel> userQuizResultModels = new List<Domain.Models.Generals.Dapper.UserQuizResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                userQuizResultModels = connection.Query<Domain.Models.Generals.Dapper.UserQuizResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userQuizResultModels;
        }

        /// <summary>
        /// To get the user lesson averages.
        /// </summary>
        /// <param name="userIds">The list of users' ids.</param>
        /// <param name="lessonIds">The list of lessons' ids.</param>
        /// <param name="planLessonIds">Optional: The plan lessons ids.</param>
        /// <param name="planId">Optional: The plan's id.</param>
        /// <returns>Returns the lesson averages list.</returns>
        public static List<PlanLessonUserModel> GetUserLessonsAverages(List<string> userIds, List<string> lessonIds = null, List<string> planLessonIds = null, string planId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT DISTINCT `id`,
                    `plan_id`,
                    `plan_lesson_id`,
                    `lesson_id`,
                    `user_id`,
                    `overall_score`,
                    `completed`,
                    `modified_by`,
                    `created_at`,
                    `updated_at`
                FROM `gs_teachers_web_db_prod`.`plan_lesson_users` ";

            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;

            if (userIds != null && userIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE `user_id` IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND `user_id` IN (";
                }

                for (int i = 0; i < userIds.Count; i++)
                {
                    query += string.Concat(" @userIds", i.ToString());
                    parameters.Add("@userIds" + i.ToString(), userIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < userIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }


            if (lessonIds != null && lessonIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE `lesson_id` IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND `lesson_id` IN (";
                }

                for (int i = 0; i < lessonIds.Count; i++)
                {
                    query += string.Concat(" @lessonId", i.ToString());
                    parameters.Add("@lessonId" + i.ToString(), lessonIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < lessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (planLessonIds != null && planLessonIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE `plan_lesson_id` IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND `plan_lesson_id` IN (";
                }

                for (int i = 0; i < planLessonIds.Count; i++)
                {
                    query += string.Concat(" @planLessonId", i.ToString());
                    parameters.Add("@planLessonId" + i.ToString(), planLessonIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < planLessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if(!string.IsNullOrWhiteSpace(planId))
            {
                if (!whereUsed)
                {
                    query += @" WHERE `plan_id` = @planId ";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND `plan_id`  = @planId ";
                }
                parameters.Add("@planId", planId);
            }

            List<PlanLessonUserModel> planLessonUserModels = new List<PlanLessonUserModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();

            try
            {
                planLessonUserModels = connection.Query<PlanLessonUserModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }
            return planLessonUserModels;
        }

        /// <summary>
        /// Returns the user quiz results by quizzess.
        /// </summary>
        /// <param name="userIds">The users ids' list.</param>
        /// <param name="quizIds">The quizzes' id.</param>
        /// <param name="planQuizIds">The plan quizzess ids' list.</param>
        /// <returns>Returns the user quiz result model list.</returns>
        public static List<Domain.Models.UserQuizzes.UserQuizResultModel> GetUserQuizResultsForTeacherPlans(List<string> userIds, List<string> quizIds, List<string> planQuizIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT DISTINCT usqui.id, usqui.try_no, usqui.user_id, usqui.quiz_id, usqui.dok, usqui.is_exam, usqui.lesson_id, usqui.subject, usqui.level, usqui.total_correct, usqui.total_questions, usqui.total_percent, usqui.created_at 
            FROM  gs_teachers_web_db_prod.plan_assignment_user_results plu 
            LEFT JOIN `gs_teachers_web_db_prod`.`user_quiz_results` usqui  ON plu.usqui_user_result_id = usqui.id";

            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;

            if (userIds != null && userIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE usqui.user_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND usqui.user_id IN (";
                }

                for (int i = 0; i < userIds.Count; i++)
                {
                    query += string.Concat(" @userIds", i.ToString());
                    parameters.Add("@userIds" + i.ToString(), userIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < userIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }


            if (quizIds != null && quizIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE usqui.quiz_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND usqui.quiz_id IN (";
                }

                for (int i = 0; i < quizIds.Count; i++)
                {
                    query += string.Concat(" @quizId", i.ToString());
                    parameters.Add("@quizId" + i.ToString(), quizIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < quizIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (planQuizIds != null && planQuizIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE plu.plan_quiz_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND plu.plan_quiz_id IN (";
                }

                for (int i = 0; i < quizIds.Count; i++)
                {
                    query += string.Concat(" @planQuizId", i.ToString());
                    parameters.Add("@planQuizId" + i.ToString(), planQuizIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < planQuizIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            var userQuizResultModels = new List<Domain.Models.UserQuizzes.UserQuizResultModel>();


            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                userQuizResultModels = connection.Query<Domain.Models.UserQuizzes.UserQuizResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userQuizResultModels;
        }

        /// <summary>
        /// Returns the user quiz results by quizzess.
        /// </summary>
        /// <param name="userIds">The users ids' list.</param>
        /// <param name="quizIds">The quizzes ids' list.</param>
        /// <param name="planQuizIds">The plan quizzess ids' list.</param>
        /// <returns>Returns the user quiz result model list.</returns>
        public static List<Domain.Models.Generals.Dapper.UserQuizResultModel> GetUserQuizResults(List<string> userIds, List<string> quizIds, List<string> planQuizIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT DISTINCT usqui.id, usqui.try_no, usqui.user_id, usqui.quiz_id, usqui.dok, usqui.is_exam, usqui.lesson_id, usqui.subject, usqui.level, usqui.total_correct, usqui.total_questions, usqui.total_percent, usqui.created_at 
            FROM `gs_teachers_web_db_prod`.`user_quiz_results` usqui 
            INNER JOIN gs_teachers_web_db_prod.plan_assignment_user_results plu ON plu.usqui_user_result_id = usqui.id ";

            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;

            if (userIds != null && userIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE usqui.user_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND usqui.user_id IN (";
                }

                for (int i = 0; i < userIds.Count; i++)
                {
                    query += string.Concat(" @userIds", i.ToString());
                    parameters.Add("@userIds" + i.ToString(), userIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < userIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }


            if (quizIds != null && quizIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE usqui.quiz_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND usqui.quiz_id IN (";
                }

                for (int i = 0; i < quizIds.Count; i++)
                {
                    query += string.Concat(" @quizId", i.ToString());
                    parameters.Add("@quizId" + i.ToString(), quizIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < quizIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (planQuizIds != null && planQuizIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE plu.plan_quiz_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND plu.plan_quiz_id IN (";
                }

                for (int i = 0; i < quizIds.Count; i++)
                {
                    query += string.Concat(" @planQuizId", i.ToString());
                    parameters.Add("@planQuizId" + i.ToString(), planQuizIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < planQuizIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            var userQuizResultModels = new List<Domain.Models.Generals.Dapper.UserQuizResultModel>();


            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                userQuizResultModels = connection.Query<Domain.Models.Generals.Dapper.UserQuizResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return userQuizResultModels;
        }

        /// <summary>
        /// To get the plan lesson pre post quizzess.
        /// </summary>
        /// <param name="quizId">The quiz's id.</param>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the plan pre post user results.</returns>
        public static List<PlanPrePostUserResultModel> GetPlanLessonPrePost(string quizId, string userId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT DISTINCT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct` , `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at` FROM `gs_teachers_web_db_prod`.`plan_pre_post_user_results` WHERE quiz_id = @quizId AND user_id = @userId;";
            var parameters = new { quizId, userId };
            List<PlanPrePostUserResultModel> planPrePostUserResultModels = new List<PlanPrePostUserResultModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planPrePostUserResultModels = connection.Query<PlanPrePostUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return planPrePostUserResultModels;
        }

        /// <summary>
        /// To get the plan pre and post results.
        /// </summary>
        /// <param name="planLessonId">The plan lesson's id.</param>
        /// <param name="studentId">The student's id.</param>
        /// <param name="teacherId">Optional: The teacher's id.</param>
        /// <returns>Returns a list of pre post user results.</returns>
        public static List<PlanPrePostUserResultModel> GetPlanPrePostResults(string planLessonId, string studentId, string teacherId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT DISTINCT re.id, re.plan_lesson_id, re.user_id, re.quiz_id, re.pre_post, re.total_correct, re.total_questions, re.score, 
            re.usqui_user_result_id, re.modified_by, re.created_at, re.updated_at 
            FROM gs_teachers_web_db_prod.plan_pre_post_user_results re 
            INNER JOIN  gs_teachers_web_db_prod.teacher_plan_lessons pll ON re.plan_lesson_id = pll.id  ";

            if (!string.IsNullOrWhiteSpace(teacherId))
            {
                query += " INNER JOIN gs_teachers_web_db_prod.teacher_plans pl ON pl.id = pll.plan_id ";
            }

            query += @"
                WHERE pll.id = @planLessonId 
                AND re.user_id = @studentId ";
            parameters.Add("@planLessonId", planLessonId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("@studentId", studentId, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            if (!string.IsNullOrWhiteSpace(teacherId))
            {
                query += "  AND pl.user_id = @teacherId;";
                parameters.Add("@teacherId", teacherId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }
            List<PlanPrePostUserResultModel> planPrePostUserResultModels = new List<PlanPrePostUserResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();

            try
            {
                planPrePostUserResultModels = connection.Query<PlanPrePostUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return planPrePostUserResultModels;
        }

        /// <summary>
        /// To get the students results by group.
        /// </summary>
        /// <param name="planId">The plan's id.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="teacherGroupId">The teacher group id (do not confuse with the main group id).</param>
        /// <returns>Returns a list of report quizzes.</returns>
        public static List<Domain.Models.Generals.Responses.Reports.ReportQuizModel> GetStudentsLessonExamsResultsByGroup(string planId, string fromDate = null, string toDate = null, string teacherGroupId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT plqui.quiz_id, plqui.due_date, plqui.due_date_timezone, pl.plan_id, pl.lesson_id, 
            usqui.user_id, usqui.score, usqui.usqui_user_result_id
            FROM gs_teachers_web_db_prod.teacher_plan_lesson_quizs plqui 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` pl ON pl.id = plqui.plan_lesson_id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plans` pla ON pla.id = pl.plan_id
			INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` tg ON tg.teacher_plan_id = pla.id
            LEFT JOIN `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` usqui ON usqui.plan_lesson_id = pl.id AND plqui.quiz_id = usqui.quiz_id 
            WHERE pl.available = 1 ";

            if(!string.IsNullOrWhiteSpace(planId))
            {
                query += @" AND pl.plan_id = @planId";
                parameters.Add("@planId", planId);
            }

            if (!string.IsNullOrWhiteSpace(teacherGroupId))
            {
                query += @" AND tg.group_id = @teacherGroupId";
                parameters.Add("@teacherGroupId", teacherGroupId);
            }

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                query += " AND plqui.due_date >= @fromDate";
                parameters.Add("@fromDate", DateTime.Parse(fromDate));
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                query += " AND plqui.due_date <= @toDate";
                parameters.Add("@toDate", DateTime.Parse(toDate));
            }

            query += " ORDER BY plqui.due_date, plqui.quiz_id, usqui.user_id ";


            var results = new List<Domain.Models.Generals.Responses.Reports.ReportQuizModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                results = connection.Query<Domain.Models.Generals.Responses.Reports.ReportQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return results;
        }

        /// <summary>
        /// To get the plan pre post results by the plan lessons ids and the student id.
        /// </summary>
        /// <param name="planLessonIds">The plan lesson ids.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns a list of plan-pre-posts results.</returns>
        public static List<PlanPrePostUserResultModel> GetPlanPrePostResultsByStudentAndPlanLessons(List<string> planLessonIds, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT `id`, `plan_lesson_id`, `user_id`, `quiz_id`, `pre_post`, `total_correct`, `total_questions`, `score`, `usqui_user_result_id`, `modified_by`, `created_at`, `updated_at` FROM `gs_teachers_web_db_prod`.`plan_pre_post_user_results` ";

            bool whereUsed = false;
            if (planLessonIds != null && planLessonIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE `plan_lesson_id` IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND `plan_lesson_id` IN (";
                }

                for (int i = 0; i < planLessonIds.Count; i++)
                {
                    query += string.Concat(" @planLessonId", i.ToString());
                    parameters.Add("@planLessonId" + i.ToString(), planLessonIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < planLessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (!string.IsNullOrWhiteSpace(studentId))
            {
                if (!whereUsed)
                {
                    query += @" WHERE `user_id` = @studentId";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND `user_id` = @studentId";
                }
                parameters.Add("@studentId", studentId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }

            List<PlanPrePostUserResultModel> planPrePostUserResultModels = new List<PlanPrePostUserResultModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planPrePostUserResultModels = connection.Query<PlanPrePostUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }
            return planPrePostUserResultModels;
        }

        /// <summary>
        /// To get the students results by group.
        /// </summary>
        /// <param name="planId">The plan's id.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="teacherGroupId">The teacher group id (do not confuse with the main group id).</param>
        /// <returns>Returns a list of report quizzes.</returns>
        public static List<Domain.Models.Generals.Responses.Reports.ReportQuizModel> GetStudentsTeacherCustomQuizzesResultsByGroup(string planId, string fromDate = null, string toDate = null, string teacherGroupId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT plqui.quiz_id, plqui.due_date, pla.id as plan_id, 
            usqui.user_id, usqui.total_percent as score, usqui.usqui_user_result_id, usqui.score as premises
            FROM gs_teachers_web_db_prod.teacher_plan_quizs plqui 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plans` pla ON pla.id = plqui.plan_id
			INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` tg ON tg.teacher_plan_id = pla.id
            LEFT JOIN `gs_teachers_web_db_prod`.`plan_assignment_user_results` usqui ON usqui.plan_quiz_id = plqui.id
            LEFT JOIN `gs_teachers_web_db_prod`.`user_quiz_results` usquir ON usquir.id = usqui.usqui_user_result_id
            WHERE plqui.available = 1 ";

            if (!string.IsNullOrWhiteSpace(planId))
            {
                query += @" AND pla.id = @planId";
                parameters.Add("@planId", planId);
            }

            if (!string.IsNullOrWhiteSpace(teacherGroupId))
            {
                query += @" AND tg.group_id = @teacherGroupId";
                parameters.Add("@teacherGroupId", teacherGroupId);
            }

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                query += " AND plqui.due_date >= @fromDate";
                parameters.Add("@fromDate", DateTime.Parse(fromDate));
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                query += " AND plqui.due_date <= @toDate";
                parameters.Add("@toDate", DateTime.Parse(toDate));
            }

            query += " ORDER BY plqui.due_date, plqui.quiz_id, usqui.user_id ";


            var results = new List<Domain.Models.Generals.Responses.Reports.ReportQuizModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                results = connection.Query<Domain.Models.Generals.Responses.Reports.ReportQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return results;
        }
        
        /// <summary>
        /// To get the student planning lesson or lessons results.
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="planId">The teacher plan's id.</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the plan lesson user models with the results.</returns>
        public static List<Domain.Models.Teachers.PlanLessonUserModel> GetStudentPlanningLessonsResults(string studentId, string planId, string lessonId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pllqur.plan_lesson_id, pllqur.user_id, pllqur.score as `overall_score`, 
            tpl.plan_id, tpl.lesson_id, tplq.quiz_id, pllqur.usqui_user_result_id
            FROM gs_teachers_web_db_prod.plan_lesson_quiz_user_results pllqur
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` tpl ON tpl.id = pllqur.plan_lesson_id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` tplq ON tplq.plan_lesson_id = tpl.id AND tplq.quiz_id = pllqur.quiz_id
            WHERE tpl.plan_id = @planId AND pllqur.user_id = @studentId ";
            parameters.Add("@planId", planId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("@studentId", studentId, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            if(!string.IsNullOrWhiteSpace(lessonId))
            {
                query += " AND tpl.lesson_id = @lessonId;";
                parameters.Add("@lessonId", lessonId);
            }
            
            List<Domain.Models.Teachers.PlanLessonUserModel> planLessonUserModels = new List<Domain.Models.Teachers.PlanLessonUserModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planLessonUserModels = connection.Query<Domain.Models.Teachers.PlanLessonUserModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return planLessonUserModels;
        }
        
         /// <summary>
        /// To get plan quiz student results by student and plan lesson id.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="planLessonIds">The id of the plan lessons.</param>
        /// <returns>Returns the plan lesson quiz user results.</returns>
        public static List<PlanLessonQuizUserResultModel> GetPlanLessonQuizStudentResults(string studentId,
            List<int> planLessonIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            var query = @"SELECT DISTINCT id, plan_lesson_id, 
            user_id, quiz_id, score, usqui_user_result_id
            FROM `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` 
            WHERE user_id = @studentId AND plan_lesson_id IN @planLessonIds; ";
            parameters.Add("@studentId", studentId);
            parameters.Add("@planLessonIds", planLessonIds);

            var planLessonQuizUserResults = new List<PlanLessonQuizUserResultModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planLessonQuizUserResults =
                    connection.Query<PlanLessonQuizUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }
            
            // Return the results.
            return planLessonQuizUserResults;
        }
        
        /// <summary>
        /// To read the plan pre post user results.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="planLessonIds">The plan lessons.</param>
        /// <returns>Returns a list of plan pre post results.</returns>
        public static List<PlanPrePostUserResultModel> GetPlanPrePostUserResultModelsByPlanIds(string studentId, List<int> planLessonIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            var query = @"SELECT id, plan_lesson_id, quiz_id, user_id FROM gs_teachers_web_db_prod.plan_pre_post_user_results
            WHERE user_id = @studentId AND plan_lesson_id IN @planLessonIds; ";
            parameters.Add("@studentId", studentId);
            parameters.Add("@planLessonIds", planLessonIds);

            var planPrePostUserResultModels = new List<PlanPrePostUserResultModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planPrePostUserResultModels =
                    connection.Query<PlanPrePostUserResultModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }
            
            // Return the results.
            return planPrePostUserResultModels;
        }
        
    }
}