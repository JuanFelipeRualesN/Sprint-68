using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;

namespace DataAnalysisAPI.Utils
{
    /// <summary>
    /// Reads the usage insights configuration (AppSettings:usage_insights_*). None of these keys is a secret.
    /// </summary>
    public static class UsageInsightsSettings
    {
        /// <summary>
        /// Name of the permission the caller session is validated against.
        /// </summary>
        public const string PermissionName = "embed-powerbi-info";

        /// <summary>
        /// Name of the data source quoted in every response.
        /// </summary>
        public const string SourceName = "mysql:gs_data_analysis";

        /// <summary>
        /// Gets the role ids that see every school.
        /// </summary>
        public static int[] GetAllSchoolsRoleIds() => ReadIntList("usage_insights_all_schools_role_ids", "2");

        /// <summary>
        /// Gets the role ids that see only the schools of their ORE.
        /// </summary>
        public static int[] GetOreRoleIds() => ReadIntList("usage_insights_ore_role_ids", "8");

        /// <summary>
        /// Gets the role ids that see only their own school.
        /// </summary>
        public static int[] GetSchoolRoleIds() => ReadIntList("usage_insights_school_role_ids", "6");

        /// <summary>
        /// Builds the query context for a caller scope with the configured rules.
        /// </summary>
        /// <param name="scope">The data the caller may see.</param>
        /// <returns>Returns the query context.</returns>
        public static UsageInsightsQueryContext BuildQueryContext(DataScope scope)
        {
            var exclusion = new TestAccountExclusion(
                !string.Equals(Read("usage_insights_exclude_test_accounts"), "false", StringComparison.OrdinalIgnoreCase),
                ReadList("usage_insights_excluded_ores", "DEMOSTRACION"),
                ReadList("usage_insights_excluded_email_domains", string.Empty),
                ReadList("usage_insights_excluded_schools", "Escuela Facilitadores MSJ"));

            return new UsageInsightsQueryContext(
                scope,
                exclusion,
                ReadInt("usage_insights_utc_offset_minutes", -240),
                ReadInt("usage_insights_command_timeout_seconds", 30),
                ReadColumnName("usage_insights_subscription_status_column", "status"),
                ReadColumnName("usage_insights_subscription_cancellation_column", "cancellation_date"));
        }

        /// <summary>
        /// Reads a column name from the configuration. It goes inside the SQL, so it must be a plain identifier (letters, digits and underscore,
        /// not starting with a digit, at most 64 characters); anything else is ignored and the default is used.
        /// </summary>
        private static string ReadColumnName(string key, string defaultName) =>
            Read(key) is { } value && ColumnNamePattern.IsMatch(value) ? value : defaultName;

        private static readonly Regex ColumnNamePattern = new("^[A-Za-z_][A-Za-z0-9_]{0,63}$", RegexOptions.Compiled);

        private static string? Read(string key) => AppConfiguration.Configuration["AppSettings:" + key];

        private static int ReadInt(string key, int defaultValue) =>
            int.TryParse(Read(key), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var value) ? value : defaultValue;

        private static string[] ReadList(string key, string defaultValue) =>
            (Read(key) ?? defaultValue).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        private static int[] ReadIntList(string key, string defaultValue) =>
            ReadList(key, defaultValue)
                .Select(item => int.TryParse(item, NumberStyles.None, CultureInfo.InvariantCulture, out var id) ? id : -1)
                .Where(id => id >= 0)
                .ToArray();
    }
}
