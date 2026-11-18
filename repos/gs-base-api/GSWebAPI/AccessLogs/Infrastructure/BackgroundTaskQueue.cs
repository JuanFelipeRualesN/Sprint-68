using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using GSWebAPI.AccessLogs.Infrastructure.Interfaces;

namespace GSWebAPI.AccessLogs.Infrastructure
{
    public sealed class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        // Channel to hold the queued background tasks (delegates)
        private readonly Channel<Func<CancellationToken, Task>> _queue;

        public BackgroundTaskQueue(IOptions<BackgroundQueueOptions> options)
        {
            // Get configuration or use default if null
            var cfg = options?.Value ?? new BackgroundQueueOptions();

            var channelOptions = new BoundedChannelOptions(cfg.Capacity)
            {
                // Important: We do NOT want to block request threads.
                // We will use TryWrite; if the queue is full, respond with 503.
                FullMode = BoundedChannelFullMode.Wait,

                SingleReader = false,
                SingleWriter = false
            };

            // Create a bounded channel for background tasks
            _queue = Channel.CreateBounded<Func<CancellationToken, Task>>(channelOptions);
        }

        // Attempts to queue a background work item. Returns false if the queue is full.
        public bool TryQueue(Func<CancellationToken, Task> workItem)
        {
            if (workItem is null) throw new ArgumentNullException(nameof(workItem));
            return _queue.Writer.TryWrite(workItem); // false if the queue is full
        }

        public ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
            => _queue.Reader.ReadAsync(cancellationToken);
    }
}
