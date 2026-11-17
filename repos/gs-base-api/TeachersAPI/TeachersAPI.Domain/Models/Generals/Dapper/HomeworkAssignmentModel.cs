using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class HomeworkAssignmentModel : AssignmentModel
    {
        public string QuizId { get; set; }
        
        public string TotalMaxScore { get; set; }
    }
}