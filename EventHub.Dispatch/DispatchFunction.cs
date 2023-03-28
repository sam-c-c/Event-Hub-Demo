using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EventHub.Common.Entities;
using EventHub.Common.Validators;
using EventHub.Common.ServiceBus;
using System;

namespace EventHub.Dispatch
{
    public class DispatchFunction
    {
        private readonly IEventValidator _eventValidator;
        private readonly IQueueMessageHandler _queueMessageHandler;

        public DispatchFunction(IEventValidator eventValidator, IQueueMessageHandler queueMessageHandler)
        {
            _eventValidator = eventValidator;
            _queueMessageHandler = queueMessageHandler;
        }

        [FunctionName("Dispatch")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("Request received.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var eventData = JsonConvert.DeserializeObject<Event>(requestBody);
                var validatorResponse = _eventValidator.IsValid(eventData);
                if (!validatorResponse)
                {
                    log.LogWarning($"Event failed validation");
                    return new BadRequestObjectResult("Invalid event object");
                }
                log.LogInformation("Send request to message queue.");
                await _queueMessageHandler.HandleMessageAsync(eventData);
                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError($"Exception thrown in Dispatch function: {ex}.");
                return new StatusCodeResult(500);
            }
            
        }
    }
}
