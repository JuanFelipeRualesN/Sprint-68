using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Dapper;
using MySqlConnector;
using UsersAPI.UsersAPI.Domain.Models.SSO;

namespace UsersAPI.UsersAPI.Services.SSO
{
    /// <summary>
    /// To handle the SSO.
    /// </summary>
    public static class SsoManager
    {
        /// <summary>
        /// To get the client details by domain and platform.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="platform">The platform. Example: AZURE.</param>
        /// <returns>Returns the client's details.</returns>
        public static SsoServiceModel GetClientDetails(string domain, string platform)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            IEnumerable<SsoServiceModel> items = null;
            string query = @"SELECT * FROM gs_users_db_prod.sso_services WHERE domain=@domain AND cloud_service_platform=@platform;";
            object parameters;
            parameters = new
            {
                domain = domain,
                platform = platform,
            };

            // Init test with dapper 
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<SsoServiceModel>(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            if(items.Count() == 0)
            {
                return null;
            }

            return items.FirstOrDefault();
        }
    }
}