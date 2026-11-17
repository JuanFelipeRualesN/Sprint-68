using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To handle the teacher pending assignments.
    /// </summary>
    public static class TeacherPendingAssignmentsHandler
    {
        /// <summary>
        /// Obtain paged pending assignments from instructional plans. 
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <param name="fromPage">From Page.</param>
        /// <param name="limiter">Limiter (how many items per page).</param>
        /// <param name="token">Session token.</param>
        /// <param name="ipAddress">Sesion network ip.</param>
        /// <returns>All pending assignment with student assigned.</returns>
        public static PendingAssignmentsResponseModel GetPendingAssignments(string teacherId, int fromPage, int limiter, string token, string ipAddress)
        {
            // Get the pending assignments with Dapper.
            List<TeacherPendingAssignmentsModel> teacherPendingAssignmentsModelList = TeacherPendingAssignments.GetTeacherPendingAssignments(teacherId);

            var teacherPendingAssignmentsModelListSorted = new List<TeacherPendingAssignmentsModel>();

            // Order by dates 
            teacherPendingAssignmentsModelListSorted = teacherPendingAssignmentsModelList.OrderBy(x => DateTime.Parse(x.DateSubmitted)).ToList();

            // Set the total 
            int totalPages = int.Parse(Math.Ceiling((double)teacherPendingAssignmentsModelListSorted.Count() / limiter).ToString());

            // Set the limit
            teacherPendingAssignmentsModelListSorted = teacherPendingAssignmentsModelListSorted.Skip(fromPage * limiter).Take(limiter).ToList();

            // Prepare response
            var teacherPendingAssignmentsResponse = new PendingAssignmentsResponseModel()
            {
                TotalPages = totalPages,
                PendingAssignments = new List<PendingAssignments>(),
            };

            foreach (var item in teacherPendingAssignmentsModelListSorted)
            {
                // Find user details from service
                UserResponse student = GetUserDetails.GetUsersSharedDetails(token, ipAddress, new List<string>() { item.StudentId }).FirstOrDefault();

                // Adding values to response
                teacherPendingAssignmentsResponse.PendingAssignments.Add(
                    new PendingAssignments()
                    {
                        PlanId = item.PlanId,
                        TeacherId = item.TeacherId,
                        PlanQuizId = item.PlanQuizId,
                        PlanName = item.PlanName,
                        AssignmentName = item.AssignmentName,
                        AssignmentId = item.AssignmentId,
                        DateSubmitted = item.DateSubmitted,
                        Student = student,
                    });
            }

            return teacherPendingAssignmentsResponse;
        }

        /// <summary>
        /// Obtain count of all assignments from instructional plans. 
        /// </summary>
        /// <param name="teacherId">The teacher's id.</param>
        /// <returns>Int count.</returns>
        public static int GetTotalPendingAssignments(string teacherId)
        {
            // Get the pending assignments with Dapper.
            List<TeacherPendingAssignmentsModel> teacherPendingAssignmentsModelList = TeacherPendingAssignments.GetTeacherPendingAssignments(teacherId);

            // Returning the count of elements.
            return teacherPendingAssignmentsModelList.Count;
        }
    }
}