using GSAIServicesAPI.Models.DTOs.Plannings;

namespace GSAIServicesAPI.Services.Plannings
{
    /// <summary>
    /// The plannings of a unit, or of one of its weeks, that a plan is generated from
    /// </summary>
    public class UnitPlanning
    {
        /// <summary>
        /// The summary of each week
        /// </summary>
        public UnitPreviewDto Preview { get; set; } = new();

        /// <summary>
        /// The days of the weeks, in order
        /// </summary>
        public List<PlanningDay> Days { get; set; } = [];
    }

    /// <summary>
    /// A day of a planning week.
    /// </summary>
    public class PlanningDay
    {
        /// <summary>
        /// The week number inside the unit
        /// </summary>
        public int WeekNumber { get; set; }

        /// <summary>
        /// The day's name
        /// </summary>
        public string Day { get; set; } = string.Empty;

        /// <summary>
        /// The title of the day's lesson
        /// </summary>
        public string LessonTitle { get; set; } = string.Empty;

        /// <summary>
        /// The day's sections by label
        /// </summary>
        public Dictionary<string, List<string>> Sections { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
