using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using Dapper;
using SchoolsAPI.CustomClasses;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.DapperHandler
{
    /// <summary>
    /// To handle details related to ORE using Dapper. Dapper is used to improve performance.
    /// </summary>
    public class OreDetails
    {
        /// <summary>
        /// To get the ore/region schools.
        /// </summary>
        /// <param name="oreId">The ore's id.</param>
        /// <returns>Returns a list of strings of school ids.</returns>
        public static List<string> GetORESchoolsIds(string oreId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT entity_id FROM gs_schools_api_db_prod.sch_addresses WHERE region_id = @oreId";
            var parameters = new { oreId };

            List<string> items = new List<string>();
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();

            try
            {

                items = connection.Query<string>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return items;
        }

        /// <summary>
        /// Check if ore can handle school id.
        /// </summary>
        /// <param name="oreId">The ore's id.</param>
        /// <param name="schoolId">The school's id</param>
        /// <returns>Returns true if can be handle, else false.</returns>
        public static bool CheckIfOreCanHandleSchool(string oreId, string schoolId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT entity_id FROM gs_schools_api_db_prod.sch_addresses WHERE region_id = @oreId AND entity_id = @schoolId";
            var parameters = new { oreId, schoolId };

            List<string> items = new List<string>();
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();

            try
            {

                items = connection.Query<string>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items.Count > 0;
        }
    }
}