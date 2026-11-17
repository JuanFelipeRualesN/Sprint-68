using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Configuration;
using TeachersAPI.CustomClasses;
using TeachersAPI.TeachersAPI.Services.Teachers;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Generals;
using System.Diagnostics;
using GSPlanning.Domain;
using GSPlanning.Services.Factories;
using GSPlanning.Services.Utilities;
using MySqlConnector;
using TeachersAPI.TeachersAPI.Infrastructure;
using TeachersAPI.Utils;
using TeacherPlanLessonQuizModel = TeachersAPI.Domain.Models.Teachers.TeacherPlanLessonQuizModel;
using TeacherPlanModel = TeachersAPI.Domain.Models.Teachers.TeacherPlanModel;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.TeachersAPI.Services.Lessons;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI;

namespace TeachersAPI.Controllers
{

    /// <summary>
    /// To handle the instructional plans of a teacher. The instructional plans are a set of lessons and assignments that are 
    /// specified by the teachers for the students to work with. The instructional plans are related to an academic period.
    /// </summary>
    [ApiController]
    [Route("teachersapi/v2")]
    public partial class PlanningControllerV2 : PlanningController
    {
        /// <summary>
        /// This is to clone a instructional plan. 
        /// Note: When executing the test on Swagger, please, be sure that the instructional plan id and any other related info is from the teacher's 
        /// account you are testing with.
        /// </summary>
        /// <param name="teacherPlanningRequestModel">The teacher plan details to create.</param>
        /// <returns>Details of instructional plan with quizzes and lessons.</returns>
        [HttpPost("planning/clone")]
        public async Task<IActionResult> ClonePlanning([FromBody] TeacherPlanModel teacherPlanningRequestModel)
        {
            // Continue with everything else 
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            // ID is required for cloning
            if (string.IsNullOrWhiteSpace(teacherPlanningRequestModel.Id) || string.IsNullOrWhiteSpace(teacherPlanningRequestModel.Name) || string.IsNullOrWhiteSpace(teacherPlanningRequestModel.SubjectCode) || string.IsNullOrWhiteSpace(teacherPlanningRequestModel.LevelCode))
            {
                return BadRequest();
            }

            // Check if planning can be added 
            if (!CheckIfPeriodIsOpened(teacherPlanningRequestModel.PeriodId))
            {
                return BadRequest(new Domain.Models.Generals.Responses.ResponseModel()
                {
                    TransactionValid = false,
                    SessionValid = true,
                    Messages = new List<string>()
                    {
                         new MessageManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).GetMessageDetails(Environment.GetEnvironmentVariable("tm_message_plan_cannot_be_created_because_expiring_date"))
                    }
                });
            }

            var oldTeacherPlanModel =
                new PlanningReaderManager(
                        AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                        responseSession.LangCode)
                    .GetTeacherPlanFullDetails(teacherPlanningRequestModel.Id, responseSession.UserId, RequestHandler.GetToken(HttpContext));

            // To create the planning 
            teacherPlanningRequestModel.Id = null;
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

            // Retrieve the dates of the period.
            var periodDetails = SchoolsDetailsHandler.GetAcademicPeriodModels(new AcademicPeriodModel()
            {
                Id = teacherPlanningRequestModel.PeriodId
            });

            if (periodDetails == null || periodDetails.Count == 0)
            {
                return BadRequest(new MessageManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode).
                    GetMessageDetails(Environment.GetEnvironmentVariable("lgap_message_no_active_academic_years")));
            }

            // Get the from date, to set all the dates as FromDate
            var dateEndRange = DateTime.Parse(periodDetails[0].PeriodFrom).AddDays(6).ToShortDateString();

            // If dateEndRange is greater than the period close date, set it to the period close date.
            if (DateTime.Parse(dateEndRange) > DateTime.Parse(periodDetails[0].PeriodTo))
            {
                dateEndRange = periodDetails[0].PeriodTo;
            }

            teacherPlanningDetailModel.ExpiredDateResults = new List<ExpiredDateResult>();

            var responseLessons = await CopyGenialLessons(responseSession, oldTeacherPlanModel, teacherPlanningDetailModel,
            periodDetails[0].PeriodFrom, dateEndRange);

