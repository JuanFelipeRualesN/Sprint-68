using Microsoft.AspNetCore.Mvc;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeachersAPI.CustomClasses;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI.Domain.Models.Generals.Responses.Reports;
using TeachersAPI;

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
                if (!string.IsNullOrWhiteSpace(generalResponseModel.UserQuizResultId))
                {
                    generalResponseModel.PDFLocation = await PDFResultGenerator.GeneratePDF(generalResponseModel);
                }
                return Ok(generalResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("profile/quiz/results/{studentId}/{userQuizId}/{planLessonId}")]
        public async Task<IActionResult> GetStudentQuizResultDetails(string studentId, string userQuizId, string planLessonId)
        {
            try
            {
                ReportPlanLessonQuizResponseModel generalResponseModel = new ReportPlanLessonQuizResponseModel();
                // Token validation in Users API
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileQuizResultDetailsRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                // End of validation

                // Check if user can access to the student details.
                // Verify if users are related.
                bool related = GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token) || responseSession.UserId == studentId;
                // If it is the same user, just allow it.
                if (!related)
                {
                    return Unauthorized();
                }
                // End of related validation
                var planningReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                // Get score and usqui_user_result_id from plan_lesson_quiz_user_results
                PlanLessonQuizUserResultModel planLessonQuizUserResultModel = planningReaderManager.GetUserQuizResponse(studentId, userQuizId, planLessonId);
                // Get User responses, send responseSession user id to see if the user can see the results from GSWebAPI by usqui_user_result_id 
                // use GSWebDetails
                UserQuizResponseModel userQuizResponseModel = GSWebDetails.GetUserResults(responseSession.Token, planLessonQuizUserResultModel.UsquiUserResultId);
                AthenasJSONQuizResponseModel modelObject = AthenasHandler.GetQuizById(userQuizId, responseSession.Token);
                // Now connect the data
                generalResponseModel.UserQuizFullDetails = new QuizUserResultsCompletedInfoResponseModel
                {
                    QuizId = userQuizResponseModel.UserQuizResult.QuizId,
                    QuizTitle = modelObject.QuizRequestModel.QuizModel.Title,
                    Instructions = modelObject.QuizRequestModel.QuizModel.Instruction,
                    ExtraInfo = modelObject.QuizRequestModel?.QuizExtraInfoModel?.ExtraInfo,
                    UserQuizId = userQuizResponseModel.UserQuizResult.Id,
                    QuestionQuizList = new List<QuestionQuiz>(),
                    TotalPercent = planLessonQuizUserResultModel.Score,
                    DoK = userQuizResponseModel.UserQuizResult.Dok,
                    TotalCorrect = userQuizResponseModel.UserQuizResult.TotalCorrect,
                    TryNo = userQuizResponseModel.UserQuizResult.TryNo,
                };
                for (int i = 0; i < modelObject.QuizRequestModel.QuestionRequestModelList.Count; i++)
                {
                    QuestionQuiz questionQuizToAdd = new QuestionQuiz
                    {
                        QuestionId = modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id,
                        Question = modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Desc,
                        QuestionResponses = new List<QuestionResponse>(),
                    };
                    List<Domain.Models.UserQuizzes.UserResponseModel> userQuizQuestionsResponsesToFind = userQuizResponseModel.UserResponsesList.FindAll(x => x.QuestionId == modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id);
                    for (int j = 0; j < userQuizQuestionsResponsesToFind.Count; j++)
                    {
                        QuizQuestionResponseModel quizQuestionResponseModelToFind = modelObject.QuizRequestModel.QuestionRequestModelList[i].QuizQuestionResponseModelList.Find(x => x.Id == userQuizQuestionsResponsesToFind[j].ResponseId);
                        if (quizQuestionResponseModelToFind != null)
                        {
                            QuestionResponse questionResponse = new QuestionResponse
                            {
                                ResponseId = userQuizQuestionsResponsesToFind[j].ResponseId,
                                Response = quizQuestionResponseModelToFind.Response,
                                ResponseUserId = userQuizQuestionsResponsesToFind[j].Id,
                                Correct = userQuizQuestionsResponsesToFind[j].Correct == "1" ? 1 : 0
                            };
                            questionQuizToAdd.QuestionResponses.Add(questionResponse);
                            generalResponseModel.UserQuizFullDetails.QuestionQuizList.Add(questionQuizToAdd);
                        }
                    }
                }
                List<UserResponse> userSharedDetails = GetUserDetails.GetUsersSharedDetails(responseSession.Token, responseSession.IpAddress, new List<string> { userQuizResponseModel.UserQuizResult.UserId });
                if (userSharedDetails != null && userSharedDetails.Count > 0)
                {
                    generalResponseModel.StudentName = userSharedDetails[0]?.Surname + ", " + userSharedDetails[0]?.GivenName;
                }
                else
                {
                    generalResponseModel.StudentName = string.Empty;
                }
                generalResponseModel.PDFLocation = await PDFResultGenerator.GeneratePDF(generalResponseModel);
                return Ok(generalResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
