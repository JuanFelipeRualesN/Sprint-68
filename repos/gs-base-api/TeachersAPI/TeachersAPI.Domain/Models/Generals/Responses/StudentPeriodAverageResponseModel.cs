using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class StudentPeriodAverageResponseModel
    {
        public StudentPeriodAverageResponseModel()
        {
            Scores = new List<decimal>();
            Details = new List<StudentPeriodAverageResponseDetail>();
        }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public string LevelCode { get; set; }

        public string AcademicGrade { get; set; }

        public decimal Average { get; set; }

        public List<decimal> Scores { get; set; }

        public List<StudentPeriodAverageResponseDetail> Details { get; set; }
    }

    public class StudentPeriodAverageResponseDetail
    {
        /// <summary>
        /// The usqui result's id
        /// </summary>
        public string UsquiId { get; set; }

        /// <summary>
        /// Gets or sets the type. H for Homework, L for lesson's quiz.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the academic grade.
        /// </summary>
        public string AcademicGrade { get; set; }

        /// <summary>
        /// Gets or sets the average.
        /// </summary>
        public decimal Average { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}