using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// Profile Activity details.
    /// </summary>
    public class ProfilesActivity
    {
        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the total percent.
        /// </summary>
        public string TotalPercent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the quiz is an exam or not.
        /// </summary>
        public bool IsExam { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public string CreatedDate { get; set; }
    }
}