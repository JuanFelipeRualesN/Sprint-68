using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Lessons.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Lessons.LessonSearchModels
{

    /// <summary>
    /// Response to handle the lessons 
    /// </summary>
    public class LessonSearchResponseModel: GeneralResponseModel
    {
        /// <summary>
        /// The Lesson details
        /// </summary>
        public LessonSearchRequestModel LessonModifierRequestModel { get; set; }
        /// <summary>
        /// List of the lessons
        /// </summary>
        public List<LessonSearchModel> LessonRequestModel { get; set; }
        /// <summary>
        /// The subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The details of the quiz
        /// </summary>
        public QuizLessonSearchRequestModel QuizRequestModel { get; set; }
        /// <summary>
        /// Id of the lesson
        /// </summary>
        public string LessonId { get; set; }
        /// <summary>
        /// The quizzes list
        /// </summary>
        public List<QuizLessonSearchModel> Quizzes { get; set; }

        /// <summary>
        /// Gets or sets whether the definition is a link or not. 
        /// </summary>
        public bool IsDefinitionALink { get; set; }

        /// <summary>
        /// Gets or sets whether the example is a link or not. 
        /// </summary>
        public bool IsExampleALink { get; set; }

        /// <summary>
        /// Gets or sets the lesson language.
        /// </summary>
        public string LessonLangCode { get; set; }
    }
}