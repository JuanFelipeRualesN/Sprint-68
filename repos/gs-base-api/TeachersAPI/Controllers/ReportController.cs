using GSPlanning.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeachersAPI;
using TeachersAPI.CustomClasses;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.Reports;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/reports")]
    public class ReportController : ControllerBase
    {
        [HttpGet("profile/average/{planId}/profile/{profileId}/lesson/{lessonId}/details")]
        public async Task<IActionResult> GetProfilePlanLessonAverageDetails(string planId, string profileId, string lessonId)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var results = StudentAssignmentsAndResults.GetStudentLessonsResultsForPlanning(profileId, planId, responseSession.Token, responseSession.IpAddress, lessonId);
            return Ok(results);
        }
        [HttpGet("average/profile/{profileId}/period/{periodId}")]
        public async Task<IActionResult> GetProfileAverage(string profileId, string periodId, string teacherPlanId = null, string subjectCode = null, string levelCode = null, bool includeDetails = false)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (profileId != responseSession.UserId)
            {
                if (!responseSession.IsStaff && !GetUserDetails.CanUserAManageUserB(responseSession.UserId, profileId, responseSession.Token))
                {
                    return Unauthorized();
                }
            }

            // Now get the overall according to profile and period, and return it according to subject and level 
            // Return subject code, level code, average, grade (A, B, C...). 
            return Ok(StudentAssignmentsAndResults.GetAccumulatedUserResultsByPeriod(periodId, profileId, teacherPlanId, null, subjectCode, levelCode, includeDetails));
        }
        [HttpGet("average/period/{periodId}")]
        public async Task<IActionResult> GetProfileOverallAverage(string periodId, string subjectCode = null, string levelCode = null, bool includeDetails = false, string userId = null)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                // Check if user can handle the other user. 
                if (!GetUserDetails.CanUserAManageUserB(responseSession.UserId, userId, responseSession.Token))
                {
                    return Unauthorized();
                }
            }

            // Now get the overall according to profile and period, and return it according to subject and level 
            // Return subject code, level code, average, grade (A, B, C...). 
            return Ok(StudentAssignmentsAndResults.GetAccumulatedUserResultsByPeriod(periodId, !string.IsNullOrWhiteSpace(userId) ? userId : responseSession.UserId, null, null, subjectCode, levelCode, includeDetails, responseSession.LangCode));
        }


        [HttpGet("profile/average/{planId}/profile/{profileId}/lesson/{lessonId}")]
        public async Task<IActionResult> GetProfilePlanLessonAverage(string planId, string profileId, string lessonId)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var results = StudentAssignmentsAndResults.GetStudentLessonsResultsForPlanning(profileId, planId, responseSession.Token, responseSession.IpAddress, lessonId);
            if (results != null && results.PlanDetails != null && results.PlanDetails.Count > 0)
            {
                return Ok(results.PlanDetails[0].OverallAveragePlan);
            }
            return Ok("-");
        }

        [HttpGet("group/scores/average/plan/{planId}")]
        public async Task<IActionResult> GetGroupsScoreLessonQuizzesAverage(string planId, bool isAssignment)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            return Ok(StudentAssignmentsAndResults.GetGroupAverage(planId, isAssignment));
        }
        [HttpGet("group/scores/assignments/plan/{planId}")]
        public async Task<IActionResult> GetGroupsOverallScoreAssignments(string planId, string groupCode = null, string teacherGroupId = null, string fromDate = null, string toDate = null, int quizLimiter = 10, int quizFromLimiter = 0, int studentLimiter = 10, int studentFromLimiter = 0)
        {
            // Check the access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }
            return Ok(StudentAssignmentsAndResults.GetGroupResultsTeacherAssignments(responseSession.Token, responseSession.IpAddress, planId, quizLimiter, quizFromLimiter, studentLimiter, studentFromLimiter, groupCode, teacherGroupId, fromDate, toDate));
        }

        /// <summary>
        /// To get the results with the PDF.
        /// </summary>
        /// <param name="planLessonId">The plan lesson id.</param>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="profileId">The profile's id.</param>
        /// <returns>Returns the results with the PDF.</returns>
        [HttpGet("plan-lesson-id/{planLessonId}/profile/{profileId}/quiz/{quizId}")]
        public async Task<IActionResult> GetProfileQuizResultDetails(string planLessonId, string quizId, string profileId)
        {
            try
            {
                /* Token validation in Users API */
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileQuizResultDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                // End of validation

                // Check if user can access to the student details.
                // Verify if users are related.
                bool related = GetUserDetails.CanUserAManageUserB(responseSession.UserId, profileId, responseSession.Token) || responseSession.UserId == profileId;
                // If it is the same user, just allow it.
                if (!related)
                {
                    return Unauthorized();
                }
                var planningReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var generalResponseModel = planningReaderManager.GetUserLessonQuizResultReport(planLessonId: planLessonId, quizId: quizId, userId: profileId, token: responseSession.Token, ipAddress: responseSession.IpAddress, responseSession.LangCode);
                return Ok(generalResponseModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception: {ex}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// To get the results of the student for a specific quiz of a plan lesson.
        /// </summary>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="userQuizId">The user quiz id.</param>
        /// <param name="planLessonId">The plan lesson id.</param>
        /// <returns>Returns the details of the student result.</returns>
        [HttpGet("profile/quiz/results/{studentId}/{userQuizId}/{planLessonId}")]
        public async Task<IActionResult> GetStudentQuizResultDetails(string studentId, string userQuizId, string planLessonId)
        {
            try
            {
                var generalResponseModel = new ReportPlanLessonQuizResponseModel();
                // Token validation in Users API
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileQuizResultDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                // End of validation

                // Check if user can access to the student details.
                // Verify if users are related.
                var related = GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token) || responseSession.UserId == studentId;
                // If it is the same user, just allow it.
                if (!related)
                {
                    return Unauthorized();
                }
                // End of related validation
                var planningReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                
                // Get score and usqui_user_result_id from plan_lesson_quiz_user_results
                var planLessonQuizUserResultModel = planningReaderManager.GetUserQuizResponse(studentId, userQuizId, planLessonId);
                // Get User responses, send responseSession user id to see if the user can see the results from GSWebAPI by usqui_user_result_id 
                // use GSWebDetails
                var userQuizResponseModel = GSWebDetails.GetUserResults(responseSession.Token, studentId, planLessonQuizUserResultModel.UsquiUserResultId);
      
                var userUsquiReport = planningReaderManager.GetUserLessonQuizResultReport(planLessonId, userQuizResponseModel.UserQuizFullDetails.QuizId,
                    studentId, responseSession.Token, responseSession.IpAddress, responseSession.LangCode);
                generalResponseModel.UserQuizFullDetails = userUsquiReport.UserQuizFullDetails;

                return Ok(generalResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
