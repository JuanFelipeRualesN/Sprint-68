using System;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// A child devired from the lesson manager lib with some extra details (Lesson Modifier Request Model).
    /// </summary>
    public class LessonModifierRequestModel : Lesson.Request.LessonModifierRequestModel
    {

        /// <summary>
        /// Gets or sets the lesson average.
        /// </summary>
        public double LessonAverage { get; set; }

        /// <summary>
        /// Gets or sets the pending practices.
        /// </summary>
        public int PendingPractices { get; set; }

        /// <summary>
        /// Gets or sets the pending exams.
        /// </summary>
        public int PendingExams { get; set; }

        /// <summary>
        /// Gets or sets the quiz request details.
        /// </summary>
        public List<QuizRequestModel> QuizRequestModelList { get; set; }

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

    /// <summary>
    /// A child devired from the lesson manager lib with some extra details (Quiz Request Model).
    /// </summary>
    public class QuizRequestModel : Lesson.Request.QuizRequestModel
    {
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

    }
}