using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Tests.Infrastructure;
using Xunit;

namespace DataAnalysisAPI.Tests
{
    /// <summary>
    /// Parameter validation: every failure is an <see cref="InvalidQueryParameterException"/> (a 400 in the controller).
    /// </summary>
    public class UsageInsightsQueryParserTests
    {
        private static UsageInsightsQueryParser Parser(string query, bool isExport = false, bool ignoreLimit = false) =>
            new(UsageInsightsTestHelper.Query(query), isExport, ignoreLimit);

        private static void AssertInvalid(Action action, string parameter, string reason)
        {
            var exception = Assert.Throws<InvalidQueryParameterException>(action);
            Assert.Equal(parameter, exception.ParameterName);
            Assert.Equal(reason, exception.ReasonCode);
        }

        [Fact]
        public void A_valid_period_is_parsed()
        {
            var (from, to) = Parser("from=2026-09-01&to=2026-09-30").ParsePeriod();

            Assert.Equal(new DateOnly(2026, 9, 1), from);
            Assert.Equal(new DateOnly(2026, 9, 30), to);
        }

        [Fact]
        public void A_missing_date_is_reported_with_its_name()
        {
            AssertInvalid(() => Parser("to=2026-09-30").ParsePeriod(), "from", "Missing");
            AssertInvalid(() => Parser("from=2026-09-30").ParsePeriod(), "to", "Missing");
            AssertInvalid(() => Parser("fromA=2026-09-01&toA=2026-09-30").ParsePeriod("fromB", "toB"), "fromB", "Missing");
        }

        [Theory]
        [InlineData("from=2026-13-01&to=2026-09-30")]
        [InlineData("from=01/09/2026&to=2026-09-30")]
        [InlineData("from=2026-9-1&to=2026-09-30")]
        [InlineData("from=yesterday&to=2026-09-30")]
        public void A_date_with_the_wrong_format_is_rejected(string query)
        {
            AssertInvalid(() => Parser(query).ParsePeriod(), "from", "DateFormat");
        }

        [Fact]
        public void A_period_that_ends_before_it_starts_is_rejected()
        {
            AssertInvalid(() => Parser("from=2026-10-01&to=2026-09-30").ParsePeriod(), "from", "DateOrder");
        }

        [Fact]
        public void Order_and_limit_have_defaults()
        {
            Assert.Equal((true, 10), Parser(string.Empty).ParseOrderAndLimit());
            Assert.Equal((false, 25), Parser("order=asc&limit=25").ParseOrderAndLimit());
        }

        [Theory]
        [InlineData("order=sideways", "order", "OrderValue")]
        [InlineData("limit=abc", "limit", "NotInteger")]
        [InlineData("limit=0", "limit", "LimitRange")]
        [InlineData("limit=51", "limit", "LimitRange")]
        [InlineData("limit=-1", "limit", "LimitRange")]
        public void An_invalid_order_or_limit_is_rejected(string query, string parameter, string reason)
        {
            AssertInvalid(() => Parser(query).ParseOrderAndLimit(), parameter, reason);
        }

        [Fact]
        public void The_export_accepts_up_to_5000_rows_and_returns_all_of_them_without_a_limit()
        {
            Assert.Equal((true, 5000), Parser(string.Empty, isExport: true).ParseOrderAndLimit());
            Assert.Equal((true, 5000), Parser("limit=5000", isExport: true).ParseOrderAndLimit());
            AssertInvalid(() => Parser("limit=5001", isExport: true).ParseOrderAndLimit(), "limit", "LimitRange");
        }

        [Fact]
        public void In_a_chart_export_the_limit_belongs_to_the_chart()
        {
            Assert.Equal((true, 5000), Parser("limit=3", isExport: true, ignoreLimit: true).ParseOrderAndLimit());
            Assert.Equal(3, Parser("limit=3", isExport: true, ignoreLimit: true).ParseChartOptions().Limit);
        }

        [Fact]
        public void Chart_options_have_defaults_and_are_validated()
        {
            Assert.Equal(new ChartOptions(), Parser(string.Empty).ParseChartOptions());
            Assert.Equal(new ChartOptions(true, "asc", true, 4), Parser("all=true&order=asc&includeZeros=true&limit=4").ParseChartOptions());
            AssertInvalid(() => Parser("all=maybe").ParseChartOptions(), "all", "NotBoolean");
            AssertInvalid(() => Parser("limit=5001").ParseChartOptions(), "limit", "LimitRange");
            AssertInvalid(() => Parser("order=up").ParseChartOptions(), "order", "OrderValue");
        }

        [Fact]
        public void The_connections_range_is_validated()
        {
            Assert.Equal((0, int.MaxValue), Parser(string.Empty).ParseConnectionsRange());
            Assert.Equal((1, 1), Parser("minConnections=1&maxConnections=1").ParseConnectionsRange());
            AssertInvalid(() => Parser("minConnections=-1").ParseConnectionsRange(), "minConnections", "NonNegativeInteger");
            AssertInvalid(() => Parser("maxConnections=x").ParseConnectionsRange(), "maxConnections", "NonNegativeInteger");
            AssertInvalid(() => Parser("minConnections=5&maxConnections=2").ParseConnectionsRange(), "minConnections", "MinGreaterThanMax");
        }

        [Fact]
        public void A_choice_must_be_one_of_the_allowed_values()
        {
            Assert.Equal("month", Parser(string.Empty).ParseChoice("granularity", "month", "month", "quarter", "year"));
            Assert.Equal("year", Parser("granularity=year").ParseChoice("granularity", "month", "month", "quarter", "year"));
            var exception = Assert.Throws<InvalidQueryParameterException>(() => Parser("granularity=dia").ParseChoice("granularity", "month", "month", "quarter", "year"));
            Assert.Equal("AllowedValues", exception.ReasonCode);
            Assert.Equal("month, quarter, year", exception.Detail);
        }

        [Fact]
        public void The_old_spanish_values_are_not_accepted()
        {
            AssertInvalid(() => Parser("granularity=mes").ParseChoice("granularity", "month", "month", "quarter", "year"), "granularity", "AllowedValues");
            AssertInvalid(() => Parser("groupBy=escuela").ParseChoice("groupBy", "total", "total", "school", "role"), "groupBy", "AllowedValues");
        }

        [Fact]
        public void An_unknown_school_is_rejected_and_an_empty_one_is_ignored()
        {
            AssertInvalid(() => Parser("school=Nowhere").ResolveSchool(_ => false), "school", "UnknownSchool");
            Assert.Null(Parser(string.Empty).ResolveSchool(_ => false));
            Assert.Equal("North High", Parser("school=North High").ResolveSchool(_ => true));
        }

        [Fact]
        public void Percentages_have_one_decimal_and_no_division_by_zero()
        {
            Assert.Equal(33.3, UsageInsightsQueryParser.Percentage(1, 3));
            Assert.Null(UsageInsightsQueryParser.Percentage(5, 0));
            Assert.Equal(new DateOnly(2026, 2, 1), UsageInsightsQueryParser.MaxDate(new DateOnly(2026, 1, 1), new DateOnly(2026, 2, 1)));
        }
    }
}
