using GSAIServicesAPI.Models.Quotas;
using GSAIServicesAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace GSAIServicesAPI.Filters
{
    /// <summary>
    /// Blocks the action when the AI kill switch is off,
    /// returning a 503 before the action runs
    /// </summary>
    public sealed class RequiresAiEnabledAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var services = context.HttpContext.RequestServices;
            var db = services.GetRequiredService<AppDbContext>();

            var settings = await db.QuotaSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == QuotaSetting.SingletonId);

            var aiEnabled = settings?.AiEnabled ?? true;

            if (!aiEnabled)
            {
                var config = services.GetRequiredService<IConfiguration>();
                var isoCode = RequestHandler.GetLanguage(context.HttpContext)?.ToLowerInvariant() == "es" ? "es" : "en";

                var message = config[$"Messages:AIDisabled:{isoCode}"] ?? config["Messages:AIDisabled:en"];

                context.Result = new ObjectResult(new { error = message, ErrorCode = "ai_disabled" })   // ErrorCode is additive: the existing field is unchanged
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable
                };
                return;
            }

            await next();
        }
    }
}
