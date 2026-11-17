using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GSWebAPI.AccessLogs.Domain.Models;
using GSWebAPI.AccessLogs.Services;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.AccessLogs.Infrastructure;

namespace GSWebAPI.Controllers.AccessLogs
{
    [ApiController]
    [Route("api")]
    public class AccessLogsController : ControllerBase
    {
        private readonly ILogger<AccessLogsController> _log;
        private readonly ILogger<EventLogsService> _eventLogsServiceLogger;
        private readonly IConfiguration _config;
        private readonly IBackgroundTaskQueue _backgroundQueue;

        public AccessLogsController(
            ILogger<AccessLogsController> log,
            ILogger<EventLogsService> eventLogsServiceLogger,
            IConfiguration config,
            IBackgroundTaskQueue backgroundQueue)
        {
            _log = log;
            _eventLogsServiceLogger = eventLogsServiceLogger;
            _config = config;
            _backgroundQueue = backgroundQueue;
        }

        /// <summary>Get the access logs for the user (requires valid session).</summary>
        [HttpPost("tracking")]
        public async Task<IActionResult> TrackEvent([FromBody] EventLogsRequestModel eventLog)
        {
            try
            {
                // Validate session (your existing method)
                ValidationResponseModel responseSession =
                    await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "AccessLogs");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var eventLogsModel = BuildEventModel(eventLog);

                // Resolve Service Bus connection
                var (connectionString, queueName) = ResolveServiceBus(_config, eventLogsModel.ProjectEnvironment);

                // Queue background processing (no System.Web queue in Core)
                _backgroundQueue.Queue(async ct =>
                {
                    try
                    {
                        var sender = ServiceBusInstance.GetSender(connectionString, queueName);
                        var service = new EventLogsService(_eventLogsServiceLogger, sender);
                        await service.LogEventAsync(eventLogsModel).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, "Error while logging the access event in background. Event: {@Event}", eventLogsModel);
                    }
                });

                Response.Headers["Connection"] = "keep-alive";
                return Accepted(new { message = "Event received for processing." });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "TrackEvent failed.");
                return StatusCode(500, "An error occurred while logging the event.");
            }
        }

        /// <summary>Get the access logs from payment area without login.</summary>
        [HttpPost("tracking-plan")]
        public IActionResult TrackPlanEvent([FromBody] EventLogsRequestModel eventLog)
        {
            try
            {
                var eventLogsModel = BuildEventModel(eventLog);

                var (connectionString, queueName) = ResolveServiceBus(_config, eventLogsModel.ProjectEnvironment);

                _backgroundQueue.Queue(async ct =>
                {
                    try
                    {
                        var sender = ServiceBusInstance.GetSender(connectionString, queueName);
                        var service = new EventLogsService(_eventLogsServiceLogger, sender);
                        await service.LogPlanEventAsync(eventLogsModel).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex, "Error while logging the plan event in background. Event: {@Event}", eventLogsModel);
                    }
                });

                Response.Headers["Connection"] = "keep-alive";
                return Accepted(new { message = "Event received for processing." });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "TrackPlanEvent failed.");
                return StatusCode(500, "An error occurred while logging the event.");
            }
        }

        private static EventLogsModel BuildEventModel(EventLogsRequestModel eventLog) =>
            new EventLogsModel
            {
                UserName = eventLog.UserName,
                InvitationCode = eventLog.InvitationCode,
                Role = eventLog.Role,
                AreaId = eventLog.AreaId,
                LessonId = eventLog.LessonId,
                LessonTitle = eventLog.LessonTitle,
                Subject = eventLog.Subject,
                Grade = eventLog.Grade,
                Details = eventLog.Details,
                PlanId = eventLog.PlanId,
                PlanName = eventLog.PlanName,
                AssignmentId = eventLog.AssignmentId,
                AssignmentTitle = eventLog.AssignmentTitle,
                ForumId = eventLog.ForumId,
                ForumTitle = eventLog.ForumTitle,
                IsUnity = eventLog.IsUnity,
                ProjectEnvironment = eventLog.ProjectEnvironment,
                SubscriptionId = eventLog.SubscriptionId,
                PlanSubscriptionId = eventLog.PlanSubscriptionId,
                PlanSubscriptionName = eventLog.PlanSubscriptionName,
                QuantityEnrollment = eventLog.QuantityEnrollment,
                ProfileInformation = eventLog.ProfileInformation,
                ClientInformation = eventLog.ClientInformation,
                CorrelationId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

        /// <summary>
        /// Reads Service Bus settings using IConfiguration (AppSettings:...).
        /// Keeps your previous key scheme:
        ///   envKey = "ServiceBus:Environment:{env}"
        ///   connectionKey = AppSettings:{envKey} (value is a key name)
        ///   connectionString = AppSettings:{connectionKey}
        ///   queueName = AppSettings:ServiceBus:QueueName
        /// </summary>
        private static (string connectionString, string queueName) ResolveServiceBus(IConfiguration config, string? environment)
        {
            var env = environment?.ToLowerInvariant() ?? string.Empty;
            var envKey = $"ServiceBus:Environment:{env}";

            var connectionKey = config[$"AppSettings:{envKey}"];
            if (string.IsNullOrWhiteSpace(connectionKey))
                throw new InvalidOperationException($"No project configuration found for environment '{environment}'.");

            var connectionString = config[$"AppSettings:{connectionKey}"];
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException($"Connection string not found for key '{connectionKey}'.");

            var queueName = config["AppSettings:ServiceBus:QueueName"];
            if (string.IsNullOrWhiteSpace(queueName))
                throw new InvalidOperationException("ServiceBus queue name not configured (AppSettings:ServiceBus:QueueName).");

            return (connectionString, queueName);
        }
    }

    public interface IBackgroundTaskQueue
    {
        void Queue(Func<CancellationToken, Task> workItem);
    }
}
