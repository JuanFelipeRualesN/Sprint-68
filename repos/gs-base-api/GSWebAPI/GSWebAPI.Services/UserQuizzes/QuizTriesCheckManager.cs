using System;
using System.Collections.Generic;
using System.Configuration;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Services.UserQuizzes
{
    /// <summary>
    /// To check the tries manager.
    /// </summary>
    public static class QuizTriesCheckManager
    {
        /// <summary>
        /// Checks if the user can make the pre or post.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="quizId">The quiz's id.</param>
        /// <returns>Returns the tries totals.</returns>
        public static QuizTriesLeftModel CanMakePrePost(string userId, string quizId)
        {
            List<UserQuizResultModel> userQuizResults = new UserQuizService(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], null).GetUserQuizResults(new UserQuizResultModel()
                {
                    UserId = userId,
                    QuizId = quizId,
                });

            if (userQuizResults == null || userQuizResults.Count == 0)
            {
                return new QuizTriesLeftModel()
                {
                    QuizId = quizId,
                    TriesLeft = 1,
                    TriesTotal = 1,
                    TriesUsed = 0,
                };
            }
            else
            {
                return new QuizTriesLeftModel()
                {
                    QuizId = quizId,
                    TriesLeft = 0,
                    TriesTotal = 0,
                    TriesUsed = 1,
                };
            }
        }

        /// <summary>
        /// To get how many tries left for a quiz.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="quizId">The quiz's id.</param>
        /// <returns>Returns the quiz left tries model.</returns>
        public static QuizTriesLeftModel GetQuizTry(string userId, string quizId)
        {
            UserQuizService userQuizManager = new UserQuizService(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], null);
            List<UserQuizResultModel> userQuizResults = userQuizManager.GetUserQuizResults(new UserQuizResultModel()
            {
                UserId = userId,
                QuizId = quizId,
            });

            if (userQuizResults == null || userQuizResults.Count == 0)
            {
                return new QuizTriesLeftModel()
                {
                    QuizId = quizId,
                    TriesLeft = int.Parse(AppConfiguration.Configuration["AppSettings:usqui_max_try_allows"]),
                    TriesTotal = int.Parse(AppConfiguration.Configuration["AppSettings:usqui_max_try_allows"]),
                    TriesUsed = 0,
                };
            }
            else
            {
                var tryAllows = int.Parse(AppConfiguration.Configuration["AppSettings:usqui_max_try_allows"]);
                return new QuizTriesLeftModel()
                {
                    QuizId = quizId,
                    TriesLeft = tryAllows - userQuizResults.Count <= 0 ? 0 : tryAllows - userQuizResults.Count,
                    TriesTotal = int.Parse(AppConfiguration.Configuration["AppSettings:usqui_max_try_allows"]),
                    TriesUsed = userQuizResults.Count,
                };
            }
        }
    }
}
