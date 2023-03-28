using EventHub.Common.Entities;

namespace EventHub.Common.ServiceBus
{
    public interface IQueueMessageHandler
    {
        Task HandleMessageAsync(Event eventData);
    }
}