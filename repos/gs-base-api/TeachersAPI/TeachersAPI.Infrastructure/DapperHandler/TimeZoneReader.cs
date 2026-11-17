using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using TeachersAPI.Domain.Models.Generals.Dapper;
using CacheHandlerLibrary;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle time zone queries using Dapper.
    /// </summary>
    public static class TimeZoneReader
    {
        /// <summary>
        /// To get the time zone.
        /// </summary>
        /// <returns>Returns the time zone model list.</returns>
        public static List<TimeZoneModel> GetTimeZones()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            IEnumerable<TimeZoneModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TimeZoneModel>("SELECT DISTINCT `code`, `desc` FROM `gs_teachers_web_db_prod`.`time_zones`;");
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<TimeZoneModel>();
        }
    }
}