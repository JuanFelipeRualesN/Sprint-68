using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using TeachersAPI.Domain.Models.Generals.Dapper;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle queries for teacher groups with Dapper for performance improvement.
    /// </summary>
    public static class TeacherGroups
    {
        /// <summary>
        /// To get the teacher's groups.
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <param name="teacherGroupId">Optional: The teacher's group id.</param>
        /// <returns>Returns the teacher group student model list.</returns>
        public static List<TeacherGroupStudentModel> GetTeacherGroups(string teacherId, string teacherGroupId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { teacherId, teacherGroupId, available = true }; 
            string query = @"SELECT DISTINCT gr.id as `group_id`, gr.teacher_id, gr.subject_code, gr.level_code, gr.name as `group_name`, gr.available, 
                st.id as `teacher_group_student_id`, st.student_id
                FROM gs_teachers_web_db_prod.teacher_groups gr 
                LEFT JOIN gs_teachers_web_db_prod.teacher_group_students st ON st.teacher_group_id = gr.id 
                WHERE 
                gr.teacher_id = @teacherId 
                AND gr.available = @available";

            if (!string.IsNullOrWhiteSpace(teacherGroupId))
            {
                query += " AND gr.id = @teacherGroupId";
            }

            query += ";";

            IEnumerable<TeacherGroupStudentModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherGroupStudentModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<TeacherGroupStudentModel>();
        }

        /// <summary>
        /// To get the groups (with their students) created by a set of teachers
        /// Used by a director to list the groups of all of their teachers.
        /// </summary>
        /// <param name="teacherIds">The teachers' ids.</param>
        /// <returns>Returns the teacher group student model list across all the given teachers.</returns>
        public static List<TeacherGroupStudentModel> GetTeacherGroupsByTeachers(List<string> teacherIds)
        {
            if (teacherIds == null || teacherIds.Count == 0)
            {
                return new List<TeacherGroupStudentModel>();
            }

            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { teacherIds, available = true };

            string query = @"SELECT DISTINCT gr.id as `group_id`, gr.teacher_id, gr.subject_code, gr.level_code, gr.name as `group_name`, gr.available,
                st.id as `teacher_group_student_id`, st.student_id
                FROM gs_teachers_web_db_prod.teacher_groups gr
                LEFT JOIN gs_teachers_web_db_prod.teacher_group_students st ON st.teacher_group_id = gr.id
                WHERE
                gr.teacher_id IN @teacherIds
                AND gr.available = @available;";

            IEnumerable<TeacherGroupStudentModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherGroupStudentModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<TeacherGroupStudentModel>();
        }

        /// <summary>
        /// To get the students of a teacher plan.
        /// </summary>
        /// <param name="planId">The id of the teacher's plan.</param>
        /// <param name="teacherGroupId">The group's id.</param>
        /// <returns>Returns a list of teacher group student model.</returns>
        public static List<TeacherGroupStudentModel> GetTeacherPlanStudents(string planId, string teacherGroupId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new { planId, teacherGroupId };
            string query = @"SELECT grs.student_id, plgr.group_id, tg.name as group_name
            FROM gs_teachers_web_db_prod.teacher_plan_groups plgr 
            INNER JOIN `gs_teachers_web_db_prod`.`teacher_groups` tg ON tg.id = plgr.group_id
            INNER JOIN gs_teachers_web_db_prod.teacher_group_students grs ON plgr.group_id = grs.teacher_group_id
            WHERE  plgr.teacher_plan_id = @planId 
            ";

            if (!string.IsNullOrWhiteSpace(teacherGroupId))
            {
                query += " AND plgr.group_id = @teacherGroupId";
            }

            query += ";";

            IEnumerable<TeacherGroupStudentModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherGroupStudentModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<TeacherGroupStudentModel>();
        }
    }
}