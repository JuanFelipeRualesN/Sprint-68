using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Request
{
    public class QuestionRequestModel
    {
        public QuizQuestionModel QuizQuestionModel { get; set; }
        public List<QuestionRubricRequestModel> QuizQuestionRubricModelList { get; set; }
        public List<QuizQuestionResponseModel> QuizQuestionResponseModelList { get; set; }
    }
}
