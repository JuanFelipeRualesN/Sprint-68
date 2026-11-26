using System;
using System.Threading.Tasks;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.FeignAPIs;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Dal = DataAnalysisAPI.DataAnalysisAPI.Infrastructure.Dapper.UsageInsights;

namespace DataAnalysisAPI.Controllers
{
    /// <summary>
    /// Handles the usage insights queries (connections, tests, teachers and subscriptions). The caller session is validated with the platform
    /// session service and the data scope (all schools, an ORE or a school) comes from that session and is applied inside the SQL.
    /// </summary>
    [ApiController]
    [Route("dataanalysisapi/usage-insights")]
    public class UsageInsightsController : ControllerBase
    {
        private readonly ILogger<UsageInsightsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageInsightsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UsageInsightsController(ILogger<UsageInsightsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the last day with data of the tests and of the connections.
        /// </summary>
        /// <returns>Returns the data status; 401 without a valid session and 403 for a role without access.</returns>
        [HttpGet("data-status")]
        public IActionResult GetDataStatus()
        {
            var language = GetLanguage();
            return Run("data-status", language, context => Ok(UsageInsights.GetDataStatus(context)));
        }

        /// <summary>
        /// Runs one usage insights query. The route name is the query name (for example connections-by-school).
        /// Parameters: from, to, order, limit, school and the ones of each query (see the API contract).
        /// </summary>
        /// <param name="query">The query name.</param>
        /// <returns>Returns the response with the references to cite; 400 for invalid parameters, 401, 403 or 503.</returns>
        [HttpGet("{query}")]
        public IActionResult GetUsageInsight(string query)
        {
            var language = GetLanguage();
            if (!UsageInsights.Exists(query))
            {
                return Failure(StatusCodes.Status404NotFound, "unknown_query", language, "UsageInsightsUnknownQuery", query);
            }

            return Run(query, language, context =>
                new ContentResult
                {
                    Content = UsageInsightsJson.Serialize(UsageInsights.Execute(query, new UsageInsightsQueryParser(Request.Query), context, language)),
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                });
        }

        /// <summary>
        /// Exports the result of a query as an Excel workbook (format=xlsx) or as a chart specification (format=chart).
        /// It accepts the same parameters as the query; the limit goes up to 5000 and, without limit, every row (up to 5000) is exported.
        /// With format=chart the limit, all, order and includeZeros parameters belong to the chart. The school search is not exportable.
        /// </summary>
        /// <param name="query">The query name.</param>
        /// <param name="format">xlsx or chart.</param>
        /// <returns>Returns the .xlsx file or the chart specification; 422 when the query has no chart.</returns>
        [HttpGet("{query}/export")]
        public IActionResult ExportUsageInsight(string query, [FromQuery] string? format = "xlsx")
        {
            var language = GetLanguage();
            if (query == "search-schools" || !UsageInsights.Exists(query))
            {
                return Failure(StatusCodes.Status404NotFound, "unknown_query", language, "UsageInsightsUnknownQuery", query);
            }

            return Run(query, language, context =>
            {
                if (format is not ("xlsx" or "chart"))
                {
                    throw new InvalidQueryParameterException("format", "FormatValue");
                }

                // The workbook and the chart work with ALL the rows (up to 5000); in the chart, "limit" is the number of preview categories, not the query limit.
                var parser = new UsageInsightsQueryParser(Request.Query, isExport: true, ignoreLimit: format == "chart");
                var chartOptions = format == "chart" ? parser.ParseChartOptions() : null;
                var response = UsageInsights.Execute(query, parser, context, language);
                var responseNode = UsageInsightsColumnFormatter.ToJsonNode(response);
                if (format == "xlsx")
                {
                    return File(UsageInsightsExcelExporter.BuildWorkbook(responseNode, language), UsageInsightsExcelExporter.ContentType,
                        UsageInsightsColumnFormatter.BuildFileName(responseNode));
                }

                return ChartSpecificationBuilder.Build(responseNode, chartOptions, language) is { } specification
                    ? new ContentResult { Content = specification.ToJsonString(UsageInsightsJson.Options), ContentType = "application/json", StatusCode = StatusCodes.Status200OK }
                    : Failure(StatusCodes.Status422UnprocessableEntity, "chart_unavailable", language, "UsageInsightsChartUnavailable");
            });
        }

        /// <summary>
        /// Validates the session, resolves the data scope and runs the action, turning the known failures into responses.
        /// When the database fails, the cause is logged WITHOUT parameters, data or Token and the caller only gets the generic error.
        /// </summary>
        private IActionResult Run(string queryName, string language, Func<UsageInsightsQueryContext, IActionResult> action)
        {
            try
            {
                ValidationResponseModel session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, UsageInsightsSettings.PermissionName);
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                {
                    return Failure(StatusCodes.Status401Unauthorized, "unauthorized", language, "UsageInsightsUnauthorized");
                }

                var unscopedContext = UsageInsightsSettings.BuildQueryContext(DataScope.All);
                var scope = DataScopeResolver.Resolve(session.RoleId, session.OreId, () => Dal.GetUserSchoolId(unscopedContext, session.UserId),
                    UsageInsightsSettings.GetAllSchoolsRoleIds(), UsageInsightsSettings.GetOreRoleIds(), UsageInsightsSettings.GetSchoolRoleIds());
                if (scope == DataScope.None)
                {
                    return Failure(StatusCodes.Status403Forbidden, "forbidden", language, "UsageInsightsForbidden");
                }

                return action(UsageInsightsSettings.BuildQueryContext(scope));
            }
            catch (InvalidQueryParameterException exception)
            {
                return Failure(StatusCodes.Status400BadRequest, "invalid_parameters", language, "UsageInsightsReason" + exception.ReasonCode,
                    exception.ParameterName, exception.Detail);
            }
            catch (MySqlException exception)
            {
                UsageInsights.LogQueryFailure(queryName, exception, _logger);
                return Failure(StatusCodes.Status503ServiceUnavailable, "database_unavailable", language, "UsageInsightsDatabaseUnavailable");
            }
        }

        private string GetLanguage() => UsageInsightsMessages.NormalizeLanguage(RequestHandler.GetLanguage(HttpContext));

        private ContentResult Failure(int statusCode, string errorCode, string language, string messageKey, params object?[] messageArguments) =>
            new()
            {
                Content = UsageInsightsJson.Serialize(new { error = UsageInsightsMessages.Get(language, messageKey, messageArguments), ErrorCode = errorCode }),
                ContentType = "application/json",
                StatusCode = statusCode,
            };
    }
}
