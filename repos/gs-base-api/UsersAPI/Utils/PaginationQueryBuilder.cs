using System;
using System.Text.RegularExpressions;
using Dapper;
using MySqlConnector;


namespace UsersAPI.Utils
{
    /// <summary>
    /// Carries the result of <see cref="PaginationQueryBuilder.BuildPagedQueryAsync"/>:
    /// the SQL string ready to be executed and the pre-computed total item count (always 0 —
    /// the count is obtained separately via <see cref="PaginationQueryBuilder.BuildCountQuery"/>).
    /// </summary>
    public class PaginationQueryBuilderResponse
    {
        public int TotalItems { get; set; }
        public string Query { get; set; }
    }

    /// <summary>
    /// Utility for applying pagination (<c>LIMIT / OFFSET</c>) and counting rows on raw SQL queries.
    /// Designed for the two-layer subquery injection pattern used across this API, where the inner
    /// subquery pre-filters user IDs and the outer query joins them back to the full user tables.
    /// </summary>
    public class PaginationQueryBuilder
    {
        /// <summary>
        /// Appends <c>LIMIT</c> and <c>OFFSET</c> to <paramref name="baseQuery"/> for the requested page.
        /// Normalizes negative page indices to 0 and a limit of 0 to 50. Values above
        /// <paramref name="maxLimitRows"/> are capped at that maximum.
        /// </summary>
        /// <param name="baseQuery">The base SQL query without a trailing <c>LIMIT</c> clause.</param>
        /// <param name="fromPage">Zero-based page index.</param>
        /// <param name="limitRows">Number of rows per page.</param>
        /// <param name="maxLimitRows">Hard cap on rows per page (default 200).</param>
        /// <returns>
        /// A <see cref="PaginationQueryBuilderResponse"/> with the paginated SQL in <c>Query</c>.
        /// </returns>
        public static PaginationQueryBuilderResponse BuildPagedQueryAsync(
            string baseQuery,
            int fromPage,
            int limitRows,
            int maxLimitRows = 200)
        {

            if (string.IsNullOrWhiteSpace(baseQuery))
                throw new ArgumentException("Query cannot be null or empty.", nameof(baseQuery));


            int normalizedFromPage = fromPage < 0 ? 0 : fromPage;

            int normalizedLimitRows = limitRows <= 0 ? 50 : limitRows;
            if (maxLimitRows > 0 && normalizedLimitRows > maxLimitRows)
                normalizedLimitRows = maxLimitRows;

            long offsetAsLong = (long)normalizedFromPage * (long)normalizedLimitRows;
            int normalizedOffsetRows = offsetAsLong > int.MaxValue ? int.MaxValue : (int)offsetAsLong;


            string normalizedBaseQuery = baseQuery.Trim().TrimEnd(';').Trim();

            PaginationQueryBuilderResponse response = new PaginationQueryBuilderResponse()
            {
                TotalItems = 0,
                Query = string.Empty
            };

            //string queryWithOutDistinct = RemoveTopLevelDistinct(baseQuery);

            response.Query = AddLimitOffset(normalizedBaseQuery, normalizedLimitRows, offsetAsLong);

            return response;


        }

