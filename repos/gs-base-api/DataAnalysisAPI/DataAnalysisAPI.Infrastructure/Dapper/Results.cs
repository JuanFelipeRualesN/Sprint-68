using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using DataAnalysisAPI.Utils;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper
{
    /// <summary>
    /// To handle the results queries.
    /// </summary>
    public static class Results
    {
        /// <summary>
        /// To read the results. 
        /// </summary>
        /// <param name="dateFrom">Optional: The date from to include periods.</param>
        /// <param name="dateTo">Optional: The date to to include periods.</param>
        /// <param name="schoolIds">Optional: The ids of the schools.</param>
        /// <param name="userIds">Optional: The ids of the users.</param>
        /// <param name="subjectCodes">Optional: The list of the subject codes.</param>
        /// <param name="levelCodes">Optional: A list of level codes.</param>
        /// <param name="sie">An specific SIE (record).</param>
        /// <param name="academicYearId">The academic year id.</param>
        /// <param name="teacherId">The id of the teacher.</param>
        /// <returns>Returns the list of connected dates.</returns>
        public static List<ResultGenericModel> GetResultGenerics(DateTime? dateFrom, DateTime? dateTo, List<int> schoolIds = null,
        List<int> userIds = null, List<string> subjectCodes = null, List<string> levelCodes = null, string sie = null, int? academicYearId = null, int? teacherId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT `re`.`id`, `re`.`user_quiz_result_id`, `re`.`quiz_taken_date`, 
            CASE WHEN `re`.`total_percent_full_quiz` IS NULL THEN 0 ELSE `re`.`total_percent_full_quiz` END AS `total_percent_full_quiz`, 
            `re`.`quiz_id`, `re`.`quiz_title`, `re`.`quiz_value`, `re`.`quiz_instruction`, `re`.`quiz_type_id`, `re`.`quiz_type`,
            `re`.`quiz_due_date`, `re`.`quiz_assigned_to_plan_date`, `re`.`plan_id`, `re`.`plan_name`, `re`.`teacher_id`, 
            `re`.`plan_currently_available`, `re`.`subject_code`, `re`.`level_code`, `re`.`academic_year_end`, `re`.`academic_year_id`, 
            `re`.`academic_year_start`, `re`.`academic_year_type`, `re`.`assignment_type`, `re`.`period_from`, `re`.`period_id`, 
            `re`.`period_no`, `re`.`period_to`, `re`.`plan_assignment_user_result_id`, `re`.`plan_quiz_id`, 
            `re`.`plan_closed_date`, `re`.`plan_lesson_id`, `re`.`lesson_id`, `re`.`plan_lesson_user_result_id`, 
            `us`.`user_id`, `us`.`username`, `us`.`email`, `us`.`first_name`, `us`.`last_name`, `us`.`role_id`, `us`.`role`,
            `us`.`school_id`, `us`.`sie`,
            `sh`.`school_name`, `sh`.`school_type`, `sh`.`city`, `sh`.`ore`, `sh`.`ore_id`, `sh`.`state_iso_code`,
            `sh`.`country`, `sh`.`country_iso`, `sh`.`school_identifier`,
            CASE WHEN `re`.`quiz_taken_date` IS NULL THEN 'Not Completed' ELSE 'Completed' END AS `note` 
            FROM gs_data_analysis.result_generic re 
            INNER JOIN gs_data_analysis.user us ON us.user_id = re.user_id 
            LEFT JOIN `gs_data_analysis`.`school` sh ON sh.school_id = us.school_id 
            WHERE us.role_id = 3 ";

            bool whereUsed = true;

            if (dateFrom != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE re.period_from >= @dateFrom";
                    whereUsed = true;
                }
                else
                {
                    query += " AND re.period_from >= @dateFrom";
                }

                parameters.Add("@dateFrom", dateFrom);
            }

            if(academicYearId != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE `re`.`academic_year_id` = @academicYearId";
                    whereUsed = true;
                }
                else
                {
                    query += " AND `re`.`academic_year_id` = @academicYearId";
                }

                parameters.Add("@academicYearId", academicYearId);
            }

            if (teacherId != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE `re`.`teacher_id` = @teacherId";
                    whereUsed = true;
                }
                else
                {
                    query += " AND `re`.`teacher_id` = @teacherId";
                }

                parameters.Add("@teacherId", teacherId);
            }

            if (dateTo != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE re.period_from <= @dateTo";
                    whereUsed = true;
                }
                else
                {
                    query += " AND re.period_from <= @dateTo";
                }

                parameters.Add("@dateTo", dateTo);
            }

            if (schoolIds != null && schoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE us.school_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += " AND us.school_id IN (";
                }


                for (int i = 0; i < schoolIds.Count; i++)
                {
                    query += string.Concat("@schoolId", i.ToString());
                    parameters.Add(string.Concat("@schoolId", i.ToString()), schoolIds[i]);
                    if (i < schoolIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (userIds != null && userIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE us.user_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += " AND us.user_id IN (";
                }


                for (int i = 0; i < userIds.Count; i++)
                {
                    query += string.Concat("@userId", i.ToString());
                    parameters.Add(string.Concat("@userId", i.ToString()), userIds[i]);
                    if (i < userIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (subjectCodes != null && subjectCodes.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE re.subject_code IN (";
                    whereUsed = true;
                }
                else
                {
                    query += " AND re.subject_code IN (";
                }


                for (int i = 0; i < subjectCodes.Count; i++)
                {
                    query += string.Concat("@subjectCode", i.ToString());
                    parameters.Add(string.Concat("@subjectCode", i.ToString()), subjectCodes[i]);
                    if (i < subjectCodes.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            if (levelCodes != null && levelCodes.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE re.level_code IN (";
                    whereUsed = true;
                }
                else
                {
                    query += " AND re.level_code IN (";
                }


                for (int i = 0; i < levelCodes.Count; i++)
                {
                    query += string.Concat("@levelCode", i.ToString());
                    parameters.Add(string.Concat("@levelCode", i.ToString()), levelCodes[i]);
                    if (i < levelCodes.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
            }

            query += " ORDER BY `re`.`id` DESC ";

            IEnumerable<ResultGenericModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<ResultGenericModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<ResultGenericModel>();
        }
    }
}