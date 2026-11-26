namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Options of a chart specification.
    /// </summary>
    /// <param name="All">False = preview (first Limit categories); true = every category.</param>
    /// <param name="Order">"desc" (largest first, default) or "asc"; only used by bar charts.</param>
    /// <param name="IncludeZeros">False (default) = leave out categories whose values are all 0 or null.</param>
    /// <param name="Limit">Number of categories of the preview.</param>
    public record ChartOptions(bool All = false, string Order = "desc", bool IncludeZeros = false, int Limit = ChartOptions.PreviewLimit)
    {
        /// <summary>
        /// Default number of categories of a preview.
        /// </summary>
        public const int PreviewLimit = 10;
    }
}
