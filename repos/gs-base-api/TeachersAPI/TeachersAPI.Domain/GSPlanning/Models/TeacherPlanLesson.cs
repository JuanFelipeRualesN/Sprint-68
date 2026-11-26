using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relation between the lesson and the teacher&apos;s instructional plan. These lessons will be the ones that the student has to complete. 
    /// </summary>
    public class TeacherPlanLesson
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// The lesson attached to the plan (This id comes from Athenas API).
        /// </summary>
        public int LessonId { get; set; }

        /// <summary>
        /// The quantity of tries that the student has to repeat the lesson&apos;s exams. As 2023.04.03, this is not used.
        /// </summary>
        public int? TriesQuantity { get; set; }

        /// <summary>
        /// 1 if Pre and Post are required, 0 if not.
        /// </summary>
        public int? PrePostRequired { get; set; }

        /// <summary>
        /// If the lesson is available for plan or not.
        /// </summary>
        public int? Available { get; set; }

        /// <summary>
        /// The due date, when the user is supposed to complete the lesson.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// 1 if it is for grading, 0 if not.
        /// </summary>
        public int? ForGrading { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlan Plan { get; set; }

        public virtual ICollection<PlanLessonQuizUserResult> PlanLessonQuizUserResults { get; set; } = new List<PlanLessonQuizUserResult>();

        public virtual ICollection<PlanLessonUser> PlanLessonUsers { get; set; } = new List<PlanLessonUser>();

        public virtual ICollection<PlanPrePostUserResult> PlanPrePostUserResults { get; set; } = new List<PlanPrePostUserResult>();

        public virtual ICollection<TeacherPlanLessonQuiz> TeacherPlanLessonQuizzes { get; set; } = new List<TeacherPlanLessonQuiz>();
    }
}
