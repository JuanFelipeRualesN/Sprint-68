using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the teacher instructional plans&apos; details created by a teacher.
    /// </summary>
    public class TeacherPlan
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user&apos;s id (author of the plan). This comes from gs_users_db_prod user&apos;s account table. 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// If the plan is available or not.
        /// </summary>
        public int Available { get; set; }

        /// <summary>
        /// The subject&apos;s code (MAT-SP, EN, SP, ...)
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// The level code (K, 1, 2, 3...)
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// The id of the period. The period comes from the school dataabse (academic details).
        /// </summary>
        public int? PeriodId { get; set; }

        /// <summary>
        /// 1 if opened, 0 if not. If not opened, the teacher should not make any changes.
        /// </summary>
        public int? Opened { get; set; }

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

        /// <summary>
        /// Teacher’s plan opening date
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// Teacher’s plan closing date
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Tacher’s plan number of week
        /// </summary>
        public int? WeekNumber { get; set; }

        /// <summary>
        /// Define the Teacher’s plan duration time. 0 = Weekly, 1 = Biweekly, 2 = Monthly
        /// </summary>
        public int? Timeframe { get; set; }

        public virtual ICollection<Forum> Forums { get; set; } = new List<Forum>();

        public virtual ICollection<PlanLessonUser> PlanLessonUsers { get; set; } = new List<PlanLessonUser>();

        public virtual ICollection<TeacherPlanAccommodation> TeacherPlanAccommodations { get; set; } = new List<TeacherPlanAccommodation>();

        public virtual ICollection<TeacherPlanCustomLesson> TeacherPlanCustomLessons { get; set; } = new List<TeacherPlanCustomLesson>();

        public virtual ICollection<TeacherPlanEvaluation> TeacherPlanEvaluations { get; set; } = new List<TeacherPlanEvaluation>();

        public virtual ICollection<TeacherPlanGroup> TeacherPlanGroups { get; set; } = new List<TeacherPlanGroup>();

        public virtual ICollection<TeacherPlanInitiativeProjectInnovation> TeacherPlanInitiativeProjectInnovations { get; set; } = new List<TeacherPlanInitiativeProjectInnovation>();

        public virtual ICollection<TeacherPlanLesson> TeacherPlanLessons { get; set; } = new List<TeacherPlanLesson>();

        public virtual ICollection<TeacherPlanMaterial> TeacherPlanMaterials { get; set; } = new List<TeacherPlanMaterial>();

        public virtual ICollection<TeacherPlanOtherSubjectIntegration> TeacherPlanOtherSubjectIntegrations { get; set; } = new List<TeacherPlanOtherSubjectIntegration>();

        public virtual ICollection<TeacherPlanPraxisReflection> TeacherPlanPraxisReflections { get; set; } = new List<TeacherPlanPraxisReflection>();

        public virtual ICollection<TeacherPlanQuiz> TeacherPlanQuizzes { get; set; } = new List<TeacherPlanQuiz>();

        public virtual ICollection<TeacherPlanStrategy> TeacherPlanStrategies { get; set; } = new List<TeacherPlanStrategy>();

        public virtual ICollection<TeacherStudentAnnotation> TeacherStudentAnnotations { get; set; } = new List<TeacherStudentAnnotation>();
    }
}
