using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
{
    /// <summary>
    /// To handle the lesson quizzes respons.
    /// </summary>
    public class LessonQuizzesResponseModel
    {
        /// <summary>
        /// Gets or sets the quizzes details.
        /// </summary>
        public List<QuizModel> Quizzes { get; set; }
    }
}