using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.CustomClasses
{
    /// <summary>
    /// To handle details related to grading system (A, B, C, D, F).
    /// </summary>
    public static class GradingSystem
    {
        /// <summary>
        /// Returns the academic grades according to DEPR.
        /// </summary>
        /// <returns>Returns a list of Academic Grades.</returns>
        public static List<AcademicGrade> GetAcademicGrades()
        {
            return new List<AcademicGrade>()
            {
                new AcademicGrade()
                {
                    MinValue = 0,
                    BelowValue = 60,
                    Grade = "F",
                },
                 new AcademicGrade()
                {
                    MinValue = 60,
                    BelowValue = 70,
                    Grade = "D",
                },
                  new AcademicGrade()
                {
                    MinValue = 70,
                    BelowValue = 80,
                    Grade = "C",
                },
                   new AcademicGrade()
                {
                    MinValue = 80,
                    BelowValue = 90,
                    Grade = "B",
                },
                    new AcademicGrade()
                {
                    MinValue = 90,
                    BelowValue = 101,
                    Grade = "A",
                }
            };
        }
    }

    /// <summary>
    /// To handle the academic grades.
    /// </summary>
    public class AcademicGrade
    {
        /// <summary>
        /// Gets or sets the min value for a grade system.
        /// </summary>
        public decimal MinValue { get; set; }

        /// <summary>
        /// Gets or sets the max value for a grade system.
        /// </summary>
        public decimal BelowValue { get; set; }

        /// <summary>
        /// Gets or sets the grade. Example: A, B, C...
        /// </summary>
        public string Grade { get; set; }
    }
}