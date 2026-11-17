using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// To get the quiz athenas moodel.
    /// </summary>
    public class QuizAthenasModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string Title { get; set; }
        public string Instruction { get; set; }
        public string LevelOfKnowledge { get; set; }
        public string IsExam { get; set; }
        public string QuizTypeId { get; set; }
        public string CustomValue { get; set; }
        public string ReadyToPublish { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Extends...
        /// <summary>
        /// Get or sets the limit date.
        /// </summary>
        public string LimitDate { get; set; }

        /// <summary>
        /// Get or sets whether the quiz is available or not.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets the actual percentage.
        /// </summary>
        public string ActualPercentage { get; set; }
    }
}