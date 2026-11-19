using System;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response
{
    /// <summary>
    /// Represents an academic period that includes academic terms,
    /// along with its related academic detail term dates.
    /// </summary>
    public class AcademicYearPeriodResponseModel
    {
        /// <summary>
        /// The start date of the academic period.
        /// </summary>
        public DateTime PeriodFrom { get; set; }

        /// <summary>
        /// The end date of the academic period.
        /// </summary>
        public DateTime PeriodTo { get; set; }

        /// <summary>
        /// The number of the academic period.
        /// </summary>
        public int PeriodNumber { get; set; }

        /// <summary>
        /// The start date of the full academic term.
        /// </summary>
        public DateTime FullTermStartDate { get; set; }

        /// <summary>
        /// The end date of the full academic term.
        /// </summary>
        public DateTime FullTermEndDate { get; set; }
    }
}
