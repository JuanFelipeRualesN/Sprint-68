using System.Collections.Generic;
using System.Linq;
using Dapper;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.UserQuizzes;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Services.UserQuizzes
{
    /// <summary>
    /// To handle the user results with Dapper.
    /// </summary>
    public static class UserResults
    {
        private static readonly string connectionStringRead = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"];

        /// <summary>
        /// To get the user lesson averages by user id, lesson id, subject code and level code.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="lessonId">Optional: The lesson id.</param>
        /// <param name="subjectCode">Optional: The subject code.</param>
        /// <param name="levelCode">Optional: The level code.</param>
        /// <returns>Return the lesson averages of the user.</returns>
        public static List<UserLessonAverageModel> GetUserLessonAverages(string userId, string lessonId = null, string subjectCode = null, string levelCode = null)
        {

            return new UserLessonAverageDAO().Read(connectionStringRead, new UserLessonAverageModel()
            {
                UserId = userId,
                LessonId = lessonId,
                SubjectCode = subjectCode,
                LevelCode = levelCode
            }, false, false);
        }

        /// <summary>
        /// Returns the user quiz results by lesson.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the user quiz result model list.</returns>
        public static List<UserQuizResultModel> GetUserQuizResults(string userId, string lessonId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]
            };
            connection.Open();

            string query = @"SELECT id, try_no, user_id, quiz_id, dok, is_exam, lesson_id, subject, level, total_correct, total_questions, total_percent, created_at FROM gs_web_api_db.usqui_user_quiz_results ";

            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                if (!whereUsed)
                {
                    query += @" WHERE user_id = @userId ";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND user_id = @userId ";
                }

                parameters.Add("@userId", userId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (!string.IsNullOrWhiteSpace(lessonId))
            {
                if (!whereUsed)
                {
                    query += @" WHERE lesson_id = @lessonId ";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND lesson_id = @lessonId ";
                }

                parameters.Add("@lessonId", lessonId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            var userQuizResults = connection.Query<UserQuizResultModel>(query, parameters).ToList();
            connection.Close();
            return userQuizResults;
        }

        /// <summary>
        /// To get the pre & post results for clients that does not apply the school curricular plans.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the user quiz results models (pre and post).</returns>
        public static List<UserQuizResultModel> GetUserPrePostResults(string userId, string lessonId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
            };
            connection.Open();

            string query = @"SELECT `id`, `try_no`, `user_id`, `quiz_id`, `dok`, `is_exam`, `lesson_id`, `subject`, `level`, `total_correct`, `total_questions`, `total_percent`, `feedback`, `status`, `modified_by`, `created_at`, `updated_at` FROM `gs_web_api_db`.`usqui_user_quiz_results` WHERE is_exam IN (2,3) AND user_id = @userId AND lesson_id = @lessonId ";
            var parameters = new { userId, lessonId };
            List<UserQuizResultModel> userQuizResults = connection.Query<UserQuizResultModel>(query, parameters).ToList();

            // Group by date and type
            var userQuizResultsToReturn = userQuizResults.GroupBy(x => x.QuizId).Select(x => x.OrderByDescending(y => System.DateTime.Parse(y.CreatedAt)).FirstOrDefault()).OrderByDescending(x => System.DateTime.Parse(x.CreatedAt)).ToList();
            connection.Close();
            return userQuizResultsToReturn;
        }

        /// <summary>
        /// To handle the max rewards by quiz.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="quizIds">The quizzes' ids list.</param>
        /// <returns>Returns the user reward max price per quiz model list.</returns>
        public static List<UserRewardMaxPricePerQuizModel> GetMaxRewardsByQuiz(string userId, List<string> quizIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
            };
            connection.Open();
            string query = @"SELECT id, user_id, quiz_id, created_at FROM gs_web_api_db.gwp_user_reward_max_price_per_quizs ";

            DynamicParameters parameters = new DynamicParameters();
            bool whereUsed = false;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                if (!whereUsed)
                {
                    query += @" WHERE user_id = @userId ";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND user_id = @userId ";
                }

                parameters.Add("@userId", userId, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            }

            if (quizIds != null && quizIds.Count > 0)
            {
                if (!whereUsed)
                {
                    query += @" WHERE quiz_id IN (";
                    whereUsed = true;
                }
                else
                {
                    query += @" AND quiz_id IN (";
                }

                for (int i = 0; i < quizIds.Count; i++)
                {
                    query += string.Concat(" @quizId", i.ToString());
                    parameters.Add("@quizId" + i.ToString(), quizIds[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                    if (i < quizIds.Count - 1)
                    {
                        query += ",";
                    }
                }

                query += ") ";
            }

            var userRewardMaxPrice = connection.Query<UserRewardMaxPricePerQuizModel>(query, parameters).ToList();
            connection.Close();
            return userRewardMaxPrice;
        }
    }
}