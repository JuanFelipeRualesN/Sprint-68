using System;
using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.LessonProgress;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.LessonProgress;

namespace GSWebAPI.GSWebAPI.Services.LessonProgress
{
    /// <summary>
    /// Service for lesson section progress. Persists which sections a
    /// student has completed and reads them back.
    /// </summary>
    public class LessonProgressManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;

        // Valid lesson sections
        private static readonly HashSet<string> ValidSections = new HashSet<string>
        {
            "concept", "vocabulary", "example", "practice", "exam", "resources"
        };

        public LessonProgressManager(string connectionString, string connectionStringRead)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// Returns true if the section name is one of the supported sections.
        /// </summary>
        public bool IsValidSection(string section)
        {
            return !string.IsNullOrWhiteSpace(section) &&
                   ValidSections.Contains(section.ToLowerInvariant());
        }

        /// <summary>
        /// Marks a lesson section as completed for the user.
        /// </summary>
        public void MarkSectionCompleted(string userId, string lessonId, string section)
        {
            new UserCompletedLessonSectionsDAO().Create(_connectionString, new UserCompletedLessonSectionModel
            {
                UserId = userId,
                LessonId = lessonId,
                Section = section.ToLowerInvariant(),
            });
        }

        /// <summary>
        /// Returns the sections the user has completed for the given lesson.
        /// </summary>
        public List<UserCompletedLessonSectionModel> GetCompletedSections(string userId, string lessonId)
        {
            return new UserCompletedLessonSectionsDAO().ReadByUserAndLesson(_connectionStringRead, userId, lessonId);
        }

        /// <summary>
        /// Marks the "exam" section as completed once the user has results for
        /// every required exam. Called after a quiz submit.
        /// The "practice" section is completed by a manual button on the front.
        /// </summary>
        /// <param name="userId">The student id.</param>
        /// <param name="lessonId">The lesson id.</param>
        /// <param name="section">The section to complete.</param>
        /// <param name="requiredQuizIds">All quiz ids the section requires.</param>
        /// <param name="completedQuizIds">Quiz ids the student already has results for.</param>
        public void MarkQuizSectionIfComplete(string userId, string lessonId, string section, IEnumerable<string> requiredQuizIds, ICollection<string> completedQuizIds)
        {
            if (requiredQuizIds == null || completedQuizIds == null) return;

            bool any = false;
            foreach (var quizId in requiredQuizIds)
            {
                any = true;
                if (!completedQuizIds.Contains(quizId)) return;
            }

            if (any)
            {
                MarkSectionCompleted(userId, lessonId, section);
            }
        }

        /// <summary>
        /// Records today as an active day for the user (for streaks).
        /// </summary>
        public void RecordActivity(string userId)
        {
            new UserLessonActivityDAO().RecordToday(_connectionString, userId);
        }

        /// <summary>
        /// Create the user's current consecutive-day streak. The number of days
        /// up to today (or yesterday) with no gaps.
        /// </summary>
        public int GetCurrentStreak(string userId)
        {
            List<DateTime> dates = new UserLessonActivityDAO().ReadActivityDates(_connectionStringRead, userId);
            if (dates == null || dates.Count == 0) return 0;

            HashSet<DateTime> daySet = new HashSet<DateTime>();
            foreach (var d in dates) daySet.Add(d.Date);

            DateTime today = DateTime.UtcNow.Date;

            // The streak is only "alive" if the most recent activity is today or yesterday
            DateTime cursor;
            if (daySet.Contains(today)) cursor = today;
            else if (daySet.Contains(today.AddDays(-1))) cursor = today.AddDays(-1);
            else return 0;

            int streak = 0;
            while (daySet.Contains(cursor))
            {
                streak++;
                cursor = cursor.AddDays(-1);
            }

            return streak;
        }
    }
}
