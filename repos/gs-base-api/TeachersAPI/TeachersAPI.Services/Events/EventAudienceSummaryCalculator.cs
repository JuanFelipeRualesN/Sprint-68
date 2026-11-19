using System.Collections.Generic;
using System.Linq;
using TeachersAPI.Domain.Models.Events;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Calculates a short summary of who an event targets (EventModel.AudienceSummary), as a
    /// comma-separated list of language-neutral codes — not display text. The frontend (the only
    /// consumer) translates each code and joins them for display, so this stays usable regardless
    /// of the UI's selected language. This is a "who, at a glance" indicator, not a description of
    /// every segmentation criterion used to build the audience — it's a strict precedence cascade
    /// (Roles > Schools > Regions > Users), not a combination of all non-empty categories. Never
    /// resolves names from UsersAPI.
    /// </summary>
    public static class EventAudienceSummaryCalculator
    {
        private static readonly (string RoleId, string Code)[] KnownRoles =
        {
            (EventAudienceRoles.Director, "director"),
            (EventAudienceRoles.Teacher, "teacher"),
            (EventAudienceRoles.Student, "student"),
        };

        private const string UnknownRoleCode = "other";
        private const string CodeSeparator = ",";

        /// <summary>
        /// Builds the audience summary as language-neutral codes.
        /// </summary>
        /// <param name="roleIds">RoleId values found in event_roles for this event (known or not).</param>
        /// <param name="hasSchools">Whether the event has at least one row in event_schools.</param>
        /// <param name="hasRegions">Whether the event has at least one row in event_regions.</param>
        /// <param name="hasUsers">Whether the event has at least one row in event_users.</param>
        /// <returns>A comma-separated list of codes, e.g. "director,teacher" or "none".</returns>
        public static string Calculate(IEnumerable<string> roleIds, bool hasSchools, bool hasRegions, bool hasUsers)
        {
            List<string> distinctRoleIds = (roleIds ?? Enumerable.Empty<string>()).Distinct().ToList();

            List<string> roleCodes = KnownRoles
                .Where(known => distinctRoleIds.Contains(known.RoleId))
                .Select(known => known.Code)
                .ToList();

            bool hasUnrecognizedRole = distinctRoleIds.Except(KnownRoles.Select(known => known.RoleId)).Any();
            if (hasUnrecognizedRole)
            {
                // Never hide that the event does have role-based targeting just because it falls
                // outside Director/Teacher/Student — those will be handled in the Teams phase.
                roleCodes.Add(UnknownRoleCode);
            }

            // Strict precedence: as soon as one category applies, stop — never combine categories.
            if (roleCodes.Count > 0)
            {
                return string.Join(CodeSeparator, roleCodes);
            }

            if (hasSchools)
            {
                return "schools";
            }

            if (hasRegions)
            {
                return "regions";
            }

            if (hasUsers)
            {
                return "specific-users";
            }

            return "none";
        }
    }
}
