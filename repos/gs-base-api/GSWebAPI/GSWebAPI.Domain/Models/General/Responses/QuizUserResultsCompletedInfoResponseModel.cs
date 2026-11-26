using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the quiz results complete info.
    /// </summary>
    public class QuizUserResultsCompletedInfoResponseModel
    {
        /// <summary>
        /// Gets or sets the quiz id.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the user quiz id.
        /// </summary>
        public string UserQuizId { get; set; }

        /// <summary>
        /// Gets or sets the quiz title.
        /// </summary>
        public string QuizTitle { get; set; }

        /// <summary>
        /// Gets or sets the total percentage of the quiz.
        /// </summary>
        public string TotalPercent { get; set; }

        /// <summary>
        /// Gets or sets the Depth of Knowledge.
        /// </summary>
        public string DoK { get; set; }

        /// <summary>
        /// Gets or sets how many were correct.
        /// </summary>
        public string TotalCorrect { get; set; }

        /// <summary>
        /// Gets or sets the try number.
        /// </summary>
        public string TryNo { get; set; }

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Gets or sets the extra information of the quiz.
        /// </summary>
        public string ExtraInfo { get; set; }

        /// <summary>
        /// Gets or sets the question quiz list.
        /// </summary>
        public List<QuestionQuiz> QuestionQuizList { get; set; }

        /// <summary>
        /// Gets or sets the created at date of the quiz result.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the lesson title related to the quiz result.
        /// </summary>
        public string LessonTitle { get; set; }

        /// <summary>
        /// Gets or sets the subject of the quiz result.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the grade/level of the quiz result.
        /// </summary>
        public string Grade { get; set; }
    }
}