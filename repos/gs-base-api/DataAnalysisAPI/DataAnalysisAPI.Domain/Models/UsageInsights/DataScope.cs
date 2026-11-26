namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Which data the caller may see. <see cref="All"/> means every school; OreId restricts to the schools of that ORE (admin-ore);
    /// SchoolId restricts to that school (director). <see cref="None"/> matches no school.
    /// </summary>
    /// <param name="OreId">ORE identifier, or null.</param>
    /// <param name="SchoolId">School identifier, or null.</param>
    public sealed record DataScope(int? OreId, int? SchoolId = null)
    {
        /// <summary>
        /// Scope without restrictions (every school).
        /// </summary>
        public static readonly DataScope All = new((int?)null);

        /// <summary>
        /// Scope that matches no school; used when the caller has no permitted data.
        /// </summary>
        public static readonly DataScope None = new(-1, -1);

        /// <summary>
        /// Gets a value indicating whether the scope has no restriction.
        /// </summary>
        public bool IsAll => OreId is null && SchoolId is null;
    }
}
