using System.Net;
using System.Text.Json;
using DataAnalysisAPI.Controllers;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace DataAnalysisAPI.Tests
{
    /// <summary>
    /// The HTTP contract of the usage insights routes: session, role, scope and error responses. The session is validated against a fake UsersAPI.
    /// </summary>
    [Collection(UsageInsightsCollection.Name)]
    public class UsageInsightsControllerTests
    {
        private const string September = "?from=2026-09-01&to=2026-09-30";

        public UsageInsightsControllerTests(UsageInsightsDatabaseFixture fixture)
        {
            _ = fixture;
            UsageInsights.ClearCache();
        }

        private static UsageInsightsController CreateController(string? token, string queryString = "", string? language = null)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Loopback;
            if (token is not null)
            {
                httpContext.Request.Headers["Token"] = token;
            }

            if (language is not null)
            {
                httpContext.Request.Headers["Accept-Language"] = language;
            }

            httpContext.Request.QueryString = new QueryString(queryString);
            return new UsageInsightsController(NullLogger<UsageInsightsController>.Instance)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext },
            };
        }

        private static JsonDocument Json(IActionResult result) => JsonDocument.Parse(((ContentResult)result).Content!);

        private static int Status(IActionResult result) => result switch
        {
            ContentResult content => content.StatusCode ?? 200,
            FileResult => 200,
            OkObjectResult ok => ok.StatusCode ?? 200,
            _ => throw new InvalidOperationException(result.GetType().Name),
        };

        [Fact]
        public void A_request_without_token_is_unauthorized()
        {
            var result = CreateController(null, September).GetUsageInsight("connections-by-school");

            Assert.Equal(401, Status(result));
            Assert.Equal("unauthorized", Json(result).RootElement.GetProperty("ErrorCode").GetString());
            Assert.Equal("Unauthorized. Please sign in and try again.", Json(result).RootElement.GetProperty("error").GetString());
        }

        [Fact]
        public void An_invalid_token_is_unauthorized()
        {
            Assert.Equal(401, Status(CreateController("nobody", September).GetUsageInsight("connections-by-school")));
        }

        [Theory]
        [InlineData(UsageInsightsDatabaseFixture.TeacherToken)]
        [InlineData(UsageInsightsDatabaseFixture.OreAdminWithoutOreToken)]
        public void A_role_without_access_is_forbidden_and_fails_closed(string token)
        {
            var result = CreateController(token, September).GetUsageInsight("connections-by-school");

            Assert.Equal(403, Status(result));
            Assert.Equal("forbidden", Json(result).RootElement.GetProperty("ErrorCode").GetString());
        }

        [Fact]
        public void An_admin_gets_every_school_with_the_references_to_cite()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.AdminToken, September).GetUsageInsight("connections-by-school");
            var root = Json(result).RootElement;

            Assert.Equal(200, Status(result));
            Assert.Equal("application/json", ((ContentResult)result).ContentType);
            Assert.Equal("connections-by-school", root.GetProperty("Query").GetString());
            Assert.Equal("all schools", root.GetProperty("Scope").GetString());
            Assert.Equal(4, root.GetProperty("TotalRows").GetInt32());
            Assert.False(root.GetProperty("HasMoreRows").GetBoolean());
            Assert.Equal("2026-10-05", root.GetProperty("DataUntil").GetString());
            Assert.Equal("2026-09-01", root.GetProperty("Period").GetProperty("From").GetString());
            Assert.Equal("North High", root.GetProperty("Results")[0].GetProperty("Name").GetString());
            Assert.Equal(0, root.GetProperty("Notices").GetArrayLength());
            Assert.True(root.TryGetProperty("GeneratedAt", out _));
            Assert.False(root.TryGetProperty("ErrorCode", out _));
        }

        [Fact]
        public void The_ore_admin_scope_comes_from_the_session_and_cannot_be_changed_by_a_parameter()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.OreAdminToken, September + "&ore=1&oreId=1&scope=all").GetUsageInsight("connections-by-school");
            var root = Json(result).RootElement;

            Assert.Equal(200, Status(result));
            Assert.Equal(1, root.GetProperty("TotalRows").GetInt32());
            Assert.Equal("East Academy", root.GetProperty("Results")[0].GetProperty("Name").GetString());
            Assert.Equal("ORE ORE TWO", root.GetProperty("Scope").GetString());
        }

        [Fact]
        public void A_school_out_of_scope_gets_the_same_answer_as_a_school_that_does_not_exist()
        {
            var outOfScope = CreateController(UsageInsightsDatabaseFixture.OreAdminToken, September + "&school=South High").GetUsageInsight("connections-by-school");
            var missing = CreateController(UsageInsightsDatabaseFixture.OreAdminToken, September + "&school=Nowhere").GetUsageInsight("connections-by-school");

            Assert.Equal(400, Status(outOfScope));
            Assert.Equal("invalid_parameters", Json(outOfScope).RootElement.GetProperty("ErrorCode").GetString());
            Assert.Equal(Json(missing).RootElement.GetProperty("error").GetString()!.Replace("Nowhere", "South High"), Json(outOfScope).RootElement.GetProperty("error").GetString());
        }

        [Fact]
        public void A_director_only_sees_the_school_of_the_session_user()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.DirectorToken, September).GetUsageInsight("connections-by-school");
            var root = Json(result).RootElement;

            Assert.Equal(200, Status(result));
            Assert.Equal(1, root.GetProperty("TotalRows").GetInt32());
            Assert.Equal("North High", root.GetProperty("Results")[0].GetProperty("Name").GetString());
            Assert.Equal("school North High", root.GetProperty("Scope").GetString());
        }

        [Fact]
        public void Invalid_parameters_give_400_with_a_localized_message_and_a_code()
        {
            var english = CreateController(UsageInsightsDatabaseFixture.AdminToken, "?from=2026-09-30&to=2026-09-01").GetUsageInsight("connections-by-school");
            var spanish = CreateController(UsageInsightsDatabaseFixture.AdminToken, "?from=2026-09-30&to=2026-09-01", "es").GetUsageInsight("connections-by-school");
            var missing = CreateController(UsageInsightsDatabaseFixture.AdminToken, "?to=2026-09-01", "es-PR,es;q=0.9").GetUsageInsight("connections-by-school");

            Assert.Equal(400, Status(english));
            Assert.Equal("invalid_parameters", Json(english).RootElement.GetProperty("ErrorCode").GetString());
            Assert.Equal("'from' cannot be later than 'to'.", Json(english).RootElement.GetProperty("error").GetString());
            Assert.Equal("'from' no puede ser posterior a 'to'.", Json(spanish).RootElement.GetProperty("error").GetString());
            Assert.Equal("Falta el parámetro 'from'.", Json(missing).RootElement.GetProperty("error").GetString());
        }

        [Fact]
        public void An_unknown_query_is_404()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.AdminToken, September).GetUsageInsight("conexiones-por-escuela");

            Assert.Equal(404, Status(result));
            Assert.Equal("unknown_query", Json(result).RootElement.GetProperty("ErrorCode").GetString());
        }

        [Fact]
        public void The_data_status_needs_a_session_and_returns_the_last_day_of_each_family()
        {
            var unauthorized = CreateController(null).GetDataStatus();
            var result = CreateController(UsageInsightsDatabaseFixture.AdminToken).GetDataStatus();

            Assert.Equal(401, Status(unauthorized));
            Assert.Equal(200, Status(result));
            var root = JsonSerializer.Serialize(((OkObjectResult)result).Value);
            Assert.Contains("2026-10-02", root);
            Assert.Contains("2026-10-05", root);
        }

        [Fact]
        public void Export_xlsx_returns_a_file_with_the_safe_name()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.AdminToken, September + "&format=xlsx").ExportUsageInsight("connections-by-school", "xlsx");

            var file = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file.ContentType);
            Assert.Equal("connections-by-school_2026-09-01_2026-09-30.xlsx", file.FileDownloadName);
            Assert.True(file.FileContents.Length > 1000);
            Assert.Equal((byte)'P', file.FileContents[0]);   // a zip container
        }

        [Fact]
        public void Export_chart_returns_the_specification()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.AdminToken, September + "&format=chart&limit=2").ExportUsageInsight("connections-by-school", "chart");
            var root = Json(result).RootElement;

            Assert.Equal(200, Status(result));
            Assert.Equal("bar", root.GetProperty("Type").GetString());
            Assert.Equal(2, root.GetProperty("ShownCategories").GetInt32());
            Assert.Equal(3, root.GetProperty("TotalCategories").GetInt32());
        }

        [Fact]
        public void Export_validates_the_format_and_the_query()
        {
            var badFormat = CreateController(UsageInsightsDatabaseFixture.AdminToken, September).ExportUsageInsight("connections-by-school", "pdf");
            var notExportable = CreateController(UsageInsightsDatabaseFixture.AdminToken, "?text=high").ExportUsageInsight("search-schools", "xlsx");
            var unknown = CreateController(UsageInsightsDatabaseFixture.AdminToken, September).ExportUsageInsight("nope", "xlsx");

            Assert.Equal(400, Status(badFormat));
            Assert.Equal("'format' must be 'xlsx' or 'chart'.", Json(badFormat).RootElement.GetProperty("error").GetString());
            Assert.Equal(404, Status(notExportable));
            Assert.Equal(404, Status(unknown));
        }

        [Fact]
        public void Export_applies_the_same_session_rules_and_the_same_scope()
        {
            var noToken = CreateController(null, September).ExportUsageInsight("connections-by-school", "xlsx");
            var teacher = CreateController(UsageInsightsDatabaseFixture.TeacherToken, September).ExportUsageInsight("connections-by-school", "xlsx");
            var oneSchoolChart = CreateController(UsageInsightsDatabaseFixture.OreAdminToken, September + "&format=chart").ExportUsageInsight("connections-by-school", "chart");

            Assert.Equal(401, Status(noToken));
            Assert.Equal(403, Status(teacher));
            Assert.Equal(422, Status(oneSchoolChart));   // ORE 2 only has one school: not enough categories for a chart
            Assert.Equal("chart_unavailable", Json(oneSchoolChart).RootElement.GetProperty("ErrorCode").GetString());
        }

        [Fact]
        public void The_responses_never_contain_the_token()
        {
            var result = CreateController(UsageInsightsDatabaseFixture.AdminToken, September).GetUsageInsight("connections-by-school");

            Assert.DoesNotContain(UsageInsightsDatabaseFixture.AdminToken, ((ContentResult)result).Content);
        }
    }
}
