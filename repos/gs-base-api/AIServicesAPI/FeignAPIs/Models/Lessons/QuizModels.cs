namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Aggregates all data for a single quiz: core fields, extra info, questions, and rubric.
    /// </summary>
    public class QuizRequestModel
    {
        /// <summary>Core quiz fields (title, instruction, type, etc.).</summary>
        public QuizModel QuizModel { get; set; }

        /// <summary>Supplementary metadata stored separately from the main quiz record.</summary>
        public QuizExtraInfoModel QuizExtraInfoModel { get; set; }

        /// <summary>Questions belonging to this quiz, each with responses and rubrics.</summary>
        public List<QuestionRequestModel> QuestionRequestModelList { get; set; }

        /// <summary>Overall rubric applied to the entire quiz (if any).</summary>
        public QuizRubricModel QuizRubricModel { get; set; }

        /// <summary>Bloom's (or equivalent) taxonomy level this quiz targets.</summary>
        public TaxonomyLevelModel TaxonomyLevel { get; set; }

        /// <summary>When <c>true</c>, the platform grades responses automatically.</summary>
        public bool AutoCorrect { get; set; }

        /// <summary>Subject area code inherited from the parent lesson.</summary>
        public string SubjectCode { get; set; }

        /// <summary>Grade/difficulty level code inherited from the parent lesson.</summary>
        public string LevelCode { get; set; }
    }

    /// <summary>
    /// Core quiz record fields.
    /// </summary>
    public class QuizModel
    {
        /// <summary>Unique quiz identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning lesson.</summary>
        public string LessonId { get; set; }

        /// <summary>Display title of the quiz.</summary>
        public string Title { get; set; }

        /// <summary>Instruction text shown to the learner before they begin.</summary>
        public string Instruction { get; set; }

        /// <summary>Cognitive level descriptor (e.g. "Knowledge", "Analysis").</summary>
        public string LevelOfKnowledge { get; set; }

        /// <summary>Indicates whether this quiz is treated as a formal exam.</summary>
        public string IsExam { get; set; }

        /// <summary>Foreign key referencing the quiz type lookup table.</summary>
        public string QuizTypeId { get; set; }

        /// <summary>Arbitrary custom value used for platform-specific configuration.</summary>
        public string CustomValue { get; set; }

        /// <summary>Indicates whether the quiz has been marked ready to publish.</summary>
        public string ReadyToPublish { get; set; }

        /// <summary>Indicates whether the quiz is currently available to learners.</summary>
        public string Available { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Supplementary metadata stored alongside the main quiz record.
    /// </summary>
    public class QuizExtraInfoModel
    {
        /// <summary>Unique extra-info record identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning quiz.</summary>
        public string QuizId { get; set; }

        /// <summary>Free-form extra information (JSON, plain text, etc.).</summary>
        public string ExtraInfo { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Overall rubric descriptor for an entire quiz.
    /// </summary>
    public class QuizRubricModel
    {
        /// <summary>Unique rubric identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning quiz.</summary>
        public string QuizId { get; set; }

        /// <summary>Text describing the rubric criteria at the quiz level.</summary>
        public string Desc { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
