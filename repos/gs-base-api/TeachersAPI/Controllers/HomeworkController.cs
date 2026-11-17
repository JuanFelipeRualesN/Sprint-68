using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeachersAPI.CustomClasses;
using TeachersAPI.Domain.Models.Generals.Requests;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.UserQuizzes;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Lessons;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI;
using Microsoft.AspNetCore.Http;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/homework")]
    public class HomeworkController : ControllerBase
    {
        /// <summary>
        /// Gets the pending assignments to be corrected from a teacher.
        /// </summary>
        /// <param name="detailsRequest">The general details. Includes th subject, level code, from page for pagination, limiter, etc. </param>
        /// <returns>All pending assignments with student asigned.</returns>
        [HttpPost("teacher/corrections/pendings")]
        public async Task<IActionResult> GetPendingCorrectionAssignments([FromBody] DetailsRequestModel detailsRequest)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherPendingAssignments");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get the pending assignments
                PendingAssignmentsResponseModel teacherPendingAssignmentsResponse =
                    TeacherPendingAssignmentsHandler.GetPendingAssignments(responseSession.UserId, detailsRequest.FromPage, detailsRequest.Limiter, responseSession.Token, responseSession.IpAddress);

                // return response
                return Ok(teacherPendingAssignmentsResponse);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// Gets the total count pending assignments to be corrected from a teacher.
        /// </summary>
        /// <returns>The count of total pending assignments</returns>
        [HttpGet("teacher/corrections/pendings/total")]
        public async Task<IActionResult> GetTotalPendingCorrectionAssignments()
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "TeacherPendingAssignments");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get the total count pending assignments
                int totalPendingAssignments = TeacherPendingAssignmentsHandler.GetTotalPendingAssignments(responseSession.UserId);

                // return response
                return Ok(new { TotalPendingAssignments = totalPendingAssignments });
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the created homework list by the teacher.
        /// </summary>
        /// <param name="onlyPublished">Call only published.</param>
        /// <returns>If call the assignments that are really to publish, then true, else, false and you will get all assignments, published or not.</returns>
        [HttpGet("user/created/{onlyPublished}")]
        public async Task<IActionResult> GetHomeworks(bool onlyPublished)
        {
            try
            {
                // Continue with everything else 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "HomeworkRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Call the code 
                QuizListResponseModel quizResponseModel = new QuizListResponseModel()
                {
                    SessionValid = true,
                    TransactionValid = true,
                    QuizModelList = new List<QuizEditModel>(),
                };

                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                quizResponseModel.QuizModelList = quizReaderManager.GetQuizModelsWithCanBeEditedEvaluation(new List<QuizModel>()
                {
                    new QuizModel()
                    {
                        ModifiedBy = responseSession.UserId,
                        Available = "1",
                        ReadyToPublish = onlyPublished ? "1": null,
                    },
                });

                // Return error about invalid request
                return Ok(quizResponseModel);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// To archive homework by homework's (quiz's) id.
        /// </summary>
        /// <param name="quizId">The homework’s id to archive.</param>
        /// <returns>Return a response model with transaction details.</returns>
        [HttpPost("archive/{quizId}")]
        public async Task<IActionResult> ArchiveHomework(string quizId)
        {
            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the assignment is from the user 
            QuizReaderManager quizReaderManager = new QuizReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
            List<QuizModel> quizModelList = quizReaderManager.GetQuizModels(new List<QuizModel>()
                {
                    new QuizModel()
                    {
                        ModifiedBy = responseSession.UserId,
                        Id = quizId
                    },
                });

            if (quizModelList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }

            // Now, check if the assignment has been used on a plan
            PlanningReaderManager planningReaderManager = new PlanningReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
            if (!planningReaderManager.CanQuizBeArchived(quizId))
            {
                // Cannot be archived because it has been used in a plan. Remove it from a plan, then remove it here. 
                return Ok(
                    new Domain.Models.Generals.Responses.ResponseModel()
                    {
                        SessionValid = true,
                        TransactionValid = false,
                        Messages = new List<string>()
                        {
                            new TeachersAPI.Services.Generals.MessageManager(
                                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_quiz_in_planning"])
                        },
                    });
            }

            // Archive it 
            QuizModifierManager quizModifierManager = new QuizModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
           Domain.Models.Lessons.Response.ResponseModel quizResponseModel = quizModifierManager.ArchiveQuiz(quizId);
            return Ok(quizResponseModel);
        }

        /// <summary>
        /// To get specific homework details by id.
        /// </summary>
        /// <param name="id">The id of the homework.</param>
        /// <returns>Quiz Response Model with all the quiz’s details. </returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetHomeworkDetails(string id)
        {
            try
            {

                // Continue with everything else 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "HomeworkRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // If session, then continue with the Quiz Read
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                QuizRequestModel quizResponseModelLib = quizReaderManager.GetQuiz(id, true, responseSession.UserId);

               Domain.Models.Generals.Responses.QuizResponseModel quizResponseModel = new Domain.Models.Generals.Responses.QuizResponseModel
                {
                    QuizRequestModel = new QuizRequestModel()
                    {
                        QuizAttachmentModelList = quizResponseModelLib?.QuizAttachmentModelList,
                        QuizModel = quizResponseModelLib.QuizModel,
                        QuestionRequestModelList = quizResponseModelLib.QuestionRequestModelList,
                        QuizRubricModel = quizResponseModelLib.QuizRubricModel,
                        QuizMatchInstructionsModel = quizResponseModelLib.QuizMatchInstructionsModel
                    }
                };

                return Ok(quizResponseModel);

            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// This allows the teacher to create a custom homework. The homework that can be created are select best answer, 
        /// true or false, open (to create assignments such as labs, essays, etc), and match. 
        /// </summary>
        /// <param name="quizModifierModel">The homework modifier model with the quiz details to create.</param>
        /// <returns>Returns a HTTP Response message.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateHomework([FromBody] QuizRequestModel quizModifierModel)
        {
            try
            {
                // Get user ID 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Create Homework
                QuizModifierManager quizModifierManager = new QuizModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
               Domain.Models.Generals.Responses.QuizResponseModel quizResponseModel = new Domain.Models.Generals.Responses.QuizResponseModel();

               Domain.Models.Lessons.Response.QuizResponseModel quizResponseModelLib =
                 quizModifierManager.CreateSimpleQuiz(quizModifierModel, responseSession.UserId);
                quizResponseModel.QuizRequestModel = new QuizRequestModel()
                {
                    QuizAttachmentModelList = quizResponseModelLib?.QuizAttachmentModelList,
                    QuizModel = quizResponseModelLib.QuizModel,
                    QuestionRequestModelList = quizResponseModelLib.QuestionRequestModelList,
                    QuizRubricModel = quizResponseModelLib.QuizRubricModel,
                    QuizMatchInstructionsModel = quizResponseModelLib.QuizMatchInstructionsModel
                };
                quizResponseModel.Messages = quizResponseModelLib.Messages;

                if (quizResponseModelLib.Valid)
                {
                    quizResponseModel.TransactionValid = true;  // Remove this after update in frontend that evaluate this.
                    quizResponseModel.SessionValid = true;  // Remove this after update in frontend that evaluate this.
                    return Created("", quizResponseModel);
                }
                quizResponseModel.TransactionValid = false;  // Remove this after update in frontend that evaluate this.
                quizResponseModel.SessionValid = true;  // Remove this after update in frontend that evaluate this.
                return BadRequest(quizResponseModel);

            }
            catch (Exception)
            {

                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// To modify a homework.
        /// Note: For Swagger, add the Id properties for each model. Since we cannot create a custom example for 
        /// creations and modifications (since it uses the same model), you may have to add the Ids manually in the example. 
        /// This is a known issue: https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/39
        /// </summary>
        /// <param name="quizModifierModel">The homework modifier model with the quiz details to modify.</param>
        /// <returns>Returns a HTTP Response message.</returns>
        [HttpPost("modify")]
        public async Task<IActionResult> UpdateHomework([FromBody] QuizRequestModel quizModifierModel)
        {
            try
            {

                // Get user ID 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Create Homework
                QuizModifierManager quizModifierManager = new QuizModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], 
                    responseSession.LangCode);
                var quizResponseModel = new Domain.Models.Generals.Responses.QuizResponseModel();

                var quizResponseModelLib =
                quizModifierManager.UpdateSimpleQuiz(quizModifierModel, responseSession.UserId);
                quizResponseModel.Messages = quizResponseModelLib.Messages;
                quizResponseModel.QuizRequestModel = new QuizRequestModel()
                {
                    QuizAttachmentModelList = quizResponseModelLib?.QuizAttachmentModelList,
                    QuizModel = quizResponseModelLib.QuizModel,
                    QuestionRequestModelList = quizResponseModelLib.QuestionRequestModelList,
                    QuizRubricModel = quizResponseModelLib.QuizRubricModel,
                    QuizMatchInstructionsModel = quizResponseModelLib.QuizMatchInstructionsModel
                };
                if (!quizResponseModelLib.Valid)
                {
                    quizResponseModel.TransactionValid = false; // Remove this after update in frontend that evaluate this.
                    quizResponseModel.SessionValid = true;  // Remove this after update in frontend that evaluate this.
                    return BadRequest(quizResponseModel);
                }
                quizResponseModel.TransactionValid = true;  // Remove this after update in frontend that evaluate this.
                quizResponseModel.SessionValid = true;  // Remove this after update in frontend that evaluate this.
                return Ok(quizResponseModel);

            }
            catch (Exception)
            {

                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// To validate if a homework can be edited after it has been published.
        /// </summary>
        /// <param name="quizId">The quiz id to check.</param>
        /// <returns>Returns valid and message details according to results.</returns>
        [HttpGet("validate-edit-published/{quizId}")]
        public async Task<IActionResult> EvaluateIfHomeworkCanBeUpdatedAfterPublish(int quizId)
        {
            try
            {
                // Get user ID 
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Check if Homework can be edited
                var quizModifierManager = new QuizModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    responseSession.LangCode);

                var quizCanBeUpdated = quizModifierManager.ValidateIfQuizCanBeUpdatedAfterPublish(quizId);
                if (quizCanBeUpdated.Valid)
                {
                    return Ok(quizCanBeUpdated);
                }
                return BadRequest(quizCanBeUpdated);
            }
            catch (Exception ex)
            {

                // Return error about invalid json
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To duplicate a homework.
        /// </summary>
        /// <param name="id">Id of the homework to duplicate.</param>
        /// <returns>Returns a HTTP Response with the duplicated assignment.</returns>
        [HttpPost("duplicate/{id}")]
        public async Task<IActionResult> DuplicateHomework(string id)
        {
            try
            {
                // Get user ID 
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Create Homework
                var quizModifierManager = new QuizModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

                var quizResponseModelLib =
                 quizModifierManager.DuplicateSimpleQuiz(id, responseSession.UserId);

                return StatusCode(quizResponseModelLib.TransactionValid ? StatusCodes.Status201Created : StatusCodes.Status400BadRequest, quizResponseModelLib);
            }
            catch (Exception)
            {

                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// This endpoint is for teachers to manually correct a student quiz.
        /// </summary>
        /// <param name="quizModifierModel">The homework to correct manually.</param>
        /// <returns>Returns a HTTP Response message.</returns>
        [HttpPost("planning/student/quiz/manual/correct")]
        public async Task<IActionResult> CorrectStudentQuizManual([FromBody] QuizManualCorrectionRequestModel quizModifierModel)
        {
            try
            {

                // Get user ID 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get how many the quiz value 
                UserQuizManager userQuizManager = new UserQuizManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var userQuizResultModels = new 
                    List<Domain.Models.UserQuizzes.UserQuizResultModel>();

                userQuizResultModels = userQuizManager.GetUsersQuizResults(new List<Domain.Models.UserQuizzes.UserQuizResultModel>()
                {
                    new Domain.Models.UserQuizzes.UserQuizResultModel()
                    {
                        Id = quizModifierModel.UserQuizResultsId,
                    }
                });

                QuizModel quizModel = new QuizModel();
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                quizModel = quizReaderManager.GetQuizModels(new List<QuizModel>()
                {
                    new QuizModel()
                    {
                        Id = userQuizResultModels[0].QuizId,
                    }
                })[0];

               Domain.Models.UserQuizzes.UserQuizResultModel response = userQuizManager.CorrectManualQuiz(quizModifierModel.UserResponseModelList, 
                    quizModifierModel.UserQuizResultsId, responseSession.UserId, decimal.Parse(quizModel.CustomValue), quizModifierModel.Feedback);

                if (!response.Valid)
                {
                    return BadRequest(response);
                }

                // Create the user plan assignment 
                if (!string.IsNullOrWhiteSpace(quizModifierModel.PlanQuizId))
                {

                    new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).AddPlanAssignmentUserResult(new List<PlanAssignmentUserResultModel>()
                    {
                        new PlanAssignmentUserResultModel()
                        {
                            PlanQuizId = quizModifierModel.PlanQuizId,
                            UserId = userQuizResultModels[0].UserId,
                            UsquiUserResultId = userQuizResultModels[0].Id,
                            ModifiedBy = responseSession.UserId,
                        }
                    }, responseSession.UserId);
                }

                return Ok(new Domain.Models.Generals.Responses.ResponseModel()
                {
                    Messages = response.Messages,
                });
            }
            catch (Exception)
            {

                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// This endpoint is for teachers to manually correct a student quiz without the student submitting it.
        /// </summary>
        /// <param name="planAssignmentUserResultModel">The plan assignment user result model.</param>
        /// <returns>Returns a HTTP Response message.</returns>
        [HttpPost("planning/{planningId}/planning-quiz/{planningQuizId}/student/{studentId}/quiz/{quizId}/manual-correction")]
        public async Task<IActionResult> CorrectStudentQuizManualWithoutUserSubmit([FromBody] PlanAssignmentUserResultModel planAssignmentUserResultModel)
        {
            try
            {
                // Get user ID 
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Homework");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get how many the quiz value 
                var quizModifierManager = new QuizModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                var result = quizModifierManager.ManuallyCorrectPlanQuizStudentWithoutStudentSubmit(planAssignmentUserResultModel, responseSession.UserId);

                return !result.Valid ? BadRequest(result) : Ok(result);
            }
            catch (Exception)
            {

                // Return error about invalid json
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the pending assignments of the student (*For students).
        /// </summary>
        /// <param name="detailsRequest">The general details. Includes th subject, level code, from page for pagination, limiter, etc. </param>
        /// <returns>Returns a HTTPResponseMessage.</returns>
        [HttpPost("pending/student")]
        public async Task<IActionResult> GetUserHomeworks([FromBody] DetailsRequestModel detailsRequest)
        {
            try
            {
                if (detailsRequest == null)
                {
                    return BadRequest();
                }

                // First, lets get the session response 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Now continue with everything else 
                return Ok(StudentAssignmentsAndResults.GetStudentsHomework(responseSession.UserId, detailsRequest.FromPage, detailsRequest.Limiter, responseSession.Token, responseSession.IpAddress, detailsRequest?.SubjectCode, detailsRequest?.LevelCode, "P", true));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the completed assignments of student (*For students).
        /// </summary>
        /// <param name="detailsRequest">The general details. Includes teh subject, level code, from page for pagination, limiter, etc. </param>
        /// <returns>Includes the homework’s details list.</returns>
        [HttpPost("completed/student")]
        public async Task<IActionResult> GetUserHomeworksSubmitted([FromBody] DetailsRequestModel detailsRequest)
        {

            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Now continue with everything else 
                return Ok(StudentAssignmentsAndResults.GetStudentsHomework(responseSession.UserId, detailsRequest.FromPage, detailsRequest.Limiter, responseSession.Token, responseSession.IpAddress, detailsRequest?.SubjectCode, detailsRequest?.LevelCode, "C", false));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the student's quizzes and overall average.
        /// </summary>
        /// <param name="planningId">The teacher’s plan id.</param>
        /// <param name="studentId">The student's user's id.</param>
        /// <returns>Returns a Http Response Message with student homework details.</returns>
        [HttpGet("planning/{planningId}/student/{studentId}")]
        public async Task<IActionResult> GetStudentQuizzes(string planningId, string studentId)
        {
            try
            {
                // Check token and if user can connect 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningAverageRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Verify if user can manager the other user 
                if (!GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token))
                {
                    return Unauthorized();
                }

                // Get the planning with the overall
                return Ok(StudentAssignmentsAndResults.GetStudentHomeworkResults(new DetailsRequestModel() { Limiter = 100, FromPage = 0 }, responseSession.Token, new List<string>() { studentId }, responseSession.IpAddress, 0, planningId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the student custom quiz results, for the student.
        /// </summary>
        /// <param name="studentQuizId">The student’s quiz id (the id of the results of the student).</param>
        /// <returns>The student’s result’s details of the homework. Includes QuizId, instructions, score, etc.</returns>
        [HttpGet("planning/for-student/quiz/{studentQuizId}")]
        public async Task<IActionResult> GetStudentQuizDetailsForTeacher(string studentQuizId)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningAverageRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }


                // Now, call the quizzes completed by the student and call the lesson detials.
                List<QuizModel> quizModelsToFind = new List<QuizModel>();
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                var userQuizResultModelsToFind = new List<Domain.Models.UserQuizzes.UserQuizResultModel>();
                UserQuizManager userQuizManager = new UserQuizManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);

                // Get User responses, send responseSession user id to see if the user can see the results.
                UserQuizResponseModel userQuizResponseModel = userQuizManager.GetUserQuizResponse(studentQuizId, responseSession.UserId);

                // Call full quiz details
                QuizRequestModel quizRequestModel = quizReaderManager.GetQuiz(
                    userQuizResponseModel.UserQuizResult.QuizId, true);

                // Check if auto correct 
                bool autoCorrect = true;
                List<QuizTypeModel> quizTypeModels = new List<QuizTypeModel>();
                quizTypeModels = quizReaderManager.GetQuizType(
                    new List<QuizTypeModel>() {
                        new QuizTypeModel()
                            {
                                Id = quizRequestModel.QuizModel.QuizTypeId,
                                AutoCorrect = "1"
                            }
                        });
                if (quizTypeModels.Count == 0)
                {
                    autoCorrect = false;
                }


                // Attach everything 
                // Now connect the data
                UserQuizFullDetailModel userQuizFullDetailModel = new UserQuizFullDetailModel()
                {
                    QuizId = userQuizResponseModel.UserQuizResult.QuizId,
                    Feedback = userQuizResponseModel.UserQuizResult.Feedback,
                    QuizTitle = quizRequestModel.QuizModel.Title,
                    Instructions = quizRequestModel.QuizModel.Instruction,
                    Status = userQuizResponseModel.UserQuizResult.Status,
                    UserQuizId = userQuizResponseModel.UserQuizResult.Id,
                    QuestionQuizList = new List<QuestionQuiz>(),
                    TotalPercent = userQuizResponseModel.UserQuizResult.TotalPercent,
                    DoK = userQuizResponseModel.UserQuizResult.Dok,
                    TotalCorrect = userQuizResponseModel.UserQuizResult.TotalCorrect,
                    TryNo = userQuizResponseModel.UserQuizResult.TryNo,
                    QuizAttachmentModelList = quizRequestModel.QuizAttachmentModelList,
                    AutoCorrect = autoCorrect,
                    CustomValue = quizRequestModel.QuizModel.CustomValue,
                    DateSubmitted = userQuizResponseModel.UserQuizResult.CreatedAt,
                    UserQuizFileModelList = userQuizResponseModel.UserQuizFileModelList,
                };

                for (int i = 0; i < quizRequestModel.QuestionRequestModelList.Count; i++)
                {
                    QuestionQuiz questionQuizToAdd = new QuestionQuiz()
                    {
                        QuestionId = quizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id,
                        Question = quizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Desc,
                        QuestionResponses = new List<QuestionResponse>(),
                    };

                    // Attach the quizzes responses
                    foreach (QuizQuestionResponseModel questionResponse in quizRequestModel.QuestionRequestModelList[i].QuizQuestionResponseModelList)
                    {

                       Domain.Models.UserQuizzes.UserResponseModel userResponse = userQuizResponseModel.UserResponsesList.Find(x => x.QuestionId == questionResponse.QuizQuestionId);

                        QuestionResponse responseToAdd = new QuestionResponse
                        {
                            ResponseId = questionResponse.Id,
                            SelectedByUser = userResponse.ResponseId == questionResponse.Id,
                            Response = questionResponse.Response,
                            CorrectAnswer = questionResponse.Correct == "1",
                            UserSelectCorrectAnswer = autoCorrect && userResponse?.ResponseId == questionResponse.Id && questionResponse.Correct == "1",
                            AssignedValue = userResponse?.ResponseId == questionResponse.Id ? userResponse.AssignedValue : "",
                            ResponseUserId = userResponse?.ResponseId,
                            ResponseExtra = userResponse?.ResponseExtra,
                            UserSelectedResponseText = "",
                            Id = userResponse.Id,
                        };

                        if (userResponse != null && !string.IsNullOrWhiteSpace(userResponse.ResponseId))
                        {
                            foreach (QuestionRequestModel questionResponseToFindUserResponse in quizRequestModel.QuestionRequestModelList)
                            {
                                if (questionResponseToFindUserResponse.QuizQuestionResponseModelList != null && questionResponseToFindUserResponse.QuizQuestionResponseModelList.Count > 0)
                                {
                                    for (int j = 0; j < questionResponseToFindUserResponse.QuizQuestionResponseModelList.Count; j++)
                                    {
                                        if (userResponse.ResponseId == questionResponseToFindUserResponse.QuizQuestionResponseModelList[j].Id)
                                        {
                                            responseToAdd.UserSelectedResponseText = questionResponseToFindUserResponse.QuizQuestionResponseModelList[j].Response;
                                        }
                                    }
                                }
                            }
                        }

                        questionQuizToAdd.QuestionResponses.Add(responseToAdd);

                    }

                    // If there are no responses, the question could be opened and we should return the open answer to that question. 
                    if (quizRequestModel.QuestionRequestModelList[i].QuizQuestionResponseModelList.Count == 0)
                    {
                       Domain.Models.UserQuizzes.UserResponseModel userResponse = userQuizResponseModel.UserResponsesList.Find(x => x.QuestionId == quizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id);
                        if (userResponse != null)
                        {
                            QuestionResponse responseToAdd = new QuestionResponse
                            {
                                AssignedValue = userResponse.AssignedValue,
                                ResponseExtra = userResponse?.ResponseExtra,
                                Id = userResponse?.Id,
                            };

                            questionQuizToAdd.QuestionResponses.Add(responseToAdd);
                        }
                    }

                    userQuizFullDetailModel.QuestionQuizList.Add(questionQuizToAdd);
                }

                return Ok(userQuizFullDetailModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// To get the student custom quiz results, so the teacher can view these results or correct them.
        /// </summary>
        /// <param name="studentId">The student’s user’s id.</param>
        /// <param name="studentQuizId">The student’s quiz id (the id of the results of the student).</param>
        /// <returns>The student’s result’s details of the homework. Includes QuizId, instructions, score, etc.</returns>
        [HttpGet("planning/student/{studentId}/quiz/{studentQuizId}")]
        public async Task<IActionResult> GetStudentQuizDetailsForTeacher(string studentId, string studentQuizId)
        {
            try
            {
                // Check token and if user can connect 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningAverageRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Verify if user can manager the other user 
                if (!GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token))
                {
                    return Unauthorized();
                }


                // Now, call the quizzes completed by the student and call the lesson detials.
                List<QuizModel> quizModelsToFind = new List<QuizModel>();
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
               var userQuizResultModelsToFind = new List<Domain.Models.UserQuizzes.UserQuizResultModel>();
                UserQuizManager userQuizManager = new UserQuizManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);

                // Get User responses
                UserQuizResponseModel userQuizResponseModel = userQuizManager.GetUserQuizResponse(studentQuizId, AppConfiguration.Configuration["AppSettings:webservice_url"]);

                // Call full quiz details
                QuizRequestModel quizRequestModel = quizReaderManager.GetQuiz(
                    userQuizResponseModel.UserQuizResult.QuizId, true);

                // Check if auto correct 
                bool autoCorrect = true;
                List<QuizTypeModel> quizTypeModels = new List<QuizTypeModel>();
                quizTypeModels = quizReaderManager.GetQuizType(new List<QuizTypeModel>() {
                new QuizTypeModel()
                {
                    Id = quizRequestModel.QuizModel.QuizTypeId,
                    AutoCorrect = "1"
                }
            });
                if (quizTypeModels.Count == 0)
                {
                    autoCorrect = false;
                }


                // Attach everything 
                // Now connect the data
                UserQuizFullDetailModel userQuizFullDetailModel = new UserQuizFullDetailModel()
                {
                    QuizId = userQuizResponseModel.UserQuizResult.QuizId,
                    Feedback = userQuizResponseModel.UserQuizResult.Feedback,
                    QuizTitle = quizRequestModel.QuizModel.Title,
                    Instructions = quizRequestModel.QuizModel.Instruction,
                    Status = userQuizResponseModel.UserQuizResult.Status,
                    UserQuizId = userQuizResponseModel.UserQuizResult.Id,
                    QuestionQuizList = new List<QuestionQuiz>(),
                    TotalPercent = userQuizResponseModel.UserQuizResult.TotalPercent,
                    DoK = userQuizResponseModel.UserQuizResult.Dok,
                    TotalCorrect = userQuizResponseModel.UserQuizResult.TotalCorrect,
                    TryNo = userQuizResponseModel.UserQuizResult.TryNo,
                    QuizAttachmentModelList = quizRequestModel.QuizAttachmentModelList,
                    AutoCorrect = autoCorrect,
                    CustomValue = quizRequestModel.QuizModel.CustomValue,
                    DateSubmitted = userQuizResponseModel.UserQuizResult.CreatedAt,
                    UserQuizFileModelList = userQuizResponseModel.UserQuizFileModelList,
                    Rubric = quizRequestModel?.QuizRubricModel?.Desc
                };

                for (int i = 0; i < quizRequestModel.QuestionRequestModelList.Count; i++)
                {
                    QuestionQuiz questionQuizToAdd = new QuestionQuiz()
                    {
                        QuestionId = quizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id,
                        Question = quizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Desc,
                        QuestionResponses = new List<QuestionResponse>(),
                    };

                    // Attach the quizzes responses
                    foreach (QuizQuestionResponseModel questionResponse in quizRequestModel.QuestionRequestModelList[i].QuizQuestionResponseModelList)
                    {

                       Domain.Models.UserQuizzes.UserResponseModel userResponse = userQuizResponseModel.UserResponsesList.Find(x => x.QuestionId == questionResponse.QuizQuestionId);

                        QuestionResponse responseToAdd = new QuestionResponse
                        {
                            ResponseId = questionResponse.Id,
                            SelectedByUser = userResponse.ResponseId == questionResponse.Id,
                            Response = questionResponse.Response,
                            CorrectAnswer = questionResponse.Correct == "1",
                            UserSelectCorrectAnswer = autoCorrect && userResponse?.ResponseId == questionResponse.Id && questionResponse.Correct == "1",
                            AssignedValue = userResponse?.ResponseId == questionResponse.Id ? userResponse.AssignedValue : "",
                            ResponseUserId = userResponse?.ResponseId,
                            ResponseExtra = userResponse?.ResponseExtra,
                            UserSelectedResponseText = "",
                            Id = userResponse.Id,
                        };

                        if (userResponse != null && !string.IsNullOrWhiteSpace(userResponse.ResponseId))
                        {
                            foreach (QuestionRequestModel questionResponseToFindUserResponse in quizRequestModel.QuestionRequestModelList)
                            {
                                if (questionResponseToFindUserResponse.QuizQuestionResponseModelList != null && questionResponseToFindUserResponse.QuizQuestionResponseModelList.Count > 0)
                                {
                                    for (int j = 0; j < questionResponseToFindUserResponse.QuizQuestionResponseModelList.Count; j++)
                                    {
                                        if (userResponse.ResponseId == questionResponseToFindUserResponse.QuizQuestionResponseModelList[j].Id)
                                        {
                                            responseToAdd.UserSelectedResponseText = questionResponseToFindUserResponse.QuizQuestionResponseModelList[j].Response;
                                        }
                                    }
                                }
                            }
                        }

                        questionQuizToAdd.QuestionResponses.Add(responseToAdd);

                    }

                    // If there are no responses, the question could be opened and we should return the open answer to that question. 
                    if (quizRequestModel.QuestionRequestModelList[i].QuizQuestionResponseModelList.Count == 0)
                    {
                       Domain.Models.UserQuizzes.UserResponseModel userResponse = userQuizResponseModel.UserResponsesList.Find(x => x.QuestionId == quizRequestModel.QuestionRequestModelList[i].QuizQuestionModel.Id);
                        if (userResponse != null)
                        {
                            QuestionResponse responseToAdd = new QuestionResponse
                            {
                                AssignedValue = userResponse.AssignedValue,
                                ResponseExtra = userResponse?.ResponseExtra,
                                Id = userResponse?.Id,
                            };

                            questionQuizToAdd.QuestionResponses.Add(responseToAdd);
                        }
                    }

                    userQuizFullDetailModel.QuestionQuizList.Add(questionQuizToAdd);
                }

                return Ok(userQuizFullDetailModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// To get the student custom quiz results, so the teacher can view these results or correct them.
        /// </summary>
        /// <param name="studentId">The student’s user’s id.</param>
        /// <param name="studentQuizId">The student’s quiz id (the id of the results of the student).</param>
        /// <returns>The student’s result’s details of the homework. Includes QuizId, instructions, score, etc.</returns>
        [HttpGet("planning/student/{studentId}/plan-quiz-id/{planQuizId}")]
        public async Task<IActionResult> GetStudentQuizDetailsForTeacher(string studentId, string planQuizId, string studentQuizId = null)
        {
            try
            {
                // Check token and if user can connect 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningAverageRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Verify if user can manager the other user 
                if (!GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token))
                {
                    return Unauthorized();
                }


                return Ok(new QuizReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null).
                        GetQuizDetailsWithStudentResults(planQuizId, studentQuizId, studentId));
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Generates an authorization code before redirecting to a student's homework created by a teacher.
        /// </summary>
        /// <param name="teacherPlanQuizId">The homework’s id of the teacher’s plan id.</param>
        /// <returns>Returns a response that includes the authorization’s code.</returns>
        [HttpPost("authorization-code/create/{teacherPlanQuizId}")]
        public async Task<IActionResult> GenerateAuthorizationCode(string teacherPlanQuizId)
        {
            // Get the user token that is asking for authorization. 
            try
            {
                // Check token and if user can connect
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AuthorizationQuizCode");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                QuizTriesLeftModel quizTriesLeft = TeacherTryEvaluator.GetUserHomeworkQuizTriesAvailable(teacherPlanQuizId, responseSession.UserId);
                if (quizTriesLeft.TriesLeft > 0)
                {
                    string accessAuthorizationCode = new UserQuizManager(
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null).
                           GenerateTokenAuthorizationForCustomQuiz(responseSession.UserId, quizTriesLeft.QuizId, responseSession.UserId);

                    if (string.IsNullOrWhiteSpace(accessAuthorizationCode))
                    {
                        return Unauthorized();
                    }

                    return Created("", new AuthorizationCodeResponseModel()
                    {
                        TriesLeft = quizTriesLeft.TriesLeft,
                        TriesTotal = quizTriesLeft.TriesTotal,
                        TriesUsed = quizTriesLeft.TriesUsed,
                        AuthorizationCode = accessAuthorizationCode,
                        TransactionValid = true,
                        SessionValid = true,
                        QuizAttachmentsURLs = new QuizReaderManager(
                            AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null).GetQuizAttachmentsURLs(quizTriesLeft.QuizId)
                    });
                }

                return Unauthorized();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the quiz based on the student authorization code.
        /// </summary>
        /// <param name="authorizationCode">An authorization code generated for the student so the student can take the quiz.</param>
        /// <returns>Includes the quiz details.</returns>
        [HttpGet("quiz/student-quiz/{authorizationCode}")]
        public async Task<IActionResult> GetTheQuizByAuthorizationCode(string authorizationCode)
        {
            try
            {
                // Check token and if user can connect
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AuthorizationQuizCode");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // First, get the quiz id and user id from the authorization code. 
                UserQuizManager userQuizManager = new UserQuizManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeModels = userQuizManager.GetUserQuizAuthorizationCodeModels(new List<UserQuizAuthorizationCodeModel>()
                {
                    new UserQuizAuthorizationCodeModel()
                    {
                        AuthorizationToken = authorizationCode,
                        UserId = responseSession.UserId,
                    }
                });

                // If the user is here, probably is tricking, since the authorization code is created before this, so we do not return too much info.
                if (userQuizAuthorizationCodeModels.Count == 0)
                {
                    return BadRequest();
                }

                // Now get the quiz details. 
                // Call full quiz details
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], null);
                QuizRequestModel quizRequest = quizReaderManager.GetQuiz(
                    userQuizAuthorizationCodeModels[0].CustomQuizId, true);

                // randomize 
                if (quizRequest != null
                    && quizRequest.QuestionRequestModelList != null
                    && quizRequest.QuestionRequestModelList.Count > 0)
                {
                    string[] unrandomizedQuizTypes = AppConfiguration.Configuration["AppSettings:gt_unrandomized_quizzes_types"].Split(',');

                    if (!unrandomizedQuizTypes.Contains(quizRequest.QuizModel.QuizTypeId))
                    {
                        Random rnd = new Random();
                        foreach (QuestionRequestModel question in quizRequest.QuestionRequestModelList)
                        {
                            if (question.QuizQuestionResponseModelList != null && question.QuizQuestionResponseModelList.Count > 0)
                            {
                                ShuffleList.Shuffle(question.QuizQuestionResponseModelList, rnd);
                            }
                        }
                    }
                }

                return Ok(quizRequest);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the quiz based on the quiz Id (only for teachers).
        /// </summary>
        /// <param name="quizId">The homework’s id.</param>
        /// <returns>Includes all the details of the quiz.</returns>
        [HttpGet("teacher-quiz/{quizId}")]
        public async Task<IActionResult> GetTheQuizByQuizId(string quizId)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Check if the quiz is from the teacher. 
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);

                // Now get the quiz details. 
                return Ok(quizReaderManager.GetQuiz(
                     quizId,
                     true,
					 responseSession.IsDirector ? null : responseSession.UserId, true));
			}
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Insert the student's completed quiz. 
        /// Corrects automatically if auto-correct, if not, it inserts the responses so the teacher can correct them later.
        /// </summary>
        /// <param name="studentQuizRequestModel">The student's quiz details to insert and correct.</param>
        /// <returns>Returns just the HTTP Response Code.</returns>
        [HttpPost("student-quiz/correction")]
        public async Task<IActionResult> CorrectStudentCustomQuiz([FromBody] StudentQuizRequestModel studentQuizRequestModel)
        {
            // Delete the Authorization Code once corrected. 
            // Check the token, and check if the user can access to the quiz. 
            // Get the user token that is asking for authorization. 
            try
            {
                // Check token and if user can connect
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AuthorizationQuizCode");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // First, get the quiz id and user id from the authorization code. 
                UserQuizManager userQuizManager = new UserQuizManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                List<UserQuizAuthorizationCodeModel> userQuizAuthorizationCodeModels = userQuizManager.GetUserQuizAuthorizationCodeModels(new List<UserQuizAuthorizationCodeModel>()
                {
                    new UserQuizAuthorizationCodeModel()
                    {
                        AuthorizationToken = studentQuizRequestModel.AuthorizationCode,
                        UserId = responseSession.UserId,
                    }
                });

                // If the user is here, probably is tricking, since the authorization code is created before this, so we do not return too much info.
                if (userQuizAuthorizationCodeModels.Count == 0)
                {
                    return Ok();
                }

                // Check the plans that have the the assignment
                // First, find the plans of the user that have the homework. 
                var planQuizzesIds = TeacherPlans.GetPlanQuizIdsAccordingToQuizAndStudent(responseSession.UserId, userQuizAuthorizationCodeModels[0].CustomQuizId);
                if (planQuizzesIds == null || planQuizzesIds.Count == 0)
                {
                    // Cannot be inserted
                    return BadRequest();
                }

                // Check the tries before doing anything 
                // Get the results 
                var userQuizResults = UserResults.GetUserQuizHomeworkPlanResults(new List<string>() { userQuizAuthorizationCodeModels[0].CustomQuizId }, responseSession.UserId);
                // Now check for plan quiz id  if there is any try left 
                if (!TeacherTryEvaluator.CheckIfUserHasTryLeftHomeworkQuiz(userQuizResults, planQuizzesIds))
                {
                    return BadRequest();
                }


                // Now, check if auto correction. If auto correction is false, just inset it. 
                // Call the quiz details 
                QuizReaderManager quizReaderManager = new QuizReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                QuizRequestModel quizRequestModel = quizReaderManager.GetQuiz(
                   userQuizAuthorizationCodeModels[0].CustomQuizId, true);

                // Prepare the user result
               var userQuizResultModelToInsert = new Domain.Models.UserQuizzes.UserQuizResultModel
                {
                    UserId = responseSession.UserId,
                    QuizId = userQuizAuthorizationCodeModels[0].CustomQuizId,
                    TotalQuestions = quizRequestModel.QuestionRequestModelList.Count.ToString(),
                    ModifiedBy = responseSession.UserId,
                    IsExam = "1",
                };

                // Prepare the user responses
                var userResponseModelListToInsert = new List<Domain.Models.UserQuizzes.UserResponseModel>();
                foreach (QuestionQuiz questionQuiz in studentQuizRequestModel.UserQuizFullDetailModel.QuestionQuizList)
                {
                   var userResponseModelToInsert = new Domain.Models.UserQuizzes.UserResponseModel
                    {
                        QuestionId = questionQuiz.QuestionId,
                        ResponseId = questionQuiz.QuestionResponses[0].ResponseUserId,
                        ResponseExtra = questionQuiz.QuestionResponses[0].ResponseExtra,
                        ModifiedBy = responseSession.UserId,
                    };
                    userResponseModelListToInsert.Add(userResponseModelToInsert);
                }

                if (quizRequestModel.AutoCorrect)
                {
                    userQuizResultModelToInsert.Status = "C"; // Completed
                    var responseUserQuiz = 
                        userQuizManager.InsertUserQuizAndResponsesFullProccess(quizRequestModel.QuestionRequestModelList,
                        userQuizResultModelToInsert, userResponseModelListToInsert, true,
                        decimal.Parse(quizRequestModel.QuizModel.CustomValue));

                    // Now prepare the results to insert 
                    List<PlanAssignmentUserResultModel> planAssignmentUserResultsToInsert = new List<PlanAssignmentUserResultModel>();
                    foreach (var teacherQuiz in planQuizzesIds)
                    {
                        planAssignmentUserResultsToInsert.Add(new PlanAssignmentUserResultModel()
                        {
                            PlanQuizId = teacherQuiz.Id,
                            UserId = userQuizResultModelToInsert.UserId,
                            UsquiUserResultId = responseUserQuiz.Id,
                            ModifiedBy = responseSession.UserId,
                        });
                    }

                    new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null).AddPlanAssignmentUserResult(planAssignmentUserResultsToInsert, responseSession.UserId);
                }
                else
                {
                    // Just insert
                    userQuizResultModelToInsert.Status = "P"; // Pending to correction.
                    userQuizResultModelToInsert = userQuizManager.InsertUserQuizResult(userQuizResultModelToInsert);
                    // Set the Id for each question 
                    foreach (var userResponse in userResponseModelListToInsert)
                    {
                        userResponse.UserQuizResultId = userQuizResultModelToInsert.Id;
                    }
                    userQuizManager.InsertUserResponseQuestionsList(userResponseModelListToInsert);

                    List<PlanAssignmentUserResultModel> planAssignmentUserResultsToInsert = new List<PlanAssignmentUserResultModel>();
                    foreach (var teacherQuiz in planQuizzesIds)
                    {
                        planAssignmentUserResultsToInsert.Add(new PlanAssignmentUserResultModel()
                        {
                            PlanQuizId = teacherQuiz.Id,
                            UserId = userQuizResultModelToInsert.UserId,
                            UsquiUserResultId = userQuizResultModelToInsert.Id,
                            ModifiedBy = responseSession.UserId,
                        });
                    }

                    new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null).AddPlanAssignmentUserResult(planAssignmentUserResultsToInsert, responseSession.UserId);
                }

                // Delete authorization code 
                userQuizManager.DeleteAuthorizationCodeCustomQuizId(studentQuizRequestModel.AuthorizationCode);
                return StatusCode(StatusCodes.Status201Created);
            }

            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the pending homework of a group of students related to the tutor.
        /// </summary>
        /// <param name="profileId">The profile's (student) id.</param>
        /// <param name="fromPage">Current page of pagination. Example: 1.</param>
        /// <param name="limiter">How many items per page. Example: 10.</param>
        /// <param name="planningId">The teacher's instrucional plan (optional).</param>
        /// <returns>Includes the homework’s response with the homework list.</returns>
        [HttpGet("pending/tutor/student/{profileId}/{fromPage}/{limiter}")]
        public async Task<IActionResult> GetTutorStudentsHomeworks(string profileId, int fromPage, int limiter, string planningId = null)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ProfileConnections");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Get the users details of the user that can hadnle. The session validation is here, because if 0 users are returned, there is not point to continue.
                List<UserResponse> userSharedDetails = GetUserDetails.GetUsersThatAccountCanHandle(responseSession.Token, responseSession.IpAddress);
                if (userSharedDetails.Count == 0)
                {
                    return Ok(new HomeworkResponseModel());
                }

                // Check if user can handle the profile
                bool profileFound = false;
                foreach(UserResponse userResponse in userSharedDetails)
                {
                    if(userResponse.UserId == profileId)
                    {
                        profileFound = true;
                        break;
                    }
                }

                if(!profileFound)
                {
                    return Ok(new HomeworkResponseModel());
                }

                return Ok(StudentAssignmentsAndResults.GetStudentsHomework(profileId, fromPage, limiter, responseSession.Token, responseSession.IpAddress, null, null, "P", false, planningId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
