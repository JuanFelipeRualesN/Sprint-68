namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Aggregates all data related to a single lesson,
    /// including definitions, objectives, quizzes, and metadata.
    /// </summary>
    public class LessonModifierRequestModel
    {
        /// <summary>Core lesson fields (title, level, subject, etc.).</summary>
        public LessonModel LessonModel { get; set; }

        /// <summary>Vocabulary definitions associated with this lesson.</summary>
        public List<LessonDefinitionModel> LessonDefinitionModelList { get; set; }

        /// <summary>Descriptive and conceptual detail for the lesson.</summary>
        public LessonDetailModel LessonDetailModel { get; set; }

        /// <summary>Worked examples that illustrate lesson content.</summary>
        public List<LessonExampleModel> LessonExampleModelList { get; set; }

        /// <summary>Learning objectives that the lesson targets.</summary>
        public List<LessonObjectiveModel> LessonObjectiveModelList { get; set; }

        /// <summary>Performance tasks learners are expected to complete.</summary>
        public List<LessonPerformanceTaskModel> LessonPerformanceTaskModelList { get; set; }

        /// <summary>Standard codes (e.g. curriculum standards) linked to the lesson.</summary>
        public List<ConstructedCodeHandleModel> ConstructedCodeHandleModelList { get; set; }

        /// <summary>Workflow status history for this lesson.</summary>
        public List<LessonStatusModel> LessonStatusModelList { get; set; }

        /// <summary>Cross-cutting themes addressed by this lesson.</summary>
        public List<LessonTransversalThemeModel> LessonTransversalThemeModelList { get; set; }

        /// <summary>Quizzes attached to this lesson.</summary>
        public List<QuizRequestModel> QuizRequestModelList { get; set; }

        /// <summary>
        /// When <c>true</c>, the lesson or quiz should be rendered in classic format
        /// (used by the Educational Platform Web Unity app only).
        /// </summary>
        public bool ClassicFormat { get; set; }

        /// <summary>Curricular map entries that position this lesson within a curriculum.</summary>
        public List<LessonCurricularMapModel> LessonCurricularMapModelList { get; set; }

        /// <summary>Instructional strategies recommended for this lesson.</summary>
        public List<LessonStrategyModel> LessonStrategyModelList { get; set; }

        /// <summary>Addresses (e.g. learning standards or competencies) this lesson satisfies.</summary>
        public List<AddressToLessonModel> AddressToLessonModelList { get; set; }

        /// <summary>Template assignments that control how the lesson is rendered.</summary>
        public List<LessonTemplateModel> LessonTemplateModelList { get; set; }

        /// <summary>Full template definitions resolved from <see cref="LessonTemplateModelList"/>.</summary>
        public List<TemplateModel> TemplateModelList { get; set; }

        /// <summary>The current publishing/workflow status of the lesson.</summary>
        public StatusModel CurrentStatusModel { get; set; }
    }
}
