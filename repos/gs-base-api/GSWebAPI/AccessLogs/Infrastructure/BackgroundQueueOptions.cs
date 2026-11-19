using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.AccessLogs.Infrastructure
{
    public sealed class BackgroundQueueOptions
    {
        // Maximum number of tasks waiting in memory (backpressure)
        public int Capacity { get; set; } = 1000;

        // How many parallel workers process the queue
        public int WorkerCount { get; set; } = 4;
    }
}
