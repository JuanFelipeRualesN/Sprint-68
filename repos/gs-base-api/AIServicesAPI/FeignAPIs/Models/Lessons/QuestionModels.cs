namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Aggregates a single quiz question with its rubrics and possible responses.
    /// </summary>
    public class QuestionRequestModel
    {
        /// <summary>Core question fields.</summary>
        public QuizQuestionModel QuizQuestionModel { get; set; }

        /// <summary>Rubric entries for this question, each with their grading criteria.</summary>
        public List<QuestionRubricRequestModel> QuizQuestionRubricModelList { get; set; }

        /// <summary>Possible answer choices (and their correctness) for this question.</summary>
        public List<QuizQuestionResponseModel> QuizQuestionResponseModelList { get; set; }
    }

    /// <summary>
    /// Core fields for a single quiz question.
    /// </summary>
    public class QuizQuestionModel
    {
        /// <summary>Indicates whether the model passed validation.</summary>
        public bool Valid { get; set; }

        /// <summary>Validation or informational messages.</summary>
        public List<string> Messages { get; set; }

        /// <summary>Unique question identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning quiz.</summary>
        public string QuizId { get; set; }

        /// <summary>Question prompt text (may include HTML or rich content).</summary>
        public string Desc { get; set; }

        /// <summary>Optional reference to an associated learning sheet.</summary>
        public string LearningSheetId { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// A single answer choice for a quiz question.
    /// </summary>
    public class QuizQuestionResponseModel
    {
        /// <summary>Unique response identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning quiz question.</summary>
        public string QuizQuestionId { get; set; }

        /// <summary>Answer text displayed to the learner.</summary>
        public string Response { get; set; }

        /// <summary>Indicates whether this response is the correct answer.</summary>
        public string Correct { get; set; }

        /// <summary>Point value awarded when this response is selected.</summary>
        public string Value { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// A rubric entry for a quiz question, containing one or more grading criteria.
    /// </summary>
    public class QuestionRubricRequestModel : QuizQuestionRubricModel
    {
        /// <summary>Grading criteria belonging to this rubric entry.</summary>
        public List<QuestionRubricCriterionRequestModel> QuestionRubricCriterionModelList { get; set; }
    }

    /// <summary>
    /// Base rubric descriptor for a single quiz question.
    /// </summary>
    public class QuizQuestionRubricModel
    {
        /// <summary>Unique rubric identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the owning quiz question.</summary>
        public string QuestionId { get; set; }

        /// <summary>Rubric title or overall description.</summary>
        public string Rubric { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// A single grading criterion within a question rubric, with its scored value descriptors.
    /// </summary>
    public class QuestionRubricCriterionRequestModel : QuizQuestionRubricCriterionModel
    {
        /// <summary>Value/score descriptors for each performance level of this criterion.</summary>
        public List<QuizQuestionRubricCriterionValueModel> QuestionRubricCriterionValueModelList { get; set; }
    }

    /// <summary>
    /// A grading criterion attached to a question-level rubric.
    /// </summary>
    public class QuizQuestionRubricCriterionModel
    {
        /// <summary>Unique criterion identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the parent rubric.</summary>
        public string QuizQuestionRubricId { get; set; }

        /// <summary>Text describing what this criterion evaluates.</summary>
        public string Criterion { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// Describes a performance level (and its point value) for a rubric criterion.
    /// </summary>
    public class QuizQuestionRubricCriterionValueModel
    {
        /// <summary>Unique value-descriptor identifier.</summary>
        public string Id { get; set; }

        /// <summary>Identifier of the parent criterion.</summary>
        public string QuizQuestionRubricCriterionId { get; set; }

        /// <summary>Description of the performance level (e.g. "Exceeds Expectations").</summary>
        public string Desc { get; set; }

        /// <summary>Points awarded at this performance level.</summary>
        public string Value { get; set; }

        /// <summary>Identifier of the user who last modified the record.</summary>
        public string ModifiedBy { get; set; }

        /// <summary>ISO 8601 timestamp of record creation.</summary>
        public string CreatedAt { get; set; }

        /// <summary>ISO 8601 timestamp of last update.</summary>
        public string UpdatedAt { get; set; }
    }
}
