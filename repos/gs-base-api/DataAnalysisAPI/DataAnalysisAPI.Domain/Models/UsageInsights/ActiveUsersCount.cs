namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Users with an active subscription and how many of them ever connected.
    /// </summary>
    /// <param name="ActiveUsers">Users with an active subscription.</param>
    /// <param name="EverConnectedUsers">Active users with at least one connection in the whole history.</param>
    public record ActiveUsersCount(int ActiveUsers, int EverConnectedUsers);
}
