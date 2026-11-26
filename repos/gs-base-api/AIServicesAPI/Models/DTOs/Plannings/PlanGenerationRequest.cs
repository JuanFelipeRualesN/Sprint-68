namespace GSAIServicesAPI.Models.DTOs.Plannings
{
    /// <summary>
    /// Plan Generation DTO
    /// </summary>
    public class PlanGenerationRequest
    {
        /// <summary>
        /// The subject's code of the plan
        /// </summary>
        public string SubjectCode { get; set; } = string.Empty;

        /// <summary>
        /// The subject's name
        /// </summary>
        public string SubjectName { get; set; } = string.Empty;

        /// <summary>
        /// The level's code of the plan
        /// </summary>
        public string LevelCode { get; set; } = string.Empty;

        /// <summary>
        /// The unit's id in Athenas
        /// </summary>
        public string? UnitId { get; set; }

        /// <summary>
        /// The week of the unit, or null for the whole unit
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// The topic the teacher wrote, used only when there is no unit
        /// </summary>
        public string? Topic { get; set; }

        /// <summary>
        /// How many weeks the plan covers
        /// </summary>
        public int Weeks { get; set; } = 1;

        /// <summary>
        /// Whether the school is on an improvement plan, which adds two sections
        /// </summary>
        public bool SchoolImprovementPlan { get; set; }

        /// <summary>
        /// The plan's school days, as YYYY-MM-DD
        /// </summary>
        public List<string> PlanDays { get; set; } = [];

        /// <summary>
        /// The options of each section, which are the only ones that can be saved
        /// </summary>
        public PlanCatalogs Catalogs { get; set; } = new();
    }

    /// <summary>
    /// The labels of the options each section of the plan offers
    /// </summary>
    public class PlanCatalogs
    {
        public List<string> Subjects { get; set; } = [];
        public List<string> Projects { get; set; } = [];
        public List<string> Materials { get; set; } = [];
        public List<string> ActiveResponses { get; set; } = [];
        public List<CategoryOptions> Accommodations { get; set; } = [];
        public List<CategoryOptions> Differentiation { get; set; } = [];
        public EvaluationOptions Evaluations { get; set; } = new();
    }

    /// <summary>
    /// The assessment types of each category.
    /// </summary>
    public class EvaluationOptions
    {
        public List<string> Diagnostic { get; set; } = [];
        public List<string> Formative { get; set; } = [];
        public List<string> Summative { get; set; } = [];
    }

    /// <summary>
    /// A category and the options inside it.
    /// </summary>
    public class CategoryOptions
    {
        public string Category { get; set; } = string.Empty;
        public List<string> Options { get; set; } = [];
    }
}
