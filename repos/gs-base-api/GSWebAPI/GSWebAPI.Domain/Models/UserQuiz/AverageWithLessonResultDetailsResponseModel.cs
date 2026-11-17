using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.UserQuiz
{
    public class AverageWithLessonResultDetailsResponseModel
    {
        public string Average { get; set; }
        public List<LessonAverageResponseModel> LessonAverageList { get; set; }
    }

    public class LessonAverageResponseModel
    {
        public string LessonId { get; set; }
        public string LessonAverage { get; set; }
    }
}
