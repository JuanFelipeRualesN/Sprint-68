using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UsersAPI.UsersAPI.Domain.Models.Metrics.UsageAndEngagement;

namespace UsersAPI.UsersAPI.Infrastructure.DapperHandler.Metrics.UsageAndEngagement
{
    /// <summary>
    /// Retrieves session frequency data.
    /// </summary>
    public static class SessionsRepository
    {
        public static async Task<List<FrequencyUsageData>> GetWeeklySessionFrequencyAsync(List<int>userIds, DateTime startDate, DateTime endDate)
        {
            var connectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"];

            string query = @"
                SELECT 
                    DAYNAME(se.created_at) AS Day, 
                    COUNT(se.id) AS SessionCount 
                FROM 
                    gs_users_db_prod.ul_sessions se
                WHERE 
                    se.user_id IN @UserIds
                    AND se.created_at >= @StartDate
                    AND se.created_at < @EndDate
                GROUP BY 
                    DAYOFWEEK(se.created_at), DAYNAME(se.created_at)
                ORDER BY
                    DAYOFWEEK(se.created_at);";

            var parameters = new
            {
                UserIds = userIds,
                StartDate = startDate.Date,
                EndDate = endDate.Date.AddDays(1)
            };

            List<FrequencyUsageData> results;

            await using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                results = (await connection.QueryAsync<FrequencyUsageData>(query, parameters)).ToList();
            }

            return results;
        }
    }
}