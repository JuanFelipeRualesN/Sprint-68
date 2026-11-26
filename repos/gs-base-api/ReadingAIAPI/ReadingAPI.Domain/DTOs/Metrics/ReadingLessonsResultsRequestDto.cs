namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// Request for the reading lessons results datatable filters
    /// </summary>
    public class ReadingLessonsResultsRequestDto : UserSessionMetricsRequestDto
    {
        public string? GivenName { get; set; }

        public string? Surname { get; set; }

        public string? Username { get; set; }
    }
}
