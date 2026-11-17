using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.Generals;


namespace UsersAPI.UsersAPI.Services.Generals
{
    /// <summary>
    /// To handle data related to "gus" specific tables.
    /// </summary>
    public static class GUSRelated
    {
        /// <summary>
        /// To get the user's activity by token.
        /// </summary>
        /// <param name="token">The access token.</param>
        /// <returns>Returns a list of user's activities if any.</returns>
        public static List<ActivityDateReportModel> GetUserActivity(string token)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT se.user_id, DATE(re.created_at) as created_at
            FROM gs_users_db_prod.ul_sessions se
            LEFT JOIN gs_users_db_prod.gus_activity_date_reports re ON re.user_id = se.user_id
            WHERE token = @token; ";
            var parameters = new { token = token };
            List<ActivityDateReportModel> activities = new List<ActivityDateReportModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
                {
                    activities = connection.Query<ActivityDateReportModel>(query, parameters).ToList();
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }
            

            return activities;
        }

        /// <summary>
        /// To insert the user activity to have a record of one connection per day if applies.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public static void InserUserActivity(string userId)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = "INSERT INTO gs_users_db_prod.gus_activity_date_reports (user_id) VALUES (@userId);";
            var parameters = new { userId = userId };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"]
            };
            connection.Open();
            try
                {
                    connection.Execute(query, parameters);
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();

                }
            
        }
    }
}