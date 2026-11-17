using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request
{
    /// <summary>
    /// The modifier request for the lesson 
    /// </summary>
    public class LessonModifierRequestModel
    {
        /// <summary>
        /// Generic lesson model 
        /// </summary>
        public LessonModel LessonModel { get; set; }
        /// <summary>
        /// The list of the definitions 
        /// </summary>
        public List<LessonDefinitionModel> LessonDefinitionModelList { get; set; }
        /// <summary>
        /// The lesson details 
        /// </summary>
        public LessonDetailModel LessonDetailModel { get; set; }
        /// <summary>
        /// The example list
        /// </summary>
        public List<LessonExampleModel> LessonExampleModelList { get; set; }
        /// <summary>
        /// The introduction audio 
        /// </summary>
        public LessonIntroAudioModel LessonIntroAudioModel { get; set; }
        /// <summary>
        /// The objetive list
        /// </summary>
        public List<LessonObjectiveModel> LessonObjectiveModelList { get; set; }
        /// <summary>
        /// The performance tasks
        /// </summary>
        public List<LessonPerformanceTaskModel> LessonPerformanceTaskModelList { get; set; }
        /// <summary>
        /// The standard codes
        /// </summary>
        public List<LessonStandardCodeModel> LessonStandardCodeModelList { get; set; }
        /// <summary>
        /// The status list
        /// </summary>
        public List<LessonStatusModel> LessonStatusModelList { get; set; }
        /// <summary>
        /// The transversal themes
        /// </summary>
        public List<LessonTransversalThemeModel> LessonTransversalThemeModelList { get; set; }
        /// <summary>
        /// The quizzes
        /// </summary>
        public List<QuizRequestModel> QuizRequestModelList { get; set; }
        /// <summary>
        /// If classic format (If the lesson or quiz has to be shown as a classic format. This is used for Genial Skills Web Unity APP only) 
        /// </summary>
        public bool ClassicFormat { get; set; }
        /// <summary>
        /// Curricular map details
        /// </summary>
        public List<LessonCurricularMapModel> LessonCurricularMapModelList { get; set; }

        /// <summary>
        /// To set the strategy models
        /// </summary>
        public List<LessonStrategyModel> LessonStrategyModelList { get; set; }

        /// <summary>
        /// Address to Lesson List Model
        /// </summary>
        public List<AddressToLessonModel> AddressToLessonModelList { get; set; }

        /// <summary>
        /// To stablish lesson templates
        /// </summary>
        public List<LessonTemplateModel> LessonTemplateModelList { get; set; }

        /// <summary>
        /// To get the templates of lesson
        /// </summary>
        public List<TemplateModel> TemplateModelList { get; set; }

        public List<ConstructedCodeCodeListModel> ConstructedCodes { get; set; }

    }
    /// <summary>
    /// List of all individual codes by lessons
    /// </summary>
    public class ConstructedCodeCodeListModel
    {
        /// <summary>
        /// To get Id of Constructed Code 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// To get if the code is main code (1 =yes, 0 =no)
        /// </summary>
        public int MainCode { get; set; }
        /// <summary>
        /// To get individual codes by lesson
        /// </summary>
        public List<ConstructedCodeCodesListModel> Codes { get; set; }

    }
    /// <summary>
    /// List get individual codes by lesson
    /// </summary>
    public class ConstructedCodeCodesListModel
    {
        /// <summary>
        /// To get the id of Constructed Code Codes 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// To get simple code 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// To get id of  Constructed Code Id 
        /// </summary>
        public int LessonConstructedCodeId { get; set; }
        /// <summary>
        /// To get code id of code 
        /// </summary>
        public int CodeId { get; set; }
        /// <summary>
        /// To get dot afeter code
        /// </summary>
        public int DotAfter { get; set; }
        /// <summary>
        /// To get Description of code
        /// </summary>

        public string Description { get; set; }

    }
}
