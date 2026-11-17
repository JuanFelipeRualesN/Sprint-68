using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.LearningGap
{
    /// <summary>
    /// To return the gap student overall score.
    /// </summary>
    public class LearningGapStudentOverallScoreModel
    {
        /// <summary>
        /// Gets or sets the subject's name.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }
    }
}