using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request
{
    public class QuestionRequestModel
    {
        public QuizQuestionModel QuizQuestionModel { get; set; }
        public List<QuestionRubricRequestModel> QuizQuestionRubricModelList { get; set; }
        public List<QuizQuestionResponseModel> QuizQuestionResponseModelList { get; set; }
    }
}
