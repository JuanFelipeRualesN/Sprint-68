using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.Reports
{
    /// <summary>
    /// To handle the reports scores overall by group.
    /// </summary>
    public class ReportScoreGroupOverallModel
    {
        /// <summary>
        /// COnstructor.
        /// </summary>
        public ReportScoreGroupOverallModel()
        {
            ReportByGroupingResultsModels = new List<ReportByGroupingResultsModel>();
            ReportGroupStudentResultsModels = new List<ReportGroupStudentResultsModel>();
        }
        /// <summary>
        /// To handle the general details of the quiz, with the overall, and so on.
        /// </summary>
        public List<ReportByGroupingResultsModel> ReportByGroupingResultsModels { get; set; }

        /// <summary>
        /// To handle the report groups students results.
        /// </summary>
        public List<ReportGroupStudentResultsModel> ReportGroupStudentResultsModels { get; set; }
    }
}