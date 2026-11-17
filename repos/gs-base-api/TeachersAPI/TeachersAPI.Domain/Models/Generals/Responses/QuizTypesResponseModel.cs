using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class QuizTypesResponseModel : ResponseModel
    {

        public List<QuizTypeModel> QuizTypeModelList { get; set; }
    }
}