            // Find the lesson name. 
            if (responseLessons.Any())
            {
                var lessonIdsToFind = responseLessons.Select(x => x.Section).ToArray();
                var lessonResponseModels = AthenasHandler.GetLessonsDetail(RequestHandler.GetToken(HttpContext), lessonIdsToFind);
                foreach (var expiredDateResult in responseLessons)
                {
                    var lesson = lessonResponseModels.FirstOrDefault(x => x.LessonModifierRequestModel.LessonModel.Id == expiredDateResult.Section);
                    if (lesson != null)
                    {
                        expiredDateResult.Section = lesson.LessonModifierRequestModel.LessonModel.LessonTitle;
                    }
                }
            }

            var responseQuizzes = await CopyGenialQuizzes(responseSession, oldTeacherPlanModel,
            teacherPlanningDetailModel, dateEndRange);

            // Find the quiz name.
            if (responseQuizzes.Any())
            {
                var quizIdsToFind = new List<QuizModel>();
                foreach (var response in responseQuizzes)
                {
                    quizIdsToFind.Add(new QuizModel()
                    {
                        Id = response.Section
                    });

                }

                var quizReaderManager = new QuizReaderManager(
               AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                var quizModelList = quizReaderManager.GetQuizModels(quizIdsToFind);

                foreach (var expiredDateResult in responseQuizzes)
                {
                    var quiz = quizModelList.FirstOrDefault(x => x.Id == expiredDateResult.Section);
                    if (quiz != null)
                    {
                        expiredDateResult.Section = quiz.Title;
                    }
                }
            }



            teacherPlanningDetailModel.ExpiredDateResults.AddRange(responseLessons);
            teacherPlanningDetailModel.ExpiredDateResults.AddRange(responseQuizzes);

            await CopyGenialForums(responseSession, oldTeacherPlanModel,
             teacherPlanningDetailModel, periodDetails[0].PeriodFrom, dateEndRange);
            await GenerateFreeContentClone(CreateTeacherPlanFromOldStructure(oldTeacherPlanModel.TeacherPlanModel, periodDetails[0].PeriodFrom, dateEndRange),
                CreateTeacherPlanFromOldStructure(teacherPlanningDetailModel, periodDetails[0].PeriodFrom, dateEndRange), periodDetails[0].PeriodFrom, dateEndRange);

            return Created(string.Empty, teacherPlanningDetailModel);
        }

