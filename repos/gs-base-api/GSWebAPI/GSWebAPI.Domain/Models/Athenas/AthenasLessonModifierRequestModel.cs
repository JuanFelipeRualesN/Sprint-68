using GSWebAPI.GSWebAPI.Domain.Models.Lesson.Request;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using GSWebAPI.GSWebAPI.Domain.Models.Codes;

namespace GSWebAPI.GSWebAPI.Domain.Models.Athenas
{
    public class AthenasLessonModifierRequestModel
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
        public List<ConstructedCodeHandleModel> ConstructedCodeHandleModelList { get; set; }
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
        /// <summary>
        /// To get or sets the status model.
        /// </summary>
        public StatusModel CurrentStatusModel { get; set; }
    }
}
