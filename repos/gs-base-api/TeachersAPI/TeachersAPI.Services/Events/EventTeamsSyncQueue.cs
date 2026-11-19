using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Queue for Events' Microsoft Teams sync work (EventTeamsManager.SyncEventCreation/Update), so
    /// CreateEvent/UpdateEvent can return to the client without waiting on audience resolution
    /// (Users API) or Graph, regardless of how large the resolved audience is.
    /// </summary>
    public interface IEventTeamsSyncQueue
    {
        ValueTask EnqueueAsync(Func<CancellationToken, Task> workItem);
        ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct);
    }

    public sealed class EventTeamsSyncQueue : IEventTeamsSyncQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue =
            Channel.CreateUnbounded<Func<CancellationToken, Task>>();

        public ValueTask EnqueueAsync(Func<CancellationToken, Task> workItem)
            => _queue.Writer.WriteAsync(workItem);

        public ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct)
            => _queue.Reader.ReadAsync(ct);
    }

    /// <summary>
    /// Runs queued Events Teams-sync work items one at a time in the background, off the
    /// create/update event HTTP request.
    /// </summary>
    public sealed class EventTeamsSyncWorker : BackgroundService
    {
        private readonly IEventTeamsSyncQueue _queue;
        private readonly ILogger<EventTeamsSyncWorker> _logger;

        public EventTeamsSyncWorker(IEventTeamsSyncQueue queue, ILogger<EventTeamsSyncWorker> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Func<CancellationToken, Task> workItem;

                try
                {
                    workItem = await _queue.DequeueAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to run a queued Events Teams-sync work item.");
                }
            }
        }
    }

    /// <summary>
    /// Static access point to the DI-registered queue, for EventModifierManager (manually
    /// constructed per-request by EventController, not DI-injected) — same pattern as
    /// UsersAPICache.Initialize in Program.cs.
    /// </summary>
    public static class EventTeamsSyncQueueAccessor
    {
        private static IEventTeamsSyncQueue _queue;

        public static void Initialize(IEventTeamsSyncQueue queue)
        {
            _queue = queue;
        }

        public static IEventTeamsSyncQueue Instance => _queue;
    }
}
