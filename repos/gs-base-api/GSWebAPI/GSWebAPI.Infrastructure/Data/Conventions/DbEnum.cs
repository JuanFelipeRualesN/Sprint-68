using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions
{
    /// <summary>
    /// Maps a C# enum to a native MySQL ENUM column. The DB spelling of each member comes from its
    /// <see cref="EnumMemberAttribute"/> (lowercase snake_case), and the ENUM order is the declaration order.
    /// </summary>
    /// <typeparam name="TEnum">The enum type; every member needs an <see cref="EnumMemberAttribute"/>.</typeparam>
    public static class DbEnum<TEnum>
        where TEnum : struct, Enum
    {
        private static readonly IReadOnlyList<(TEnum Value, string DbValue)> Members = typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => (
                (TEnum)field.GetValue(null)!,
                field.GetCustomAttribute<EnumMemberAttribute>()?.Value
                    ?? throw new InvalidOperationException($"{typeof(TEnum).Name}.{field.Name} needs [EnumMember(Value = ...)].")))
            .ToList();

        private static readonly Dictionary<TEnum, string> ToDb = Members.ToDictionary(m => m.Value, m => m.DbValue);

        private static readonly Dictionary<string, TEnum> FromDb = Members.ToDictionary(m => m.DbValue, m => m.Value);

        /// <summary>The column type, for example <c>enum('draft','active')</c>.</summary>
        public static string ColumnType { get; } = "enum(" + string.Join(",", Members.Select(m => $"'{m.DbValue}'")) + ")";

        /// <summary>The DB spellings of all members, in declaration order.</summary>
        public static IEnumerable<string> DbValues => Members.Select(m => m.DbValue);

        /// <summary>Returns the DB spelling of a member.</summary>
        /// <param name="value">A declared member of <typeparamref name="TEnum"/>.</param>
        /// <returns>The <see cref="EnumMemberAttribute"/> value of the member.</returns>
        /// <exception cref="KeyNotFoundException">The value is not a declared member.</exception>
        public static string ToDbValue(TEnum value) => ToDb[value];

        /// <summary>Returns the member for a DB spelling (case-sensitive).</summary>
        /// <param name="value">The value read from the column.</param>
        /// <returns>The matching member.</returns>
        /// <exception cref="KeyNotFoundException">The value is not a known spelling.</exception>
        public static TEnum FromDbValue(string value) => FromDb[value];
    }

    /// <summary>Value converter between <typeparamref name="TEnum"/> and its MySQL ENUM spelling.</summary>
    /// <typeparam name="TEnum">The enum type, mapped through <see cref="DbEnum{TEnum}"/>.</typeparam>
    public sealed class DbEnumConverter<TEnum> : ValueConverter<TEnum, string>
        where TEnum : struct, Enum
    {
        /// <summary>Creates the converter using <see cref="DbEnum{TEnum}.ToDbValue"/> and <see cref="DbEnum{TEnum}.FromDbValue"/>.</summary>
        public DbEnumConverter()
            : base(v => DbEnum<TEnum>.ToDbValue(v), v => DbEnum<TEnum>.FromDbValue(v))
        {
        }
    }

    /// <summary>Property-builder helpers for native MySQL ENUM columns.</summary>
    public static class DbEnumPropertyBuilderExtensions
    {
        /// <summary>Stores the property as a native MySQL ENUM with the snake_case values of <typeparamref name="TEnum"/>.</summary>
        /// <typeparam name="TEnum">The enum type of the property.</typeparam>
        /// <param name="builder">The property builder.</param>
        /// <returns>The same builder, for chaining.</returns>
        public static PropertyBuilder<TEnum> IsDbEnum<TEnum>(this PropertyBuilder<TEnum> builder)
            where TEnum : struct, Enum
        {
            return builder
                .HasConversion(new DbEnumConverter<TEnum>())
                .HasColumnType(DbEnum<TEnum>.ColumnType);
        }
    }
}
