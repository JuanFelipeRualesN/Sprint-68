using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using System.Collections.Generic;


namespace DataAnalysisAPI.DataAnalysisAPI.Services
{
    /// <summary>
    /// TO handle the generics requests.
    /// </summary>
    public class Generics
    {
        /// <summary>
        /// To get the schools.
        /// </summary>
        /// <param name="userIdRequester">The user that is making the request.</param>
        /// <param name="isStaff">If the user making the request is a staff.</param>
        /// <param name="oreId">Optional: The id of the ORE.</param>
        /// <param name="schoolIds">Optional: The school ids.</param>
        /// <returns>Returns a list of schools.</returns>
        public static List<SchoolModel> GetSchools(int userIdRequester, bool isStaff, int? oreId = null, List<int> schoolIds = null)
        {
            if(isStaff)
            {
                return Infrastructure.Dapper.Generics.GetSchools(oreId, schoolIds);
            }

            // If not an staff, return the schools related to the user.
            return Infrastructure.Dapper.Generics.GetSchools(oreId, schoolIds, userIdRequester);
        }
        
        /// <summary>
        /// To get the countries.
        /// </summary>
        /// <returns>Return a list of countries.</returns>
        public static List<CountryModel> GetCountries()
        {
            return Infrastructure.Dapper.Generics.GetCountries();
        }

        /// <summary>
        /// To get the OREs (region).
        /// </summary>
        /// <param name="oreId">Optional: The ore id.</param>
        /// <param name="countryIso">Optional: The country ISO.</param>
        /// <returns>Return a list of OREs.</returns>
        public static List<OreModel> GetOres(int? oreId = null, string countryIso = null)
        {
            return Infrastructure.Dapper.Generics.GetOres(oreId, countryIso);
        }

        /// <summary>
        /// To get the states.
        /// </summary>
        /// <param name="countryIso">Optional: The country ISO.</param>
        /// <returns>Returns a list of states.</returns>
        public static List<StateProvinceModel> GetStates(string countryIso = null)
        {
            return Infrastructure.Dapper.Generics.GetStates(countryIso);
        }

        /// <summary>
        /// To get the cities.
        /// </summary>
        /// <param name="oreId">Optional: The ORE id.</param>
        /// <param name="countryIso">Optional: The country iso to filter the cities.</param>
        /// <param name="stateIso">Optional: The state iso to filter the cities.</param>
        /// <returns>Returns the list of cities.</returns>
        public static List<CityModel> GetCities(int? oreId = null, string countryIso = null, string stateIso = null)
        {
            return Infrastructure.Dapper.Generics.GetCities(oreId, countryIso, stateIso);
        }

        /// <summary>
        /// To get the subjects.
        /// </summary>
        /// <param name="subjectCode">Optional: The subject code to filter.</param>
        /// <returns>Returns a list of subjects.</returns>
        public static List<SubjectModel> GetSubjects(string subjectCode = null)
        {
            return Infrastructure.Dapper.Generics.GetSubjects(subjectCode);
        }

        /// <summary>
        /// To get the levels
        /// </summary>
        /// <param name="levelCode">OPtional: The level code to filter.</param>
        /// <returns>Returns a list of levels.</returns>
        public static List<LevelModel> GetLevels(string levelCode = null)
        {
            return Infrastructure.Dapper.Generics.GetLevels(levelCode);
        }

        /// <summary>
        /// To get the roles.
        /// </summary>
        /// <returns>Returns a list of roles.</returns>
        public static List<RoleModel> GetRoles()
        {
            return Infrastructure.Dapper.Generics.GetRoles();
        }
    }
}