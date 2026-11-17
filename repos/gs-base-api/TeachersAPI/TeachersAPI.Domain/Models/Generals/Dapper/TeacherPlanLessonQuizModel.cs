using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class TeacherPlanLessonQuizModel : Teachers.TeacherPlanLessonQuizModel
    {
        /// <summary>
        /// Gets or sets the teacher's id.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the tries quantity for lesson/quiz.
        /// </summary>
        public int TriesQuantity { get; set; }

        /// <summary>
        /// Gets or sets whether is for grading (1) or not (0).
        /// </summary>
        public int ForGrading { get; set; }

        /// <summary>
        /// Gets or sets the plan's id.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the lesson's id.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the lesson's title.
        /// </summary>
        public string LessonTitle { get; set; }

        /// <summary>
        /// Gets or sets the user that sent the quiz.
        /// </summary>
        public string SentBy { get; set; }

    }
}