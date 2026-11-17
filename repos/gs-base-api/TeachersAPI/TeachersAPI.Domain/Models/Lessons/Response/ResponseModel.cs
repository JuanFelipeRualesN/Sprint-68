using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    /// <summary>
    /// Generic Response
    /// </summary>
    public class ResponseModel
    {
        public bool Valid { get; set; }
        public List<string> Messages { get; set; }
        public List<string> MessageCodes { get; set; }
        public string LessonId { get; set; }
        public string QuizId { get; set; }
    }
}
