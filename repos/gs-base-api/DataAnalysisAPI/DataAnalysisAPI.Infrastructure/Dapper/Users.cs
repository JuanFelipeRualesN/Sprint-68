using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs;
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
    /// To handle the users queries with dapper.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// To get the users.
        /// </summary>
        /// <param name="dateFrom">Optional: Date from connected.</param>
        /// <param name="dateTo">Optional: Date to connected.</param>
        /// <param name="schoolIds">Optional: The schools to filter.</param>
        /// <param name="userIds">Optional: The user ids to filter.</param>
        /// <param name="status">Optional: The user status to filter.</param>
        /// <returns>Returns the list of connections.</returns>
        public static List<UserSchoolModel> GetUsers(string sie = null, List<int> roleIds = null, List<int> schoolIds = null,
            List<int> userIds = null, string status = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT DISTINCT us.user_id, us.username, 
                us.email, us.first_name, us.last_name, us.role_id, us.role, us.school_id, us.sie,
                `sh`.`school_name`, `sh`.`school_type`, `sh`.`city`, `sh`.`ore`, `sh`.`ore_id`, `sh`.`state_iso_code`,
                `sh`.`country`, `sh`.`country_iso`, `sh`.`school_identifier`, sph.status as subscription_status, sph.subscription_id, sph.cancellation_date, sph.plan_name
                FROM `gs_data_analysis`.`user` us
                LEFT JOIN `gs_data_analysis`.`school` sh ON sh.school_id = us.school_id
                LEFT JOIN `gs_data_analysis`.`subscription_profile_history` sph ON us.user_id = sph.user_id ";

            bool whereUsed = false;
            if (sie != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE us.sie LIKE @sie";
                    whereUsed = true;
                }
                else
                {
                    query += " AND us.sie LIKE @sie";
                }

                parameters.Add("@sie", "%" + sie + "%");
            }

            if (roleIds != null && roleIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE us.role_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += " AND us.role_id IN (";
                }


                for (int i = 0; i < roleIds.Count; i++)
                {
                    query += string.Concat("@roleId", i.ToString());
                    parameters.Add(string.Concat("@roleId", i.ToString()), roleIds[i]);
                    if (i < roleIds.Count - 1)
                    {
                        query += ",";
                    }
                }
                query += ") ";
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

            if (status != null)
            {
                status = status.ToLower();

                string statusLogic = @"(
                                (@status = 'active' AND sph.status = 'active' AND sph.cancellation_date  >= NOW())
                                OR
                                (@status = 'inactive' AND ((sph.status = 'active' AND sph.cancellation_date  < NOW()) OR (sph.status <> 'active' OR sph.status IS NULL)))
                           )";

                if (!whereUsed)
                {
                    query += " WHERE " + statusLogic;
                    whereUsed = true;
                }
                else
                {
                    query += " AND " + statusLogic;
                }

                parameters.Add("@status", status);
            }


            IEnumerable<UserSchoolModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<UserSchoolModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<UserSchoolModel>();
        }

        /// <summary>
        /// To get the teacher relations.
        /// </summary>
        /// <param name="userId">The user id (teacher's id).</param>
        /// <returns>Returns the relation details list.</returns>
        public static List<UserRelationModel> GetTeacherRelations(int userId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT * FROM gs_data_analysis.user_relation WHERE teacher_id = @userId";
            parameters.Add("@userId", userId);

            IEnumerable<UserRelationModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<UserRelationModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<UserRelationModel>();
        }
    }
}