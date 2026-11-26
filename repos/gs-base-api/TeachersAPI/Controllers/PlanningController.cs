using Microsoft.AspNetCore.Mvc;
using TeachersAPI.CustomClasses;
using TeachersAPI.TeachersAPI.Services.UserQuizzes;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas;
using TeachersAPI.Domain.Models.Generals.Requests;
using TeachersAPI.Domain.Models.Generals.Requests.APIs;
using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Generals;
using System.Diagnostics;
using TeachersAPI;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/planning/")]
    public partial class PlanningController : ControllerBase
    {

        /// <summary>
		/// This is to retrieve the created instructional plans by the user making the request (teacher).
		/// </summary>
		/// <param name="teacherId">The id of the teacher.</param>
		/// <param name="subjectCode">The grade subject code.</param>
		/// <param name="level">The grade level.</param>
		/// <param name="name">The name of the plan.</param>
        /// <param name="teacherGroup">Teacher students group.</param>
		/// <param name="fromDate">The starting date from which the query should retrieve data.</param>
		/// <param name="toDate">Represents the ending date until which the query should retrieve data.</param>
		/// <returns>An array of teacher’s instructional’s plans.</returns>
		[HttpGet("user/created")]
        public async Task<IActionResult> GetPlannings(string teacherId = null, string subjectCode = null, string level = null, string name = null, string teacherGroup = null, string fromDate = null, string toDate = null)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                string userId = string.IsNullOrEmpty(teacherId) ? responseSession.UserId : teacherId;
                var result = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode)
                    .GetTeacherPlansFullDetails(RequestHandler.GetToken(HttpContext), userId, subjectCode, level, name, teacherGroup, fromDate, toDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This is to get the detail of a teacher’s instructional plan.
        /// </summary>
        /// <param name="id">Id of the instructional plan.</param>
        /// <param name="limit">Limit of the number of records to be retrieved.</param>
        /// <param name="pageNumber">Page number to be retrieved.</param>
        /// <returns>Details of instructional plan with quizzes and lessons.</returns>
        [HttpGet]
        [Route("detail/{id}")]
        public async Task<IActionResult> GetPlanningDetails(string id, int? limit = null, int? pageNumber = null)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                return Ok(new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode)
                    .GetTeacherPlanFullDetails(id, responseSession.UserId, RequestHandler.GetToken(HttpContext), limit, pageNumber));
            }
            catch (Exception ex)
            {
                return BadRequest( ex.ToString());
            }
        }

        /// <summary>
        /// To modify the planning name.
        /// </summary>
        /// <param name="id">The id of the plan.</param>
        /// <param name="teacherPlanModel">The teacher plan model which should include the new name.</param>
        /// <returns>Returns an HTTP Response.</returns>
        [HttpPatch]
        [Route("detail/{id}/name")]
        public async Task<IActionResult> ModifyPlanningName(int id, Domain.Models.Teachers.TeacherPlanModel teacherPlanModel)
        {
            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var result = new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).ModifyPlanName(id, teacherPlanModel?.Name, responseSession.UserId);

            return result.Valid ? Ok() : BadRequest();
        }

        /// <summary>
        /// This is to archive a instructional plan.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="planningId">Id of the instructional plan that we want to archive.</param>
        /// <returns>Response with transaction details.</returns>
        [HttpPost]
        [Route("archive/{planningId}/")]
        public async Task<IActionResult> ArchivePlanning(string planningId)
        {
            try
            {
                // Check access
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Check if the planning is from the user 
                var planningReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], responseSession.LangCode);
                var teacherPlanList =
                    planningReaderManager.GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
                {
               new Domain.Models.Generals.Responses.TeacherPlanModel()
               {
                   Id = planningId,
                   UserId = responseSession.UserId
               }
                });

                if (teacherPlanList.Count == 0)
                {
                    // It doesn't belong to the user. 
                    return Unauthorized();
                }

                // Archive it 
                var planningModifierManager =
                    new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

                var responseModel =
                    await planningModifierManager.ArchivePlanning(planningId, responseSession);

                if (!responseModel.Valid)
                {
                    return BadRequest(responseModel);
                }

                return Ok(responseModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// This is to modify a instructional plan.
        /// </summary>
        /// <param name="teacherPlanningRequestModel">Id of the instructional plan.</param>
        /// <returns>Response with transaction details.</returns>
        [HttpPost]
        [Route("modify")]
        public async Task<IActionResult> ModifyPlanning(Domain.Models.Teachers.TeacherPlanModel teacherPlanningRequestModel)
        {

            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the planning is from the user 
            List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanList = new PlanningReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
               new Domain.Models.Generals.Responses.TeacherPlanModel()
               {
                   Id = teacherPlanningRequestModel.Id,
                   UserId = responseSession.UserId
               }
            });

            if (teacherPlanList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }

            // Archive it 
            Domain.Models.Teachers.TeacherPlanModel responseModel = await
                 new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                 responseSession.LangCode).ModifyPlanning(teacherPlanningRequestModel, responseSession, HttpContext);

            if (!responseModel.Valid)
            {
                return BadRequest(responseModel);
            }

            return Ok(responseModel);
        }

        /// <summary>
        /// This is to create a instructional plan. An instructional plan are a sets of lessons and custom assignments that the teacher 
        /// will assign by a period. The teacher will chose which lessons and assignments count for score and which are not.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="teacherPlanningRequestModel">The teacher plan details to create.</param>
        /// <returns>Details of instructional plan with quizzes and lessons.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePlanning(Domain.Models.Teachers.TeacherPlanModel teacherPlanningRequestModel)
        {
            // Continue with everything else 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(teacherPlanningRequestModel.Name) || string.IsNullOrWhiteSpace(teacherPlanningRequestModel.SubjectCode) || string.IsNullOrWhiteSpace(teacherPlanningRequestModel.LevelCode))
            {
                return BadRequest();
            }

            // Check if planning can be added 
            if(!CheckIfPeriodIsOpened(teacherPlanningRequestModel.PeriodId))
            {
                return BadRequest(
                    new Domain.Models.Generals.Responses.ResponseModel()
                    {
                        TransactionValid = false,
                        SessionValid = true,
                        Messages = new List<string>()
                        {
                             new MessageManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    responseSession.LangCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_plan_cannot_be_created_because_expiring_date"])
                        }
                    }
                    );
            }

            // To create the planning 
            teacherPlanningRequestModel.UserId = responseSession.UserId;
            teacherPlanningRequestModel.ModifiedBy = responseSession.UserId;
            var teacherPlanningDetailModel =
            new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
            AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
            CreatePlanning(teacherPlanningRequestModel, responseSession.UserId);


           

            if (!teacherPlanningDetailModel.Valid)
            {
                return BadRequest(teacherPlanningDetailModel);
            }

            return CreatedAtAction(nameof(GetPlanningDetails), new { id = teacherPlanningDetailModel.Id }, teacherPlanningDetailModel);
        }

        /// <summary>
        /// This is to attach a quiz/homework/assignment to a instructional plan.
        /// Note: When using Swagger, since this model is reused for custom quiz attachment, just send the 
        /// PlanId, QuizId, Available, DueDate, ForGrading properties.
        /// If attaching a lesson quiz, then send everything except the QuizId property that it is the main model. The QuizId in the 
        /// nested list, should be sent.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="teacherPlanAttachRequestModel">The teacher’s instructional plan with the quiz’s id to attach.</param>
        /// <returns>Response with transaction details.</returns>
        [HttpPost]
        [Route("attach-quiz/")]
        public async Task<IActionResult> AttachQuiz([FromBody] TeacherPlanAttachRequestModel teacherPlanAttachRequestModel)
        {

            // Check access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the planning is from the user 
            var teacherPlanList = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
                GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
               new Domain.Models.Generals.Responses.TeacherPlanModel()
               {
                   Id = teacherPlanAttachRequestModel.PlanningId,
                   UserId = responseSession.UserId
               }
            });

            if (teacherPlanList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }

            if (!CheckIfDateAllowedAccordingToPeriod(teacherPlanList[0].PeriodId, teacherPlanAttachRequestModel.DueDate))
            {
                return BadRequest(
                    new Domain.Models.Generals.Responses.ResponseModel()
                    {
                        TransactionValid = false,
                        SessionValid = true,
                        Messages = new List<string>()
                        {
                             new MessageManager(
                                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                responseSession.LangCode).GetMessageDetails(
                                 AppConfiguration.Configuration["AppSettings:tm_message_plan_cannot_be_created_because_expiring_date"])
                        }
                    }
                    );
            }


            // To create the planning 
            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode);
            var responseModel = planningModifierManager.AttachQuizToPlanning(
                new Domain.Models.Teachers.TeacherPlanQuizModel()
                {
                    PlanId = teacherPlanAttachRequestModel.PlanningId,
                    DueDate = teacherPlanAttachRequestModel.DueDate,
                    QuizId = teacherPlanAttachRequestModel.QuizId,
                    ForGrading = teacherPlanAttachRequestModel.ForGrading,
                    ModifiedBy = responseSession.UserId,
                    TriesQuantity = teacherPlanAttachRequestModel.TriesQuantity,
                });

            if (responseModel.Valid)
            {
                try
                {
                    // Find plan instructional detalis.
                    var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                    TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(teacherPlanAttachRequestModel.PlanningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));
                    await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail, teacherPlanAttachRequestModel.QuizId, responseSession, true, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                }

                return Created("", responseModel);
            }
            else
            {
                return BadRequest(responseModel);
            }
        }

        /// <summary>
        /// To modify the attached homework to a plan. (For example, if we want to specify to count it for score or not).
        /// </summary>
        /// Note: When using Swagger, since this model is reused for custom quiz attachment, just send the 
        /// PlanId, QuizId, Available, DueDate, ForGrading properties.
        /// If attaching a lesson quiz, then send everything except the QuizId property that it is the main model. The QuizId in the 
        /// nested list, should be sent.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// <param name="teacherPlanAttachRequestModel">Entity with the details to modify.</param>
        /// <returns>Returns a HTTP Response with Response Model with transaction details.</returns>
        [HttpPost]
        [Route("attach-quiz/modify")]
        public async Task<IActionResult> ModifiedAttachedQuiz(TeacherPlanAttachRequestModel teacherPlanAttachRequestModel)
        {
            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the planning is from the user 
            List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanList = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
                GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
               new Domain.Models.Generals.Responses.TeacherPlanModel()
               {
                   Id = teacherPlanAttachRequestModel.PlanningId,
                   UserId = responseSession.UserId
               }
            });

            if (teacherPlanList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }

            if (!CheckIfDateAllowedAccordingToPeriod(teacherPlanList[0].PeriodId, teacherPlanAttachRequestModel.DueDate))
            {
                return BadRequest(
                    new Domain.Models.Generals.Responses.ResponseModel()
                    {
                        TransactionValid = false,
                        SessionValid = true,
                        Messages = new List<string>()
                        {
                             new MessageManager(
                                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                responseSession.LangCode).GetMessageDetails(
                                 AppConfiguration.Configuration["AppSettings:tm_message_plan_cannot_be_created_because_expiring_date"])
                        }
                    }
                    );
            }


            // To modify the planning
            PlanningModifierManager planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

            Domain.Models.Teachers.TeacherPlanQuizModel responseModel = planningModifierManager.ModifyTeacherPlaQuizModel(
                new Domain.Models.Teachers.TeacherPlanQuizModel()
                {
                    Id = teacherPlanAttachRequestModel.Id,
                    Available = string.IsNullOrWhiteSpace(teacherPlanAttachRequestModel.Available) ? "1" : teacherPlanAttachRequestModel.Available,
                    DueDate = teacherPlanAttachRequestModel.DueDate,
                    ForGrading = teacherPlanAttachRequestModel.ForGrading,
                    ModifiedBy = responseSession.UserId,
                    TriesQuantity = teacherPlanAttachRequestModel.TriesQuantity,
                    PlanId = teacherPlanAttachRequestModel.PlanningId,
                }, responseSession.UserId);
            if (responseModel.Valid)
            {
                try
                {
                    var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                    TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(teacherPlanAttachRequestModel.PlanningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));

                    await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail, teacherPlanAttachRequestModel.Id, responseSession, false, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                }
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(responseModel);
            }
        }

        /// <summary>
        /// To get the plan lesson details.
        /// </summary>
        /// <param name="planLessonId">The plan lesson id to verify the details.</param>
        /// <returns>Returns the plan lesson full details.</returns>
        [HttpGet]
        [Route("plan-lesson-detail/{planLessonId}")]
        public async Task<IActionResult> GetPlanLessonDetails(string planLessonId)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var planningReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                return Ok(planningReaderManager.GetTeacherPlaningLessonDetail(planLessonId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the plan lesson quizzes.
        /// </summary>
        /// <param name="planLessonId">The plan lesson id to call.</param>
        /// <returns>Returns the plan lesson's quizzes.</returns>
        [HttpGet]
        [Route("plan-lesson-detail/{planLessonId}/quizzes")]
        public async Task<IActionResult> GetPlanLessonQuizzes(string planLessonId)
        {
            try
            {
                // Check access
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var planningReaderManager = new PlanningReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    responseSession.LangCode);
                return Ok(planningReaderManager.GetQuizzesForPlanLesson(planLessonId, responseSession.Token));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This is to attach a Genial Lesson (Athenas) to the teacher’s instructional plan. This should include the dates when the exams
        /// are going to take place. Also, it should required if it is for grading or not.
        /// Note: When using Swagger, since this model is reused for custom quiz attachment, just send the 
        /// PlanId, QuizId, Available, DueDate, ForGrading properties.
        /// If attaching a lesson quiz, then send everything except the QuizId property that it is the main model. The QuizId in the 
        /// nested list, should be sent.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="teacherPlanAttachRequestModel">The teacher’s instructional plan with the lesson’s id to attach.</param>
        /// <returns>Response with transaction details.</returns>
        [HttpPost]
        [Route("attach-lesson/")]
        public async Task<IActionResult> AttachPlan(TeacherPlanAttachRequestModel teacherPlanAttachRequestModel)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                PlanningModifierManager planningModifierManager = new PlanningModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    responseSession.LangCode);

                // Get available courses from Subscriptions API
                var availableCourses = SubscriptionHandler.GetUserCourses(responseSession.Token);

                // Validate available courses
                Domain.Models.Teachers.ResponseModel lessonValidation =
                    await planningModifierManager.IsLessonInTeacherCourses(teacherPlanAttachRequestModel.LessonId, availableCourses);

                if (!lessonValidation.Valid)
                {
                    Domain.Models.Teachers.ResponseModel response = new Domain.Models.Teachers.ResponseModel()
                    {
                        Valid = false,
                        Messages = lessonValidation.Messages
                    };

                    return BadRequest(response);
                }

                // Check if the planning is from the user 
                List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanList = new PlanningReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
                {
                   new Domain.Models.Generals.Responses.TeacherPlanModel()
                   {
                       Id = teacherPlanAttachRequestModel.PlanningId,
                       UserId = responseSession.UserId
                   }
                });

                if (teacherPlanList.Count == 0)
                {
                    // It doesn't belong to the user. 
                    return Unauthorized();
                }

                // exclude teacher plan lesson quizzess validation when there is a range of dates because never comes together.
                if (teacherPlanAttachRequestModel.TeacherPlanLessonDateModelList.Count == 0)
                {
                    var checkIfQuizzesCanBeAdded = CheckIfTestsCanBeAdded(teacherPlanList[0].PeriodId,
                    teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList,
                    teacherPlanAttachRequestModel.DueDate, responseSession.LangCode);

                    if (!checkIfQuizzesCanBeAdded.TransactionValid)
                    {
                        return BadRequest(checkIfQuizzesCanBeAdded);
                    }
                }

                TeacherPlanLessonModel responseModel = planningModifierManager.AttachLessonToPlanning(
                    new TeacherPlanLessonModel()
                    {
                        PlanId = teacherPlanAttachRequestModel.PlanningId,
                        LessonId = teacherPlanAttachRequestModel.LessonId,
                        DueDate = teacherPlanAttachRequestModel.DueDate,
                        ModifiedBy = responseSession.UserId,
                        ForGrading = teacherPlanAttachRequestModel.ForGrading,
                        TriesQuantity = teacherPlanAttachRequestModel.TriesQuantity,
                        PrePostRequired = teacherPlanAttachRequestModel.PrePostRequired,
                    }, teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList,
                        teacherPlanAttachRequestModel.TeacherPlanLessonDateModelList,
                    responseSession.UserId, false);

                if (responseModel.Valid)
                {
                    try
                    {
                        // Find plan instructional detalis.
                        var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                        TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(teacherPlanAttachRequestModel.PlanningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));

                        await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail, teacherPlanAttachRequestModel.LessonId, responseSession, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                    }

                    return Created("",responseModel);
                }
                else
                {
                    return BadRequest(responseModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To modify the attached lesson of a plan. For example, if we want to change the lesson exam's dates.
        /// Note: When using Swagger, since this model is reused for custom quiz attachment, just send the 
        /// PlanId, QuizId, Available, DueDate, ForGrading properties.
        /// If attaching a lesson quiz, then send everything except the QuizId property that it is the main model. The QuizId in the 
        /// nested list, should be sent.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="teacherPlanAttachRequestModel">Entity with the details to modify.</param>
        /// <returns>Returns a HTTP Response with Response Model with transaction details.</returns>
        [HttpPost]
        [Route("attach-lesson/modify")]
        public async Task<IActionResult> ModifiedAttachedLesson(TeacherPlanAttachRequestModel teacherPlanAttachRequestModel)
        {
            // Check access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the planning is from the user 
            var planingReaderManager = new PlanningReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

            var teacherPlanList = planingReaderManager.GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
                   new Domain.Models.Generals.Responses.TeacherPlanModel()
                   {
                       Id = teacherPlanAttachRequestModel.PlanningId,
                       UserId = responseSession.UserId
                   }
            });

            if (teacherPlanList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }
            // exclude teacher plan lesson quizzess validation when there is a range of dates because never comes together.
            if (teacherPlanAttachRequestModel.TeacherPlanLessonDateModelList.Count == 0)
            {
                var checkIfQuizzesCanBeAdded = CheckIfTestsCanBeAdded(teacherPlanList[0].PeriodId,
                teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList, teacherPlanAttachRequestModel.DueDate, responseSession.LangCode);

                if (!checkIfQuizzesCanBeAdded.TransactionValid)
                {
                    return BadRequest(checkIfQuizzesCanBeAdded);
                }

                // Attach the plan lesson id 
                foreach (var teacherPlanQuiz in teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList)
                {
                    teacherPlanQuiz.PlanLessonId = teacherPlanAttachRequestModel.Id;
                }
            }

            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                 responseSession.LangCode);

            var responseModel = planningModifierManager.ModifyTeacherPlanLessonModel(
                new TeacherPlanLessonModel()
                {
                    Id = teacherPlanAttachRequestModel.Id,
                    Available = string.IsNullOrWhiteSpace(teacherPlanAttachRequestModel.Available) ? "1" : teacherPlanAttachRequestModel.Available,
                    DueDate = teacherPlanAttachRequestModel.DueDate,
                    ForGrading = teacherPlanAttachRequestModel.ForGrading,
                    ModifiedBy = responseSession.UserId,
                    TriesQuantity = teacherPlanAttachRequestModel.TriesQuantity,
                    PlanId = teacherPlanAttachRequestModel.PlanningId,
                    PrePostRequired = teacherPlanAttachRequestModel.PrePostRequired,
                }, teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList, teacherPlanAttachRequestModel.TeacherPlanLessonDateModelList, responseSession.UserId);

            if (responseModel.Valid)
            {
                try
                {
                    var planningDetail = planingReaderManager.GetTeacherPlanFullDetails(teacherPlanAttachRequestModel.PlanningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));
                    await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail, teacherPlanAttachRequestModel.Id, responseSession, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                }

                return Ok(responseModel);
            }
            else
            {
                return BadRequest(responseModel);
            }
        }

        /*
        [HttpPost]
        [Route("planning/attach-quizzes/modify")]
        public async Task<HttpResponseMessage> ModifiedAttachedLessonQuiz(TeacherPlanAttachRequestModel teacherPlanAttachRequestModel)
        {
            // Check access
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                return Request.CreateResponse(HttpStatusCode.Unauthorized, Configuration.Formatters.JsonFormatter);

            // Check if the planning is from the user 
            var planingReaderManager = new PlanningReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

            var teacherPlanList = planingReaderManager.GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
                   new Domain.Models.Generals.Responses.TeacherPlanModel()
                   {
                       Id = teacherPlanAttachRequestModel.PlanningId,
                       UserId = responseSession.UserId
                   }
            });

            if (teacherPlanList.Count == 0)
                return Request.CreateResponse(HttpStatusCode.Unauthorized, Configuration.Formatters.JsonFormatter);

            //var checkIfQuizzesCanBeAdded = CheckIfTestsCanBeAdded(teacherPlanList[0].PeriodId, teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList, teacherPlanAttachRequestModel.DueDate, responseSession.LangCode);

            //if (!checkIfQuizzesCanBeAdded.TransactionValid)
            //	return Request.CreateResponse(HttpStatusCode.BadRequest, checkIfQuizzesCanBeAdded, Configuration.Formatters.JsonFormatter);

            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                 responseSession.LangCode);

            TeacherPlanLessonModel responseModel = planningModifierManager.ModifyAttachedLessonQuizzesToPlanning(
                new TeacherPlanLessonModel()
                {
                    Id = teacherPlanAttachRequestModel.Id,
                    PlanId = teacherPlanAttachRequestModel.PlanningId,
                    ModifiedBy = responseSession.UserId,
                    DueDate = teacherPlanAttachRequestModel.DueDate,
                }, teacherPlanAttachRequestModel.TeacherPlanLessonQuizModelList, responseSession.UserId);

            if (responseModel.Valid)
            {
                try
                {
                    TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(teacherPlanAttachRequestModel.PlanningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));
                    await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail, teacherPlanAttachRequestModel.Id, responseSession, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseModel, Configuration.Formatters.JsonFormatter);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, responseModel, Configuration.Formatters.JsonFormatter);
            }
        } */

        /// <summary>
        /// This is to delete a quiz/homework/assignment from a instructional plan.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="planningId">Id of the instructional plan.</param>
        /// <param name="planningQuizId">The id of the relation of the quiz and instructional plan.</param>
        /// <returns>Response with transaction details.</returns>
        [HttpPost]
        [Route("delete/{planningId}/planning-quiz/{planningQuizId}")]
        public async Task<IActionResult> DeleteQuizFromPlanning(string planningId, string planningQuizId)
        {

            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the planning is from the user 
            List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanList = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
               new Domain.Models.Generals.Responses.TeacherPlanModel()
               {
                   Id = planningId,
                   UserId = responseSession.UserId
               }
            });

            if (teacherPlanList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }

            // To create the planning 
            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                 responseSession.LangCode);

            // Find plan instructional detalis.
            var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
            TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(planningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));

            Domain.Models.Teachers.ResponseModel responseModel = planningModifierManager.DeleteQuizFromPlanning(
                new Domain.Models.Teachers.TeacherPlanQuizModel()
                {
                    Id = planningQuizId,
                    PlanId = planningId,
                });

            Domain.Models.Generals.Responses.ResponseModel responseModelToReturn = new Domain.Models.Generals.Responses.ResponseModel()
            {
                Messages = responseModel.Messages,
            };

            if (!responseModel.Valid)
            {
                return BadRequest(responseModelToReturn);
            }

            try
            {
                await planningModifierManager.RemoveEventsFromParticipantsTeamCalendar(planningDetail, planningQuizId, responseSession, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
            }

            return Ok(responseModelToReturn);
        }

        /// <summary>
        /// This is to delete a lesson from a instructional plan.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="planningId">PId of the instructional plan.</param>
        /// <param name="planningLessonId">The id of the relation of the lesson and instructional plan.</param>
        /// <returns>Returns a HTTP Response with transaction details.</returns>
        [HttpPost]
        [Route("delete/{planningId}/planning-lesson/{planningLessonId}")]
        public async Task<IActionResult> DeleteLessonFromPlanning(string planningId, string planningLessonId)
        {
            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Check if the planning is from the user 
            List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanList = new PlanningReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
            {
               new Domain.Models.Generals.Responses.TeacherPlanModel()
               {
                   Id = planningId,
                   UserId = responseSession.UserId
               }
            });

            if (teacherPlanList.Count == 0)
            {
                // It doesn't belong to the user. 
                return Unauthorized();
            }

            // To create the planning
            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

            // Find plan instructional detalis.
            var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
            TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(planningId, responseSession.UserId, RequestHandler.GetToken(HttpContext));

            Domain.Models.Teachers.ResponseModel responseModel = planningModifierManager.DeleteLessonFromPlanning(
                new Domain.Models.Teachers.TeacherPlanLessonModel()
                {
                    Id = planningLessonId,
                    PlanId = planningId,
                });

            Domain.Models.Generals.Responses.ResponseModel responseModelToReturn = new Domain.Models.Generals.Responses.ResponseModel()
            {
                Messages = responseModel.Messages,
            };

            if (!responseModel.Valid)
            {
                return BadRequest(responseModelToReturn);
            }

            try
            {
                await planningModifierManager.RemoveEventsFromParticipantsTeamCalendar(planningDetail, planningLessonId, responseSession);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
            }
            return Ok(responseModelToReturn);
        }

        /// <summary>
        /// This is to get the instructional plans of the students. If filterByTeacher, the token will be evaluated to retrieve the instructional plans of that teacher.
        /// </summary>
        /// <param name="studentId">The student's user id.</param>
        /// <param name="filterByTeacher">true if filter by teacher's user id, else false.</param>
        /// <param name="callExtraDetails">To include details as subject's name, teacher's name, etc. It is optional. The default is false.</param>
        /// <returns>A list of instructional plans.</returns>
        [HttpGet]
        [Route("student/{studentId}/filter/{filterByTeacher}")]
        public async Task<IActionResult> GetStudentsPlannings(string studentId, bool filterByTeacher = true, bool callExtraDetails = false)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                List<TeacherPlanGroupStudentModel> teacherPlanGroupStudentModels = TeacherPlans.GetTeacherPlanWithGroupDetails(null, new List<string>() { studentId }, filterByTeacher ? responseSession.UserId : null);

                if (!callExtraDetails)
                {
                    return Ok(teacherPlanGroupStudentModels);
                }

                if (teacherPlanGroupStudentModels != null && teacherPlanGroupStudentModels.Count > 0)
                {
                    // Call the full subject name 
                    List<SubjectModel> subjects = AthenasHandler.GetAthenasSubjects(responseSession.LangCode);
                    List<string> userIdsToCall = new List<string>();
                    foreach (TeacherPlanGroupStudentModel teacherGroupStudentModel in teacherPlanGroupStudentModels)
                    {
                        if (subjects != null && subjects.Count > 0)
                        {
                            SubjectModel subject = subjects.Find(x => x.Code == teacherGroupStudentModel.SubjectCode);
                            if (subject != null)
                            {
                                teacherGroupStudentModel.SubjectName = subject.Name;
                            }
                        }
                        userIdsToCall.Add(teacherGroupStudentModel.TeacherId);
                    }

                    // Call teacher name. 

                    List<UserResponse> userSharedDetails = new List<UserResponse>();
                    try
                    {
                        userSharedDetails = GetUserDetails.GetUsersSharedDetails(responseSession.Token, responseSession.IpAddress, userIdsToCall);

                        if (userSharedDetails != null && userSharedDetails.Count > 0)
                        {
                            foreach (TeacherPlanGroupStudentModel teacherGroupStudentModel in teacherPlanGroupStudentModels)
                            {
                                UserResponse user = userSharedDetails.Find(x => x.UserId == teacherGroupStudentModel.TeacherId);
                                if (user != null)
                                {
                                    teacherGroupStudentModel.TeacherName = user.GivenName + " " + user.Surname;
                                }
                                userIdsToCall.Add(teacherGroupStudentModel.TeacherId);
                            }
                        }

                    }
                    catch (Exception)
                    {
                        // Ignore. This is not an stopper.
                    }
                }

                return Ok(teacherPlanGroupStudentModels);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// FOR STUDENTS***: To get the lessons attached to the instructional plan that the student still has pending. (For students only. It will evaluate the student's access Token).
        /// </summary>
        /// <param name="fromPage">The current page of the pagination. Example: 1</param>
        /// <param name="limiter">How many items per page. Example: 10</param>
        /// <param name="subjectCode">The subject code. Example: mat-sp, sci-sp, etc.</param>
        /// <param name="levelCode">The level code to filter. Example: k, 1, 2, 3...</param>
        /// <returns>A list of pending lessons.</returns>
        [HttpGet]
        [Route("student/lessons/{fromPage}/{limiter}")]
        public async Task<IActionResult> GetStudentPlanningLessons(int fromPage, int limiter, string subjectCode = null, string levelCode = null)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                return Ok(StudentAssignmentsAndResults.GetStudentPendingLessons(responseSession.UserId, fromPage, limiter, RequestHandler.GetLanguage(HttpContext) == "en" ? "Test" : "Examen", subjectCode, levelCode));
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the lessons attached to the instructional plan that the student still has
        /// pending and that are due within the given month, for the student's calendar
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <param name="subjectCode">The subject code.</param>
        /// <param name="levelCode">The level code to filter.</param>
        /// <returns>A list of pending lessons due within the month.</returns>
        [HttpGet]
        [Route("student/lessons/calendar")]
        public async Task<IActionResult> GetStudentPlanningLessonsByMonth(int month, int year, string subjectCode = null, string levelCode = null)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (month < 1 || month > 12)
                {
                    return BadRequest("Month must be between 1 and 12.");
                }

                if (year < 1)
                {
                    return BadRequest("Enter a valid year.");
                }

                return Ok(StudentAssignmentsAndResults.GetStudentPendingLessonsByMonth(responseSession.UserId, 
                    month, year, RequestHandler.GetLanguage(HttpContext) == "en" ? "Test" : "Examen", subjectCode, levelCode, langCode: responseSession.LangCode));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// This endpoint is for students. It returns the exams, including the pre and posts, that are available for the student for an specific lesson.
        /// </summary>
        /// <param name="lessonId">The lesson Id to check.</param>
        /// <returns>Returns the list of exams of the lesson, with details as the due date, if worked in or not, etc.</returns>
        [HttpGet]
        [Route("student/lessons/{lessonId}/exams/")]
        public async Task<IActionResult> GetLessonExamsForStudents(string lessonId)
        {
            try
            {
                // Check access
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var planningReaderManager = new PlanningReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

                return Ok(planningReaderManager.ReadStudentAvailableExams(lessonId, responseSession.UserId, 
                SubscriptionHandler.CheckIStudentIsHomeschool(responseSession.Token),
                responseSession.Token));
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// To manually correct exam for students.
        /// </summary>
        /// <param name="planLessonQuizUserResultModel">The plan lesson quiz user result model to correct.</param>
        /// <returns>Returns the manually exam for students.</returns>
        [HttpPost]
        [Route("{planningId}/planning-lesson/{planningLessonId}/student/{studentId}/quiz/{quizId}/manual-correction/")]
        public async Task<IActionResult> ManuallyCorrectExamForStudents(PlanLessonQuizUserResultModel planLessonQuizUserResultModel)
        {
            try
            {
                // Check access
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var planningModifierManager = new PlanningModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

                var result = planningModifierManager.ManuallyCorrectPlanLessonQuizStudent(planLessonQuizUserResultModel,
                    responseSession.UserId);

                return Ok(result);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// To send the instructional plan created by the teacher to the group of students. The student then will have access to 
        /// the exams and homework assigned by the teacher.
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="planningGroupAttachRequest">Attach the planning to group model.</param>
        /// <returns>Return the transaction details.</returns>
        [HttpPost]
        [Route("groups/send-plan/")]
        public async Task<IActionResult> SendGroupsPlanning(List<TeacherPlanGroupModel> planningGroupAttachRequest)
        {
            try
            {

                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var planningModifierManager = new PlanningModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                     AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                     responseSession.LangCode);
                TeacherPlanGroupModel responseTransaction = planningModifierManager.AttachGroupToPlanning(planningGroupAttachRequest, responseSession.UserId);

                if (!responseTransaction.Valid)
                {
                    return BadRequest(responseTransaction);
                }

                try
                {
                    var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);

                    foreach (var item in planningGroupAttachRequest)
                    {
                        await planningModifierManager.ManageCalendarDesignationsInGroupsOfParticipants(item.TeacherPlanId, item.GroupId, responseSession, planingReaderManager, HttpContext);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                }

                return Ok(new Domain.Models.Generals.Responses.ResponseModel()
                {
                    Messages = responseTransaction.Messages,
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the annotations of the student of the instructional plan.
        /// </summary>
        /// <param name="planningId">Id of the instructional plan.</param>
        /// <param name="studentId">The student’s user’s id.</param>
        /// <param name="fromPage">The current page of the pagination. Example: 1.</param>
        /// <param name="limiter">How many items per page. Example: 10.</param>
        /// <returns>Returns a HttpResponse Message with transaction details.</returns>
        [HttpGet]
        [Route("{planningId}/students/{studentId}/annotation/{fromPage}/{limiter}")]
        public async Task<IActionResult> GetStudentAnnotations(string planningId, string studentId, int fromPage, int limiter)
        {

            // Check access
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            TeacherAnnotationReaderManager teacherAnnotationReaderManager = new TeacherAnnotationReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
            Domain.Models.Teachers.TeacherStudentAnnotationResponseModel teacherStudentAnnotationResponseLib = teacherAnnotationReaderManager.GetTeacherPlanStudentAnnotations(new List<TeacherStudentAnnotationModel>()
            {
                new TeacherStudentAnnotationModel()
                {
                    StudentId = studentId,
                    TeacherPlanId = planningId,
                    TeacherId = responseSession.UserId,
                }
            }, fromPage, limiter);


            var teacherStudentAnnotationResponseModel = new Domain.Models.Generals.Responses.TeacherStudentAnnotationResponseModel()
            {
                TeacherStudentAnnotationModelList = teacherStudentAnnotationResponseLib.TeacherStudentAnnotationModelList,
                TotalPages = int.Parse(Math.Ceiling((double)teacherStudentAnnotationResponseLib.TotalResults / limiter).ToString()),
            };

            return Ok(teacherStudentAnnotationResponseModel);

        }

        /// <summary>
        /// To create an annotation about the student in the instructional plan. Example of an annotation: The student is doing great. She has shown 
        /// some improvement during...
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="annotationRequestModel">Student annotation model.</param>
        /// <returns>Returns a Http Response Message with all details.</returns>
        [HttpPost]
        [Route("annotation/create")]
        public async Task<IActionResult> CreateStudentAnnotation(TeacherStudentAnnotationModel annotationRequestModel)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Now return what we want.
                annotationRequestModel.TeacherId = responseSession.UserId;
                Domain.Models.Teachers.ResponseModel teacherStudentAnnotationResponseLib = new
                    TeacherAnnotationModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                     AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
                    MakeAnnotation(annotationRequestModel, responseSession.UserId);

                Domain.Models.Generals.Responses.ResponseModel teacherStudentAnnotationResponseModel = new Domain.Models.Generals.Responses.ResponseModel()
                {
                    Messages = teacherStudentAnnotationResponseLib.Messages,
                };

                if (!teacherStudentAnnotationResponseLib.Valid)
                {
                    return BadRequest(teacherStudentAnnotationResponseModel);
                }

                return Ok(teacherStudentAnnotationResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the student’s average for the instructional plan. (For assigned lessons only. This DOES NOT include homework created by teachers).
        /// </summary>
        /// <param name="planningId">Id of the instructional plan.</param>
        /// <param name="studentId">The student’s user’s id.</param>
        /// <returns>Returns a Http Response with lesson planning details.</returns>
        [HttpGet]
        [Route("{planningId}/student/{studentId}/average")]
        public async Task<IActionResult> GetStudentPlanningLessonDetails(string planningId, string studentId)
        {
            try
            {
                // First, lets check the permissions 
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningAverageRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (responseSession.UserId != studentId)
                {
                    // Check if user can handle the other user 
                    if (!GetUserDetails.CanUserAManageUserB(responseSession.UserId, studentId, responseSession.Token))
                    {
                        return Unauthorized();
                    }
                }

                return Ok(StudentAssignmentsAndResults.GetStudentLessonResults(new DetailsRequestModel() { FromPage = 0, Limiter = 100 }, responseSession.Token, new List<string>() { studentId }, responseSession.IpAddress, 0, planningId));
            }
            catch (Exception)
            {
                // Bad request
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the pending lessons for the student’s tutor (for example, a parent could see the student's pending assignment)..
        /// </summary>
        /// <param name="profileId">The student user's id.</param>
        /// <param name="planningId">(optional) To filter for an specific planning id.</param>
        /// <param name="fromPage">The current page of the pagination. Example: 1.</param>
        /// <param name="limiter">How many items per page. Example: 10.</param>
        /// <returns>A response model for homework that includes some extra details than regular model.</returns>
        [HttpGet]
        [Route("pending/exams/tutor/student/{profileId}/{fromPage}/{limiter}")]
        public async Task<IActionResult> GetTutorStudentsLessonsExams(string profileId, int fromPage, int limiter, string planningId=null)
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

                // New algo 
                // Check if user can handle the profile
                bool profileFound = false;
                foreach (UserResponse userResponse in userSharedDetails)
                {
                    if (userResponse.UserId == profileId)
                    {
                        profileFound = true;
                        break;
                    }
                }

                if (!profileFound)
                {
                    return Ok(new HomeworkResponseModel());
                }

                return Ok(StudentAssignmentsAndResults.GetStudentPendingQuizzes(profileId, fromPage, limiter, responseSession.Token, responseSession.IpAddress, null, planningId));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To check if the students that are handled by the user, or the user making the request has a instructional plan assigned.
        /// </summary>
        /// <returns>If the student has a instructional plan assigned, this will be true, else false.</returns>
        [HttpGet]
        [Route("planning-available-students/")]
        public async Task<IActionResult> CheckIfUserHasPlannings()
        {
            try
            {
                // Get the users details of the user that can handle. The session validation is here, because if 0 users are returned, there is not point to continue.
                UserResponseModel userSharedDetails = GetUserDetails.GetStudentsThatCanBeHandled(new UserRequestModel() { CallOnlyProfilesThatCanBeManaged = true }, RequestHandler.GetToken(HttpContext));

                // Prepare the users to find
                List<string> userIdsToFind = new List<string>();
                // If there is any user that can be handle by the user accessing the point, then add it to user ids to filter to find the plans by group.
                if (userSharedDetails != null && userSharedDetails.UserInfo != null && userSharedDetails.UserInfo.Count > 0)
                {
                    foreach (UserResponse userShared in userSharedDetails.UserInfo)
                    {
                        userIdsToFind.Add(userShared.UserId);
                    }
                }
                else
                {
                    // If not, then add the user in session to find. 
                    ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                    if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    {
                        return Unauthorized();
                    }
                    userIdsToFind.Add(responseSession.UserId);
                }

                if (userIdsToFind.Count == 0)
                {
                    return Ok(false);
                }

                List<TeacherPlanGroupStudentModel> teacherPlanGroups = TeacherPlans.GetTeacherPlanWithGroups(new List<string> { userIdsToFind[0] });
                return Ok(teacherPlanGroups != null && teacherPlanGroups.Count > 0);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// For STUDENTS: To get the tries for the lesson. 
        /// </summary>
        /// <param name="lessonId">The lesson's id to check.</param>
        /// <param name="quizId">The quiz's id to check.</param>
        /// <returns>Returns the lesson quiz tries.</returns>
        [HttpGet]
        [Route("students/lessons/tries/{lessonId}/{quizId}")]
        public async Task<IActionResult> GetLessonsTriesForStudent(string lessonId, string quizId)
        {
            // Check Token, and if user can be connected with the user or not. 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(TeacherTryEvaluator.GetUserLessonQuizTriesAvailable(lessonId, responseSession.UserId, quizId));
        }

        /// <summary>
        /// Checks if the lesson was assigned at least the last year (this is to prevent Athenas/Genial Lessons to archive a Lesson).
        /// </summary>
        /// <param name="lessonId">Id of the lesson.</param>
        /// <returns>Returns a HTTPResponse with a bool. If the lesson was assigned in the last year, this will be true, else false.</returns>
        [HttpGet]
        [Route("was-assigned/lesson/{lessonId}")]
        public async Task<IActionResult> WasLessonAsignedAtLeastLastYear(string lessonId)
        {

            if (TeacherPlans.GetTeacherPlanLessonSimple(null, null, new List<string>() { lessonId }).Count == 0)
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }
        }

        /// <summary>
        /// To get the lesson’s PDF.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <returns>A string with the url of the PDF.</returns>
        [HttpGet]
        [Route("pdf/lesson/{lessonId}")]
        public async Task<IActionResult> GetPDFLesson(string lessonId)
        {
            // Check Token, and if user can be connected with the user or not. 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PDFLessonRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(AthenasHandler.GetAthenasLessonPDFUrl(RequestHandler.GetLanguage(HttpContext), lessonId, false));
        }

        /// <summary>
        /// To get the lesson’s PDF.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="quizId">Id of the quiz.</param>
        /// <param name="outPutFormat">type of file </param>
        /// <returns>A string with the url of the PDF.</returns>
        [HttpGet]
        [Route("pdf/lesson-quiz/{lessonId}/{quizId}/{outPutFormat}")]
        public async Task<IActionResult> GetPDFLessonQuiz(string lessonId, string quizId, string outPutFormat = "pdf")
        {
            // Check Token, and if user can be connected with the user or not. 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PDFLessonRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            return Ok(AthenasHandler.GetAthenasLessonQuizPDFUrl(null, lessonId, quizId, outPutFormat));
        }

        /// <summary>
        /// To get the teacher’s instructional plans by teacher user’s id. Example of use: When a principal wants to view the 
        /// instructional plans of a teacher.
        /// </summary>
        /// <param name="teacherUserId">Id of the teacher’s user’s id.</param>
        /// <returns>A response with a list of teachers’ instructional plans.</returns>
        [HttpGet]
        [Route("teacher-plans/{teacherUserId}")]
        public async Task<IActionResult> GetTeacherPlanByTeacherId(string teacherUserId)
        {
            try
            {
                // Check Access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "GetTeacherPlansSuperiorRole");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Call the plan 
                List<Domain.Models.Generals.Dapper.TeacherPlanModel> teacherPlans = TeacherPlans.GetTeacherPlans(teacherUserId);
                if (teacherPlans.Count == 0)
                {
                    return Ok(new List<Domain.Models.Generals.Dapper.TeacherPlanModel>());
                }

                // Check if same school 
                if (!responseSession.IsStaff && teacherPlans[0].TeacherId != teacherUserId)
                {
                    if (!GetUserDetails.CheckIfUsersFromSameSchool(responseSession.Token, responseSession.IpAddress, teacherUserId))
                        return Unauthorized();
                }

                // Return error about invalid request
                return Ok(teacherPlans);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the dates of a instructional plans homework, lessons and exams due dates, etc. 
        /// </summary>
        /// <param name="teacherPlanId">Id of the instructional plan.</param>
        /// <returns>A list with the planning details and dates.</returns>
        [HttpGet]
        [Route("dates/teacher-plans/{teacherPlanId}")]
        public async Task<IActionResult> GetTeacherPlanLessonsAndHomeworkDates(string teacherPlanId)
        {
            // Check access 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Get the plan details using Dapper.
            List<TeacherPlanFullDetailsModel> teacherPlanFullDetails = TeacherPlans.GetTeacherPlanFullDetails(new List<string>() { teacherPlanId });
            if (teacherPlanFullDetails.Count == 0)
            {
                return BadRequest();
            }

            if (teacherPlanFullDetails[0].TeacherPlanModel.TeacherId != responseSession.UserId && !responseSession.IsStaff)
            {
                if (!GetUserDetails.CheckIfUsersFromSameSchool(responseSession.Token, responseSession.IpAddress, teacherPlanFullDetails[0].TeacherPlanModel.TeacherId))
                {
                    return Unauthorized();
                }
            }

            // Prepare lessons titles
            if (teacherPlanFullDetails[0].LessonAssignmentModels.Count > 0)
            {
                // Call the lesson titles 
                List<LessonModel> lessonModelsToCall = new List<LessonModel>();
                foreach (LessonAssignmentModel teacherPlanLesson in teacherPlanFullDetails[0].LessonAssignmentModels)
                {
                    lessonModelsToCall.Add(new LessonModel()
                    {
                        Id = teacherPlanLesson.LessonId,
                    });
                }

                // Now call lessons titles 
                LessonsResponseModel lessonsResponseModel = AthenasHandler.GetAthenasLessons(lessonModelsToCall, false);
                if (lessonsResponseModel != null && lessonsResponseModel.LessonModelList != null && lessonsResponseModel.LessonModelList.Count > 0)
                {
                    foreach (LessonAssignmentModel teacherPlanLesson in teacherPlanFullDetails[0].LessonAssignmentModels)
                    {
                        teacherPlanLesson.Title = lessonsResponseModel.LessonModelList.Find(x => x.Id == teacherPlanLesson.LessonId)?.LessonTitle;
                    }
                }
            }

            return Ok(teacherPlanFullDetails[0]);
        }

        /// <summary>
        /// To get the students that are participants of a instructional plan.
        /// </summary>
        /// <param name="teacherPlanId">Id of the instructional plan.</param>
        /// <returns>Returns the list of students.</returns>
        [HttpGet]
        [Route("groups/teacher-plans/{teacherPlanId}")]
        public async Task<IActionResult> GetPlanningGroups(string teacherPlanId)
        {
            try
            {
                // Check access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                List<TeacherPlanGroupStudentModel> teacherGroups = TeacherPlans.GetTeacherPlanWithGroups(new List<string> { teacherPlanId });

                if (teacherGroups.Count == 0)
                {
                    return Ok(
                        new List<TeacherPlanGroupStudentModel>()
                    );
                }

                return Ok(teacherGroups);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To remove a group from a instructional plan. Once the group is removed, the student will not have access to the details related 
        /// to plan.
        /// </summary>
        /// <param name="teacherPlanId">Id of the instructional plan.</param>
        /// <param name="groupId">The group's id.</param>
        /// <returns>A HTTP response code only.</returns>
        [HttpPost]
        [Route("groups/remove/teacher-plans/{teacherPlanId}/{groupId}")]
        public async Task<IActionResult> DeleteGroupsFromPlan(string teacherPlanId, string groupId)
        {
            // Check Token, and if user can be connected with the user or not. 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Validate if teacher can handle the plan.
            PlanningReaderManager planningReaderManager = new PlanningReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
            List<Domain.Models.Teachers.TeacherPlanModel> teacherPlans = planningReaderManager.GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
                {
                    new Domain.Models.Generals.Responses.TeacherPlanModel()
                    {
                        Id = teacherPlanId,
                        Available = "1",
                        UserId = responseSession.UserId,
                    },
                });

            if (teacherPlans.Count == 0)
            {
                return BadRequest();
            }

            // Delete the group from planning.
            PlanningModifierManager planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                 AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
            planningModifierManager.DeleteGroupFromPlanning(new TeacherPlanGroupModel()
            {
                TeacherPlanId = teacherPlanId,
                GroupId = groupId,
            });

            try
            {
                var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                await planningModifierManager.ManageCalendarDesignationsInGroupsOfParticipants(teacherPlanId, groupId, responseSession, planingReaderManager, HttpContext, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
            }

            return Ok();
        }

        /// <summary>
        /// To submit a student result id from GSWeb/Athenas to this API for faster reads.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="planLessonQuizUserResultModel">The plan lesson quiz user result model.</param>
        /// <returns>Returns a HTTP Response with transaction details.</returns>
        [HttpPost]
        [Route("lessons/{lessonId}/quiz/submit")]
        public async Task<IActionResult> InsertStudentGSWebResults(string lessonId, PlanLessonQuizUserResultModel planLessonQuizUserResultModel)
        {
            try
            {
                // Access validation
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AuthorizationQuizCode");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                // End of access validation

                // First, find the plans of the user that have the lessons. 
                List<TeacherPlanLessonModel> planLessonIds = TeacherPlans.GetPlanLessonIdsAccordingToLessonAndStudents(lessonId, responseSession.UserId);
                if (planLessonIds == null || planLessonIds.Count == 0)
                {
                    // Cannot be inserted
                    return BadRequest();
                }

                // Prepare the lesson quiz users to insert 
                List<PlanLessonQuizUserResultModel> planLessonQuizToInsert = new List<PlanLessonQuizUserResultModel>();
                List<string> planLessonIdsStringList = new List<string>();
                foreach (TeacherPlanLessonModel planLessons in planLessonIds)
                {
                    planLessonQuizToInsert.Add(new PlanLessonQuizUserResultModel()
                    {
                        PlanLessonId = planLessons.Id,
                        Score = planLessonQuizUserResultModel.Score,
                        QuizId = planLessonQuizUserResultModel.QuizId,
                        UserId = responseSession.UserId,
                        UsquiUserResultId = planLessonQuizUserResultModel.UsquiUserResultId,
                    });

                    planLessonIdsStringList.Add(planLessons.Id);
                }

                // Check if the quiz can be inserted according to the quiz specific details.
                List<PlanLessonQuizUserResultModel> planLessonQuizUserResultModels = UserResults.GetUserLessonQuizPlanResults(new List<string>() { planLessonQuizUserResultModel.QuizId }, responseSession.UserId);
                // Now check for plan lesson id  if there is any try left 
                if (!TeacherTryEvaluator.CheckIfUserHasTryLeftLessonQuiz(planLessonQuizUserResultModels, planLessonIds))
                {
                    // Cannot be entered 
                    return BadRequest();
                }

                // If there is any try left, then continue with the insertion. 

                // Do the bulk insertion
                var planningModifierManager =
                new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                     AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                     null);

                planningModifierManager.AddPlanLessonQuizResultModels(planLessonQuizToInsert, responseSession.UserId);

                // To update the completion status of the lesson, we need to get the quizzes for each plan lesson.
                foreach (TeacherPlanLessonModel planLessons in planLessonIds)
                {
                    var teacherPlanLessonQuizzes = new TeacherPlanLessonQuizDAO().Read(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                              new Domain.Models.Teachers.TeacherPlanLessonQuizModel() { PlanLessonId = planLessons.Id }, false, false);
                    planningModifierManager.CreateStudentPlanLessonIfNotExists(responseSession.UserId, planLessons.Id, responseSession.UserId);
                    planningModifierManager.UpdateLessonCompletionStatus(planLessons, responseSession.UserId, teacherPlanLessonQuizzes.Select(q => q.QuizId).ToList(), responseSession.UserId);

                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// For STUDENTS: To validate if student can submit the exam.
        /// </summary>
        /// <param name="quizId">The quiz's id to check.</param>
        /// <param name="allowGraceTime">To allow 5 minutes of grace.</param>
        /// <returns>Returns the lesson quiz tries.</returns>
        [HttpGet]
        [Route("students/quiz/{quizId}/available")]
        public async Task<IActionResult> CheckIfExamCanBeSubmitted(string quizId, bool allowGraceTime)
        {
            // Check Token, and if user can be connected with the user or not. 
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // Get the pending quizzes.
            var teacherPlanQuizzess = TeacherPlans.GetStudentPendingLessonQuizzes(responseSession.UserId, quizId);

            if (teacherPlanQuizzess == null || teacherPlanQuizzess.Count == 0)
            {
                return Ok(false);
            }

            // Now evaluate
            foreach(Domain.Models.Generals.Dapper.TeacherPlanLessonQuizModel teacherPlanQuiz in teacherPlanQuizzess)
            {
                if(TeacherTryEvaluator.IsExamAvailableForDateTime(DateTime.Parse(teacherPlanQuiz.DueDate), allowGraceTime ? int.Parse(teacherPlanQuiz.Duration) + 5 : int.Parse(teacherPlanQuiz.Duration), teacherPlanQuiz.DueDateTimezone))
                {
                    return Ok(true);
                }
            }

            return Ok(false);
        }

        /// <summary>
        /// To get the lessons with quizzes.
        /// </summary>
        /// <param name="planningId">The id of the plan.</param>
        /// <param name="limit">How many items per page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>Returns the planning lessons.</returns>
        [HttpGet]
        [Route("{planningId}/lessons")]
        public async Task<IActionResult> GetPlanningLessonsWithQuizzes(string planningId, int? limit = null, int? pageNumber = null)
        {
            try
            {
                // Check access/planning/detail
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                // Return error about invalid request
                return Ok(PlanningReaderManager.GetPlanningLessonsWithQuizzes(planningId, limit, pageNumber));
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{planningId}/lesson/quizzes/{quizId}")]
        public async Task<IActionResult> DeletePlanLessonQuiz(string planningId, string quizId)
        {
            try
            {
                // Check the access
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Check if the planning is from the user 
                PlanningReaderManager planningReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"], responseSession.LangCode);
                var teacherPlanList = planningReaderManager.GetTeacherPlanModels(new List<Domain.Models.Teachers.TeacherPlanModel>()
                {
                   new Domain.Models.Generals.Responses.TeacherPlanModel()
                   {
                       Id = planningId,
                       UserId = responseSession.UserId
                   }
                });

                if (teacherPlanList.Count == 0)
                    return Unauthorized();

                var response = new PlanningModifierManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                                     AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                     null).DeletePlanLessonQuiz(planningId, quizId);

                if (!response.Valid)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To check if the date is allowed to be handle with the period.
        /// </summary>
        /// <param name="periodId">The id of the period.</param>
        /// <param name="utcDateToEvaluate">The UTC date to evaluate.</param>
        /// <returns>Returns true if allowed, else false.</returns>
        protected bool CheckIfDateAllowedAccordingToPeriod(string periodId, string utcDateToEvaluate)
        {
            // Check if period is valid
            var academicPeriods = SchoolsDetailsHandler.GetAcademicPeriodModels(new AcademicPeriodModel()
            {
                Id = periodId,
            });

            if(academicPeriods.Count == 0)
            {
                return false;
            }
            foreach(var academicPeriod in academicPeriods)
            {
                if(DateTime.Parse(academicPeriod.PeriodTo) < DateTime.Parse(utcDateToEvaluate) || DateTime.Parse(
                    academicPeriod.PeriodFrom) > DateTime.Parse(utcDateToEvaluate))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the period is opened.
        /// </summary>
        /// <param name="periodId">The period id.</param>
        /// <returns>Returns true if opened, else, false.</returns>
        protected bool CheckIfPeriodIsOpened(string periodId)
        {
            // Check if period is valid
            var academicPeriods = SchoolsDetailsHandler.GetAcademicPeriodModels(new AcademicPeriodModel()
            {
                Id = periodId,
            });

            if (academicPeriods.Count == 0)
            {
                return false;
            }
            foreach (var academicPeriod in academicPeriods)
            {
                if (DateTime.Now >= DateTime.Parse(academicPeriod.PeriodTo))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// To check if the list of tests can be added.
        /// </summary>
        /// <param name="periodId">The id of the period</param>
        /// <param name="quizModelsToAttachList">The quizzes to add.</param>
        /// <param name="langCode">The language code.</param>
        /// <param name="assignedLessonDate">The assigned lesson date.</param>
        /// <returns>Returns a response model with the details. TransactionValid true if can be added, else false.</returns>
        private Domain.Models.Generals.Responses.ResponseModel CheckIfTestsCanBeAdded(string periodId,
            List<Domain.Models.Teachers.TeacherPlanLessonQuizModel> quizModelsToAttachList,
            string langCode, string assignedLessonDate)
        {
            var academicPeriods = SchoolsDetailsHandler.GetAcademicPeriodModels(new AcademicPeriodModel()
            {
                Id = periodId,
            });
            if (academicPeriods.Count == 0)
            {
                return new Domain.Models.Generals.Responses.ResponseModel()
                {
                    TransactionValid = false,
                    SessionValid = true,
                    Messages = new List<string>()
                        {
                                new MessageManager(
                                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                    langCode).GetMessageDetails(
                                    AppConfiguration.Configuration["AppSettings:tm_message_plan_test_cannot_be_created_because_expiring_date"])
                        }
                };
            }

            // Check the assigned lesson date 
            var lessonDateTime = DateTime.Parse(assignedLessonDate);
            if (lessonDateTime < DateTime.Parse(academicPeriods[0].PeriodFrom) || lessonDateTime > DateTime.Parse(academicPeriods[0].PeriodTo))
            {
                return new Domain.Models.Generals.Responses.ResponseModel()
                {
                    TransactionValid = false,
                    SessionValid = true,
                    Messages = new List<string>()
                        {
                                new MessageManager(
                                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                    langCode).GetMessageDetails(
                                    AppConfiguration.Configuration["AppSettings:tm_message_plan_test_cannot_be_created_because_expiring_date"])
                        }
                };
            }

            foreach (var teacherPlanLessonQuizModel in quizModelsToAttachList)
            {
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(teacherPlanLessonQuizModel.DueDateTimezone);
                DateTime utcConvertedDate = TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.Parse(teacherPlanLessonQuizModel.DueDate), timeZone);
                if(utcConvertedDate < DateTime.Parse(academicPeriods[0].PeriodFrom) || utcConvertedDate >
                    DateTime.Parse(academicPeriods[0].PeriodTo))
                {
                    return new Domain.Models.Generals.Responses.ResponseModel()
                    {
                        TransactionValid = false,
                        SessionValid = true,
                        Messages = new List<string>()
                        {
                                new MessageManager(
                                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                                    langCode).GetMessageDetails(
                                    AppConfiguration.Configuration["AppSettings:tm_message_plan_test_cannot_be_created_because_expiring_date"])
                        }
                    };
                }
            }

            return new Domain.Models.Generals.Responses.ResponseModel()
            {
                TransactionValid = true,
                SessionValid = true,
            };
        }
    }
}
