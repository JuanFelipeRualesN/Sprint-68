using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.Lessons
{

    /// <summary>
    /// To handle the quiz response details.
    /// </summary>
    public class QuizResponseModel
    {
        /// <summary>
        /// Gets or sets the quiz's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the quiz's id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the quiz's type.
        /// </summary>
        public string IsExam { get; set; }

        /// <summary>
        /// Gets or sets the level of knowledge.
        /// </summary>
        public string LevelOfKnowledge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user unlocked diamonds or not.
        /// </summary>
        public bool GotDiamonds { get; set; }

        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the quiz was passed or not.
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has work with the quiz.
        /// </summary>
        public bool HasUserWorkIn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the quiz is available for user or not.
        /// </summary>
        public bool AvailableForUser { get; set; }

        /// <summary>
        /// Gets or sets the data available for taking the quiz.
        /// </summary>
        public string DateAvailable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the quiz was finished by the user or not.
        /// </summary>
        public bool FinishedByUser { get; set; }
        public string DueDateTimezone { get; set; }
        public int Duration { get; set; }
        public DateTime? DateTimeUTCNow { get; set; }
        public DateTime? DateTimeConvertedUTCFrom { get; set; }
        public DateTime? DateTimeConvertedUTCTo { get; set; }
    }
}