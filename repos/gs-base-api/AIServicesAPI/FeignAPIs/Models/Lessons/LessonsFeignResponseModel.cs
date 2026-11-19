namespace GSAIServicesAPI.FeignAPIs.Models.Lessons
{
    /// <summary>
    /// Top-level response wrapper returned by the Lessons Feign client.
    /// </summary>
    public class LessonsFeignResponseModel
    {
        /// <summary>Full lesson details including nested sub-models.</summary>
        public LessonsResponseModel LessonsDetails { get; set; }
    }
}
