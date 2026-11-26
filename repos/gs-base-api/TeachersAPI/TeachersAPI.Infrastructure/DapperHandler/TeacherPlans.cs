using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Teachers;
using TeacherPlanLessonQuizModel = TeachersAPI.Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel;
using System.Diagnostics;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle the teacher plans with Dapper.
    /// </summary>
    public static class TeacherPlans
    {
        /// <summary>
        /// To get the student pending lesson quizzes.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="quizId">The quiz's id.</param>
        /// <returns>Returns a list of teacher plan quiz model.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> GetStudentPendingLessonQuizzes(string userId, string quizId = null, string planId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"
            SELECT DISTINCT lqui.`id`, pl.id as `plan_id`, lqui.`plan_lesson_id`, pll.lesson_id, pl.user_id as `teacher_id`, lqui.`quiz_id`, lqui.`due_date`, lqui.`due_date_timezone`, lqui.`duration`, lqui.`modified_by`, lqui.`created_at`, lqui.`updated_at`
            FROM gs_teachers_web_db_prod.teacher_plan_lesson_quizs lqui
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` pll ON pll.id = lqui.plan_lesson_id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plans` pl ON pl.id = pll.plan_id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gr ON gr.id = plgr.group_id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` grstu ON grstu.teacher_group_id = gr.id
            LEFT JOIN `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` re ON re.plan_lesson_id = pll.id AND re.quiz_id = lqui.quiz_id AND re.user_id = grstu.student_id
            WHERE gr.available = 1 AND pl.available = 1 AND pll.available = 1
            AND re.id IS NULL AND grstu.student_id = @userId ";
            parameters.Add("@userId", userId);

            if (!string.IsNullOrWhiteSpace(quizId))
            {
                query += " AND lqui.quiz_id = @quizId ";
                parameters.Add("@quizId", quizId);
            }

            if (!string.IsNullOrWhiteSpace(planId))
            {
                query += " AND pl.id = @planId ";
                parameters.Add("@planId", planId);
            }

            IEnumerable<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }



            return items != null ? items.ToList() : null;
        }

        /// <summary>
        /// To get the teacher plan lesson simple.
        /// </summary>
        /// <param name="teacherPlanLessonIds">The list of teacher plan ids.</param>
        /// <returns>The teacher plan lessons.</returns>
        public static List<TeacherPlanLessonModel> GetTeacherPlanLessonSimple(List<string> teacherPlanLessonIds = null, List<string> teacherPlanIds = null, List<string> lessonIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT `id`,`plan_id`,`lesson_id`,`tries_quantity`,`available`,`due_date`, `for_grading`, `pre_post_required`, `modified_by`,`created_at`,`updated_at` FROM `gs_teachers_web_db_prod`.`teacher_plan_lessons` WHERE available = 1 ";
            if (teacherPlanLessonIds != null && teacherPlanLessonIds.Count > 0)
            {
                query += " AND id IN (";
                for (int i = 0; i < teacherPlanLessonIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanLessonId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanLessonId", i.ToString()), teacherPlanLessonIds[i]);
                    if (i < teacherPlanLessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }
            }

            if (teacherPlanIds != null && teacherPlanIds.Count > 0)
            {
                query += " AND plan_id IN (";
                for (int i = 0; i < teacherPlanIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                    if (i < teacherPlanIds.Count - 1)
                    {
                        query += ",";
                    }
                }
            }

            if (lessonIds != null && lessonIds.Count > 0)
            {
                query += " AND lesson_id IN (";
                for (int i = 0; i < lessonIds.Count; i++)
                {
                    query += string.Concat("@lessonId", i.ToString());
                    parameters.Add(string.Concat("@lessonId", i.ToString()), lessonIds[i]);
                    if (i < lessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }
            }

            query += "); ";

            IEnumerable<TeacherPlanLessonModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherPlanLessonModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : null;
        }

        /// <summary>
        /// Return the teacher plan lessons by teacher plan lesson ids.
        /// </summary>
        /// <param name="teacherPlanLessonIds">Teacher plan lesson ids to find.</param>
        /// <returns>Returns a list of teacher plan lesson models.</returns>
        public static List<TeacherPlanLessonModel> GetTeacherPlanLessonByTeacherPlanLessonIds(List<string> teacherPlanLessonIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query =
                @"SELECT DISTINCT ls.id, ls.plan_id, ls.lesson_id, ls.tries_quantity, ls.available, ls.due_date, ls.`pre_post_required`,
                ls.for_grading, ls.modified_by, ls.created_at, ls.updated_at
                 FROM gs_teachers_web_db_prod.teacher_plan_lessons ls 
                INNER JOIN gs_teachers_web_db_prod.teacher_plans pl ON pl.id = ls.plan_id
                WHERE 
                pl.available = 1 AND pl.opened = 1 AND ls.available = 1 AND ls.id IN (";
            for (int i = 0; i < teacherPlanLessonIds.Count; i++)
            {
                query += string.Concat("@teacherPlanLessonId", i.ToString());
                parameters.Add(string.Concat("@teacherPlanLessonId", i.ToString()), teacherPlanLessonIds[i]);
                if (i < teacherPlanLessonIds.Count - 1)
                {
                    query += ",";
                }
            }
            query += "); ";

            IEnumerable<TeacherPlanLessonModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherPlanLessonModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }



            return items != null ? items.ToList() : null;
        }

        /// <summary>
        /// To read the plan lessons quizzes by student.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Returns the teacher plan lesson quiz models with tries details.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> GetPlanLessonQuizzesAccordingToLessonIdAndStudentId(string lessonId, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { lessonId, studentId };
            string query = @"SELECT DISTINCT qui.`id`,
                qui.`plan_lesson_id`,
                qui.`for_grading`,
                qui.`required`,
                qui.`quiz_id`,
                qui.`due_date`,
                qui.`due_date_timezone`,
                qui.`duration`,
                qui.`modified_by`,
                qui.`created_at`,
                qui.`updated_at`, 
                ls.tries_quantity
                FROM
                `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` qui
                INNER JOIN  `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls ON ls.id = qui.plan_lesson_id
                INNER JOIN gs_teachers_web_db_prod.teacher_plans pl ON pl.id = ls.plan_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                WHERE
                gro.available = 1
                AND pl.opened = 1
                AND pl.available = 1
                AND ls.lesson_id = @lessonId
                AND ls.available = 1
                AND gs.student_id = @studentId";

            IEnumerable<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return items != null ? items.ToList() : null;


        }

        /// <summary>
        /// To get the plan lessons according to lesson and student.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Return a list of strings of the plans lessons.</returns>
        public static List<TeacherPlanLessonModel> GetPlanLessonIdsAccordingToLessonAndStudents(string lessonId, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { lessonId, studentId };
            string query = @"SELECT DISTINCT ls.id, ls.plan_id, ls.lesson_id, ls.tries_quantity, ls.available, ls.due_date, ls.for_grading, ls.`pre_post_required`, 
                ls.modified_by, ls.created_at, ls.updated_at
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls ON ls.plan_id = pl.id
                WHERE 
                gro.available = 1 
                AND pl.opened = 1
                AND pl.available = 1
                AND ls.lesson_id = @lessonId 
                AND ls.available = 1
                AND gs.student_id = @studentId;";

            List<TeacherPlanLessonModel> teacherPlanLessonList = new List<TeacherPlanLessonModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanLessonList = connection.Query<TeacherPlanLessonModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return teacherPlanLessonList;
        }

        /// <summary>
        /// To get the assignments according to quiz id and student's id.
        /// </summary>
        /// <param name="quizId">The quid (homework) id to find.</param>
        /// <param name="studentId">The student's id.</param>
        /// <param name="planQuizId">Optional: The plan quiz's id to filter by it.</param>
        /// <returns>Returns the teache plan quiz models list.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> GetPlanQuizIdsAccordingToQuizAndStudent(string studentId, string quizId = null, string planQuizId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT DISTINCT qui.id, qui.plan_id, qui.quiz_id, qui.tries_quantity, qui.available, qui.due_date, qui.for_grading, qui.modified_by, qui.created_at, qui.updated_at,
                 (SELECT SUM(custom_value) 
                 FROM gs_teachers_web_db_prod.quiz_questions 
                 WHERE quiz_id = qui.quiz_id) AS total_max_score
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_quizs` qui ON qui.plan_id = pl.id
                WHERE 
                gro.available = 1 
                AND pl.opened = 1 AND pl.available = 1
                AND qui.available = 1 ";

            if (!string.IsNullOrWhiteSpace(quizId))
            {
                query += "  AND qui.quiz_id = @quizId ";
                parameters.Add("@quizId", quizId);
            }

            if (!string.IsNullOrWhiteSpace(planQuizId))
            {
                query += "  AND qui.id = @planQuizId ";
                parameters.Add("@planQuizId", planQuizId);
            }
            query += "  AND gs.student_id = @studentId;";
            parameters.Add("@studentId", studentId);


            List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> teacherPlanQuizModels = new List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanQuizModels = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return teacherPlanQuizModels;
        }

        /// <summary>
        /// To get the teacher plan with group's details.
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <returns>Returns a list of teacher plans with group details model.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanModel> GetTeacherPlans(string teacherId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { teacherId };
            string query = @"SELECT DISTINCT pl.id as `plan_id`, pl.name as `plan_name`, pl.user_id as `teacher_id`, pl.available as `plan_available`, 
                            pl.subject_code, pl.level_code, pl.period_id, pl.opened as `is_plan_open`,  pl.created_at 
                            FROM gs_teachers_web_db_prod.teacher_plans pl 
                            WHERE  pl.user_id = @teacherId AND pl.available = 1 AND pl.opened = 1;";

            List<Domain.Models.Generals.Dapper.TeacherPlanModel> teacherPlanModels = new List<Domain.Models.Generals.Dapper.TeacherPlanModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanModels = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return teacherPlanModels;
        }

        /// <summary>
        /// To get the teacher plan with group's details.
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <param name="subjectCode">The grade subject code.</param>
        /// <param name="level">The grade level.</param>
        /// <param name="name">The name of the plan.</param>
        /// <param name="teacherGroup"></param>
        /// <param name="fromDate">The starting date from which the query should retrieve data.</param>
        /// <param name="toDate">Represents the ending date until which the query should retrieve data.</param>
        /// <returns>Returns a list of teacher plans with group details model.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanModel> GetTeacherPlans(string teacherId, string subjectCode = null, string level = null, string name = null, string teacherGroup = null, string fromDate = null, string toDate = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.id as `plan_id`, pl.name as `plan_name`, pl.user_id as `teacher_id`, pl.available as `plan_available`, 
                            pl.subject_code, pl.level_code, pl.period_id, pl.opened as `is_plan_open`,  pl.created_at,
                            pl.open_date, pl.close_date
                            FROM gs_teachers_web_db_prod.teacher_plans pl
                            LEFT JOIN gs_teachers_web_db_prod.teacher_plan_groups pg ON pg.teacher_plan_id = pl.id
                            WHERE  pl.available = 1 AND pl.opened = 1 ";

            if (!string.IsNullOrWhiteSpace(teacherId))
            {
                query += " AND pl.user_id = @teacherId ";
                parameters.Add("@teacherId", teacherId);
            }

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += "  AND pl.subject_code = @subjectCode ";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(level))
            {
                query += "  AND pl.level_code = @level ";
                parameters.Add("@level", level);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query += " AND pl.name like concat('%', @name, '%') ";
                parameters.Add("@name", name);
            }

            if (!string.IsNullOrWhiteSpace(teacherGroup))
            {
                query += "  AND pg.group_id = @teacherGroup";
                parameters.Add("@teacherGroup", teacherGroup);
            }

            if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
            {
                query += @" AND (pl.open_date between @fromDate and @toDate or 
                                 pl.close_date between @fromDate and @toDate) ";

                parameters.Add("@fromDate", fromDate);
                parameters.Add("@toDate", toDate);
            }

            query += ";";

            List<Domain.Models.Generals.Dapper.TeacherPlanModel> teacherPlanModels =
                new List<Domain.Models.Generals.Dapper.TeacherPlanModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanModels = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanModel>(query, parameters)
                    .ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }


            return teacherPlanModels;
        }

        /// <summary>
        /// To call the groups related to the plans.
        /// </summary>
        /// <param name="planIds">The plan ids.</param>
        /// <returns>Return a list of the groups.</returns>
        public static List<TeacherPlanGroupStudentModel> GetTeacherPlansGroups(List<string> planIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pg.teacher_plan_id as `plan_id`, tg.id as `group_id`, tg.name as `group_name`
                            FROM gs_teachers_web_db_prod.teacher_plan_groups pg
                            LEFT JOIN gs_teachers_web_db_prod.teacher_groups tg ON tg.id = pg.group_id
                            WHERE pg.teacher_plan_id IN (";

            for (int i = 0; i < planIds.Count; i++)
            {
                query += string.Concat("@PlanId", i.ToString());
                parameters.Add(string.Concat("@PlanId", i.ToString()), planIds[i]);
                if (i < planIds.Count - 1)
                {
                    query += ",";
                }
            }

            query += ") AND tg.available = 1;";

            Debug.WriteLine("Query: " + query);

            var teacherPlanModels = new List<TeacherPlanGroupStudentModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanModels = connection.Query<TeacherPlanGroupStudentModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return teacherPlanModels;
        }

        /// <summary>
        /// To get the teacher plan with group's details.
        /// </summary>
        /// <param name="teacherPlanIds">The teacher plan id.</param>
        /// <param name="subject">Optional: The subject code to filter by subject.</param>
        /// <param name="level">Optional: The level code to filter by level.</param>
        /// <returns>Returns a list of teacher plans with group details model.</returns>
        public static List<TeacherPlanFullDetailsModel> GetTeacherPlanFullDetails(List<string> teacherPlanIds, string subject = null, string level = null)
        {

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            DynamicParameters parameters = new DynamicParameters();
            string queryIn = " IN (";
            for (int i = 0; i < teacherPlanIds.Count; i++)
            {
                queryIn += string.Concat("@teacherPlanId", i.ToString());
                parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                if (i < teacherPlanIds.Count - 1)
                {
                    queryIn += ",";
                }
            }
            queryIn += ") ";

            string querySubectLevel = "";
            if (!string.IsNullOrWhiteSpace(subject))
            {
                querySubectLevel += " AND pl.subject_code = @subjectCode";
                parameters.Add("@subjectCode", subject);
            }

            if (!string.IsNullOrWhiteSpace(level))
            {
                querySubectLevel += " AND pl.level_code = @levelCode";
                parameters.Add("@levelCode", level);
            }


            string queryPlan =
		@"SELECT pl.id as `plan_id`, pl.name as `plan_name`, pl.user_id as `teacher_id`, pl.available as `plan_available`, pl.subject_code, pl.level_code, pl.period_id, pl.opened as `is_plan_open`,
                pl.open_date, pl.close_date, pl.week_number, pl.timeframe
                
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                WHERE pl.available = 1 AND pl.opened = 1 AND pl.id " + queryIn + querySubectLevel + ";";

            string queryHomeworks =
                @"SELECT pq.id, pq.quiz_id, qui.title, pq.plan_id, pq.tries_quantity, pq.available, pq.due_date, pq.for_grading, 
                 (SELECT SUM(custom_value) 
                 FROM gs_teachers_web_db_prod.quiz_questions 
                 WHERE quiz_id = pq.quiz_id) AS total_max_score
                FROM gs_teachers_web_db_prod.teacher_plan_quizs pq 
                INNER JOIN `gs_teachers_web_db_prod`.`quizs` qui ON qui.id = pq.quiz_id 
                WHERE pq.available = 1 AND pq.plan_id " + queryIn + ";";

            string queryPlanLessons =
                @"SELECT pl.id, pl.lesson_id, pl.plan_id, pl.tries_quantity, pl.available, pl.due_date, pl.for_grading, pl.`pre_post_required`
                FROM gs_teachers_web_db_prod.teacher_plan_lessons pl 
                WHERE pl.available = 1 AND pl.plan_id " + queryIn + ";";

            IEnumerable<Domain.Models.Generals.Dapper.TeacherPlanModel> planModels = null;
            IEnumerable<HomeworkAssignmentModel> planHomeworks = null;
            IEnumerable<LessonAssignmentModel> planLessons = null;
            List<TeacherPlanFullDetailsModel> teacherPlanFullDetailModels = new List<TeacherPlanFullDetailsModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planModels = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanModel>(queryPlan, parameters).ToList();
                planHomeworks = connection.Query<HomeworkAssignmentModel>(queryHomeworks, parameters).ToList();
                planLessons = connection.Query<LessonAssignmentModel>(queryPlanLessons, parameters).ToList();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Dispose();
            }


            foreach (Domain.Models.Generals.Dapper.TeacherPlanModel teacherPlan in planModels)
            {
                teacherPlanFullDetailModels.Add(new Domain.Models.Generals.Dapper.TeacherPlanFullDetailsModel()
                {
                    TeacherPlanModel = teacherPlan,
                    HomeworkAssignmentModels = planHomeworks.Where(x => x.PlanId == teacherPlan.PlanId)?.ToList(),
                    LessonAssignmentModels = planLessons.Where(x => x.PlanId == teacherPlan.PlanId)?.ToList(),
                });
            }

            // Call the dates of the lesson, and also call the lesson quizzes. 


            return teacherPlanFullDetailModels;

        }

        /// <summary>
        /// To get the teacher plan with group's details.
        /// </summary>
        /// <param name="teacherPlanIds">The teacher plans ids.</param>
        /// <param name="studentIds">The students' ids list.</param>
        /// <param name="teacherId">The reacher's id.</param>
        /// <param name="periodId">The period's id.</param>
        /// <returns>Returns a list of teacher plans with group details model.</returns>
        public static List<TeacherPlanGroupStudentModel> GetTeacherPlanWithGroupDetails(List<string> teacherPlanIds = null, List<string> studentIds = null, string teacherId = null, string periodId = null, string subjectCode = null, string levelCode = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.id as `plan_id`, pl.name as `plan_name`, pl.user_id as `teacher_id`, 
                pl.available as `plan_available`, pl.subject_code, pl.level_code, pl.period_id, pl.opened as `is_plan_open`, 
                plgr.id as `teacher_plan_group_id`, gro.id as `group_id`, gro.name as `group_name`, gs.id as `teacher_group_student_id`, gs.student_id
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                LEFT JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1 ";

            if (teacherPlanIds != null && teacherPlanIds.Count > 0)
            {
                query += " AND pl.id IN (";
                for (int i = 0; i < teacherPlanIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                    if (i < teacherPlanIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }


            if (studentIds != null && studentIds.Count > 0)
            {
                query += " AND gs.student_id IN (";
                for (int i = 0; i < studentIds.Count; i++)
                {
                    query += string.Concat("@studentId", i.ToString());
                    parameters.Add(string.Concat("@studentId", i.ToString()), studentIds[i]);
                    if (i < studentIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }
            if (!string.IsNullOrWhiteSpace(teacherId))
            {
                query += "AND  pl.user_id = @teacherId ";
                parameters.Add("@teacherId", teacherId);
            }

            if (!string.IsNullOrWhiteSpace(periodId))
            {
                query += "AND  pl.period_id = @periodId ";
                parameters.Add("@periodId", periodId);
            }

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += "AND  pl.subject_code = @subjectCode ";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                query += "AND  pl.level_code = @levelCode ";
                parameters.Add("@levelCode", levelCode);
            }


            IEnumerable<TeacherPlanGroupStudentModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherPlanGroupStudentModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return items != null ? items.ToList() : null;
        }

        /// <summary>
        /// To get the teacher plan with group's details.
        /// </summary>
        /// <param name="teacherPlanIds">The teacher plans ids.</param>
        /// <param name="studentIds">The students' ids list.</param>
        /// <param name="teacherId">The reacher's id.</param>
        /// <param name="periodId">The period's id.</param>
        /// <returns>Returns a list of teacher plans with group details model.</returns>
        public static List<TeacherPlanGroupStudentModel> GetTeacherPlanWithGroups(List<string> teacherPlanIds = null, List<string> studentIds = null, string teacherId = null, string periodId = null, string subjectCode = null, string levelCode = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.id as `plan_id`, pl.name as `plan_name`, pl.user_id as `teacher_id`, 
                pl.available as `plan_available`, pl.subject_code, pl.level_code, pl.period_id, pl.opened as `is_plan_open`, 
                plgr.id as `teacher_plan_group_id`, gro.id as `group_id`, gro.name as `group_name`
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1 ";

            if (teacherPlanIds != null && teacherPlanIds.Count > 0)
            {
                query += " AND pl.id IN (";
                for (int i = 0; i < teacherPlanIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                    if (i < teacherPlanIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (!string.IsNullOrWhiteSpace(teacherId))
            {
                query += "AND  pl.user_id = @teacherId ";
                parameters.Add("@teacherId", teacherId);
            }

            if (!string.IsNullOrWhiteSpace(periodId))
            {
                query += "AND  pl.period_id = @periodId ";
                parameters.Add("@periodId", periodId);
            }

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += "AND  pl.subject_code = @subjectCode ";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                query += "AND  pl.level_code = @levelCode ";
                parameters.Add("@levelCode", levelCode);
            }


            IEnumerable<TeacherPlanGroupStudentModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherPlanGroupStudentModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : null;
        }

        /// <summary>
        /// To get the teacher plan lesson quizzess by teacher plans.
        /// </summary>
        /// <param name="teacherPlanIds">The teacher plans.</param>
        /// <returns>Returns the teacher plan lesson quizzess.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> GetTeacherPlanLessonQuizzes(List<string> teacherPlanIds = null, List<string> teacherPlanLessonIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.id as plan_lesson_id,  
            pl.plan_id, pl.lesson_id, 
            pl.tries_quantity, 
            pl.due_date as `lesson_due_date`, 
            pl.for_grading,
            pl.`pre_post_required`,
            qui.id AS plan_lesson_quiz_id, 
            qui.quiz_id, 
            qui.due_date,
            qui.`due_date_timezone`,
            qui.`duration`
            FROM gs_teachers_web_db_prod.teacher_plan_lesson_quizs qui 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` pl ON pl.id = qui.plan_lesson_id
            WHERE pl.available = 1 
             ";

            if (teacherPlanIds != null && teacherPlanIds.Count > 0)
            {
                query += " AND pl.plan_id IN (";
                for (int i = 0; i < teacherPlanIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                    if (i < teacherPlanIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (teacherPlanLessonIds != null && teacherPlanLessonIds.Count > 0)
            {
                query += " AND pl.id IN (";
                for (int i = 0; i < teacherPlanLessonIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanLessonId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanLessonId", i.ToString()), teacherPlanLessonIds[i]);
                    if (i < teacherPlanLessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            query += ";";

            List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> teacherPlanLessonQuizModelLists = new List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanLessonQuizModelLists = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return teacherPlanLessonQuizModelLists;
        }

        /// <summary>
        /// To get the required teacher plan lesson quizzes by teacher plans.
        /// </summary>
        /// <param name="teacherPlanIds">The teacher plans.</param>
        /// <returns>Returns the teacher plan lesson quizzes.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>
            GetTeacherRequiredPlanLessonQuizzes(List<string> teacherPlanIds = null,
                List<string> teacherPlanLessonIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.id as plan_lesson_id,  
            pl.plan_id, pl.lesson_id, 
            pl.tries_quantity, 
            pl.due_date as `lesson_due_date`, 
            pl.`pre_post_required`,
            qui.id AS plan_lesson_quiz_id, 
            qui.quiz_id, 
            qui.due_date,
            qui.`due_date_timezone`,
            qui.`duration`,
            qui.for_grading,
            qui.required
            FROM gs_teachers_web_db_prod.teacher_plan_lesson_quizs qui 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` pl ON pl.id = qui.plan_lesson_id
            WHERE pl.available = 1  AND qui.required = 1 
             ";

            if (teacherPlanIds != null && teacherPlanIds.Count > 0)
            {
                query += " AND pl.plan_id IN (";
                for (int i = 0; i < teacherPlanIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                    if (i < teacherPlanIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            if (teacherPlanLessonIds != null && teacherPlanLessonIds.Count > 0)
            {
                query += " AND pl.id IN (";
                for (int i = 0; i < teacherPlanLessonIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanLessonId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanLessonId", i.ToString()), teacherPlanLessonIds[i]);
                    if (i < teacherPlanLessonIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            query += ";";

            List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel> teacherPlanLessonQuizModelLists =
                new List<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanLessonQuizModelLists = connection
                    .Query<Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return teacherPlanLessonQuizModelLists;
        }

        /// <summary>
        /// Simple query to get the teacher plan custom homework. The custom homework are the ones created by the teacher.
        /// </summary>
        /// <param name="teacherPlanIds">The teacher plan ids.</param>
        /// <returns>Returns the list of teacher plan custom homework.</returns>
        public static List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> GetTeacherPlanCustomHomework(List<string> teacherPlanIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pq.`id`, pq.`plan_id`, pq.`quiz_id`, pq.`tries_quantity`, pq.`available`, pq.`due_date`, pq.`for_grading`, pq.`modified_by`, pq.`created_at`, pq.`updated_at` , qui.title,
            (SELECT SUM(custom_value) 
             FROM gs_teachers_web_db_prod.quiz_questions 
             WHERE quiz_id = pq.quiz_id) AS total_max_score
            FROM `gs_teachers_web_db_prod`.`teacher_plan_quizs` pq 
            INNER JOIN gs_teachers_web_db_prod.quizs qui ON pq.quiz_id = qui.id
            WHERE pq.available = 1
             ";

            if (teacherPlanIds != null && teacherPlanIds.Count > 0)
            {
                query += " AND plan_id IN (";
                for (int i = 0; i < teacherPlanIds.Count; i++)
                {
                    query += string.Concat("@teacherPlanId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanId", i.ToString()), teacherPlanIds[i]);
                    if (i < teacherPlanIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            query += ";";

            List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> teacherPlanQuizModelLists = new List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                teacherPlanQuizModelLists = connection.Query<Domain.Models.Generals.Dapper.TeacherPlanQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return teacherPlanQuizModelLists;
        }

        /// <summary>
        /// To get the teacher plan lesson full details.
        /// </summary>
        /// <param name="planLessonId">The plan lesson id.</param>
        /// <returns>Returns the full details of the plan lesson</returns>
        public static TeacherPlanLessonFullDetailModel GetTeacherPlanLessonFullDetails(string planLessonId)
        {

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string queryPlan =
                @"SELECT DISTINCT `id`, `plan_id`, `lesson_id`, `tries_quantity`, `available`, `due_date`, `for_grading`, `pre_post_required`, `modified_by`,  `created_at`, `updated_at` FROM `gs_teachers_web_db_prod`.`teacher_plan_lessons` WHERE id = @planLessonId AND available = 1;";
            string queryLesson = @"SELECT `id`, `plan_lesson_id`, `for_grading`, `required`, `quiz_id`, `due_date`, `due_date_timezone`,`duration`, `modified_by`, `created_at`, `updated_at` FROM `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` WHERE plan_lesson_id = @planLessonId;";
            parameters.Add("@planLessonId", planLessonId);

            IEnumerable<TeacherPlanLessonModel> planLessonModels = null;
            IEnumerable<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel> planLessonQuizzessModels = null;

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planLessonModels = connection.Query<TeacherPlanLessonModel>(queryPlan, parameters);
                planLessonQuizzessModels = connection.Query<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel>(queryLesson, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {

                connection.Dispose();
            }

            return new Domain.Models.Generals.Responses.TeacherPlanLessonFullDetailModel()
            {
                TeacherPlanLessonModel = planLessonModels.First(),
                TeacherPlanLessonQuizModels = planLessonQuizzessModels.ToList(),
            };
        }

        /// <summary>
        /// To retrieve the plan lessons by plan ids.
        /// </summary>
        /// <param name="planIds">The ids of the plans.</param>
        /// <returns>Returns the plan lesson model.</returns>
        /// <param name="planLessonIds">(optional) Restricts the result to these plan lesson ids.</param>
        public static List<TeacherPlanLessonModel> GetPlanLessonsByPlanIds(List<int> planIds, List<int> planLessonIds = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            var query = @"SELECT DISTINCT ls.lesson_id, ls.due_date, 
                  ls.tries_quantity, ls.pre_post_required, ls.id, ls.plan_id
                  FROM gs_teachers_web_db_prod.teacher_plan_lessons ls
                  WHERE ls.available = 1 
                  AND ls.plan_id IN @planIds";
            parameters.Add("@planIds", planIds);

            if (planLessonIds != null)
            {
                query += " AND ls.id IN @planLessonIds";
                parameters.Add("@planLessonIds", planLessonIds);
            }

            var studentLessonAssignmentDetails = new List<TeacherPlanLessonModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                studentLessonAssignmentDetails = connection.Query<TeacherPlanLessonModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
                // Handle exception
            }
            finally
            {
                connection.Close();
            }

            return studentLessonAssignmentDetails;
        }

        /// <summary>
        /// To retrieve the ids of the plan lessons that have at least one required quiz due within the given range.
        /// Used by the student's calendar to narrow the whole pending calculation down to a month.
        /// </summary>
        /// <param name="planIds">The ids of the plans.</param>
        /// <param name="from">The start of the date range.</param>
        /// <param name="to">The end of the date range.</param>
        /// <returns>Returns the ids of the matching plan lessons</returns>
        public static List<int> GetPlanLessonIdsWithQuizzesDueWithin(List<int> planIds, DateTime from, DateTime to)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();

            var query = @"SELECT DISTINCT plq.plan_lesson_id
                  FROM gs_teachers_web_db_prod.teacher_plan_lesson_quizs plq
                  INNER JOIN gs_teachers_web_db_prod.teacher_plan_lessons ls
                          ON ls.id = plq.plan_lesson_id AND ls.available = 1
                  WHERE plq.required = 1
                  AND ls.plan_id IN @planIds
                  AND plq.due_date >= @from
                  AND plq.due_date < @to";

            parameters.Add("@planIds", planIds);
            parameters.Add("@from", from);
            parameters.Add("@to", to);

            var planLessonIds = new List<int>();
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                planLessonIds = connection.Query<int>(query, parameters).ToList();
            }
            catch (Exception)
            {
                // Handle exception
            }
            finally
            {
                connection.Close();
            }

            return planLessonIds;
        }

        /// <summary>
        /// To retrieve the plan lesson quizzes by plan ids.
        /// </summary>
        /// <param name="planLessonIds">Plan lesson ids to call.</param>
        /// <returns>Returns the plan lesson quiz models.</returns>
        public static List<TeacherPlanLessonQuizModel> GetPlanLessonQuizzesByPlanIds(List<int> planLessonIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            var query = @"SELECT plq.plan_lesson_id, plq.for_grading, plq.required, plq.quiz_id, plq.id, 
                    plq.due_date, plq.due_date_timezone, plq.duration
                  FROM gs_teachers_web_db_prod.teacher_plan_lesson_quizs plq
                  WHERE plq.required = 1 
                  AND plq.plan_lesson_id IN @planLessonIds";
            parameters.Add("@planLessonIds", planLessonIds);

            var studentLessonAssignmentDetails = new List<TeacherPlanLessonQuizModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                studentLessonAssignmentDetails = connection.Query<TeacherPlanLessonQuizModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
                // Handle exception
            }
            finally
            {
                connection.Close();
            }
            
            return studentLessonAssignmentDetails;
        }
        
        /// <summary>
        /// A query used for students to return the pending lessons.
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <returns>Return a list of pending homeworks.</returns>
        public static List<StudentLessonAssignmentDetail> GetStudentPendingLessons(string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT ls.lesson_id, pl.subject_code as 'subject', pl.level_code, ls.due_date, 
                ls.tries_quantity, ls.for_grading, ls.`pre_post_required`, pl.user_id as 'teacher_id', 
                ls.id as `teacher_plan_lesson_id`
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls ON ls.plan_id = pl.id
                LEFT JOIN `gs_teachers_web_db_prod`.`plan_lesson_users` lu ON lu.plan_lesson_id = ls.id  AND lu.user_id = @studentId
                WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1
                AND ls.available = 1 AND (lu.completed = 0 OR lu.completed IS NULL)
                AND gs.student_id = @studentId;";
            parameters.Add("@studentId", studentId);

            List<StudentLessonAssignmentDetail> studentLessonAssignmentDetails = new List<StudentLessonAssignmentDetail>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                studentLessonAssignmentDetails = connection.Query<StudentLessonAssignmentDetail>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            // Now, retrieve the pending exam details. 
            List<string> planLessonIds = new List<string>();
            foreach(var studentLessonAssignment in studentLessonAssignmentDetails)
            {
                planLessonIds.Add(studentLessonAssignment.TeacherPlanLessonId);
            }

            if(planLessonIds.Count > 0)
            {
                parameters = new DynamicParameters();
                string queryExams = @"SELECT DISTINCT lsq.id, lsq.quiz_id, 
                lsq.due_date, lsq.due_date_timezone, lsq.duration, ls.id as `plan_lesson_id`,
                CASE WHEN lsqr.id IS NOT NULL THEN 1 ELSE 0 END as 'completed'
                FROM `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` lsq ON lsq.plan_lesson_id = ls.id
                LEFT JOIN  `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` lsqr ON lsqr.plan_lesson_id = ls.id 
                AND lsqr.quiz_id = lsq.quiz_id AND lsqr.user_id = @studentId   
                WHERE ls.id IN (";
                parameters.Add("@studentId", studentId);

                for (int i = 0; i < planLessonIds.Count; i++)
                {
                    queryExams += string.Concat("@teacherPlanLessonId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanLessonId", i.ToString()), planLessonIds[i]);
                    if (i < planLessonIds.Count - 1)
                    {
                        queryExams += ",";
                    }
                }
                queryExams += ");";

                List<StudentExamModel> studentExams = new List<StudentExamModel>();
                connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
                };
                connection.Open();
                try
                {
                    studentExams = connection.Query<StudentExamModel>(queryExams, parameters).ToList();
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }

                if(studentExams != null && studentExams.Count > 0)
                {
                    // Now, relate everything. 
                    foreach (var studentLessonAssignment in studentLessonAssignmentDetails)
                    {
                        studentLessonAssignment.StudentExamModels.AddRange(studentExams.FindAll(x => x.PlanLessonId == studentLessonAssignment.TeacherPlanLessonId));
                    }
                }
            }

            return studentLessonAssignmentDetails;
        }

        /// <summary>
        /// A query used for students to return the pending lessons.
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="subjectCode">The subject code. Example: mat-sp, en, es...</param>
        /// <param name="levelCode">The level code. Example: k, 1, 3, etc...</param>
        /// <param name="applyFilterOutLessonsThatHasBeenAssignedLessThanToday">To filter out lessons that have not been assigned.</param>
        /// <returns>Return a list of pending homeworks.</returns>
        public static List<StudentLessonAssignmentDetail> GetStudentPendingLessonsQuizzes(string studentId,
            bool applyFilterOutLessonsThatHasBeenAssignedLessThanToday, string subjectCode = null,
            string levelCode = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT ls.lesson_id, pl.subject_code as 'subject', pl.level_code, ls.due_date, 
                ls.tries_quantity, plq.for_grading, plq.required, ls.`pre_post_required`, pl.user_id as 'teacher_id', 
                ls.id as `teacher_plan_lesson_id`
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls ON ls.plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` plq ON plq.plan_lesson_id = ls.id 
                LEFT JOIN `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` plqur ON plqur.quiz_id = plq.quiz_id 
    AND plqur.plan_lesson_id = plq.plan_lesson_id AND gs.student_id = plqur.user_id
                WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1  
                AND ls.available = 1 AND plq.required = 1 
                AND plqur.id IS NULL
                AND gs.student_id = @studentId  ";
            parameters.Add("@studentId", studentId);

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += " AND pl.subject_code = @subjectCode ";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                query += " AND pl.level_code = @levelCode ";
                parameters.Add("@levelCode", levelCode);
            }

            if (applyFilterOutLessonsThatHasBeenAssignedLessThanToday)
            {
                query += " AND ls.due_date <= CAST(CONCAT(CURDATE(), ' 00:00:00') AS DATETIME) ";
                query +=
                    " AND DATE_ADD(plq.due_date, INTERVAL (plq.duration + 5) MINUTE) >= DATE_SUB(CAST(CONCAT(CURDATE(), ' 00:00:00') AS DATETIME), INTERVAL 5 DAY) ";
            }

            List<StudentLessonAssignmentDetail> studentLessonAssignmentDetails =
                new List<StudentLessonAssignmentDetail>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                studentLessonAssignmentDetails =
                    connection.Query<StudentLessonAssignmentDetail>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            // Now, retrieve the pending exam details. 
            List<string> planLessonIds = new List<string>();
            foreach (var studentLessonAssignment in studentLessonAssignmentDetails)
            {
                planLessonIds.Add(studentLessonAssignment.TeacherPlanLessonId);
            }

            if (planLessonIds.Count > 0)
            {
                parameters = new DynamicParameters();
                string queryExams = @"SELECT DISTINCT lsq.id, lsq.quiz_id, 
                lsq.required, lsq.for_grading,
                lsq.due_date, lsq.due_date_timezone, lsq.duration, ls.id as `plan_lesson_id`,
                CASE WHEN lsqr.id IS NOT NULL THEN 1 ELSE 0 END as 'completed'
                FROM `gs_teachers_web_db_prod`.`teacher_plan_lessons` ls
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` lsq ON lsq.plan_lesson_id = ls.id
                LEFT JOIN  `gs_teachers_web_db_prod`.`plan_lesson_quiz_user_results` lsqr ON lsqr.plan_lesson_id = ls.id 
                AND lsqr.quiz_id = lsq.quiz_id AND lsqr.user_id = @studentId   
                WHERE lsq.required = '1' AND ls.id IN (";
                parameters.Add("@studentId", studentId);

                for (int i = 0; i < planLessonIds.Count; i++)
                {
                    queryExams += string.Concat("@teacherPlanLessonId", i.ToString());
                    parameters.Add(string.Concat("@teacherPlanLessonId", i.ToString()), planLessonIds[i]);
                    if (i < planLessonIds.Count - 1)
                    {
                        queryExams += ",";
                    }
                }

                queryExams += ");";

                List<StudentExamModel> studentExams = new List<StudentExamModel>();
                connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
                };
                connection.Open();
                try
                {
                    studentExams = connection.Query<StudentExamModel>(queryExams, parameters).ToList();
                }
                catch (Exception)
                {
                }
                finally
                {
                    connection.Close();
                }

                if (studentExams != null && studentExams.Count > 0)
                {
                    // Removed duplicates.
                    studentLessonAssignmentDetails = studentLessonAssignmentDetails.GroupBy(x => x.TeacherPlanLessonId)
                        .Select(x => x.First()).ToList();

                    // Now, relate everything. 
                    foreach (var studentLessonAssignment in studentLessonAssignmentDetails)
                    {
                        // Check if lesson does not exist in the list.
                        var lessonCheckIfExists = studentLessonAssignment.StudentExamModels.Find(x => x.LessonId == studentLessonAssignment.LessonId && x.PlanLessonId == studentLessonAssignment.TeacherPlanLessonId);

                        if (lessonCheckIfExists == null)
                            studentLessonAssignment.StudentExamModels.AddRange(studentExams.FindAll(x => x.PlanLessonId == studentLessonAssignment.TeacherPlanLessonId));
                    }
                }
            }

            return studentLessonAssignmentDetails;
        }

        /// <summary>
        /// A query used for students to return the pending homework. (homework are created by teacher).
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <returns>Return a list of pending homeworks.</returns>
        public static List<StudentHomeworkDetail> GetStudentHomework(string studentId, string subjectCode, string levelCode, string pendingOrComplete, string planId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT qu.id AS 'teacher_plan_quiz_id', qu.plan_id, CASE WHEN quire.id IS NULL THEN 'P' ELSE 'C' END as `status`, 
                qui.title,  qu.quiz_id, pl.subject_code as 'subject', pl.level_code, 
                qu.due_date, qu.tries_quantity, qu.for_grading, pl.user_id as 'teacher_id', re.created_at, quire.total_percent as 'total', quire.id as 'user_quiz_result_id', 
                quire.created_at as `submitted_date`, quire.total_percent
                FROM gs_teachers_web_db_prod.teacher_plans pl 
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_quizs` qu ON qu.plan_id = pl.id
                INNER JOIN `gs_teachers_web_db_prod`.`quizs` qui ON qui.id = qu.quiz_id
                LEFT JOIN `gs_teachers_web_db_prod`.`plan_assignment_user_results` re ON re.plan_quiz_id = qu.id AND re.user_id = @studentId
                LEFT JOIN `gs_teachers_web_db_prod`.`user_quiz_results` quire ON quire.quiz_id = qui.id AND quire.user_id = @studentId
                WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1
                AND qu.available = 1 ";

            if (pendingOrComplete.ToLower() == "p")
            {
                query += " AND quire.id IS NULL ";
            }
            else if (pendingOrComplete.ToLower() == "c")
            {
                query += " AND re.id IS NOT NULL ";
            }

            query += " AND gs.student_id = @studentId ";
            parameters.Add("@studentId", studentId);

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += " AND pl.subject_code = @subjectCode";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                query += " AND pl.level_code = @levelCode";
                parameters.Add("@levelCode", levelCode);
            }

            if (!string.IsNullOrWhiteSpace(planId))
            {
                query += " AND pl.id = @planId";
                parameters.Add("@planId", planId);
            }

            query += ";";

            List<StudentHomeworkDetail> studnetHomeworkDetails = new List<StudentHomeworkDetail>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                studnetHomeworkDetails = connection.Query<StudentHomeworkDetail>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return studnetHomeworkDetails;
        }

        /// <summary>
        /// A query used for students to return the pending homework. (homework are created by teacher).
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="filterOutPastDates">To filter the ones that has expired.</param>
        /// <param name="pendingOrComplete">Pending or completed.</param>
        /// <param name="planId">The id of the plan.</param>
        /// <param name="from">(optional)The start of date the range.</param>
        /// <param name="to">(optional) The end of the date range.</param>
        /// <returns>Return a list of pending homeworks.</returns>
        public static List<StudentHomeworkDetail> GetStudentHomework(string studentId, string subjectCode,
            string levelCode, string pendingOrComplete, bool filterOutPastDates, string planId = null, DateTime? from = null, DateTime? to = null)
        {
            var studentOpenPlans = GetStudentOpenPlans(studentId, subjectCode, levelCode, planId);
            if (studentOpenPlans.Count == 0)
            {
                return new List<StudentHomeworkDetail>();
            }

            // Get the plan quizs by plan ids.
            var planIds = new List<int>();
            foreach (var studentOpenPlan in studentOpenPlans)
            {
                planIds.Add(studentOpenPlan.PlanId);
            }

            if (planIds.Count == 0)
            {
                return new List<StudentHomeworkDetail>();
            }

            var studentPlanQuizzes = GetPlanQuizByPlanIds(planIds, filterOutPastDates, from, to);
            if (studentPlanQuizzes.Count == 0)
            {
                return new List<StudentHomeworkDetail>();
            }

            // Get the plan quiz results.
            var studentPlanQuizResults = GetPlanQuizIdResults(
                studentPlanQuizzes.Select(x => int.Parse(x.TeacherPlanQuizId)).ToList(),
                studentId);

            // Now, relate everything.
            var studentHomeworkDetails = new List<StudentHomeworkDetail>();

            foreach (var studentPlanQuiz in studentPlanQuizzes)
            {
                var planDetails = studentOpenPlans.Find(x => x.PlanId == studentPlanQuiz.PlanId);
                var studentPlanQuizResult =
                    studentPlanQuizResults.Find(x => x.TeacherPlanQuizId == studentPlanQuiz.TeacherPlanQuizId);
                var studentHomeWorkDetail = new StudentHomeworkDetail()
                {
                    TeacherPlanQuizId = studentPlanQuiz.TeacherPlanQuizId,
                    PlanId = studentPlanQuiz.PlanId,
                    Subject = planDetails.Subject,
                    Title = studentPlanQuiz.Title,
                    LevelCode = planDetails.LevelCode,
                    PlanSubject = planDetails.Subject,
                    DueDate = studentPlanQuiz.DueDate,
                    TriesQuantity = studentPlanQuiz.TriesQuantity,
                    ForGrading = studentPlanQuiz.ForGrading,
                    TeacherId = planDetails.TeacherId,
                    Score = studentPlanQuizResult?.Score,
                    TotalPercent = studentPlanQuizResult?.TotalPercent ?? 0,
                    SubmittedDate = studentPlanQuizResult?.SubmittedDate,
                    Status = studentPlanQuizResult == null ? "P" : studentPlanQuizResult.Status,
                    UserQuizResultId = studentPlanQuizResult?.UserQuizResultId,
                    QuizId = studentPlanQuiz.QuizId,
                };
                
                if(pendingOrComplete == "P" && studentHomeWorkDetail.Status == "P" && studentHomeWorkDetail.UserQuizResultId == null)
                   studentHomeworkDetails.Add(studentHomeWorkDetail);
                
                if(pendingOrComplete == "C" && studentHomeWorkDetail.Status == "C")
                    studentHomeworkDetails.Add(studentHomeWorkDetail);
            }

            return studentHomeworkDetails;
        }

        /// <summary>
        /// To retrieve student forums without library dependency.
        /// </summary>
        /// <param name="studentId">The student's id.</param>
        /// <param name="subjectCode">The subject's code (optional).</param>
        /// <param name="levelCode">The level's code (optional).</param>
        /// <returns>Returns the list of forums.</returns>
        public static List<Domain.Models.Generals.Models.ForumModel> GetStudentForums(string studentId, string subjectCode = null, string levelCode = null, string forumId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.subject_code, pl.level_code, fo.id, fo.teacher_plan_id, fo.title, fo.active, fo.author, fo.description, fo.modified_by, fo.created_at
            FROM gs_teachers_web_db_prod.teacher_plans pl
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id
            INNER JOIN `gs_teachers_web_db_prod`.`forums` fo ON fo.teacher_plan_id = pl.id
            WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1 AND fo.active = 1 AND gs.student_id = @studentId";
            parameters.Add("@studentId", studentId);

            if (!string.IsNullOrWhiteSpace(forumId))
            {
                query += " AND fo.id = @forumId";
                parameters.Add("@forumId", forumId);
            }

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += " AND pl.subject_code = @subjectCode";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                query += " AND pl.level_code = @levelCode";
                parameters.Add("@levelCode", levelCode);
            }

            query += " ORDER BY fo.created_at DESC;";

            List<Domain.Models.Generals.Models.ForumModel> forums = new List<Domain.Models.Generals.Models.ForumModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                forums = connection.Query<Domain.Models.Generals.Models.ForumModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return forums;
        }

        /// <summary>
        /// To check if user can post in forum.
        /// </summary>
        /// <param name="forumId">The id of the forum.</param>
        /// <returns>Returns if the forum is active or not.</returns>
        public static bool CheckIfForumActive(string forumId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT fo.id
            FROM gs_teachers_web_db_prod.teacher_plans pl
            INNER JOIN `gs_teachers_web_db_prod`.`forums` fo ON fo.teacher_plan_id = pl.id
            WHERE pl.opened = 1 AND pl.available = 1 AND fo.active = 1 AND fo.id = @forumId";
            parameters.Add("@forumId", forumId);

            List<Domain.Models.Generals.Models.ForumModel> forums = new List<Domain.Models.Generals.Models.ForumModel>();

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                forums = connection.Query<Domain.Models.Generals.Models.ForumModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return forums != null && forums.Count > 0;
        }

        /// <summary>
        /// To get the student open plans.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="subjectCode">The subject's code (optional).</param>
        /// <param name="levelCode">The level's code (optional).</param>
        /// <param name="planId">The ID of the plan.</param>
        /// <returns>Returns the teacher open plans.</returns>
        public static List<StudentLessonAssignmentDetail> GetStudentOpenPlans(string studentId,
            string subjectCode = null, string levelCode = null, string planId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            string query = @"SELECT DISTINCT pl.subject_code as 'subject', pl.level_code, 
            pl.user_id as 'teacher_id', pl.id as plan_id
            FROM gs_teachers_web_db_prod.teacher_plans pl 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_groups` plgr ON plgr.teacher_plan_id = pl.id
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` gro ON gro.id = plgr.group_id 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_group_students` gs ON gs.teacher_group_id = gro.id 
            WHERE gro.available = 1 AND pl.opened = 1 AND pl.available = 1 
            AND gs.student_id = @studentId ";
            parameters.Add("@studentId", studentId);

            if (!string.IsNullOrWhiteSpace(subjectCode))
            {
                query += " AND pl.subject_code = @subjectCode";
                parameters.Add("@subjectCode", subjectCode);
            }

            if (!string.IsNullOrWhiteSpace(levelCode))
            {
                query += " AND pl.level_code = @levelCode";
                parameters.Add("@levelCode", levelCode);
            }

            if (!string.IsNullOrWhiteSpace(planId))
            {
                query += " AND pl.id = @planId";
                parameters.Add("@planId", planId);
            }

            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            var studentAssesmentDetails = new List<StudentLessonAssignmentDetail>();
            connection.Open();
            try
            {
                studentAssesmentDetails = connection.Query<StudentLessonAssignmentDetail>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return studentAssesmentDetails;
        }

        /// <summary>
        /// To get the plan quiz id by plan ids.
        /// </summary>
        /// <param name="planIds">The id of the plans.</param>
        /// <param name="filterOutPastDates">To filter out past dates.</param>
        /// <param name="from">(optional)The start of date the range.</param>
        /// <param name="to">(optional) The end of date the range.</param>
        /// <returns>Return the list of student lessons.</returns>
        private static List<StudentHomeworkDetail> GetPlanQuizByPlanIds(List<int> planIds, bool filterOutPastDates, DateTime? from = null, DateTime? to = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            var query = @"SELECT DISTINCT qu.id AS 'teacher_plan_quiz_id', qu.plan_id, 
                qui.title,  qu.quiz_id, 
                qu.due_date, qu.tries_quantity,
                qu.for_grading
                FROM `gs_teachers_web_db_prod`.`teacher_plan_quizs` qu 
                INNER JOIN `gs_teachers_web_db_prod`.`quizs` qui ON qui.id = qu.quiz_id
                WHERE qu.available = 1 AND qu.plan_id IN @planIds ";
            parameters.Add("@planIds", planIds);
            if (filterOutPastDates)
            {
                query += " AND qu.due_date >= DATE_SUB(NOW(), INTERVAL 12 HOUR) ";
            }

            if (from.HasValue && to.HasValue)
            {
                query += " AND qu.due_date >= @from AND qu.due_date < @to ";
                parameters.Add("@from", from.Value);
                parameters.Add("@to", to.Value);
            }

            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            var studentAssesmentDetails = new List<StudentHomeworkDetail>();
            connection.Open();
            try
            {
                studentAssesmentDetails = connection.Query<StudentHomeworkDetail>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return studentAssesmentDetails;
        }

		public static Dictionary<string, Object> GetPlanningLessonsWithQuizzes(string planningId, int? limit = null, int? pageNumber = null)
		{
			DefaultTypeMap.MatchNamesWithUnderscores = true;
			DynamicParameters parameters = new DynamicParameters();
			List<TeacherPlanLessonFullDetailModel> response = new List<TeacherPlanLessonFullDetailModel>();
            var result = new Dictionary<string, Object>();

			string queryPlan = @"SELECT DISTINCT le.`id`,le.`plan_id`,le.`lesson_id`,le.`tries_quantity`,le.`available`,le.`due_date`,le.`for_grading`,le.`pre_post_required`,le.`modified_by`, le.`created_at`,le.`updated_at` 
								FROM `gs_teachers_web_db_prod`.`teacher_plans` pl
								INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lessons` le ON le.plan_id = pl.id
								WHERE pl.id = @planningId AND pl.available = 1 AND le.available = 1;";

			string queryLesson = @"SELECT q.`id`, q.`plan_lesson_id`, q.`for_grading`, q.`required`, q.`quiz_id`, q.`due_date`, q.`due_date_timezone`,q.`duration`, q.`modified_by`, q.`created_at`, q.`updated_at` 
								FROM `gs_teachers_web_db_prod`.`teacher_plan_lessons` le
								INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` q ON q.plan_lesson_id = le.id
                                WHERE le.id IN ({plan_lesson_id})
                                ORDER BY q.`id`
                                LIMIT @limit OFFSET @offset;";

            string queryTotalCount = @"SELECT COUNT(*) 
                                FROM `gs_teachers_web_db_prod`.`teacher_plan_lessons` le
                                INNER JOIN `gs_teachers_web_db_prod`.`teacher_plan_lesson_quizs` q ON q.plan_lesson_id = le.id
                                WHERE le.id IN ({plan_lesson_id});";


            parameters.Add("@planningId", planningId);
            parameters.Add("@limit", limit ?? int.MaxValue);
            parameters.Add("@offset", (limit.HasValue && pageNumber.HasValue) ? limit.Value * pageNumber.Value : 0);

            List<TeacherPlanLessonModel> planLessonModels = null;
			IEnumerable<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel> planLessonQuizzessModels = null;

			MySqlConnection connection = new MySqlConnection
			{
				ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
			};
			connection.Open();
			try
			{
				planLessonModels = connection.Query<TeacherPlanLessonModel>(queryPlan, parameters)?.ToList();

				string queryIn = "";

                if (planLessonModels != null && planLessonModels.Count > 0)
                {

                    for (int index = 0; index < planLessonModels.Count; index++)
                    {
                        var planLesson = planLessonModels[index];

                        queryIn += string.Concat("@planningLessonId", index.ToString());
                        parameters.Add(string.Concat("@planningLessonId", index.ToString()), planLesson.Id);

                        if (index < planLessonModels.Count - 1)
                            queryIn += ",";
                    }

                    planLessonQuizzessModels = connection.Query<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel>(queryLesson.Replace("{plan_lesson_id}", queryIn), parameters);

                    int totalQuizzes = connection.ExecuteScalar<int>(queryTotalCount.Replace("{plan_lesson_id}", queryIn), parameters);

                    var relevantLessonIds = planLessonQuizzessModels.Select(q => q.PlanLessonId).Distinct().ToList();
                    var filteredLessons = planLessonModels.Where(l => relevantLessonIds.Contains(l.Id)).ToList();


                    foreach (var planLesson in filteredLessons)
                    {
                        response.Add(new TeacherPlanLessonFullDetailModel()
                        {
                            TeacherPlanLessonModel = planLesson,
                            TeacherPlanLessonQuizModels = planLessonQuizzessModels.Where(q => q.PlanLessonId == planLesson.Id).ToList(),
                        });
                    }
                    ;

                    result.Add("QuizzesItems", response);
                    result.Add("TotalQuizzes", totalQuizzes);
                }
                else
                {
                    result.Add("QuizzesItems", new List<TeacherPlanLessonFullDetailModel>());
                    result.Add("TotalQuizzes", 0);
                }
			}
			catch (Exception)
			{

			}
			finally
			{
				connection.Dispose();
			}

			return result;
		}

        /// <summary>
        /// To get the plan quiz id results.
        /// </summary>
        /// <param name="planQuizIds">The id of the plan quizzes.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="pendingOrComplete">Pending or complete.</param>
        /// <returns>Returns the list of student results.</returns>
        private static List<StudentHomeworkDetail> GetPlanQuizIdResults(List<int> planQuizIds, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            var query = @"SELECT DISTINCT  re.plan_quiz_id as 'teacher_plan_quiz_id', 
			CASE WHEN quire.id IS NULL THEN 'P' ELSE 'C' END as `status`, 
			re.created_at, quire.total_percent as 'total', quire.id as 'user_quiz_result_id', 
			quire.created_at as `submitted_date`, quire.total_percent
			FROM  `gs_teachers_web_db_prod`.`plan_assignment_user_results` re 
			LEFT JOIN `gs_teachers_web_db_prod`.`user_quiz_results` quire ON quire.id = re.usqui_user_result_id
			WHERE re.user_id = @studentId AND re.plan_quiz_id IN @planQuizIds ";

            parameters.Add("@studentId", studentId);
            parameters.Add("@planQuizIds", planQuizIds);
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            var studentAssessmentDetails = new List<StudentHomeworkDetail>();
            connection.Open();
            try
            {
                studentAssessmentDetails = connection.Query<StudentHomeworkDetail>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return studentAssessmentDetails;
        }
    }
}