        /// <summary>
        /// To clone the free content.
        /// </summary>
        /// <param name="oldTeacherPLan">The old teacher plan.</param>
        /// <param name="newTeacherPlan">The new teacher plan.</param>
        /// <param name="suggestedCloseRangeDate">The suggested closing date of the range.</param>
        /// <param name="suggestedOpenDateRange">The suggested open date of the range. </param>
        private async Task GenerateFreeContentClone(GSPlanning.Domain.TeacherPlan oldTeacherPLan,
        GSPlanning.Domain.TeacherPlan newTeacherPlan, string suggestedOpenDateRange, string suggestedCloseRangeDate)
        {
            using (var dbConn = CreateMySqlConnection())
            {
                var teacherPlanAccommodationService = TeacherPlanAccommodationServiceFactory.CreateService(dbConn);
                var teacherPlanCustomLessonService = TeacherPlanCustomLessonServiceFactory.CreateService(dbConn);
                var teacherPlanEvaluationService = TeacherPlanEvaluationServiceFactory.CreateService(dbConn);
                var teacherPlanInitiativeProjectInnovationService = TeacherPlanInitiativeProjectInnovationServiceFactory.CreateService(dbConn);
                var teacherPlanMaterialService = TeacherPlanMaterialServiceFactory.CreateService(dbConn);
                var teacherPlanOtherSubjectIntegrationService = TeacherPlanOtherSubjectIntegrationServiceFactory.CreateService(dbConn);
                var teacherPlanPraxisReflectionService = TeacherPlanPraxisReflectionServiceFactory.CreateService(dbConn);
                var teacherPlanStrategyService = TeacherPlanStratergyServiceFactory.CreateService(dbConn);

                await teacherPlanAccommodationService.CopyTeacherPlanAccommodation(oldTeacherPLan, newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanCustomLessonService.CopyTeacherPlanCustomLessons(oldTeacherPLan,
                    newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanEvaluationService.CopyTeacherPlanEvaluation(oldTeacherPLan, newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanInitiativeProjectInnovationService.CopyTeacherPlanInitiativeProjectInnovation(
                    oldTeacherPLan, newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanMaterialService.CopyTeacherPlanAccommodation(oldTeacherPLan, newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanOtherSubjectIntegrationService.CopyTeacherPlanOtherSubjectIntegration(oldTeacherPLan,
                    newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanPraxisReflectionService.CopyTeacherPlanPraxisReflection(oldTeacherPLan,
                    newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
                await teacherPlanStrategyService.CopyTeacherPlanStrategy(oldTeacherPLan, newTeacherPlan, suggestedOpenDateRange, suggestedCloseRangeDate);
            }
        }

        /// <summary>
        /// TO copy Genial Lessons. 
        /// </summary>
        /// <param name="responseSession">The "session" of the user.</param>
        /// <param name="oldTeacherPlanDetail">The old teacher plan.</param>
        /// <param name="newTeachPlanModel">The new teacher plan.</param>
        /// <param name="periodFromDate">The from period date to set as default dates. </param>
        /// <param name="dateRangeClose">The date range close. </param>
        private async Task<List<ExpiredDateResult>> CopyGenialLessons(ValidationResponseModel responseSession,
        TeacherPlanningDetailResponseModel oldTeacherPlanDetail,
        TeacherPlanModel newTeachPlanModel,
        string periodFromDate,
        string dateRangeClose)
        {
            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode);
            var expiredDatesResult = new List<ExpiredDateResult>();

            // Prepare the lessons.  
            foreach (var planningAttachedDetailsModel in oldTeacherPlanDetail.LessonAttachedModelList)
            {
                // Prepare the dates
                var teacherPlanLessonDateModelsToAdd = new List<TeacherPlanLessonDateModel>();
                var teacherPlanLessonQuizzesModelsToAdd = new List<TeacherPlanLessonQuizModel>();

                // If the period id is distinct, add the dates range between the suggested dates. 
                if (oldTeacherPlanDetail.AcademicPeriodFullDetail.PeriodId != newTeachPlanModel.PeriodId)
                {
                    // Set the dates for the lesson.
                    var dates = DateUtil.CreateDates(DateTime.Parse(periodFromDate), DateTime.Parse(dateRangeClose));
                    foreach (var datesModel in dates)
                    {
                        teacherPlanLessonDateModelsToAdd.Add(new TeacherPlanLessonDateModel()
                        {
                            ImplementationDate = datesModel.ToString(),
                            ModifiedBy = responseSession.UserId,
                        });
                    }
                    // Set the dates for the quizzes. 
                    foreach (var quiz in planningAttachedDetailsModel.TeacherPlanLessonQuizModels)
                    {
                        var newDueDate = DateTime.Parse(periodFromDate).AddDays(1).ToString();
                        if (DateTime.TryParse(periodFromDate, out var dueDate))
                        {
                            if (DateTime.Parse(newDueDate).AddDays(1) < DateTime.Now.Date)
                            {
                                expiredDatesResult.Add(new ExpiredDateResult
                                {
                                    Section = planningAttachedDetailsModel.ItemId,
                                    DateExpired = true
                                });
                            }
                        }

                        teacherPlanLessonQuizzesModelsToAdd.Add(new TeacherPlanLessonQuizModel()
                        {
                            QuizId = quiz.QuizId,
                            DueDateTimezone = quiz.DueDateTimezone,
                            DueDate = newDueDate,
                            ModifiedBy = responseSession.UserId,
                        });
                    }
                }
                else
                {
                    // Clean it 
                    foreach (var date in planningAttachedDetailsModel.TeacherPlanLessonDateModels)
                    {
                        teacherPlanLessonDateModelsToAdd.Add(new TeacherPlanLessonDateModel()
                        {
                            ImplementationDate = date.ImplementationDate.ToString(),
                            ModifiedBy = responseSession.UserId,
                        });
                    }

                    foreach (var quiz in planningAttachedDetailsModel.TeacherPlanLessonQuizModels)
                    {
                        if (DateTime.TryParse(quiz.DueDate, out var dueDate))
                        {
                            if (dueDate.Date < DateTime.Now.Date)
                            {
                                expiredDatesResult.Add(new ExpiredDateResult
                                {
                                    Section = planningAttachedDetailsModel.ItemId,
                                    DateExpired = true
                                });
                            }
                        }

                        teacherPlanLessonQuizzesModelsToAdd.Add(new TeacherPlanLessonQuizModel()
                        {
                            QuizId = quiz.QuizId,
                            DueDateTimezone = quiz.DueDateTimezone,
                            DueDate = quiz.DueDate,
                            ModifiedBy = responseSession.UserId,
                        });
                    }
                }

                planningAttachedDetailsModel.TeacherPlanLessonDateModels = teacherPlanLessonDateModelsToAdd;
                planningAttachedDetailsModel.TeacherPlanLessonQuizModels = teacherPlanLessonQuizzesModelsToAdd;

                var responseModel = planningModifierManager.AttachLessonToPlanning(
                    new TeacherPlanLessonModel()
                    {
                        PlanId = newTeachPlanModel.Id,
                        LessonId = planningAttachedDetailsModel.ItemId,
                        DueDate = oldTeacherPlanDetail.AcademicPeriodFullDetail.PeriodId != newTeachPlanModel.PeriodId ?
                        periodFromDate : planningAttachedDetailsModel.DueDate,
                        ModifiedBy = responseSession.UserId,
                        ForGrading = planningAttachedDetailsModel.ForGrading,
                        TriesQuantity = planningAttachedDetailsModel.TriesQuantity,
                        PrePostRequired = planningAttachedDetailsModel.PrePostRequired,
                    },
                    planningAttachedDetailsModel.TeacherPlanLessonQuizModels,
                    planningAttachedDetailsModel.TeacherPlanLessonDateModels,
                    responseSession.UserId,
                    true);

                if (responseModel.Valid)
                {
                    try
                    {
                        // Find plan instructional detalis.
                        var planingReaderManager = new PlanningReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], responseSession.LangCode);
                        TeacherPlanningDetailResponseModel planningDetail = planingReaderManager.GetTeacherPlanFullDetails(newTeachPlanModel.Id, responseSession.UserId, RequestHandler.GetToken(HttpContext));

                        await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail, planningAttachedDetailsModel.ItemId, responseSession, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                    }
                }
            }

            // Clear the duplicated lessons.
            if (expiredDatesResult.Any())
            {
                // Eliminate the duplicated lessons.
                expiredDatesResult = expiredDatesResult
                    .GroupBy(x => x.Section)
                    .Select(g => new ExpiredDateResult
                    {
                        Section = g.Key,
                        DateExpired = g.Any(x => x.DateExpired)
                    }).ToList();
            }

            return expiredDatesResult;
        }

        /// <summary>
        /// To copy the planning quizzes from an old plan to a new plan.
        /// </summary>
        /// <param name="responseSession">The user "session".</param>
        /// <param name="oldTeacherPlanDetail">The plan to clone.</param>
        /// <param name="newTeachPlanModel">The new plan.</param>
        /// <param name="quizzesDate">The date to assign the quiz. </param>
        private async Task<List<ExpiredDateResult>> CopyGenialQuizzes(ValidationResponseModel responseSession,
        TeacherPlanningDetailResponseModel oldTeacherPlanDetail,
        TeacherPlanModel newTeachPlanModel,
        string quizzesDate)
        {
            var expiredDatesResult = new List<ExpiredDateResult>();

            // To create the planning 
            var planningModifierManager = new PlanningModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode);
            foreach (var planningAttachedDetailsModel in oldTeacherPlanDetail.QuizzesAttachedModelList)
            {
                string dueDateString = newTeachPlanModel.PeriodId != oldTeacherPlanDetail.AcademicPeriodFullDetail.PeriodId
                ? quizzesDate
                : planningAttachedDetailsModel.DueDate;

                if (DateTime.TryParse(dueDateString, out var dueDate))
                {
                    if (dueDate.Date < DateTime.Now.Date)
                    {
                        expiredDatesResult.Add(new ExpiredDateResult
                        {
                            Section = planningAttachedDetailsModel.ItemId,
                            DateExpired = true
                        });
                    }
                }

                var responseModel = planningModifierManager.AttachQuizToPlanning(
                    new Domain.Models.Teachers.TeacherPlanQuizModel()
                    {
                        PlanId = newTeachPlanModel.Id,
                        DueDate = newTeachPlanModel.PeriodId != oldTeacherPlanDetail.AcademicPeriodFullDetail.PeriodId ?
                          TeachersAPI.Services.Teachers.CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(DateTime.Parse(quizzesDate)) : TeachersAPI.Services.Teachers.CustomClasses.DateTimeZoneManager.ConvertToMySQLDateString(DateTime.Parse(planningAttachedDetailsModel.DueDate)),
                        QuizId = planningAttachedDetailsModel.ItemId,
                        ForGrading = planningAttachedDetailsModel.ForGrading,
                        ModifiedBy = responseSession.UserId,
                        TriesQuantity = planningAttachedDetailsModel.TriesQuantity,
                    }, true);

                if (responseModel.Valid)
                {
                    try
                    {
                        // Find plan instructional detalis.
                        var planingReaderManager = new PlanningReaderManager(
                            AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
                                , responseSession.LangCode);
                        var planningDetail =
                            planingReaderManager.GetTeacherPlanFullDetails(newTeachPlanModel.Id,
                                responseSession.UserId, RequestHandler.GetToken(HttpContext));
                        await planningModifierManager.AddOrUpdateDesignationToParticipantsTeamCalendar(planningDetail,
                            planningAttachedDetailsModel.ItemId, responseSession, true, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                    }
                }
            }

            return expiredDatesResult;
        }

        /// <summary>
        /// To copy the forums.
        /// </summary>
        /// <param name="responseSession">The user "session".</param>
        /// <param name="oldTeacherPlanDetail">The plan to clone.</param>
        /// <param name="newTeachPlanModel">The new plan.</param>
        /// <param name="dateStartPeriod">The date when starting the period. To make a suggestion of opening the forums when the period id is distinct.</param>
        /// <param name="dateRangeClose">The date of range close. To make a suggestions of closing forum range when the period id is distinct. </param>
        /// <returns>Returns the task. </returns>
        private Task CopyGenialForums(ValidationResponseModel responseSession,
        TeacherPlanningDetailResponseModel oldTeacherPlanDetail,
        TeacherPlanModel newTeachPlanModel,
        string dateStartPeriod,
        string dateRangeClose)
        {
            var forumManager = new ForumManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode);

            var forumsResponseModel = forumManager.ReadTeacherForums(token: responseSession.Token,
                ipAddress: responseSession.IpAddress, createdAt: null, title: null,
                oldTeacherPlanDetail.TeacherPlanModel.Id,
                userIdMakingRequest: responseSession.UserId, 0,
                100);

            foreach (var forumModel in forumsResponseModel.ForumModelList)
            {
                // Prepare the dates if period id distincts. 
                if (oldTeacherPlanDetail.AcademicPeriodFullDetail.PeriodId != newTeachPlanModel.PeriodId)
                {
                    // Create the dates 
                    var forumDates = new List<ForumDatesModel>();
                    var datesRange = DateUtil.CreateDates(DateTime.Parse(dateStartPeriod),
                        DateTime.Parse(dateRangeClose));

                    foreach (var date in datesRange)
                    {
                        forumDates.Add(
                        new ForumDatesModel()
                        {
                            ImplementationDate = date.ToShortDateString(),
                        });
                    }
                    // clean old forum and set new datas
                    forumModel.Id = null;
                    forumModel.CreatedAt = null;
                    forumModel.UpdatedAt = null;
                    forumModel.TeacherPlanId = newTeachPlanModel.Id;
                    forumModel.ImplementationDates = forumDates;
                }
                else
                {
                    var dates =
                     DateUtil.CreateDates(oldTeacherPlanDetail.TeacherPlanModel.GetDateRange(),
                     newTeachPlanModel.GetDateRange(),
                     forumModel.ImplementationDates.Select(x => DateTime.Parse(x.ImplementationDate)).ToArray())
                    .Select(x => new ForumDatesModel() { ImplementationDate = x.ToString("yyyy-MM-dd") })
                    .ToList();
                    // clean old forum and set new datas
                    forumModel.Id = null;
                    forumModel.CreatedAt = null;
                    forumModel.UpdatedAt = null;
                    forumModel.TeacherPlanId = newTeachPlanModel.Id;
                    forumModel.ImplementationDates = dates;
                }

                forumManager.CreateForum(forumModel, responseSession.UserId);
            }

            return Task.CompletedTask;
        }

        private GSPlanning.Domain.TeacherPlan CreateTeacherPlanFromOldStructure(
            Domain.Models.Teachers.TeacherPlanModel teacherPlanModel,
            string periodStartDate,
            string suggestedPlanCloseDate)
        {
            return new TeacherPlan
            {
                Id = Convert.ToInt32(teacherPlanModel.Id),
                ModifiedBy = Convert.ToInt32(teacherPlanModel.UserId),
                UserId = Convert.ToInt32(teacherPlanModel.UserId),
                OpenDate = DateTime.Parse(periodStartDate),
                CloseDate = DateTime.Parse(suggestedPlanCloseDate),
                PeriodId = Convert.ToInt32(teacherPlanModel.PeriodId),
            };
        }

        private static MySqlConnection CreateMySqlConnection()
        {
            MySqlConnection dbConn = new DBConnection(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"]).GetConnection();
            return dbConn;
        }
    }
}
