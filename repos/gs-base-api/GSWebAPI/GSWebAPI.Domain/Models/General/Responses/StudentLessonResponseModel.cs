using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// Handles the student lesson details.
    /// </summary>
    public class StudentLessonResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentLessonResponseModel"/> class.
        /// </summary>
        public StudentLessonResponseModel()
        {
            LessonModifierRequestModel = new LessonModifierRequestModel()
            {
                QuizRequestModelList = new List<QuizRequestModel>(),
                LessonExampleModelList = new List<LessonExampleModel>(),
                AddressToLessonModelList = new List<AddressToLessonModel>(),
                LessonCurricularMapModelList = new List<LessonCurricularMapModel>(),
                TemplateModelList = new List<TemplateModel>(),
                LessonTransversalThemeModelList = new List<LessonTransversalThemeModel>(),
                LessonTemplateModelList = new List<LessonTemplateModel>(),
                LessonPerformanceTaskModelList = new List<LessonPerformanceTaskModel>(),
                LessonStandardCodeModelList = new List<LessonStandardCodeModel>(),
                LessonStatusModelList = new List<LessonStatusModel>(),
                LessonStrategyModelList = new List<LessonStrategyModel>(),
                LessonDefinitionModelList = new List<LessonDefinitionModel>(),
                LessonObjectiveModelList = new List<LessonObjectiveModel>(),
            };
        }

        /// <summary>
        /// Gets or sets the lesson modifier requests.
        /// </summary>
        public LessonModifierRequestModel LessonModifierRequestModel { get; set; }
    }
}