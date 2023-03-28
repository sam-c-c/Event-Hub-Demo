using EventHub.Common.Configuration;
using EventHub.Common.Entities;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace EventHub.Common.ServiceBus
{
    public class QueueMessageHandler : IQueueMessageHandler
    {
        private readonly ServiceBusConfiguration configuration;

        public QueueMessageHandler(IOptions<ServiceBusConfiguration> options)
        {
            configuration = options.Value;
        }

        public async Task HandleMessageAsync(Event eventData)
        {
            QueueClient tc = new QueueClient(configuration.ConnectionString, configuration.QueueName);

            var message = new Message() { Body = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(eventData)) };
            await tc.SendAsync(message);
            await tc.CloseAsync();
        }
    }
}
