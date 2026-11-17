using TeachersAPI.Domain.Models.Lessons.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Lessons.LessonSearchModels
{
	public class QuestionLessonSearchRequestModel
	{
        public QuizQuestionLessonSearchModel QuizQuestionModel { get; set; }
        public List<QuestionRubricRequestModel> QuizQuestionRubricModelList { get; set; }
        public List<QuizQuestionResponseModel> QuizQuestionResponseModelList { get; set; }
    }
}