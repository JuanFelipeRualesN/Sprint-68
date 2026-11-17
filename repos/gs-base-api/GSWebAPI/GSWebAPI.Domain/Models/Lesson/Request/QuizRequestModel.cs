using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request
{
    /// <summary>
    /// The quiz request model
    /// </summary>
    public class QuizRequestModel
    {
        /// <summary>
        /// The model of the quiz with the main details 
        /// </summary>
        public QuizModel QuizModel { get; set; }
        /// <summary>
        /// The extra information 
        /// </summary>
        public QuizExtraInfoModel QuizExtraInfoModel { get; set; }
        /// <summary>
        /// The question with the details model
        /// </summary>
        public List<QuestionRequestModel> QuestionRequestModelList { get; set; }

        /// <summary>
        /// For Quiz Attachments.
        /// </summary>
        public List<QuizAttachmentModel> QuizAttachmentModelList { get; set; }

        /// <summary>
        /// Gets or sets the quiz rubric.
        /// </summary>
        public QuizRubricModel QuizRubricModel { get; set; }

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
