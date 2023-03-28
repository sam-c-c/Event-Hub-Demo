using EventHub.Common.Entities;

namespace EventHub.Common.Validators
{
    public interface IEventValidator
    {
        bool IsValid(Event eventData);
    }
}
