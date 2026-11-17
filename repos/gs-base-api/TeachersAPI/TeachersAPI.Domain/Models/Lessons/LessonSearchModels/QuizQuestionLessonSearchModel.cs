using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Lessons.LessonSearchModels
{
	public class QuizQuestionLessonSearchModel: IResponse
	{
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string QuizId { get; set; }
        public string Desc { get; set; }
        public string LearningSheetId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}