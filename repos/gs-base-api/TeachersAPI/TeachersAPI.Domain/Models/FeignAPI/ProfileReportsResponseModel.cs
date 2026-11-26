using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// Manages the student results.
    /// </summary>
    public class ProfileReportsResponseModel
    {
        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the quiz details.
        /// </summary>
        public QuizUserResultsCompletedInfoResponseModel UserQuizFullDetails { get; set; }

        /// <summary>
        /// Gets or sets the PDF location.
        /// </summary>
        public string PDFLocation { get; set; }
    }
}
