namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Paginated collection of lessons with full modifier details.
    /// </summary>
    public class LessonsResponseModel
    {
        /// <summary>The list of lessons, each with their associated sub-models.</summary>
        public List<LessonModifierRequestModel> Lessons { get; set; }

        /// <summary>Total number of lessons available (before pagination).</summary>
        public int TotalResults { get; set; }
    }
}
