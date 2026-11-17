using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Lessons.Request;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    public class LessonsResponseModel
    {
        public List<LessonSearchRequestModel> Lessons { get; set; }
        public int TotalResults { get; set; }
    }
}
