using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapNotificationModel
    {
        public string Id { get; set; }
        public string NotificationText { get; set; }
        public string AcademicYearId { get; set; }
        public string LessonId { get; set; }
        public string SubjectCode { get; set; }
        public string TeacherId { get; set; }
        public string LessonTitle { get; set; }
        public string LimitDate { get; set; }
        public string IsRead { get; set; }
        public string StudentId { get; set; }

        /// <summary>
        /// Foreign key referencing the id in the learning_gap_assigned_lessons table
        /// </summary>
        public string AssignedLessonId { get; set; }

        /// <summary>
        /// Foreign key referencing the id in the learning_gap_diagnostic_year_scores table
        /// </summary>
        public string DiagnosticTestId { get; set; }

        /// <summary>
        /// Type of the item assigned to the notification either lesson or diagnostic
        /// </summary>
        public string NotificationType { get; set; }

        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}