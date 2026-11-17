using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Lessons.Response;

namespace TeachersAPI.Domain.Models.Lessons.Request
{
    /// <summary>
    /// The quiz request model
    /// </summary>
    public class QuizRequestModel
    {
        /// <summary>
        /// The model of the quiz with the main details 
        /// </summary>
        public QuizEditModel QuizModel { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether if finished by user.
        /// </summary>
        public bool FinishedByUser { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether the user got diamonds or not.
        /// </summary>
        public bool GotDiamonds { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether quiz was passed or not.
        /// </summary>
        public bool Passed { get; set; }
        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        public double TotalScore { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether has work on the quiz before or not.
        /// </summary>
        public bool HasUserWorkIn { get; set; }
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
        /// Gets or sets the quiz match instructions.
        /// </summary>
        public List<QuizMatchInstructionsModel> QuizMatchInstructionsModel { get; set; }

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
