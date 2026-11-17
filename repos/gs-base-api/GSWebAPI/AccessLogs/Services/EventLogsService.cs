using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using GSWebAPI.AccessLogs.Domain.Models;
using GSWebAPI.AccessLogs.Infrastructure.Interfaces;
using GSWebAPI.AccessLogs.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace GSWebAPI.AccessLogs.Services
{
    /// <summary>
    /// Service for handling event logs.
    /// </summary>
    public class EventLogsService
    {
        private readonly ILogger<EventLogsService> _logger;
        private readonly IServiceBusRepository<EventLogsModel> _serviceBusRepository;

        public EventLogsService(ILogger<EventLogsService> logger, ServiceBusSender sender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusRepository = new ServiceBusRepository<EventLogsModel>(sender);
        }


        /// <summary>
        /// Service method to log an event asynchronously.
        /// </summary>
        public async Task LogEventAsync(EventLogsModel eventLog)
        {
            if (eventLog == null) throw new ArgumentNullException(nameof(eventLog));

            // Validate the event log before proceeding
            if (await ValidateEventLog(eventLog) is var errors && errors.Any())
            {
                _logger.LogError("Validation errors occurred while logging event. Errors: {Errors}, EventLog: {EventLog}", string.Join(Environment.NewLine, errors), eventLog);
                throw new ArgumentException("Validation errors occurred: " + string.Join(Environment.NewLine, errors));
            }

            try
            {
                await _serviceBusRepository.SendMessageAsync(eventLog);
                _logger.LogInformation("Event log successfully inserted. CorrelationId: {CorrelationId}", eventLog.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting event log. eventLog: {eventLog}", eventLog);
                throw;
            }
        }

        /// <summary>
        /// Service method to log and plan event asynchronoysly
        /// </summary>
        public async Task LogPlanEventAsync(EventLogsModel eventLog)
        {
            if (eventLog == null) throw new ArgumentNullException(nameof(eventLog));

            try
            {
                await _serviceBusRepository.SendMessageAsync(eventLog);
                _logger.LogInformation("Plan Event log successfully inserted. CorrelationId: {CorrelationId}", eventLog.CorrelationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting plan event log. eventLog: {eventLog}", eventLog);
                throw;
            }
        }

        /// <summary>
        /// Validates the required fields of the EventLogsModel.
        /// logging if any required field is null, empty or not.
        /// </summary>
        /// <param name="eventLog">The event model to validate.</param>
        public async Task<IEnumerable<string>> ValidateEventLog(EventLogsModel eventLog)
        {
            var errors = new List<string>();
            var roleOptions = new[] { "student", "teacher", "director", "client" };
            var json = JsonConvert.SerializeObject(eventLog, Formatting.Indented);


            if (string.IsNullOrEmpty(eventLog.UserName))
            {
                errors.Add("UserName is required but was null or empty. eventLog: " + json);
            }

            if (string.IsNullOrWhiteSpace(eventLog.InvitationCode))
            {
                errors.Add("InvitationCode is required but was null or empty. eventLog: " + json);
            }

            if (string.IsNullOrWhiteSpace(eventLog.CorrelationId))
            {
                errors.Add("CorrelationId is required but was null or empty. eventLog: " + json);
            }

            if (string.IsNullOrWhiteSpace(eventLog.Role) || !roleOptions.Contains(eventLog.Role))
            {
                errors.Add($"Role is required and must be one of the following: {string.Join(", ", roleOptions)}. Provided Role: {eventLog.Role} ");
            }

            if (string.IsNullOrWhiteSpace(eventLog.AreaId))
            {
                errors.Add($"AreaId is required but was null or empty. eventLog: {json}");
            }

            return await Task.FromResult(errors);

        }

    }
}