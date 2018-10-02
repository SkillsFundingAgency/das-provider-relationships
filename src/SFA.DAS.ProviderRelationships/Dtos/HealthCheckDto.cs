using System;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class HealthCheckDto
    {
        public int Id { get; set; }
        public virtual DateTime SentApprenticeshipInfoServiceApiRequest { get; set; }
        public virtual DateTime? ReceivedApprenticeshipInfoServiceApiResponse { get; set; }
        public DateTime PublishedProviderRelationshipsEvent { get; set; }
        public DateTime? ReceivedProviderRelationshipsEvent { get; set; }
    }
}