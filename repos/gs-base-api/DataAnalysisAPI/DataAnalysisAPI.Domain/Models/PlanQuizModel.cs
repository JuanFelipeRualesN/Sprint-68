using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class PlanQuizModel
    {
        public int PlanQuizId { get; set; }
        public int? PlanId { get; set; }
        public int? QuizId { get; set; }

    }
}