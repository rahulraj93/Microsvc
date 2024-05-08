using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsvc.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string connectionString = "";
        public async Task PublishMessage(object message, string topicQueueName)
        {

            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicQueueName);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
