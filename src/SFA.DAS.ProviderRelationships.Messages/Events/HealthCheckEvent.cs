﻿using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class HealthCheckEvent : Event
    {
        public int Id { get; set; }
    }
}