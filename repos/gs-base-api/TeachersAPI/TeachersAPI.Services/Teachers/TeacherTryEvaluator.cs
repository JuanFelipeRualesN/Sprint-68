using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Generals.Dapper;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{ 
    /// <summary>
    /// To handle the tries related to a quiz or exam.
    /// </summary>
    public static class TeacherTryEvaluator
    {
        /// <summary>
        /// To verify if exam is available for date time.
        /// </summary>
        /// <param name="dueDate">The exam due date.</param>
        /// <param name="duration">The exam duration in minutes.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>Returns true if available, else, returns false.</returns>
        public static bool IsExamAvailableForDateTime(DateTime dueDate, int duration, string timeZone)
        {
            DateTime dueDateMaxOG = dueDate.AddMinutes(duration);
            DateTime dueDateMax = DateTimeZoneManager.ConvertTimeZoneAndDateToUniversalDateTime(dueDateMaxOG, timeZone);

            DateTime dueDateMinOG = dueDate;
            DateTime dueDateMin = DateTimeZoneManager.ConvertTimeZoneAndDateToUniversalDateTime(dueDateMinOG, timeZone);

            return DateTime.UtcNow >= dueDateMin && DateTime.UtcNow <= dueDateMax;
        }

        /// <summary>
        /// To check if there is any user lesson quiz try available.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="quizId">The quiz's id.</param>
        /// <param name="evaluateTime">To evaluate the time to submit.</param>
        /// <returns>Returns the quiz tries left model.</returns>
        public static QuizTriesLeftModel GetUserLessonQuizTriesAvailable(string lessonId, string userId, string quizId, bool evaluateTime = false, int graceMinutes = 5)
        {
            List<TeacherPlanLessonModel> teacherPlanLessonsToEvaluate = TeacherPlans.GetPlanLessonIdsAccordingToLessonAndStudents(lessonId, userId);
            if (teacherPlanLessonsToEvaluate == null || teacherPlanLessonsToEvaluate.Count == 0)
            {
                // Cannot be inserted
                return new QuizTriesLeftModel();
            }

            // Get the quizzes results to calculate the total 
            List<PlanLessonQuizUserResultModel> planLessonQuizUserResultModels = UserResults.GetUserLessonQuizPlanResults(new List<string>() { quizId }, userId);

            // Sum the totals
            QuizTriesLeftModel quizTriesLeftModel = new QuizTriesLeftModel();

            foreach (TeacherPlanLessonModel teacherPlan in teacherPlanLessonsToEvaluate)
            {
                // Count the results 
                List<PlanLessonQuizUserResultModel> results = planLessonQuizUserResultModels.FindAll(x => x.PlanLessonId == teacherPlan.Id);
                quizTriesLeftModel.TriesTotal += int.Parse(teacherPlan.TriesQuantity);
                quizTriesLeftModel.TriesUsed += results.Count;
                quizTriesLeftModel.TriesLeft += int.Parse(teacherPlan.TriesQuantity) - results.Count;
            }

            if (quizTriesLeftModel.TriesLeft <= 0)
            {
                quizTriesLeftModel.TriesLeft = 0;
            }

            return quizTriesLeftModel;
        }

        /// <summary>
        /// To check if the user can make the pre or post with the tries left model being returned.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="quizId">The quiz's id.</param>
        /// <returns>Returns the quiz tries left model with the quantity detail.</returns>
        public static QuizTriesLeftModel GetUserLessonPrePostTriesAvailable(string lessonId, string userId, string quizId)
        {
            List<TeacherPlanLessonModel> planLessonIds = TeacherPlans.GetPlanLessonIdsAccordingToLessonAndStudents(lessonId, userId);
            if (planLessonIds == null || planLessonIds.Count == 0)
            {
                // Cannot be inserted
                return new QuizTriesLeftModel();
            }

            List<string> planLessonIdsString = new List<string>();
            bool prePostRequired = false;
            foreach (TeacherPlanLessonModel teacherPlanLesson in planLessonIds)
            {
                planLessonIdsString.Add(teacherPlanLesson.Id);
                if (teacherPlanLesson.PrePostRequired == "1")
                {
                    prePostRequired = true;
                }
            }

            // If the user has not tried before the quiz, then return 1 Try Left, 1 Try total, and Tries Used (1 try per pre/post).
            if (CheckIfUserHasPrePostTryAvailable(planLessonIdsString, userId, quizId))
            {
                return new QuizTriesLeftModel()
                {
                    QuizId = quizId,
                    TriesLeft = 1,
                    TriesTotal = 1,
                    TriesUsed = 0,
                    PrePostRequired = prePostRequired
                };
            }
            else
                return new QuizTriesLeftModel()
                {
                    QuizId = quizId,
                };
        }

        /// <summary>
        /// Check if user has any try left for lesson exams. PlanLessonQuizzesUserResult and teacherPlanLessonModel objects are required and must have data (the data search results must be ready).
        /// </summary>
        /// <param name="planLessonQuizUserResultModels">The plan lesson quizzes user result models.</param>
        /// <param name="planLessonIds">The plan lesson ids.</param>
        /// <returns>Returns true if any try left, else returns false.</returns>
        public static bool CheckIfUserHasTryLeftLessonQuiz(List<PlanLessonQuizUserResultModel> planLessonQuizUserResultModels, List<TeacherPlanLessonModel> planLessonIds)
        {
            // Now check for plan lesson id  if there is any try left 
            bool anyTryLeft = false;
            foreach (TeacherPlanLessonModel teacherPlan in planLessonIds)
            {
                // Count the results 
                if (planLessonQuizUserResultModels.FindAll(x => x.PlanLessonId == teacherPlan.Id).Count < int.Parse(teacherPlan.TriesQuantity))
                {
                    anyTryLeft = true;
                }
            }
            return anyTryLeft;
        }

        /// <summary>
        /// Check if the user has a pre or post already.
        /// </summary>
        /// <param name="planLessonId">The plan lesson's id.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="quizId">he quiz's id.</param>
        /// <returns>Returns true if available, else returns false.</returns>
        public static bool CheckIfUserHasPrePostTryAvailable(List<string> planLessonIds, string userId, string quizId)
        {
            List<PlanPrePostUserResultModel> prePostPlanList = UserResults.GetPlanLessonPrePost(quizId, userId);
            if (prePostPlanList.Count == 0)
                return true;

            // Now verify for each planLessonId
            foreach (string planLessonId in planLessonIds)
            {
                if (prePostPlanList.Find(x => x.PlanLessonId == planLessonId) == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// To check if there is any user lesson quiz try available.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="planQuizId">The plan quiz's id.</param>
        /// <returns>Returns the quiz tries left model.</returns>
        public static QuizTriesLeftModel GetUserHomeworkQuizTriesAvailable(string planQuizId, string userId)
        {
            List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> planQuizzesIds = TeacherPlans.GetPlanQuizIdsAccordingToQuizAndStudent(userId, null, planQuizId);
            if (planQuizzesIds == null || planQuizzesIds.Count == 0)
            {
                // Cannot be inserted
                return new QuizTriesLeftModel();
            }

            // Get the quizzes results to calculate the total 
            List<UserQuizResultModel> planLessonQuizUserResultModels = UserResults.GetUserQuizHomeworkPlanResults(new List<string>() { planQuizzesIds[0].QuizId }, userId);

            // Sum the totals
            QuizTriesLeftModel quizTriesLeftModel = new QuizTriesLeftModel();

            foreach (Domain.Models.Generals.Dapper.TeacherPlanQuizModel teacherPlan in planQuizzesIds)
            {
                // Count the results 
                List<UserQuizResultModel> results = planLessonQuizUserResultModels.FindAll(x => x.PlanQuizId == teacherPlan.Id);
                quizTriesLeftModel.TriesTotal += int.Parse(teacherPlan.TriesQuantity);
                quizTriesLeftModel.TriesUsed += results.Count;
                quizTriesLeftModel.TriesLeft += int.Parse(teacherPlan.TriesQuantity) - results.Count;
                quizTriesLeftModel.QuizId = teacherPlan.QuizId;
            }

            if (quizTriesLeftModel.TriesLeft <= 0)
            {
                quizTriesLeftModel.TriesLeft = 0;
            }

            return quizTriesLeftModel;
        }

        /// <summary>
        /// Check if user has any try left for homeworks. UserQuizResultModel list and TeacherPlanQuizModel objects are required and must have data (the data search results must be ready).
        /// </summary>
        /// <param name="planQuizzesIds">The plan homework quizzes user result models.</param>
        /// <param name="planQuizzesIds">The plan quizzes' ids.</param>
        /// <returns>Returns true if any try left, else returns false.</returns>
        public static bool CheckIfUserHasTryLeftHomeworkQuiz(List<UserQuizResultModel> planQuizUserResultModels, List<Domain.Models.Generals.Dapper.TeacherPlanQuizModel> planQuizzesIds)
        {
            // Now check for plan lesson id  if there is any try left 
            bool anyTryLeft = false;
            foreach (Domain.Models.Generals.Dapper.TeacherPlanQuizModel teacherPlan in planQuizzesIds)
            {
                // Count the results 
                if (planQuizUserResultModels.FindAll(x => x.PlanQuizId == teacherPlan.Id).Count < int.Parse(teacherPlan.TriesQuantity))
                {
                    anyTryLeft = true;
                }
            }
            return anyTryLeft;
        }
    }
}