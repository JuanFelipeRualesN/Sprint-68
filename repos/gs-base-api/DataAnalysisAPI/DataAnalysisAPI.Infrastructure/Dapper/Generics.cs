using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using MySqlConnector;
using DataAnalysisAPI.Utils;

namespace DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper
{
    /// <summary>
    /// To handle the generics details queries, such as schools, levels, countries, etc.
    /// </summary>
    public static class Generics
    {
        /// <summary>
        /// To get the schools.
        /// </summary>
        /// <param name="oreId">Optional. The ORE (region Id).</param>
        /// <param name="schoolIds">Optional: The id of the schools.</param>
        /// <param name="userIdRequester">Optional: The user that is making the request.</param>
        /// <returns>Returns the list of schools.</returns>
        public static List<SchoolModel> GetSchools(int? oreId = null, List<int> schoolIds = null, int? userIdRequester = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT DISTINCT `sch`.`school_id`, `sch`.`school_name`, `sch`.`school_type`, `sch`.`city`, `sch`.`ore`, `sch`.`ore_id`, 
            `sch`.`state_iso_code`, `sch`.`country`, `sch`.`country_iso`, `sch`.`school_identifier` FROM `gs_data_analysis`.`school` sch 
            LEFT JOIN gs_data_analysis.user us ON us.school_id = sch.school_id";

            bool whereUsed = false;

            if (oreId != null)
            {
                query += " WHERE sch.ore_id = @oreId";
                parameters.Add("@oreId", oreId);
                whereUsed = true;
            }

            if (schoolIds != null && schoolIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE sch.school_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += " AND sch.school_id IN (";
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

            if(userIdRequester != null && userIdRequester.Value > 0)
            {
                if (!whereUsed)
                {
                    query += " WHERE us.user_id = @userIdRequester";
                    whereUsed = true;
                }
                else
                {
                    query += " AND  us.user_id = @userIdRequester";
                }
                parameters.Add("@userIdRequester", userIdRequester);
            }

            IEnumerable<SchoolModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<SchoolModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<SchoolModel>();
        }

        /// <summary>
        /// To return the countries.
        /// </summary>
        /// <returns>Returns a list of countries.</returns>
        public static List<CountryModel> GetCountries()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"SELECT * FROM `gs_data_analysis`.`country`";

            IEnumerable<CountryModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<CountryModel>(query);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<CountryModel>();
        }

        /// <summary>
        /// To get the regions.
        /// </summary>
        /// <param name="oreId">The ORE (region) id.</param>
        /// <param name="countryIso">The country ISO.</param>
        /// <returns>Returns a list of OREs.</returns>
        public static List<OreModel> GetOres(int? oreId = null, string countryIso = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT * FROM `gs_data_analysis`.`ore`";

            bool whereUsed = false;
            if (oreId != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE ore_id = @oreId";
                    whereUsed = true;
                }
                else
                {
                    query += " AND ore_id = @oreId";
                }

                parameters.Add("@oreId", oreId);
            }

            if (countryIso != null)
            {
                if(!whereUsed)
                {
                    query += " WHERE country_iso = @countryIso";
                    whereUsed = true;
                }
                else
                {
                    query += " AND country_iso = @countryIso";
                }

                parameters.Add("@countryIso", countryIso);
            }

            IEnumerable<OreModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<OreModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<OreModel>();
        }

        /// <summary>
        /// To get the states or provinces of a country.
        /// </summary>
        /// <param name="countryIso">The country ISO.</param>
        /// <returns>Returns a list of states.</returns>
        public static List<StateProvinceModel> GetStates(string countryIso = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT * FROM `gs_data_analysis`.`state_province`";

            bool whereUsed = false;

            if (countryIso != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE country_iso = @countryIso";
                    whereUsed = true;
                }
                else
                {
                    query += " AND country_iso = @countryIso";
                }

                parameters.Add("@countryIso", countryIso);
            }

            IEnumerable<StateProvinceModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<StateProvinceModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<StateProvinceModel>();
        }

        /// <summary>
        /// To get the cities.
        /// </summary>
        /// <param name="oreId">The ORE (region) id.</param>
        /// <param name="countryIso">The country ISO.</param>
        /// <param name="stateIso">The state ISO.</param>
        /// <returns>Returns a list of cities.</returns>
        public static List<CityModel> GetCities(int? oreId = null, string countryIso = null, string stateIso = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT * FROM `gs_data_analysis`.`city`";

            bool whereUsed = false;
            if (oreId != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE ore_id = @oreId";
                    whereUsed = true;
                }
                else
                {
                    query += " AND ore_id = @oreId";
                }

                parameters.Add("@oreId", oreId);
            }

            if (countryIso != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE country_iso = @countryIso";
                    whereUsed = true;
                }
                else
                {
                    query += " AND country_iso = @countryIso";
                }

                parameters.Add("@countryIso", countryIso);
            }

            if (stateIso != null)
            {
                if (!whereUsed)
                {
                    query += " WHERE state_iso_code = @stateIso";
                    whereUsed = true;
                }
                else
                {
                    query += " AND state_iso_code = @stateIso";
                }

                parameters.Add("@stateIso", stateIso);
            }

            IEnumerable<CityModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<CityModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<CityModel>();
        }

        /// <summary>
        /// To get the subjects.
        /// </summary>
        /// <param name="subjectCode">Optional. The subject code.</param>
        /// <returns>Returns the list of subjects.</returns>
        public static List<SubjectModel> GetSubjects(string subjectCode = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT * FROM `gs_data_analysis`.`subject`";

            if (subjectCode != null)
            {
                query += " WHERE subject_code = @subjectCode";
                parameters.Add("@subjectCode", subjectCode);
            }

            IEnumerable<SubjectModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<SubjectModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<SubjectModel>();
        }

        /// <summary>
        /// To get the levels.
        /// </summary>
        /// <param name="levelCode">Optional. The level code.</param>
        /// <returns>Returns the list of levels.</returns>
        public static List<LevelModel> GetLevels(string levelCode = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT * FROM `gs_data_analysis`.`level`";

            if (levelCode != null)
            {
                query += " WHERE level_code = @levelCode";
                parameters.Add("@levelCode", levelCode);
            }

            IEnumerable<LevelModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<LevelModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<LevelModel>();
        }

        /// <summary>
        /// To return the roles.
        /// </summary>
        /// <returns>Returns a list of roles.</returns>
        public static List<RoleModel> GetRoles()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"SELECT * FROM `gs_data_analysis`.`role`";

            IEnumerable<RoleModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<RoleModel>(query);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<RoleModel>();
        }

        /// <summary>
        /// To return the subscription status.
        /// </summary>
        /// <returns>Returns a list of status.</returns>
        public static List<StatusModel> GetStatus()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"SELECT * FROM `gs_data_analysis`.`status`";

            IEnumerable<StatusModel> items = null;
            MySqlConnection connection = Configurations.GetMySqlConnection();
            connection.Open();
            try
            {
                items = connection.Query<StatusModel>(query);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return items != null ? items.ToList() : new List<StatusModel>();
        }
    }
}