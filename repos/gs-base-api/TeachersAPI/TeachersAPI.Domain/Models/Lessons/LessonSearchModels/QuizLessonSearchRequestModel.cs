using TeachersAPI.Domain.Models.Lessons.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Lessons.LessonSearchModels
{
    /// <summary>
    /// The quiz request model for lesson search
    /// </summary>
    public class QuizLessonSearchRequestModel
	{
        /// <summary>
        /// The model of the quiz with the main details 
        /// </summary>
        public QuizLessonSearchModel QuizModel { get; set; }
        /// <summary>
        /// The extra information 
        /// </summary>
        public QuizExtraInfoModel QuizExtraInfoModel { get; set; }
        /// <summary>
        /// The question with the details model
        /// </summary>
        public List<QuestionLessonSearchRequestModel> QuestionRequestModelList { get; set; }

        /// <summary>
        /// Gets or sets the quiz rubric.
        /// </summary>
        public QuizRubricLessonSearchModel QuizRubricModel { get; set; }

        /// <summary>
        /// To read the taxonomy
        /// </summary>
        public TaxonomyLevelModel TaxonomyLevel { get; set; }

        /// <summary>
        /// Gets or sets wheter the quiz is auto correct or not.
        /// </summary>
        public bool AutoCorrect { get; set; }

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level code.
        /// </summary>
        public string LevelCode { get; set; }
    }
}