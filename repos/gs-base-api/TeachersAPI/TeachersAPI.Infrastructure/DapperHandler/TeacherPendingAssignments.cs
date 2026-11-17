using Dapper;
using TeachersAPI.Domain.Models.Generals.Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle the teacher pending assignments with Dapper.
    /// </summary>
    public static class TeacherPendingAssignments
    {
        /// <summary>
        /// To get the student pending lesson quizzes.
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <returns>Returns a list of pending assignments model.</returns>
        public static List<TeacherPendingAssignmentsModel> GetTeacherPendingAssignments(string teacherId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { teacherId };
            string query = @"select 
	                            tp.id as plan_id, tpq.id as plan_quiz_id, tp.user_id as teacher_id, tp.name as plan_name, 
                                lq.title as assignment_name,
	                            paur.usqui_user_result_id as assignment_id, 
                                uqr.user_id as student_id,
                                paur.created_at as date_submitted
                            from gs_teachers_web_db_prod.teacher_plans tp 
	                            inner join gs_teachers_web_db_prod.teacher_plan_quizs tpq
		                            on tp.id = tpq.plan_id 
                                    and tp.available = 1 #plan no archivado
                                    and tp.opened = 1 #plan abierto
                                    and tpq.available = 1
	                            inner join gs_teachers_web_db_prod.plan_assignment_user_results paur
		                            on paur.plan_quiz_id = tpq.id
	                            inner join gs_teachers_web_db_prod.user_quiz_results uqr
		                            on uqr.id = paur.usqui_user_result_id 
                                    and uqr.status = 'P' #Pendiente corregir
	                            inner join gs_teachers_web_db_prod.quizs lq
		                            on lq.id = tpq.quiz_id
                            where tp.user_id = @teacherId;";

            IEnumerable<TeacherPendingAssignmentsModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherPendingAssignmentsModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<TeacherPendingAssignmentsModel>();
        }
    }
}