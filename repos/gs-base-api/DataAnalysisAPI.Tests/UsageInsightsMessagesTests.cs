using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Tests.Infrastructure;
using DataAnalysisAPI.Utils;
using Xunit;

namespace DataAnalysisAPI.Tests
{
    /// <summary>
    /// The localized texts and the settings that feed the usage insights.
    /// </summary>
    [Collection(UsageInsightsCollection.Name)]
    public class UsageInsightsMessagesTests
    {
        public UsageInsightsMessagesTests(UsageInsightsDatabaseFixture fixture)
        {
            _ = fixture;
        }

        [Fact]
        public void Every_message_exists_in_english_and_spanish_with_the_same_placeholders()
        {
            using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "appsettings.dataanalysis.json")));
            var messages = document.RootElement.GetProperty("Messages");
            var count = 0;

            foreach (var message in messages.EnumerateObject())
            {
                var english = message.Value.GetProperty("en").GetString();
                var spanish = message.Value.GetProperty("es").GetString();
                Assert.False(string.IsNullOrWhiteSpace(english), message.Name + " (en)");
                Assert.False(string.IsNullOrWhiteSpace(spanish), message.Name + " (es)");
                Assert.Equal(Placeholders(english!), Placeholders(spanish!));
                count++;
            }

            Assert.True(count > 150);
        }

        private static string Placeholders(string text) =>
            string.Join(",", Regex.Matches(text, @"\{\d+\}").Select(match => match.Value).Distinct().OrderBy(value => value));

        [Fact]
        public void The_catalog_covers_every_key_the_source_asks_for()
        {
            var sourceFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "DataAnalysisAPI"));
            if (!Directory.Exists(sourceFolder))
            {
                return;   // the sources are not next to the binaries (packaged run)
            }

            var keys = Directory.EnumerateFiles(sourceFolder, "*.cs", SearchOption.AllDirectories)
                .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") && !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"))
                .SelectMany(path => Regex.Matches(File.ReadAllText(path), "(?<!cref=)\"(UsageInsights[A-Za-z0-9]+)\"(?!\\s*/>)").Select(match => match.Groups[1].Value))
                .Where(key => key != "UsageInsightsController" && key != "UsageInsightsColumn" && key != "UsageInsightsReason" && key != "UsageInsightsChartTitle" && key != "UsageInsightsChartAxis"
                    && key != "UsageInsightsWeekday" && key != "UsageInsightsGranularity" && key != "UsageInsightsChartTitleUsersBySubscription")
                .Distinct()
                .ToList();

            Assert.True(keys.Count > 50);
            foreach (var key in keys)
            {
                Assert.NotEqual(key, UsageInsightsMessages.Get("en", key));
                Assert.NotEqual(key, UsageInsightsMessages.Get("es", key));
            }
        }

        [Theory]
        [InlineData("es", "es")]
        [InlineData("es-PR", "es")]
        [InlineData("es-PR,es;q=0.9,en;q=0.8", "es")]
        [InlineData("ES", "es")]
        [InlineData("en", "en")]
        [InlineData("en-US", "en")]
        [InlineData("fr", "en")]
        [InlineData("", "en")]
        [InlineData(null, "en")]
        public void The_language_is_reduced_to_a_supported_code(string? raw, string expected)
        {
            Assert.Equal(expected, UsageInsightsMessages.NormalizeLanguage(raw));
        }

        [Fact]
        public void An_unknown_key_comes_back_as_the_key_and_a_missing_language_falls_back_to_english()
        {
            Assert.Equal("UsageInsightsNoSuchKey", UsageInsightsMessages.Get("en", "UsageInsightsNoSuchKey"));
            Assert.Equal("all schools", UsageInsightsMessages.Get("fr", "UsageInsightsScopeAllSchools"));
            Assert.Equal("ORE X", UsageInsightsMessages.Get("en", "UsageInsightsScopeOre", "X"));
        }

        [Fact]
        public void Join_skips_the_empty_parts()
        {
            Assert.Equal("a b", UsageInsightsMessages.Join("a", null, "", "b"));
            Assert.Null(UsageInsightsMessages.Join(null, ""));
        }

        [Theory]
        [InlineData("status_v2", "status_v2")]
        [InlineData("_private", "_private")]
        [InlineData("status; DROP TABLE user", "status")]
        [InlineData("status`", "status")]
        [InlineData("1status", "status")]
        [InlineData("a-b", "status")]
        [InlineData("", "status")]
        public void A_column_name_of_the_configuration_must_be_a_plain_identifier_or_the_default_is_used(string configured, string expected)
        {
            var original = AppConfiguration.Configuration;
            try
            {
                AppConfiguration.Initialize(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["AppSettings:usage_insights_subscription_status_column"] = configured,
                    ["AppSettings:usage_insights_subscription_cancellation_column"] = new string('a', 65),
                }).Build());

                var context = UsageInsightsSettings.BuildQueryContext(DataScope.All);

                Assert.Equal(expected, context.SubscriptionStatusColumn);
                Assert.Equal("cancellation_date", context.SubscriptionCancellationColumn);   // 65 characters: too long
            }
            finally
            {
                AppConfiguration.Initialize(original);
            }
        }

        [Fact]
        public void The_settings_are_read_from_the_configuration_with_safe_defaults()
        {
            var context = UsageInsightsSettings.BuildQueryContext(DataScope.All);

            Assert.True(context.Exclusion.IsActive);
            Assert.Equal(new[] { "DEMOSTRACION" }, context.Exclusion.Ores);
            Assert.Equal(new[] { "internal.example" }, context.Exclusion.EmailDomains);
            Assert.Equal(new[] { "Facilitators School" }, context.Exclusion.Schools);
            Assert.Equal(-240, context.UtcOffsetMinutes);
            Assert.Equal(30, context.CommandTimeoutSeconds);
            Assert.Equal("status", context.SubscriptionStatusColumn);
            Assert.Equal("cancellation_date", context.SubscriptionCancellationColumn);
            Assert.Equal(new[] { 2 }, UsageInsightsSettings.GetAllSchoolsRoleIds());
            Assert.Equal(new[] { 8 }, UsageInsightsSettings.GetOreRoleIds());
            Assert.Equal(new[] { 6 }, UsageInsightsSettings.GetSchoolRoleIds());
            Assert.Equal("embed-powerbi-info", UsageInsightsSettings.PermissionName);
        }
    }
}
