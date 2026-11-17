using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class LessonAssignmentModel : AssignmentModel
    {
        public string LessonId { get; set; }
    }
}