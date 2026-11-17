using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.AccessLogs.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for a service bus repository with generic type T
    /// </summary>
    public interface IServiceBusRepository<T> where T : class
    {
        /// <summary>
        /// Sends a single message of type T to the service bus.
        /// </summary>
        Task SendMessageAsync(T message);

        /// <summary>
        /// Sends multiple messages of type T to the service bus.
        /// </summary>
        Task SendManyMessagesAsync(IEnumerable<T> messages);
    }
}
