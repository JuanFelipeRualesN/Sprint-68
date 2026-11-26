using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSWebAPI.AccessLogs.Infrastructure;
using GSWebAPI.AccessLogs.Infrastructure.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GSWebAPI.AccessLogs.Services
{
    public sealed class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue;
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly BackgroundQueueOptions _options;

        public QueuedHostedService(
            IBackgroundTaskQueue queue,
            IOptions<BackgroundQueueOptions> options,
            ILogger<QueuedHostedService> logger)
        {
            _queue = queue;
            _logger = logger;
            _options = options?.Value ?? new BackgroundQueueOptions();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workers = new List<Task>(Math.Max(1, _options.WorkerCount));

            for (int i = 0; i < Math.Max(1, _options.WorkerCount); i++)
            {
                var workerId = i;
                workers.Add(Task.Run(() => WorkerLoop(workerId, stoppingToken), stoppingToken));
            }

            return Task.WhenAll(workers);
        }

        private async Task WorkerLoop(int workerId, CancellationToken stoppingToken)
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
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Worker {WorkerId}: error reading from the queue.", workerId);
                    continue;
                }

                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Worker {WorkerId}: error executing task.", workerId);
                }
            }
        }
    }
}
