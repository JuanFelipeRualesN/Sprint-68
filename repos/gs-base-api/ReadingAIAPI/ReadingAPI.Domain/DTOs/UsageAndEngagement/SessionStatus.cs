namespace ReadingAPI.ReadingAPI.Domain.DTOs.UsageAndEngagement
{
    public enum SessionStatus
    {
        Active = 0,
        Closed = 1,
        NotFound = 2,
        Unauthorized = 3,
        Error = 4,
        InvalidEventType = 5
    }
}
