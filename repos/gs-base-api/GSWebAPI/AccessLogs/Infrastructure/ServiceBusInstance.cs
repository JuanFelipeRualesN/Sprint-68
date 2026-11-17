using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;

namespace GSWebAPI.AccessLogs.Infrastructure
{
    /// <summary>
    /// Static helper that holds a singleton ServiceBusClient
    /// </summary>
    public static class ServiceBusInstance
    {
        private static readonly ConcurrentDictionary<string, ServiceBusClient> _clients = new ConcurrentDictionary<string, ServiceBusClient>();
        private static readonly ConcurrentDictionary<string, ServiceBusSender> _senders = new ConcurrentDictionary<string, ServiceBusSender>();

        public static ServiceBusClient GetClient(string connectionString)
        {
            return _clients.GetOrAdd(connectionString, cs => new ServiceBusClient(cs));
        }

        public static ServiceBusSender GetSender(string connectionString, string queueName)
        {
            string key = $"{connectionString}|{queueName}";
            return _senders.GetOrAdd(key, _ => GetClient(connectionString).CreateSender(queueName));
        }
    }
}
