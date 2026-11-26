using System.Collections.Generic;
using System.Linq;
using System;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Lessons;
using System.Configuration;
using TeachersAPI.Domain.Models.Generals;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas;
using TeachersAPI.Domain.Models.Lessons.Request;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI.Domain.Models.Generals.Responses.Reports;
using TeachersAPI.Domain.Models.Lessons;
using LessonsResponseModel = TeachersAPI.Domain.Models.Generals.Responses.APIs.LessonsResponseModel;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To handle the reads of the instructional plans.
    /// </summary>
    public class PlanningReaderManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code of the user.</param>
        public PlanningReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// For general use when a process is not an automatic or repeated, as checking if something exists, etc. 
        /// </summary>
        /// <param name="teacherPlanQuizModels">Models to filter.</param>
        /// <returns>Returns a list of teacher plan quiz.</returns>
        public List<TeacherPlanQuizModel> GetTeacherPlanQuizModels(List<TeacherPlanQuizModel> teacherPlanQuizModels)
        {
            return new TeacherPlanQuizDAO().Read(_connectionStringRead, teacherPlanQuizModels);
        }

        /// <summary>
        /// Generic call for teacher plan models. This is the highest flexibility, since you can filter by any of the properties. 
        /// </summary>
        /// <param name="teacherPlanModels">List to filter.</param>
        /// <returns>Returns the teacher plan model list filtered.</returns>
        public List<Domain.Models.Teachers.TeacherPlanModel> GetTeacherPlanModels(List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanModels)
        {
            return new TeacherPlanDAO().Read(_connectionStringRead, teacherPlanModels);
        }


        /// <summary>
        /// Verifies if a quiz can be archived or not.
        /// </summary>
        /// <param name="quizId">Id of quiz.</param>
        /// <returns>Returns true if quiz can be archived, false if not. </returns>
        public bool CanQuizBeArchived(string quizId)
        {
            // First, let check if exists in a plan
            List<TeacherPlanQuizModel> teacherPlanQuizModels = GetTeacherPlanQuizModels(new List<TeacherPlanQuizModel>()
            {
                new TeacherPlanQuizModel()
                {
                    QuizId = quizId,
                },
            });

            // If it is not in a plan, it can be deleted
            if (teacherPlanQuizModels.Count == 0)
            {
                return true;
            }

            // Now, for each plan, verify if all of them are archive
            List<Domain.Models.Teachers.TeacherPlanModel> teacherPlanModelToCheck = new List<Domain.Models.Teachers.TeacherPlanModel>();
            foreach (TeacherPlanQuizModel teacherPlanQuiz in teacherPlanQuizModels)
            {
                teacherPlanModelToCheck.Add(new Domain.Models.Teachers.TeacherPlanModel()
                {
                    Available = "1",
                    Id = teacherPlanQuiz.PlanId
                });
            }

            // If nothing was found, then return true that can be archived
            return new TeacherPlanDAO().Read(_connectionStringRead, teacherPlanModelToCheck).Count == 0;
        }

        /// <summary>
        /// To get the teacher plan full details.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="subjectCode">The grade subject code.</param>
        /// <param name="level">The grade level.</param>
        /// <param name="name">The name of the plan.</param>
        /// <param name="teacherGroup">The groups of the plan.</param>
        /// <param name="fromDate">The starting date from which the query should retrieve data.</param>
        /// <param name="toDate">Represents the ending date until which the query should retrieve data.</param>
        /// <param name="userAccessToken">The user's access token.</param>
        /// <returns>Returns the full instructional plans of the user.</returns>
        public List<Domain.Models.Generals.Dapper.TeacherPlanModel> GetTeacherPlansFullDetails(string userAccessToken, string userId, string subjectCode = null, string level = null, string name = null, string teacherGroup = null, string fromDate = null, string toDate = null)
        {
            // Get Plans from Dapper 
            var teacherPlans = TeacherPlans.GetTeacherPlans(userId, subjectCode, level, name, teacherGroup, fromDate, toDate);

            // Create a list of the teacher plan ids. 
            var planIds = new List<string>();
            foreach (var teacherPlan in teacherPlans)
            {
                planIds.Add(teacherPlan.PlanId);
            } 

            // Get the teacher plans with group details.
            var teacherPlansWithGroupDetails = TeacherPlans.GetTeacherPlansGroups(planIds);

            foreach (var teacherPlan in teacherPlans)
            {
                // Find all groups associated with the current plan
                var groupsForPlan = teacherPlansWithGroupDetails
                    .Where(groupDetail => groupDetail.PlanId == teacherPlan.PlanId)
                    .Select(groupDetail => groupDetail.GroupName)
                    .ToList();

                teacherPlan.TeacherGroups = groupsForPlan;
            }

            // Do not repeat teacher plans. 
            teacherPlans = teacherPlans.GroupBy(x => x.PlanId).Select(x => x.First()).ToList();
            
            List<SubjectModel> subjects = AthenasHandler.GetAthenasSubjects(this._langCode);
            List<string> periodIds = new List<string>();
            foreach (var teacherPlan in teacherPlans)
            {
                teacherPlan.SubjectName = subjects.Find(x => x.Code.ToLower() == teacherPlan.SubjectCode.ToLower())?.Name;
                periodIds.Add(teacherPlan.PeriodId);
            }
           
            // Now, find the periods
            var academicPeriodsFullDetails = SchoolsDetailsHandler.GetAcademicPeriodsFullDetails(userAccessToken, periodIds);

            foreach (var teacherPlan in teacherPlans)
            {
                teacherPlan.AcademicPeriodFullDetail = academicPeriodsFullDetails.FirstOrDefault(x => x.AcademicPeriods.Any(period => period.Id == teacherPlan.PeriodId) && x.PeriodId == teacherPlan.PeriodId);
            }

            if (teacherPlans.Count > 0)
            {
                try
                {
                    teacherPlans = teacherPlans.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).ToList();
                }
                catch (Exception)
                {
                    // continue
                }
            }

            return teacherPlans;
        }

        /// <summary>
        /// To get the teacher plan full details, including the period details.
        /// </summary>
        /// <param name="planId">Id of the plan.</param>
        /// <param name="userId">The teacher's id.</param>
        /// <param name="userAccessToken">The user access token.</param>
        /// <returns>Returns the teacher planning details, including groups, assignments, periods, etc.</returns>
        public Domain.Models.Generals.Responses.TeacherPlanningDetailResponseModel GetTeacherPlanFullDetails(string planId, string userId, string userAccessToken, int? limit = null, int? pageNumber = null)
        {
            var teacherFullPlanDetailsList = TeacherPlans.GetTeacherPlanFullDetails(new List<string>() { planId });
            if (teacherFullPlanDetailsList == null || teacherFullPlanDetailsList.Count == 0 || teacherFullPlanDetailsList[0].TeacherPlanModel == null)
            {
                return new Domain.Models.Generals.Responses.TeacherPlanningDetailResponseModel();
            }

			var subjects = AthenasHandler.GetAthenasSubjects(this._langCode);
            var teacherPlan = teacherFullPlanDetailsList[0].TeacherPlanModel;

			teacherPlan.SubjectName = subjects?.Find(x => string.Equals(x.Code?.Trim(), teacherPlan.SubjectCode?.Trim(), StringComparison.OrdinalIgnoreCase))?.Name;

			// Prepare object to response 
			var teacherPlanningDetailResponseModel = new Domain.Models.Generals.Responses.TeacherPlanningDetailResponseModel()
                {
                    TeacherPlanModel = new Domain.Models.Generals.Responses.TeacherPlanModel()
                    {
                        Id = teacherPlan.PlanId,
                        SubjectCode = teacherPlan.SubjectCode,
						SubjectName = teacherPlan.SubjectName,
                        Available = teacherPlan.PlanAvailable ? "1" : "0",
                        LevelCode = teacherPlan.LevelCode,
                        CreatedAt = teacherPlan.CreatedAt,
                        Name = teacherPlan.PlanName,
                        Opened = teacherPlan.IsPlanOpen ? "1" : "0",
                        PeriodId = teacherPlan.PeriodId,
                        UserId = teacherPlan.TeacherId,
                        OpenDate = teacherPlan.OpenDate,
                        CloseDate = teacherPlan.CloseDate,
                        WeekNumber = teacherPlan.WeekNumber.ToString(),
                        Timeframe = teacherPlan.Timeframe.ToString()
                }
            };

            // Total lesson for pagination
            teacherPlanningDetailResponseModel.TotalLessons = teacherFullPlanDetailsList[0].LessonAssignmentModels.Count;

            // pagination
            if (limit.HasValue && pageNumber.HasValue)
            { 
                teacherFullPlanDetailsList[0].LessonAssignmentModels = teacherFullPlanDetailsList[0].LessonAssignmentModels.Skip(pageNumber.Value * limit.Value).Take(limit.Value).ToList();
            }

            // Find the period. 
            var academicPeriodsFullDetails = SchoolsDetailsHandler.GetAcademicPeriodsFullDetails(userAccessToken, new List<string>() { teacherFullPlanDetailsList[0].TeacherPlanModel.PeriodId });

            if (academicPeriodsFullDetails != null && academicPeriodsFullDetails.Count > 0)
            {
                teacherPlanningDetailResponseModel.AcademicPeriodFullDetail = academicPeriodsFullDetails[0];
            }

            // Prepare lessons attached 
            if (teacherFullPlanDetailsList[0].LessonAssignmentModels != null && teacherFullPlanDetailsList[0].LessonAssignmentModels.Count > 0)
            {
                List<LessonModel> lessonModelsToCall = new List<LessonModel>();
                var lessonQuizToCall = new List<Domain.Models.Teachers.TeacherPlanLessonQuizModel>();
                foreach (Domain.Models.Generals.Dapper.LessonAssignmentModel lessonAssignmentModel in teacherFullPlanDetailsList[0].LessonAssignmentModels)
                {
                    lessonModelsToCall.Add(new LessonModel()
                    {
                        Id = lessonAssignmentModel.LessonId
                    });
                    lessonQuizToCall.Add(new Domain.Models.Teachers.TeacherPlanLessonQuizModel()
                    {
                        PlanLessonId = lessonAssignmentModel.Id
                    });
                }

                // Call the assignments
                var teacherPlanLessonQuizResults = new List<Domain.Models.Teachers.TeacherPlanLessonQuizModel>();
                if (lessonQuizToCall.Count > 0)
                {
                    teacherPlanLessonQuizResults = new TeacherPlanLessonQuizDAO().Read(this._connectionStringRead,
                        lessonQuizToCall);
                }

                // Call Athenas 
                var lessonsResponseModel = AthenasHandler.GetAthenasLessons(lessonModelsToCall, false);

                var teacherPlanLessonDateModels = new TeacherPlanLessonDateDAO().GetTeacherPlanLessonDates(teacherFullPlanDetailsList[0]
                    .LessonAssignmentModels.Select(x => Convert.ToInt32(x.Id)).ToList());
                    
                foreach (Domain.Models.Generals.Dapper.LessonAssignmentModel lessonAssignmentModel in teacherFullPlanDetailsList[0].LessonAssignmentModels)
                {
                    var lesson =
                        lessonsResponseModel?.LessonModelList?.Find(x => x.Id == lessonAssignmentModel.LessonId);
                    var lessonTitle = _langCode == "en"
                        ? AppConfiguration.Configuration["AppSettings:text_lesson_not_found_en"]
                        : AppConfiguration.Configuration["AppSettings:text_lesson_not_found_es"];
                    if (lesson != null && !string.IsNullOrEmpty(lesson.LessonTitle))
                    {
                        lessonTitle = lesson.LessonTitle;
                    }

                    teacherPlanningDetailResponseModel.LessonAttachedModelList.Add(new Domain.Models.Generals.Responses.PlanningAttachedDetailsModel()
                    {
                        DueDate = lessonAssignmentModel.DueDate,
                        ForGrading = lessonAssignmentModel.ForGrading ? "1" : "0",
                        Id = lessonAssignmentModel.Id,
                        ItemId = lessonAssignmentModel.LessonId,
                        Title = lessonTitle,
                        PrePostRequired = lessonAssignmentModel.PrePostRequired,
                        TriesQuantity = lessonAssignmentModel.TriesQuantity.ToString(),
                        isGapClosing = lessonsResponseModel?.LessonModelList?.Find(x => x.Id == lessonAssignmentModel.LessonId).IsGapClosing == "1" ? "1" : "0",
                        TeacherPlanLessonDateModels = teacherPlanLessonDateModels
                            .Where(x => x.TeacherPlanLessonId == Convert.ToInt32(lessonAssignmentModel.Id))
                            .ToList(),
                        TeacherPlanLessonQuizModels = teacherPlanLessonQuizResults?.FindAll(x => x.PlanLessonId == lessonAssignmentModel.Id)
                    });
                }
            }
            teacherPlanningDetailResponseModel.TotalAssignments = teacherFullPlanDetailsList[0].HomeworkAssignmentModels.Count();
            // And prepare the quizzess
            foreach (Domain.Models.Generals.Dapper.HomeworkAssignmentModel homeworkAssignmentModel in teacherFullPlanDetailsList[0].HomeworkAssignmentModels)
            {
                teacherPlanningDetailResponseModel.QuizzesAttachedModelList.Add(new Domain.Models.Generals.Responses.PlanningAttachedDetailsModel()
                {
                    DueDate = homeworkAssignmentModel.DueDate,
                    ForGrading = homeworkAssignmentModel.ForGrading ? "1" : "0",
                    Id = homeworkAssignmentModel.Id,
                    ItemId = homeworkAssignmentModel.QuizId,
                    Title = homeworkAssignmentModel.Title,
                    TriesQuantity = homeworkAssignmentModel.TriesQuantity.ToString()
                });
            }

            // Prepare groups
            var teacherGroups = TeacherPlans.GetTeacherPlanWithGroupDetails(new List<string> { planId }, null, userId);
            foreach (var teacherPlanGroup in teacherGroups)
            {
                teacherPlanningDetailResponseModel.GroupsAttachedList.Add(new Domain.Models.Generals.Responses.TeacherPlanGroupResponseModel()
                {
                    GroupId = teacherPlanGroup.GroupId,
                    GroupName = teacherPlanGroup.GroupName,
                    TeacherPlanGroupId = teacherPlanGroup.TeacherPlanGroupId
                });
            }

            return teacherPlanningDetailResponseModel;
        }

        /// <summary>
        /// To get the teacher planning lesson details.
        /// </summary>
        /// <param name="planLessonId">The id of the plan lesson.</param>
        /// <returns>Returns the teacher plan lesson full details.</returns>
        public TeacherPlanLessonFullDetailModel GetTeacherPlaningLessonDetail(string planLessonId)
        {
            // Read the plan lesson details.
            var teacherPlanFullDetails = TeacherPlans.GetTeacherPlanLessonFullDetails(planLessonId);
            // If there is a plan lesson, find the lesson details in Athenas to add the title of the quizzes. 
            if (teacherPlanFullDetails?.TeacherPlanLessonModel != null)
            {
                // Call the quizzes
                var lessonQuizzesAthenasResponseModel = AthenasHandler.GetLessonQuizzes(
                    teacherPlanFullDetails.TeacherPlanLessonModel.LessonId);

                // If there are quizzes, then add the titles to the teacher plan lesson quiz models.
                if (lessonQuizzesAthenasResponseModel?.Quizzes != null)
                {
                    // Get only the exams 
                    lessonQuizzesAthenasResponseModel.Quizzes =
                        lessonQuizzesAthenasResponseModel.Quizzes.FindAll(x => x.IsExam == "1");

                    // Now attach the quiz details
                    foreach (var quiz in lessonQuizzesAthenasResponseModel.Quizzes)
                    {
                        // Check if quiz exists in teacher plan quiz models 
                        var teacherPlanQuiz =
                            teacherPlanFullDetails.TeacherPlanLessonQuizModels.Find(x => x.QuizId == quiz.Id);
                        if (teacherPlanQuiz != null)
                        {
                            teacherPlanQuiz.QuizTitle = quiz.Title;
                            teacherPlanQuiz.QuizId = quiz.Id;
                        }
                        else
                        {
                            teacherPlanFullDetails.TeacherPlanLessonQuizModels.Add(
                                new Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel()
                                {
                                    PlanLessonId = planLessonId,
                                    QuizId = quiz.Id,
                                    QuizTitle = quiz.Title,
                                });
                        }
                    }
                }
            }

            return teacherPlanFullDetails;
        }

        /// <summary>
        /// To read the user available exams for an specific lesson.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <param name="isStudentHomeschool">Indicates if the student is a homeschool student.</param>
        /// <param name="userToken">The token of the user.</param>
        /// <returns>Returns the lesson quiz response model list.</returns>
        public List<LessonQuizResponseModel> ReadStudentAvailableExams(string lessonId,  string studentId, bool isStudentHomeschool, string userToken)
        {
            var lessonQuizzesResponseModelListToReturn = new List<LessonQuizResponseModel>();

            // Get the quizzes on Athenas, so we can set the titles.
            var lessonAthenasQuizzes = AthenasHandler.GetLessonQuizzes(lessonId);

            // Remove the practices (0) and get only the exams, pre and post.
            var quizAthenasModelsToEvaluate = lessonAthenasQuizzes.Quizzes.FindAll(x => x.IsExam != "0").ToList();

            // If student is homeschool, just return everything as available, since there are no teacher plans for homeschool students.
            if (isStudentHomeschool)
            {
                // Call the homeschool app to check the same details. 
              return GSWebDetails.GetStudentHomeschoolResultsPerLesson(userToken);
            }

            // Now get the plans for that lesson
            var teacherPlanQuizzes =
                TeacherPlans.GetPlanLessonQuizzesAccordingToLessonIdAndStudentId(lessonId, studentId);

            // If there are no quizzes assigned, just return the list of quizzes from Athenas. 
            if (teacherPlanQuizzes == null || teacherPlanQuizzes.Count == 0)
            {
                lessonQuizzesResponseModelListToReturn.AddRange(quizAthenasModelsToEvaluate.Select(quizModel =>
                    new LessonQuizResponseModel()
                    {
                        Title = quizModel.Title,
                        Id = quizModel.Id,
                        AvailableForUser = false,
                        TotalScore = 0,
                        DateAvailable = null,
                        FinishedByUser = false,
                        GotDiamonds = false,
                        HasUserWorkIn = false,
                        IsExam = quizModel.IsExam,
                        LevelOfKnowledge = quizModel.LevelOfKnowledge,
                        Passed = false,
                    }));
                return lessonQuizzesResponseModelListToReturn;
            }

            // Since we have quizzes assigned, continue. 

            // Let's start with the pretest
            // Now, find the pre 
            bool preAvailable = CheckIfPreAvailableForStudent(lessonAthenasQuizzes, lessonId, studentId);
            bool postAvailable = CheckIfPostAvailableForStudent(lessonAthenasQuizzes, lessonId, studentId);

            // Now get the results of the quizzes
            // Select the quizzes ids from lessonAthenasQuizzes.Quizzes
            var quizIdsToFindResults = lessonAthenasQuizzes.Quizzes.Select(x => x.Id).ToList();

            var planLessonQuizUserResultModels = new List<PlanLessonQuizUserResultModel>();
            if (quizIdsToFindResults.Count > 0)
            {
                planLessonQuizUserResultModels =
                    UserResults.GetUserLessonQuizPlanResults(quizIdsToFindResults, studentId);
            }

            // For each quiz from the lesson, we should evaluate if it is available for the user.
            foreach (var quizModel in quizAthenasModelsToEvaluate)
            {
                var lessonQuizResponseModelToAdd = new LessonQuizResponseModel()
                {
                    Title = quizModel.Title,
                    Id = quizModel.Id,
                    IsExam = quizModel.IsExam,
                    GotDiamonds =
                        false, // We do not care about this on this API. This is a reward in GSWebAPI, however, to make the life easier for frontend, we use the property
                    LevelOfKnowledge = quizModel.LevelOfKnowledge,
                };

                // If Pre is available
                if (quizModel.IsExam == "2")
                {
                    if (preAvailable)
                    {
                        lessonQuizResponseModelToAdd.AvailableForUser = true;
                    }

                    lessonQuizzesResponseModelListToReturn.Add(lessonQuizResponseModelToAdd);
                    continue;
                }

                // If post is available
                if (quizModel.IsExam == "3")
                {
                    if (postAvailable)
                    {
                        lessonQuizResponseModelToAdd.AvailableForUser = true;
                    }

                    lessonQuizzesResponseModelListToReturn.Add(lessonQuizResponseModelToAdd);
                    continue;
                }

                // Skip the practices just in case.
                if (quizModel.IsExam == "0")
                {
                    continue;
                }

                // Since it is an exam, we should evaluate if the user has results for that exam.

                // If the Athenas exam is not in the teacher plans, then it should not be available for the user.
                var teacherPlanLessonQuizzesFiltered =
                    teacherPlanQuizzes.FindAll(x => x.QuizId == quizModel.Id);
                if (teacherPlanLessonQuizzesFiltered.Count == 0)
                {
                    lessonQuizResponseModelToAdd.AvailableForUser = false;
                    lessonQuizzesResponseModelListToReturn.Add(lessonQuizResponseModelToAdd);
                    continue;
                }

                // Since there are many possible teacher plans for the same quiz,
                // we should check if there are results for all of them. 
                // If there is a single quiz without results, then it should be available for the user.
                foreach (var teacherPlanQuizFiltered in teacherPlanLessonQuizzesFiltered)
                {
                    // Retrieve the results for that quiz
                    var planLessonQuizUserResult = planLessonQuizUserResultModels.Find(x =>
                        x.QuizId == quizModel.Id && x.PlanLessonId == teacherPlanQuizFiltered.PlanLessonId);

                    // Check if exists already and is available. If it is available, do not revaluate unless the date is less than the date available 
                    var lessonQuizResponseToCheckExists =
                        lessonQuizzesResponseModelListToReturn.Find(x => x.Id == quizModel.Id);
                    if (lessonQuizResponseToCheckExists != null)
                    {
                        // If it is available, just continue
                        if (lessonQuizResponseToCheckExists.AvailableForUser)
                        {
                            continue;
                        }

                        // We check again the quiz to check if there is an availability 
                        lessonQuizResponseToCheckExists.DueDateTimezone = teacherPlanQuizFiltered.DueDateTimezone;
                        lessonQuizResponseToCheckExists.Duration =teacherPlanQuizFiltered.Duration;
                        // Since there are results for that plan, we do not have to update anything and it is not available 
                        if (planLessonQuizUserResult != null)
                        {
                            lessonQuizResponseToCheckExists.FinishedByUser = true;
                            lessonQuizResponseToCheckExists.DateAvailable = teacherPlanQuizFiltered.DueDate;
                            lessonQuizResponseToCheckExists.AvailableForUser = false;
                            lessonQuizResponseToCheckExists.Passed = double.Parse(planLessonQuizUserResult.Score) >= 70;
                            lessonQuizResponseToCheckExists.TotalScore = double.Parse(planLessonQuizUserResult.Score);
                            continue; // Sorry, we cannot make this available, since there are results. 
                        }

                        // IF there are not results, then we can update the availability. 
                        lessonQuizResponseModelToAdd.AvailableForUser = teacherPlanQuizFiltered.Required == "1" &&
                                                                        TeacherTryEvaluator.IsExamAvailableForDateTime(
                                                                            DateTime.Parse(teacherPlanQuizFiltered
                                                                                .DueDate),
                                                                            int.Parse(teacherPlanQuizFiltered.Duration),
                                                                            teacherPlanQuizFiltered.DueDateTimezone);
                        lessonQuizResponseToCheckExists.DateAvailable = teacherPlanQuizFiltered.DueDate;
                        lessonQuizResponseToCheckExists.HasUserWorkIn = false;
                        lessonQuizResponseToCheckExists.Passed = false;
                        continue;
                    }

                    // If it is not added yet to the lesson quizzes response to return, then add it.
                    lessonQuizResponseModelToAdd.DueDateTimezone = teacherPlanQuizFiltered.DueDateTimezone;
                    lessonQuizResponseModelToAdd.Duration = teacherPlanQuizFiltered.Duration;
                    lessonQuizResponseModelToAdd.DateTimeUTCNow = DateTime.UtcNow.ToString();
                    lessonQuizResponseModelToAdd.DateTimeConvertedUTCFrom = DateTimeZoneManager
                        .ConvertTimeZoneAndDateToUniversalDateTime(DateTime.Parse(teacherPlanQuizFiltered.DueDate),
                            teacherPlanQuizFiltered.DueDateTimezone).ToString();
                    lessonQuizResponseModelToAdd.DateTimeConvertedUTCTo = DateTimeZoneManager
                        .ConvertTimeZoneAndDateToUniversalDateTime(
                            DateTime.Parse(teacherPlanQuizFiltered.DueDate)
                                .AddMinutes(int.Parse(teacherPlanQuizFiltered.Duration)),
                            teacherPlanQuizFiltered.DueDateTimezone).ToString();

                    // Not available because there are results.
                    if (planLessonQuizUserResult != null)
                    {
                        lessonQuizResponseModelToAdd.FinishedByUser = true;
                        lessonQuizResponseModelToAdd.DateAvailable = teacherPlanQuizFiltered.DueDate;
                        lessonQuizResponseModelToAdd.AvailableForUser = false;
                        lessonQuizResponseModelToAdd.Passed = double.Parse(planLessonQuizUserResult.Score) >= 70;
                        lessonQuizResponseModelToAdd.TotalScore = double.Parse(planLessonQuizUserResult.Score);
                    }
                    else
                    {
                        lessonQuizResponseModelToAdd.AvailableForUser = teacherPlanQuizFiltered.Required == "1" &&
                                                                        TeacherTryEvaluator.IsExamAvailableForDateTime(
                                                                            DateTime.Parse(teacherPlanQuizFiltered
                                                                                .DueDate),
                                                                            int.Parse(teacherPlanQuizFiltered.Duration),
                                                                            teacherPlanQuizFiltered.DueDateTimezone);
                        lessonQuizResponseModelToAdd.DateAvailable = teacherPlanQuizFiltered.DueDate;
                        lessonQuizResponseModelToAdd.HasUserWorkIn = false;
                        lessonQuizResponseModelToAdd.Passed = false;
                    }

                    lessonQuizzesResponseModelListToReturn.Add(lessonQuizResponseModelToAdd);
                }
            }

            if (lessonQuizzesResponseModelListToReturn.Count > 0)
            {
                lessonQuizzesResponseModelListToReturn = lessonQuizzesResponseModelListToReturn.OrderBy(x => x.IsExam)
                    .ThenBy(x => x.LevelOfKnowledge).ThenBy(x => x.Title).ToList();
            }

            return lessonQuizzesResponseModelListToReturn;
        }

        /// <summary>
        /// To generate the user report of a lesson quiz.
        /// </summary>
        /// <param name="planLessonId">The plan lesson's id.</param>
        /// <param name="quizId">The id of the quiz.</param>
        /// <param name="userId">The id of the user (student).</param>
        /// <param name="token">Token.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>Returns the plan lesson quiz response model.</returns>
        public ReportPlanLessonQuizResponseModel GetUserLessonQuizResultReport(string planLessonId, string quizId, string userId, string token, string ipAddress, string langCode)
        {
            
            // Get the lesson id by plan lesson id.
            var planLesson = TeacherPlans.GetTeacherPlanLessonFullDetails(planLessonId);
            if (planLesson?.TeacherPlanLessonModel == null)
            {
                // Return an error message.
                return new ReportPlanLessonQuizResponseModel()
                {
                    Note = AppConfiguration.Configuration["AppSettings:text_message_lesson_not_found_" + langCode]
                };
            }
            
            // Get the plan quiz details.
            var planLessonQuiz = TeacherPlans.GetPlanLessonQuizzesAccordingToLessonIdAndStudentId(
                    planLesson.TeacherPlanLessonModel.LessonId,
                    userId)
                .Find(x => x.QuizId == quizId);

            if (planLessonQuiz == null)
            {
                // Return an error message.
                return new ReportPlanLessonQuizResponseModel()
                {
                    Note = AppConfiguration.Configuration["AppSettings:text_message_quiz_not_found_" + langCode]
                };
            }
            
            // Call Athenas Lesson.
            var quizAthenas = AthenasHandler.GetQuizById(quizId, token);
            
            var lessonAthenas = AthenasHandler.GetAthenasLessons(new 
                List<Domain.Models.Lessons.LessonModel>()
            {
                new LessonModel(){
                Id = planLesson.TeacherPlanLessonModel.LessonId
            }}, false);

            var reportPlanLessonQuiz = new ReportPlanLessonQuizResponseModel
            {
                // Set the quiz result details.
                DueDate = planLessonQuiz.DueDate,
                DueDateTimeZone = planLessonQuiz.DueDateTimezone,
                Duration = int.Parse(planLessonQuiz.Duration),
                ForGrading = planLessonQuiz.ForGrading
            };
            
            // Set the lesson title.
            reportPlanLessonQuiz.LessonTitle = lessonAthenas.LessonModelList[0].LessonTitle;
            
            // Set quiz details.
            if(quizAthenas == null || quizAthenas?.QuizRequestModel == null)
            {
                reportPlanLessonQuiz.Note = AppConfiguration.Configuration["AppSettings:text_message_quiz_not_found_" + langCode];
                return reportPlanLessonQuiz;
            }
            
            reportPlanLessonQuiz.UserQuizFullDetails = new QuizUserResultsCompletedInfoResponseModel
            {
                QuizId = quizId,
                QuizTitle = quizAthenas.QuizRequestModel.QuizModel.Title,
                Instructions = quizAthenas.QuizRequestModel.QuizModel.Instruction,
                ExtraInfo = quizAthenas.QuizRequestModel?.QuizExtraInfoModel?.ExtraInfo,
                QuestionQuizList = new List<QuestionQuiz>(),
            };
            
            // Call the profile details (name)
            var user = GetUserDetails.GetProfileConnectionsFullNames(token, ipAddress,new List<string>() { userId });
            
            if (user != null && user.Count > 0)
            {
                reportPlanLessonQuiz.StudentName = user[0].Surname + ", " + user[0].GivenName;
            }
            else
            {
                reportPlanLessonQuiz.StudentName = string.Empty;
            }
            
            // Retrieve the results for that quiz
            var planLessonQuizUserResult = UserResults.GetUserLessonQuizPlanResults(new List<string>() { quizId },
                userId, planLessonId);

            if (planLessonQuizUserResult == null || planLessonQuizUserResult.Count == 0)
            {
                reportPlanLessonQuiz.Note = AppConfiguration.Configuration["AppSettings:text_message_quiz_student_has_not_taken_quiz_" + langCode];
                return reportPlanLessonQuiz; 
            }
          
            reportPlanLessonQuiz.Score = planLessonQuizUserResult[0].Score;
            reportPlanLessonQuiz.ManuallyCorrectedByTeacher =
                planLessonQuizUserResult[0].ManuallyCorrectedByTeacher == "1";
            if(reportPlanLessonQuiz.ManuallyCorrectedByTeacher)
            {
                reportPlanLessonQuiz.Note = 
                    AppConfiguration.Configuration["AppSettings:text_message_manually_corrected_by_teacher_" + langCode];
            }
            
            // Stop here if the user does not have any results of submission.
            if (string.IsNullOrWhiteSpace(planLessonQuizUserResult[0].UsquiUserResultId))
            {
                return reportPlanLessonQuiz; 
            }
            
            // Call the GS Web to get the user results.
            var userQuizResponseModel = GSWebDetails.GetUserResults(token, userId, planLessonQuizUserResult[0].UsquiUserResultId);
            
            if(userQuizResponseModel == null || userQuizResponseModel.UserQuizFullDetails == null || userQuizResponseModel.UserQuizFullDetails.QuestionQuizList.Count == 0)
            {
                reportPlanLessonQuiz.Note = 
                    AppConfiguration.Configuration["AppSettings:text_message_student_results_not_found_" + langCode];
                return reportPlanLessonQuiz; 
            }

            reportPlanLessonQuiz.UserQuizFullDetails.TotalPercent = userQuizResponseModel.UserQuizFullDetails.TotalPercent;
            reportPlanLessonQuiz.UserQuizFullDetails.TotalCorrect = userQuizResponseModel.UserQuizFullDetails.TotalCorrect;
            reportPlanLessonQuiz.UserQuizFullDetails.CreatedAt = userQuizResponseModel.UserQuizFullDetails.CreatedAt;
            reportPlanLessonQuiz.UserQuizResultId = planLessonQuizUserResult[0].UsquiUserResultId;
            reportPlanLessonQuiz.UserId = userId;
            
            // Set the results 
            foreach (var questionAthenas in quizAthenas.QuizRequestModel.QuestionRequestModelList)
            {
                var questionQuizToAdd = new QuestionQuiz()
                {
                    QuestionId = questionAthenas.QuizQuestionModel.Id,
                    Question = questionAthenas.QuizQuestionModel.Desc,
                    QuestionResponses = new List<QuestionResponse>(),
                };

                // Now find the response of the user 
                var questionUser =
                        userQuizResponseModel.UserQuizFullDetails.QuestionQuizList.Find(x => x.QuestionId == questionAthenas.QuizQuestionModel.Id);
                var correctResponseAthenas = questionAthenas.QuizQuestionResponseModelList.Find(x => x.Correct == "1");

                foreach (var questionResponseAthenas in questionAthenas.QuizQuestionResponseModelList)
                {
                    var questionResponse = new QuestionResponse()
                    {
                        Correct = int.Parse(questionResponseAthenas.Correct),
                        Id = questionResponseAthenas.Id,
                        Response = questionResponseAthenas.Response,
                        ResponseId = questionResponseAthenas.Id,
                    };

                    // Find the response selected by the user 
                    var responseUser = questionUser?.QuestionResponses.FirstOrDefault();
                    questionResponse.UserSelectCorrectAnswer = responseUser != null ? responseUser.SelectedByUser : false;
                    questionResponse.UserSelectedResponseText = responseUser != null && questionAthenas != null ?
                        questionAthenas.QuizQuestionResponseModelList.Find(x => x.Id == responseUser.ResponseId)?.Response : null;

                    questionQuizToAdd.QuestionResponses.Add(questionResponse);
                }
            }

            reportPlanLessonQuiz.PDFLocation = userQuizResponseModel.PDFLocation;
            return reportPlanLessonQuiz;
        }

        /// <summary>
        /// To get the quizzes for a plan lesson.
        /// </summary>
        /// <param name="planLessonId">The plan lesson id.</param>
        /// <param name="token">The token of the user so we can call Athenas details.</param>
        /// <returns>Returns the plan lesson quiz model details.</returns>
        public List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel> GetQuizzesForPlanLesson(string planLessonId,  string token)
        {
            // First, get the details of the plan lesson quizzes by plan lesson id.
            var planLessonQuizzes = TeacherPlans.GetTeacherPlanLessonFullDetails(planLessonId);
            if (planLessonQuizzes == null || planLessonQuizzes.TeacherPlanLessonQuizModels == null)
            {
                return new List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel>();
            }
            
            // Now get the lesson id by plan lesson id.
            var lessonId = planLessonQuizzes.TeacherPlanLessonModel.LessonId;
            var lessonQuizzesAthenasResponseModel = AthenasHandler.GetLessonQuizzes(lessonId);
            if (lessonQuizzesAthenasResponseModel == null || lessonQuizzesAthenasResponseModel.Quizzes == null)
            {
                return new List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel>();
            }

            var quizzes = new List<Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel>();
            foreach (var planLessonQuiz in planLessonQuizzes.TeacherPlanLessonQuizModels)
            {
                // If it is required, then add it to the list.
                if (planLessonQuiz.Required == "1")
                {
                    var quizToAdd = new Domain.Models.Generals.Responses.TeacherPlanLessonQuizModel
                    {
                        ForGrading = planLessonQuiz.ForGrading,
                        Id = planLessonQuiz.Id,
                        DueDate = planLessonQuiz.DueDate,
                        Duration = planLessonQuiz.Duration,
                        PlanLessonId = planLessonId,
                        QuizId = planLessonQuiz.QuizId,
                        Required = planLessonQuiz.Required,
                        DueDateTimezone = planLessonQuiz.DueDateTimezone,
                        // Set the title.
                        QuizTitle = lessonQuizzesAthenasResponseModel.Quizzes.Find(x => x.Id == 
                                                                                        planLessonQuiz.QuizId)?.Title
                    };

                    quizzes.Add(quizToAdd);
                }
            }

            return quizzes;
        }
        
        /// <summary>
        /// returns user quiz  response
        /// </summary>
        /// <param name="studentId">The student user’s id.</param>
        /// <param name="userQuizId">The user quiz result’s id.</param>
        /// <param name="planLessonId">The user quiz result’s id.</param>
        /// <returns>Retruns the user quiz response from gs_teachers_web_db_prod.plan_lesson_quiz_user_results</returns>
        public PlanLessonQuizUserResultModel GetUserQuizResponse(string studentId, string userQuizId, string planLessonId)
        {
            var planLessonQuizUserResult = UserResults.GetUserLessonQuizPlanResults(new List<string>() { userQuizId },
                studentId, planLessonId);

            if (planLessonQuizUserResult == null || planLessonQuizUserResult.Count == 0)
            {
                return new PlanLessonQuizUserResultModel();
            }
            
            return planLessonQuizUserResult[0];
                
        }


		public static Dictionary<string, Object> GetPlanningLessonsWithQuizzes(string planningId, int? limit = null, int? pageNumber = null)
		{
            var rawResult = limit.HasValue && pageNumber.HasValue? TeacherPlans.GetPlanningLessonsWithQuizzes(planningId, limit, pageNumber) : TeacherPlans.GetPlanningLessonsWithQuizzes(planningId);
            var lessons = (List<TeacherPlanLessonFullDetailModel>)rawResult["QuizzesItems"];

            var lessonIds = lessons.Select(l => new LessonModel() { Id = l.TeacherPlanLessonModel.LessonId }).ToList();
			var athenasLessons = AthenasHandler.GetAthenasLessons(lessonIds, false);

			foreach (var lesson in lessons)
			{
                if (lesson?.TeacherPlanLessonModel is null)
                    continue;

				var lessonQuizzes = AthenasHandler.GetLessonQuizzes(lesson.TeacherPlanLessonModel.LessonId);

				if (lessonQuizzes?.Quizzes is null)
					continue;

				var lessonModel = athenasLessons.LessonModelList.FirstOrDefault(l => l.Id == lesson.TeacherPlanLessonModel.LessonId);

                if (lessonModel != null)
                    lesson.TeacherPlanLessonModel.LessonTitle = lessonModel.LessonTitle;

				lessonQuizzes.Quizzes = lessonQuizzes.Quizzes.FindAll(x => x.IsExam == "1");

				foreach (var quiz in lessonQuizzes.Quizzes)
				{
					var teacherPlanQuiz = lesson.TeacherPlanLessonQuizModels.Find(x => x.QuizId == quiz.Id);
					if (teacherPlanQuiz is null)
						continue;

					teacherPlanQuiz.QuizTitle = quiz.Title;
					teacherPlanQuiz.QuizId = quiz.Id;
				}
			}
            rawResult["QuizzesItems"]= lessons;

			return rawResult;
		}

		/// <summary>
		/// To check if the pre is available for the student.
		/// </summary>
		/// <param name="lessonAthenasQuizzes">The list of quizzes of Athenas to identify the pre.</param>
		/// <param name="lessonId">The id of the lesson.</param>
		/// <param name="studentId">The id of the student.</param>
		/// <returns>Returns true if available, else false.</returns>
		private bool CheckIfPreAvailableForStudent(LessonQuizzesResponseModel lessonAthenasQuizzes, string lessonId,
            string studentId)
        {
            bool preAvailable = true;
            var preTest = lessonAthenasQuizzes.Quizzes.Find(x => x.IsExam == "2");
            if (preTest != null)
            {
                var preTestTries =
                    TeacherTryEvaluator.GetUserLessonPrePostTriesAvailable(lessonId, studentId, preTest.Id);
                if (preTestTries.TriesLeft <= 0)
                {
                    preAvailable = false;
                }
            }
            else
            {
                preAvailable = false;
            }

            return preAvailable;
        }

        /// <summary>
        /// To check if the post is available for the student.
        /// </summary>
        /// <param name="lessonAthenasQuizzes">The list of quizzes of Athenas to identify the post.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="studentId">The id of the student.</param>
        /// <returns>Returns true if available, else false.</returns>
        private bool CheckIfPostAvailableForStudent(LessonQuizzesResponseModel lessonAthenasQuizzes, string lessonId,
            string studentId)
        {
            // And find the post 
            bool postAvailable = true;
            var postTest = lessonAthenasQuizzes.Quizzes.Find(x => x.IsExam == "3");
            if (postTest != null)
            {
                var postTestTries =
                    TeacherTryEvaluator.GetUserLessonPrePostTriesAvailable(lessonId, studentId, postTest.Id);
                if (postTestTries.TriesLeft <= 0)
                {
                    postAvailable = false;
                }
            }
            else
            {
                postAvailable = false;
            }

            return postAvailable;
        }
    }
}