using EventHub.Common.Entities;

namespace EventHub.Common.Validators
{
    public class EventValidator : IEventValidator
    {
        public bool IsValid(Event eventData)
        {
            if (eventData == null || string.IsNullOrEmpty(eventData.ApplicationId)
                || string.IsNullOrEmpty(eventData.ApplicationName) || eventData.EventId == default
                || string.IsNullOrEmpty(eventData.EventName)) 
                return false;
            return true;
        }
    }
}
