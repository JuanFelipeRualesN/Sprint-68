using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using MySqlConnector;
using DataAnalysisAPI.Utils;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs;

namespace DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper
{
    /// <summary>
    /// To handle the details of connections using dapper.
    /// </summary>
    public static class Connections
    {
        /// <summary>
        /// To get the user connections.
        /// </summary>
        /// <param name="dateFrom">Optional: Date from connected.</param>
        /// <param name="dateTo">Optional: Date to connected.</param>
        /// <param name="schoolIds">Optional: The schools to filter.</param>
        /// <param name="userIds">Optional: The user ids to filter.</param>
        /// <returns>Returns the list of connections.</returns>
        public static List<UserSchoolModel> GetUsersConnections(DateTime? dateFrom, DateTime? dateTo, List<int> schoolIds = null, 
            List<int> userIds = null, string status = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT dt.connected_date, us.user_id, us.username, 
	            us.email, us.first_name, us.last_name, us.role_id, us.role, us.school_id, us.sie, 
	            `sh`.`school_name`, `sh`.`school_type`, `sh`.`city`, `sh`.`ore`, `sh`.`ore_id`, `sh`.`state_iso_code`,
	            `sh`.`country`, `sh`.`country_iso`, `sh`.`school_identifier`, dt.subscription_status
	            FROM gs_data_analysis.connected_date dt 
	            INNER JOIN gs_data_analysis.user us ON us.user_id = dt.user_id
                LEFT JOIN gs_data_analysis.school sh ON sh.school_id = us.school_id ";

            bool whereUsed = false;
            if (dateFrom != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE dt.connected_date >= @dateFrom";
                    whereUsed = true;
                }
                else
                {
                    query += " AND dt.connected_date >= @dateFrom";
                }

                parameters.Add("@dateFrom", dateFrom);
            }

            if (dateTo != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE dt.connected_date <= @dateTo";
                    whereUsed = true;
                }
                else
                {
                    query += " AND dt.connected_date <= @dateTo";
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

            if (status != null)
            {
                status = status.ToLower();

                string statusLogic = @"(
                                (@status = 'active' AND dt.subscription_status = 'active')
                                OR
                                (@status = 'inactive' AND (dt.subscription_status <> 'active' OR dt.subscription_status IS NULL))
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

            if (!whereUsed)
            {
                query += " WHERE us.role_id <> 1 ";
                whereUsed = true;
            }
            else
            {
                query += " AND us.role_id <> 1 ";
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

    }
}