using System.Collections.Generic;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Teachers;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class QuestionTypesResponseModel : ResponseModel
    {
        public List<QuizQuestionTypesModel> QuestionTypeModelList { get; set; }
    }
}