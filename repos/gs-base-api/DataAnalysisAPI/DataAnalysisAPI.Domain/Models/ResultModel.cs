using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class ResultModel
    {
        public int UserId { get; set; }
        public long? UserQuizResultId { get; set; }
        public DateTime QuizTakenDate { get; set; }
        public long? QuestionId { get; set; }
        public string Question { get; set; }
        public long? ResponseId { get; set; }
        public string StudentSelectedResponse { get; set; }
        public string StudentExplanation { get; set; }
        public float? AssignedValuePerResponse { get; set; }
        public int? TotalQuestionsCorrectFullQuiz { get; set; }
        public float? TotalPercentFullQuiz { get; set; }
        public string FeedbackFullQuiz { get; set; }
        public string UserQuizStatus { get; set; }
        public long? QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int? QuizValue { get; set; }
        public string QuizInstruction { get; set; }
        public int? QuizTypeId { get; set; }
        public string QuizType { get; set; }
        public DateTime QuizDueDate { get; set; }
        public DateTime QuizAssignedToPlanDate { get; set; }
        public int? PlanId { get; set; }
        public string PlanName { get; set; }
        public int? TeacherId { get; set; }
        public int? PlanCurrentlyAvailable { get; set; }
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }
        public int Id { get; set; }
        public DateTime? AcademicYearEnd { get; set; }
        public int? AcademicYearId { get; set; }
        public DateTime? AcademicYearStart { get; set; }
        public string AcademicYearType { get; set; }
        public string AssignmentType { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public long? PeriodId { get; set; }
        public int? PeriodNo { get; set; }
        public DateTime? PeriodTo { get; set; }
        public long? PlanAssignmentUserResultId { get; set; }
        public DateTime? PlanClosedDate { get; set; }
        public long? PlanLessonId { get; set; }
        public long? PlanLessonUserResultId { get; set; }
        public long? LessonId { get; set; }
        public long? PlanQuizId { get; set; }

    }
}