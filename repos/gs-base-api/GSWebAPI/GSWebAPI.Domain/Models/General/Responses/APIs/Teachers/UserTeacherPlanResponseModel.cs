using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Teachers
{
    /// <summary>
    /// To handle the user teacher plan responses.
    /// </summary>
    public class UserTeacherPlanResponseModel
    {
        /// <summary>
        /// Gets or sets the student id.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets the Id of the quiz.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the lesson.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets whether the quiz is for grading or not.
        /// </summary>
        public string ForGrading { get; set; }

        /// <summary>
        /// Gets or sets the tries that the student has to repeat the exam or assignment.
        /// </summary>
        public int TriesQuantity { get; set; }

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the period id.
        /// </summary>
        public string PeriodId { get; set; }

        /// <summary>
        /// Gets or sets if opened (1), else 0.
        /// </summary>
        public string Opened { get; set; }
    }
}