using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class UserQuizResultModel : UserQuizzes.UserQuizResultModel
    {
        public string PlanQuizId { get; set; }
        
        public string PlanAssignmentUserResultId { get; set; }
        
        public string Title { get; set; }
        
        public string ManuallyCorrectedByTeacher { get; set; }
        
        public string Score { get; set; }
        
        public string TotalPercent { get; set; }
        
        public string UsquiUserResultId { get; set; }
        
        public string TotalMaxScore { get; set; }

    }
}