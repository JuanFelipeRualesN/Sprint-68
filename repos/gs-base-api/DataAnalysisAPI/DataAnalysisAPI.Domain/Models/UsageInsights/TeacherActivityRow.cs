namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Teacher activity of one school in one academic period (distinct teachers).
    /// </summary>
    /// <param name="School">School name.</param>
    /// <param name="PeriodFrom">Academic period start (yyyy-MM-dd).</param>
    /// <param name="PeriodTo">Academic period end (yyyy-MM-dd).</param>
    /// <param name="Teachers">Distinct teachers.</param>
    /// <param name="WithConnections">Teachers who connected within the period.</param>
    /// <param name="WithPlans">Teachers who created instructional plans in the period.</param>
    /// <param name="PlansWithData">Teachers whose plans have quizzes or lessons.</param>
    /// <param name="WithGroups">Teachers who created groups in the academic year.</param>
    /// <param name="WithAssignments">Teachers who ever created an assignment.</param>
    public record TeacherActivityRow(string School, string PeriodFrom, string PeriodTo, int Teachers, int WithConnections, int WithPlans, int PlansWithData, int WithGroups, int WithAssignments);
}
