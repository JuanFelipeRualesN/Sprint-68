using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;
using GSWebAPI.AccessLogs.Infrastructure.Interfaces;

namespace GSWebAPI.AccessLogs.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for sending messages to Azure Service Bus using a generic type T.
    /// </summary>
    public class ServiceBusRepository<T> : IServiceBusRepository<T> where T : class
    {
        private readonly ServiceBusSender _serviceBusSender;

        public ServiceBusRepository(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }

        /// <summary>
        /// Sends a single message of type T to the Azure Service Bus.
        /// </summary>
        /// <param name="message">The message object to send.</param>
        public async Task SendMessageAsync(T message)
        {
            string serializedMessage = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new ServiceBusMessage(serializedMessage)
            {
                ContentType = "application/json"
            };

            try
            {
                await _serviceBusSender.SendMessageAsync(serviceBusMessage).ConfigureAwait(false);
            }
            catch (ServiceBusException ex) when (ex.IsTransient)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends multiple messages of type T to the Azure Service Bus in batches.
        /// </summary>
        /// <param name="messages">The collection of message objects to send.</param>
        public async Task SendManyMessagesAsync(IEnumerable<T> messages)
        {
            ServiceBusMessageBatch messageBatch = await _serviceBusSender.CreateMessageBatchAsync().ConfigureAwait(false);

            foreach (var message in messages)
            {
                string serializedMessage = JsonConvert.SerializeObject(message);
                var serviceBusMessage = new ServiceBusMessage(serializedMessage)
                {
                    ContentType = "application/json"
                };

                if (!messageBatch.TryAddMessage(serviceBusMessage))
                {
                    await _serviceBusSender.SendMessagesAsync(messageBatch).ConfigureAwait(false);
                    messageBatch = await _serviceBusSender.CreateMessageBatchAsync().ConfigureAwait(false);

                    if (!messageBatch.TryAddMessage(serviceBusMessage))
                        throw new InvalidOperationException("Message exceeds the maximum batch size.");
                }
            }

            if (messageBatch.Count > 0)
                await _serviceBusSender.SendMessagesAsync(messageBatch).ConfigureAwait(false);
        }
    }
}
