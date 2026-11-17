using TeachersAPI.Domain.Models.Teachers;
using System.Collections.Generic;


namespace TeachersAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To attach quizzes and lessons
    /// </summary>
    public class TeacherPlanAttachRequestModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TeacherPlanAttachRequestModel()
        {
            TeacherPlanLessonDateModelList = new List<TeacherPlanLessonDateModel>();
        }
        /// <summary>
        /// Gets or sets the Id to modify. 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the planning Id that we want to attach the lesson or quiz.
        /// </summary>
        public string PlanningId { get; set; }

        /// <summary>
        /// Gets or sets the due date of the assignment or lesson
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets the lesson Id to attach.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the quiz Id to attach.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets if for grading or not (1 or 0).
        /// </summary>
        public string ForGrading { get; set; }

        /// <summary>
        /// Gets or sets if the pre post is required (1 or 0).
        /// </summary>
        public string PrePostRequired { get; set; }

        /// <summary>
        /// Gets or sets if available or not (1 or 0).
        /// </summary>
        public string Available { get; set; }

        /// <summary>
        /// Gets or sets how many times the student can repeat the exam or homework.
        /// </summary>
        public string TriesQuantity { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan quizzess list with  the due date details.
        /// </summary>
        public List<TeacherPlanLessonQuizModel> TeacherPlanLessonQuizModelList { get; set; }
        
        /// <summary>
        /// Gets or sets the teacher plan lesson dates list
        /// </summary>
        public List<TeacherPlanLessonDateModel> TeacherPlanLessonDateModelList { get; set; }
    }
}