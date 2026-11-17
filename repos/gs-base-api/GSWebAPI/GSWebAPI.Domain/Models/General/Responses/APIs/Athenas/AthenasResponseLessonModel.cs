using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
{
    /// <summary>
    /// To handle the v2 of Athenas Response.
    /// </summary>
    public class AthenasResponseLessonModel
    {
        /// <summary>
        /// Gets or sets the Lesson details.
        /// </summary>
        public LessonModifierRequestModel LessonModifierRequestModel { get; set; }

        /// <summary>
        /// Gets or sets list of the lessons.
        /// </summary>
        public List<LessonModel> LessonRequestModel { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the details of the quiz.
        /// </summary>
        public Lesson.Request.QuizRequestModel QuizRequestModel { get; set; }

        /// <summary>
        /// Gets or sets id of the lesson.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the quizzes list.
        /// </summary>
        public List<QuizModel> Quizzes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the definition is a link or not.
        /// </summary>
        public bool IsDefinitionALink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the example is a link or not.
        /// </summary>
        public bool IsExampleALink { get; set; }

        /// <summary>
        /// Gets or sets the lesson language.
        /// </summary>
        public string LessonLangCode { get; set; }
    }
}