        /// <summary>
        /// Injects <c>LIMIT / OFFSET</c> into <paramref name="query"/>.
        /// For queries using the subquery-IDs pattern (<c>ORDER BY us.user_id ) ids INNER JOIN …</c>
        /// or the admin variant with <c>ro.user_id</c>), the clause is placed inside the subquery
        /// before the closing <c>) ids</c> so only the required ID slice is materialized.
        /// For all other queries, it is appended at the end.
        /// Returns the query unchanged if it already contains a <c>LIMIT</c> clause.
        /// </summary>
        private static string AddLimitOffset(string query, int limitParam, long offsetParam)
        {
            // Avoid double pagination if LIMIT is already present
            bool hasTrailingLimit = Regex.IsMatch(query, @"\blimit\b[\s\S]*$", RegexOptions.IgnoreCase);
            if (hasTrailingLimit) return query;

            // Look for the specific pattern of the subquery-IDs approach and inject LIMIT/OFFSET before the closing ") ids" and JOIN to user_accounts.
            string idsPattern = @"(ORDER\s+BY\s+us\.user_id)(\s*\)\s*ids\s*INNER\s+JOIN\s+`gs_users_db_prod`\.`user_accounts`\s+ac\s+ON\s+ac\.id\s*=\s*ids\.user_id)";

            // Admin variant of the pattern with ro.user_id instead of us.user_id
            string idsPatternAdmin = @"(ORDER\s+BY\s+ro\.user_id)(\s*\)\s*ids\s*INNER\s+JOIN\s+`gs_users_db_prod`\.`user_accounts`\s+ac\s+ON\s+ac\.id\s*=\s*ids\.user_id)";

            // onlyUserInfo=true variant: subquery starts from user_accounts, alias is ids.id instead of ids.user_id
            string idsPatternOnlyUserInfo = @"(ORDER\s+BY\s+ac\.id)(\s*\)\s*ids\s*INNER\s+JOIN\s+`gs_users_db_prod`\.`user_accounts`\s+ac\s+ON\s+ac\.id\s*=\s*ids\.id)";

            if (Regex.IsMatch(query, idsPattern, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(
                    query,
                    idsPattern,
                    $"$1 LIMIT {limitParam} OFFSET {offsetParam}$2",
                    RegexOptions.IgnoreCase
                );
            }

            if (Regex.IsMatch(query, idsPatternAdmin, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(
                    query,
                    idsPatternAdmin,
                    $"$1 LIMIT {limitParam} OFFSET {offsetParam}$2",
                    RegexOptions.IgnoreCase
                );
            }

            if (Regex.IsMatch(query, idsPatternOnlyUserInfo, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(
                    query,
                    idsPatternOnlyUserInfo,
                    $"$1 LIMIT {limitParam} OFFSET {offsetParam}$2",
                    RegexOptions.IgnoreCase
                );
            }


            return $"{query} LIMIT {limitParam} OFFSET {offsetParam}";
        }

        private static string RemoveTopLevelDistinct(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return query;

            return Regex.Replace(
                query.Trim(),
                @"^\s*select\s+distinct\b",
                "SELECT",
                RegexOptions.IgnoreCase
            );
        }

        /// <summary>
        /// Executes a <c>COUNT(DISTINCT ac.id)</c> version of <paramref name="baseQuery"/> and
        /// returns the total number of matching rows. Any trailing <c>LIMIT</c> clause is stripped
        /// before the count is run. Intended to be called with the same subquery string and parameters
        /// used by <see cref="BuildPagedQueryAsync"/> so the count and the list are always consistent.
        /// </summary>
        /// <param name="baseQuery">The inner subquery SQL (without LIMIT) to count rows against.</param>
        /// <param name="parameters">The bound parameters for <paramref name="baseQuery"/>.</param>
        /// <param name="connectionString">MySQL connection string to use for the count query.</param>
        /// <returns>Total number of distinct <c>ac.id</c> rows matching the query, or 0 on error.</returns>
        public static int BuildCountQuery(string baseQuery, DynamicParameters parameters, string connectionString)
        {

            string queryWithoutLimit = RemoveTrailingLimit(baseQuery);

            var selectFromPattern = @"^\s*select\s+[\s\S]*?\s+from\s+";

            var queryWithCount = Regex.Replace(queryWithoutLimit, selectFromPattern, "SELECT COUNT(DISTINCT ac.id) FROM ", RegexOptions.IgnoreCase);

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = connectionString
            };
            connection.Open();

            int totalItems = 0;

            try
            {
                totalItems = connection.QuerySingle<int>(queryWithCount, parameters);

            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }

            return totalItems;
        }

        /// <summary>Removes any trailing <c>LIMIT …</c> clause from <paramref name="query"/>.</summary>
        private static string RemoveTrailingLimit(string query)
        {
            return Regex.Replace(query, @"\blimit\b[\s\S]*$", "", RegexOptions.IgnoreCase).Trim();
        }
    }
}