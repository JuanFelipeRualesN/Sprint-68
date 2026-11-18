using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.AccessLogs.Infrastructure.Interfaces
{
    // Interface for a background task queue that allows enqueuing and dequeuing asynchronous work items.
    public interface IBackgroundTaskQueue
    {
        // Non-blocking: if the queue is full, returns false.
        bool TryQueue(Func<CancellationToken, Task> workItem);

        // Dequeues a work item asynchronously. Waits if the queue is empty.
        ValueTask<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
