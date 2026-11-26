namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Participation of one school: users connected in the period over users with an active subscription.
    /// </summary>
    /// <param name="Name">School name.</param>
    /// <param name="ActiveUsers">Users with an active subscription.</param>
    /// <param name="ConnectedUsers">Distinct users with a connection in the period.</param>
    /// <param name="ParticipationPercentage">Connected users divided by active users; null when there are no active users.</param>
    public record SchoolParticipationRow(string Name, int ActiveUsers, int ConnectedUsers, double? ParticipationPercentage);
}
