using EventHub.Common.Configuration;
using EventHub.Common.ServiceBus;
using EventHub.Common.Validators;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EventHub.Dispatch.Startup))]
namespace EventHub.Dispatch
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ServiceBusConfiguration>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("ServiceBusConfiguration").Bind(settings);
            });

            builder.Services.AddTransient<IEventValidator, EventValidator>();
            builder.Services.AddTransient<IQueueMessageHandler, QueueMessageHandler>();
        }
    }
}
