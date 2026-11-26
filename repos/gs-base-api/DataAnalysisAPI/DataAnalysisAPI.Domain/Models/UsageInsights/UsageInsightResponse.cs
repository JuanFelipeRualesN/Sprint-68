using System;
using System.Collections.Generic;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// Common response of every usage insights query. It carries the references the assistant must cite: which query ran, over which date column,
    /// the period and filters applied, how far the loaded data goes, how many rows came out and when it was generated.
    /// </summary>
    /// <param name="Query">Query name (kebab-case route name).</param>
    /// <param name="Source">Data source.</param>
    /// <param name="DateColumn">Column the period was filtered on.</param>
    /// <param name="Period">Period (From/To) or two periods (A/B).</param>
    /// <param name="Filters">Filters applied.</param>
    /// <param name="DataUntil">Last day with loaded data.</param>
    /// <param name="Results">Result rows.</param>
    /// <param name="Note">How to read the figures (localized).</param>
    /// <param name="Summary">Totals and highlights of the whole result.</param>
    public record UsageInsightResponse(string Query, string Source, string DateColumn, object Period, object Filters, DateOnly? DataUntil,
        object Results, string? Note = null, object? Summary = null)
    {
        /// <summary>
        /// Gets the data the caller was allowed to see: all schools, an ORE or a school (localized).
        /// </summary>
        public string? Scope { get; init; }

        /// <summary>
        /// Gets a value indicating whether the result was cut and the list, totals and percentages are not complete.
        /// </summary>
        public bool HasMoreRows { get; init; }

        /// <summary>
        /// Gets the localized notices the caller must not hide (data loaded only until a date, ties, truncated lists).
        /// </summary>
        public IReadOnlyList<string> Notices { get; init; } = new List<string>();

        /// <summary>
        /// Gets the number of result rows.
        /// </summary>
        public int TotalRows => Results is System.Collections.ICollection collection ? collection.Count : 0;

        /// <summary>
        /// Gets the UTC instant when the response was generated.
        /// </summary>
        public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
    }
}
