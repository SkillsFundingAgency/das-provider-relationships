using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages
{
    public class HealthCheckEvent : Event
    {
        public int Id { get; set; }
    }
}