namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Full Quiz average. School and Subject are null when the result is not grouped by that axis.
    /// </summary>
    /// <param name="School">School name.</param>
    /// <param name="Subject">Subject name.</param>
    /// <param name="AssignedTests">Tests assigned in the period.</param>
    /// <param name="CompletedTests">Tests already taken.</param>
    /// <param name="AverageCompletedOnly">Average of the tests already taken.</param>
    /// <param name="AverageAllAssigned">Average of all assigned tests, where tests not taken count as 0.</param>
    public record FullQuizAverageRow(string? School, string? Subject, int AssignedTests, int CompletedTests, double? AverageCompletedOnly, double AverageAllAssigned);
}
