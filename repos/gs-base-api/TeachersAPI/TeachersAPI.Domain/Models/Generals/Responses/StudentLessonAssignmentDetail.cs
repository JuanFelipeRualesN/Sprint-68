using System.Collections.Generic;
using TeachersAPI.Domain.Models.Generals.Dapper; 

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class StudentLessonAssignmentDetail
    {
        public StudentLessonAssignmentDetail()
        {
            StudentExamModels = new List<StudentExamModel>();
        }
        
        /// <summary>
        /// Gets or sets the id of the plan.
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Gets or sets the lesson's id.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets level's code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets the original tries.
        /// </summary>
        public int TriesQuantity { get; set; }

        /// <summary>
        /// Gets or sets whether is for grading or not.
        /// </summary>
        public string ForGrading { get; set; }

        /// <summary>
        /// Gets or sets the send by
        /// </summary>
        public string SentBy { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the teacher's id.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the submitted date.
        /// </summary>
        public string SubmittedDate { get; set; }

        /// <summary>
        /// Gets or sets the student id.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the current available tries.
        /// </summary>
        public int CurrentAvailableTries { get; set; }

        /// <summary>
        /// Gets or sets the student's name.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan lesson's ids.
        /// </summary>
        public string TeacherPlanLessonId { get; set; }

        /// <summary>
        /// Gets or sets the total percent.
        /// </summary>
        public decimal TotalPercent { get; set; }

        /// <summary>
        /// Gets or sets the student exam models.
        /// </summary>
        public List<StudentExamModel> StudentExamModels { get; set;}
    }
}