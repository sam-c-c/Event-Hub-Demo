using EventHub.Common.Entities;
using EventHub.Common.Validators;
using EventHub.Listener.Exceptions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventHub.Listener
{
    public class ListenerFunction
    {
        private readonly IEventValidator _eventValidator;

        public ListenerFunction(IEventValidator eventValidator)
        {
            _eventValidator = eventValidator;
        }

        [FunctionName("Listener")]
        public void Run([ServiceBusTrigger("events", Connection = "ServiceBusConfiguration:ConnectionString")]string message, ILogger log)
        {
            log.LogInformation($"Processing message: {message}");

            var eventData = JsonConvert.DeserializeObject<Event>(message);
            if (!_eventValidator.IsValid(eventData)) 
            {
                log.LogError($"Invalid event data: {message}");
                throw new UnableToProcessEventException("Invalid event data");
            }

            log.LogInformation("Message processed successfully");
        }
    }
}
