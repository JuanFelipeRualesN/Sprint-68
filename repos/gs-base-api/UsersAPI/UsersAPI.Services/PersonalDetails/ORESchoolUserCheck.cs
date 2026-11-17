using System.Collections.Generic;
using UsersAPI.UsersAPI.Services.FeignAPIs;

namespace UsersAPI.UsersAPI.Services.PersonalDetails
{
    /// <summary>
    /// To handle the ORE details.
    /// </summary>
    public static class ORESchoolUserCheck
    {
        /// <summary>
        /// To check if school in ore.
        /// </summary>
        /// <param name="schoolId">The school id.</param>
        /// <param name="oreId">The ore id.</param>
        /// <returns>Returns true if school in ORE.</returns>
        public static bool CheckIfSchoolInOre(string schoolId, string oreId)
        {
            List<string> oreSchoolIds = SchoolDetailsHandler.GetORESchoolIds(oreId);
            return oreSchoolIds.Contains(schoolId);
        }

        /// <summary>
        /// TO check if staff of ore can handle user.
        /// </summary>
        /// <param name="oreId">The ore's id.</param>
        /// <param name="profileId">The profile id to check if it is from a school of the ore.</param>
        /// <returns>Returns true if can be handle, else returns false.</returns>
        public static bool CheckIfStaffOreCanHandleUser(string oreId, string profileId)
        {
            // Get profile school 
            List<string> oreSchoolIds = SchoolDetailsHandler.GetORESchoolIds(oreId);
            UserSchoolManager userSchoolManager = new UserSchoolManager(
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"], null);
           List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel> userSchools = userSchoolManager.GetUserSchools(new List<UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel>() {
                new UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel()
                {
                    UserId = profileId,
            } });

            foreach(UsersAPI.Domain.Models.PersonalDetails.UserSchoolModel userSchool in userSchools)
            {
                if(oreSchoolIds.Contains(userSchool.SchoolId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}