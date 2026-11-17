using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Request
{
    public class QuestionRubricRequestModel : QuizQuestionRubricModel
    {
        public List<QuestionRubricCriterionRequestModel> QuestionRubricCriterionModelList { get; set; }
    }
}
