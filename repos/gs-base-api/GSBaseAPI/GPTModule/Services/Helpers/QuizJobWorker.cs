namespace GSBaseAPI.GPTModule.Services.Helpers
{
    using System.Threading.Channels;

    public interface IQuizJobQueue
    {
        ValueTask EnqueueAsync(Func<CancellationToken, Task> workItem);
        ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct);
    }

    public sealed class QuizJobQueue : IQuizJobQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue =
            Channel.CreateUnbounded<Func<CancellationToken, Task>>();

        public ValueTask EnqueueAsync(Func<CancellationToken, Task> workItem)
            => _queue.Writer.WriteAsync(workItem);

        public ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken ct)
            => _queue.Reader.ReadAsync(ct);
    }

    public sealed class QuizJobWorker : BackgroundService
    {
        private readonly IQuizJobQueue _queue;
        private readonly ILogger<QuizJobWorker> _logger;

        public QuizJobWorker(IQuizJobQueue queue, ILogger<QuizJobWorker> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _queue.DequeueAsync(stoppingToken);
                try { await workItem(stoppingToken); }
                catch (Exception ex) { _logger.LogError(ex, "Quiz job failed"); }
            }
        }
    }
}
