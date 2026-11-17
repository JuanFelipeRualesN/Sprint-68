using TeachersAPI.Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Domain.Models.Lessons.Response;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class QuizListResponseModel : ResponseModel
    {
        public List<QuizEditModel> QuizModelList { get; set; }
    }
}