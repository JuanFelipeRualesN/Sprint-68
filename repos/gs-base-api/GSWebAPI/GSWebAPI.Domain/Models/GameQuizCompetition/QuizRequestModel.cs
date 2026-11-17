using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request;

namespace GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition
{
    class QuizRequestModel
    {
        List<QuestionRequestModel> _questionRequestModelList;
        public List<QuestionRequestModel> QuestionRequestModelList { get => _questionRequestModelList; set => _questionRequestModelList = value; }
    }
}
