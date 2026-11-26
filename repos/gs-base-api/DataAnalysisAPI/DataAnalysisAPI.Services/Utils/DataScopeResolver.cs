using System;
using System.Globalization;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Turns a validated platform session into the data scope of a usage insights query. It fails closed: when in doubt, nothing is returned.
    /// </summary>
    public static class DataScopeResolver
    {
        /// <summary>
        /// Resolves the data scope of a caller.
        /// </summary>
        /// <param name="roleId">Role id of the session.</param>
        /// <param name="oreId">ORE id of the session (required for the ORE roles).</param>
        /// <param name="getUserSchoolId">Reads the school of the user; only called for the school roles.</param>
        /// <param name="allSchoolsRoleIds">Roles that see every school.</param>
        /// <param name="oreRoleIds">Roles that see the schools of their ORE.</param>
        /// <param name="schoolRoleIds">Roles that see only their own school.</param>
        /// <returns>
        /// <see cref="DataScope.All"/> for the all-schools roles; the ORE for the ORE roles; the school for the school roles;
        /// <see cref="DataScope.None"/> for any other role, or when the ORE or the school is missing.
        /// </returns>
        public static DataScope Resolve(string? roleId, string? oreId, Func<int?> getUserSchoolId,
            int[] allSchoolsRoleIds, int[] oreRoleIds, int[] schoolRoleIds)
        {
            if (!int.TryParse(roleId, NumberStyles.None, CultureInfo.InvariantCulture, out var role))
            {
                return DataScope.None;
            }

            if (Array.IndexOf(allSchoolsRoleIds, role) >= 0)
            {
                return DataScope.All;
            }

            if (Array.IndexOf(oreRoleIds, role) >= 0)
            {
                return int.TryParse(oreId, NumberStyles.None, CultureInfo.InvariantCulture, out var ore)
                    ? new DataScope(ore)
                    : DataScope.None;
            }

            if (Array.IndexOf(schoolRoleIds, role) >= 0)
            {
                return getUserSchoolId() is { } schoolId ? new DataScope(null, schoolId) : DataScope.None;
            }

            return DataScope.None;
        }
    }
}
