using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using Xunit;

namespace DataAnalysisAPI.Tests
{
    /// <summary>
    /// The scope resolver is pure: it only maps role, ORE and school to a data scope, and it fails closed.
    /// </summary>
    public class DataScopeResolverTests
    {
        private static readonly int[] AllSchoolsRoles = { 2 };
        private static readonly int[] OreRoles = { 8 };
        private static readonly int[] SchoolRoles = { 6 };

        private static DataScope Resolve(string? roleId, string? oreId, int? schoolId = null) =>
            DataScopeResolver.Resolve(roleId, oreId, () => schoolId, AllSchoolsRoles, OreRoles, SchoolRoles);

        [Fact]
        public void Admin_sees_every_school()
        {
            var scope = Resolve("2", null);

            Assert.True(scope.IsAll);
            Assert.Equal(DataScope.All, scope);
        }

        [Fact]
        public void Admin_ore_is_restricted_to_its_ore()
        {
            var scope = Resolve("8", "7");

            Assert.Equal(new DataScope(7), scope);
            Assert.False(scope.IsAll);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("-3")]
        public void Admin_ore_without_a_valid_ore_gets_no_data(string? oreId)
        {
            Assert.Equal(DataScope.None, Resolve("8", oreId));
        }

        [Fact]
        public void Director_is_restricted_to_its_school()
        {
            Assert.Equal(new DataScope(null, 12), Resolve("6", null, 12));
        }

        [Fact]
        public void Director_without_a_school_gets_no_data()
        {
            Assert.Equal(DataScope.None, Resolve("6", null, null));
        }

        [Fact]
        public void The_school_is_only_read_for_the_school_roles()
        {
            var calls = 0;
            DataScopeResolver.Resolve("2", null, () => { calls++; return 1; }, AllSchoolsRoles, OreRoles, SchoolRoles);
            DataScopeResolver.Resolve("8", "1", () => { calls++; return 1; }, AllSchoolsRoles, OreRoles, SchoolRoles);

            Assert.Equal(0, calls);
        }

        [Theory]
        [InlineData("5")]
        [InlineData("3")]
        [InlineData("1")]
        [InlineData("7")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("not-a-number")]
        public void Any_other_role_gets_no_data(string? roleId)
        {
            Assert.Equal(DataScope.None, Resolve(roleId, "1", 1));
        }

        [Fact]
        public void The_role_lists_come_from_the_configuration()
        {
            var scope = DataScopeResolver.Resolve("9", null, () => null, new[] { 9, 2 }, OreRoles, SchoolRoles);

            Assert.True(scope.IsAll);
        }

        [Fact]
        public void None_matches_no_school_and_is_not_all()
        {
            Assert.False(DataScope.None.IsAll);
            Assert.Equal(-1, DataScope.None.OreId);
            Assert.Equal(-1, DataScope.None.SchoolId);
        }
    }
}
