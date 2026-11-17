using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas
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