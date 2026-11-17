using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    public class AutoFilters
    {
        /// <summary>
        /// To get the users ids of a director.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the user ids of the director.</returns>
        public static List<int> GetUsersIdByDirectors(int userId, List<int> userIdsToFilter)
        {
            // First, identify the school of the user. 
            List<UserSchoolModel> userResults = Infrastructure.Dapper.Users.GetUsers(null, null, null, new List<int> { userId }); 
            // Retrieves the user ids by the same school. 
            
            if(userResults == null || userResults.Count == 0 || userResults[0].SchoolId == null || userResults[0].SchoolId == 0)
            {
                return null; 
            }

            // Now, get the users by schools
            List<int> schoolIds = new List<int>();
            foreach (UserModel user in userResults)
            {
                if (user.SchoolId != null)
                {
                    schoolIds.Add(user.SchoolId.Value);
                }
            }

            // Call the users from the schools 
            List<UserSchoolModel> userResultsSchools = Infrastructure.Dapper.Users.GetUsers(null, null, schoolIds, null);
            List<int> userIds = new List<int>();
            foreach(UserModel user in userResultsSchools)
            {
                userIds.Add(user.UserId);
            }

            if(userIdsToFilter == null || userIdsToFilter.Count == 0)
            {
                return userIds;
            }

            // Now compare that with the user ids to filter 
            List<int> userIdsToReturn = new List<int>(); 
       
            foreach(int userIdToFilter in userIdsToFilter)
            {
                if(userIds.Contains(userIdToFilter))
                {
                    userIdsToReturn.Add(userIdToFilter);
                }
            }
           
            return userIdsToReturn;
        }

        /// <summary>
        /// Get the school ids of the ore.
        /// </summary>
        /// <param name="oreId">The id of the ORE.</param>
        /// <param name="schoolIdsToFilterFromOre">The school ids to filter from the ORE.</param>
        /// <returns>Returns a list of ints that represent the schools ids.</returns>
        public static List<int> GetOreSchoolIds(int oreId, List<int> schoolIdsToFilterFromOre)
        {
            List<int> schoolIdsFinale = new List<int>();
            var schools = Infrastructure.Dapper.Generics.GetSchools(oreId, schoolIdsToFilterFromOre);

            for(int i = 0;i < schools.Count; i++)
            {
                schoolIdsFinale.Add(schools[i].SchoolId);
            }

            /*
            List<int> schoolIdsToFilter = new List<int>();
            foreach (var school in schools)
            {
                schoolIdsToFilter.Add(school.SchoolId);
            }

            // Now, if schools
            if (schoolIdsToFilterFromOre != null && schoolIdsToFilterFromOre.Count > 0)
            {
                // Check if school ids 
                foreach (var schoolId in schoolIdsToFilterFromOre)
                {
                    // Check if exists 
                    if (schoolIdsToFilter.Contains(schoolId))
                    {
                        schoolIdsFinale.Add(schoolId);
                    }
                }
            }
            else
            {
                // Add the schools of the ORE
                schoolIdsFinale = schoolIdsToFilter;
            } */

            return schoolIdsFinale;
        }

        /// <summary>
        /// To filter the user ids if the user is non staff and non director.
        /// </summary>
        /// <param name="userId">The user requester id.</param>
        /// <param name="userIdsToFilter">The user ids to filter.</param>
        /// <returns>Return a list of user ids.</returns>
        public static List<int> GetUsersIdsByNonDirectorNonStaffUser(int userId, List<int> userIdsToFilter)
        {
            // Get the users that can be related.
            List<UserRelationModel> userRelations = Infrastructure.Dapper.Users.GetTeacherRelations(userId);

            // Retrieve the student ids. 
            List<int> userIdsToEvaluate = new List<int>(); 
            foreach(UserRelationModel userRelation in userRelations)
            {
                if (userRelation.StudentId != null)
                {
                    userIdsToEvaluate.Add(userRelation.StudentId.Value);
                }
            }

            if(userIdsToFilter == null || userIdsToFilter.Count == 0)
            {
                return userIdsToEvaluate;
            }

            // If not null, check if the userIdsToFilter exists in userIdsToEvaluate
            List<int> userIdsToReturn = new List<int>();
            foreach(var userIdToFilter in userIdsToFilter)
            {
                if(userIdsToEvaluate.Contains(userIdToFilter))
                {
                    userIdsToReturn.Add(userIdToFilter);
                }
            }

            return userIdsToReturn;
        }
    }
}