using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request
{
    public class QuestionRubricCriterionRequestModel : QuizQuestionRubricCriterionModel
    {
        public List<QuizQuestionRubricCriterionValueModel> QuestionRubricCriterionValueModelList { get; set; }
    }
}
