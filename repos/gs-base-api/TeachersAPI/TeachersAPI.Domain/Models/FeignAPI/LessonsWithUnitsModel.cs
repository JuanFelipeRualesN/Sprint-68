namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// Mirrors AthenasAPI's LessonWithUnitsModel for deserializing the
    /// lessons-with-units response.
    /// </summary>
    public class LessonWithUnitsModel
    {
        public string LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string Blueprint { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
    }
}
