namespace EventHub.Common.Entities
{
    public class Event
    {
        public string? ApplicationId { get; set; }
        public string? ApplicationName { get; set; }

        public int EventId { get; set; }

        public string? EventName { get; set; }
    }
}
