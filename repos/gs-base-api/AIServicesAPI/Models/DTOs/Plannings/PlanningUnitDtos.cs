namespace GSAIServicesAPI.Models.DTOs.Plannings
{
    /// <summary>
    /// A curriculum unit with plannings in the index.
    /// </summary>
    public class CurriculumUnitDto
    {
        /// <summary>
        /// The unit's id in Athenas
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The unit's name (e.g. "4.3")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The unit's description
        /// </summary>
        public string Desc { get; set; } = string.Empty;

        /// <summary>
        /// The subject's code of the unit, which may differ from the requested one after a redirect
        /// </summary>
        public string SubjectCode { get; set; } = string.Empty;

        /// <summary>
        /// The level's code of the unit
        /// </summary>
        public string LevelCode { get; set; } = string.Empty;

        /// <summary>
        /// The unit's duration in Athenas
        /// </summary>
        public string Duration { get; set; } = string.Empty;

        /// <summary>
        /// The week numbers with plannings, in order. Some units have gaps
        /// </summary>
        public List<int> Weeks { get; set; } = [];
    }

    /// <summary>
    /// What the plannings of a unit contain, to show the teacher what the plan will be based on.
    /// </summary>
    public class UnitPreviewDto
    {
        /// <summary>
        /// The unit's id in Athenas
        /// </summary>
        public string UnitId { get; set; } = string.Empty;

        /// <summary>
        /// The unit's name in Athenas (e.g. "4.3").
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The unit's description in Athenas
        /// </summary>
        public string Desc { get; set; } = string.Empty;

        /// <summary>
        /// The unit as the plannings name it
        /// </summary>
        public string PlanningUnit { get; set; } = string.Empty;

        /// <summary>
        /// The weeks included in the preview
        /// </summary>
        public List<WeekPreviewDto> Weeks { get; set; } = [];
    }

    /// <summary>
    /// The summary of a planning week.
    /// </summary>
    public class WeekPreviewDto
    {
        /// <summary>
        /// The week number inside the unit
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The week code (e.g. "4.3.1")
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The week's theme
        /// </summary>
        public string Theme { get; set; } = string.Empty;

        /// <summary>
        /// The cross-curricular themes
        /// </summary>
        public List<string> TransversalThemes { get; set; } = [];

        /// <summary>
        /// The generating themes
        /// </summary>
        public List<string> GeneratedThemes { get; set; } = [];

        /// <summary>
        /// The objectives of the week, without the introductory phrases
        /// </summary>
        public List<string> Objectives { get; set; } = [];

        /// <summary>
        /// The standards of the week
        /// </summary>
        public List<string> Standards { get; set; } = [];

        /// <summary>
        /// The expectation codes of the week
        /// </summary>
        public List<string> Expectations { get; set; } = [];

        /// <summary>
        /// The materials of the week's days, without repeating them
        /// </summary>
        public List<string> Materials { get; set; } = [];

        /// <summary>
        /// The integrations with other subjects of the week's days
        /// </summary>
        public List<string> Integrations { get; set; } = [];

        /// <summary>
        /// The evaluations of the week's days
        /// </summary>
        public List<string> Evaluations { get; set; } = [];

        /// <summary>
        /// The differentiated instruction strategies of the week's days. Few plannings have them
        /// </summary>
        public List<string> DifferentiatedInstruction { get; set; } = [];

        /// <summary>
        /// The innovative initiatives or projects of the week's days. Few plannings have them
        /// </summary>
        public List<string> Initiatives { get; set; } = [];

        /// <summary>
        /// The reasonable accommodations of the week's days. Few plannings have them
        /// </summary>
        public List<string> Accommodations { get; set; } = [];

        /// <summary>
        /// The days of the week with their lesson and activities in order
        /// </summary>
        public List<DayPreviewDto> Days { get; set; } = [];
    }

    /// <summary>
    /// The lesson of a planning day.
    /// </summary>
    public class DayPreviewDto
    {
        /// <summary>
        /// The day's name
        /// </summary>
        public string Day { get; set; } = string.Empty;

        /// <summary>
        /// The title of the day's lesson
        /// </summary>
        public string LessonTitle { get; set; } = string.Empty;

        /// <summary>
        /// The introductory activity
        /// </summary>
        public List<string> InitialActivity { get; set; } = [];

        /// <summary>
        /// The development activity
        /// </summary>
        public List<string> DevelopmentActivity { get; set; } = [];

        /// <summary>
        /// The closing activity
        /// </summary>
        public List<string> ClosingActivity { get; set; } = [];
    }
}
