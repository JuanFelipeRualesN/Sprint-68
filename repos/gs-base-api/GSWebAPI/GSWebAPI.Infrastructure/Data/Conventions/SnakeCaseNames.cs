using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions
{
    /// <summary>
    /// Gives every column a snake_case name (CustomPlanClientId -> custom_plan_client_id). Configurations do not
    /// set column names; table names are always set explicitly with ToTable.
    /// </summary>
    public static class SnakeCaseNames
    {
        /// <summary>Sets the column name of every property of every entity type to <see cref="ToSnakeCase"/> of its name.</summary>
        /// <param name="modelBuilder">The model builder, after all configurations are applied.</param>
        public static void Apply(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }
            }
        }

        /// <summary>
        /// Converts a PascalCase name to snake_case. An underscore goes before an uppercase letter that follows a
        /// lowercase letter or digit, or that starts a new word after an acronym (GwpURLId -> gwp_url_id).
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>The lowercase snake_case name.</returns>
        public static string ToSnakeCase(string name)
        {
            var builder = new StringBuilder(name.Length + 8);
            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (char.IsUpper(c))
                {
                    var previousIsLowerOrDigit = i > 0 && (char.IsLower(name[i - 1]) || char.IsDigit(name[i - 1]));
                    var nextIsLower = i + 1 < name.Length && char.IsLower(name[i + 1]);
                    var previousIsUpper = i > 0 && char.IsUpper(name[i - 1]);
                    if (i > 0 && (previousIsLowerOrDigit || (previousIsUpper && nextIsLower)))
                    {
                        builder.Append('_');
                    }

                    builder.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}
