using TeachersAPI.Domain.Models.Generals.Requests.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Utils
{
    /// <summary>
    /// class to manage business logic of different manager services, avoiding code redundancy.
    /// </summary>
    public static class GlobalManager
    {
        /// <summary>
        /// returns the information of all users belonging to the teacher's group. 
        /// </summary>
        /// <param name="teacherGroupStudentModelList">List of teacher groups.</param>
        /// <param name="responseSession">Session info.</param>
        /// <returns>Model with the list of users.</returns>
        public static UserResponseModel GetUsersIdsFromTeacherGroup(List<Domain.Models.Generals.Dapper.TeacherGroupStudentModel> teacherGroupStudentModelList, ValidationResponseModel responseSession)
        {
            if (teacherGroupStudentModelList != null && teacherGroupStudentModelList.Count > 0)
            {
                // Users ids to call details.
                List<string> userIdsToCall = teacherGroupStudentModelList
                                                .Where(teacherGroup => teacherGroup.StudentId != null)
                                                .Select(teacherGroup => teacherGroup.StudentId.ToString())
                                                .ToList();

                // Set the student info if there is any student to call.
                if (userIdsToCall.Count > 0)
                {
                    // Users that an event will be created in Microsoft Teams.
                    UserResponseModel students = GetUserDetails.GetStudentsThatCanBeHandled(new UserRequestModel()
                    {
                        UserIds = userIdsToCall,
                        SameSchoolRequired = true,
                        RoleId = "3"
                    }, responseSession.Token);

                    return students;
                }
            }

            return new UserResponseModel();
        }

        /// <summary>
        /// returns student by Id.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="responseSession">Session info.</param>
        /// <returns></returns>
        public static UserResponse GetStudent(string userId, ValidationResponseModel responseSession)
        {
            UserResponseModel student = GetUserDetails.GetStudentsThatCanBeHandled(new UserRequestModel()
            {
                UserIds = new List<string>() { userId },
                SameSchoolRequired = true,
                RoleId = "3"
            }, responseSession.Token);

            if (student.UserInfo.Count == 0)
                return null;

            return student.UserInfo.FirstOrDefault();
        }
    }
}