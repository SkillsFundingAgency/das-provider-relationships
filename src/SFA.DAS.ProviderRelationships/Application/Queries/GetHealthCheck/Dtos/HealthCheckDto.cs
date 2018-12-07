using System;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck.Dtos
{
    public class HealthCheckDto
    {
        public int Id { get; set; }
        public DateTime SentApprenticeshipInfoServiceApiRequest { get; set; }
        public DateTime? ReceivedApprenticeshipInfoServiceApiResponse { get; set; }
        public DateTime SentProviderRelationshipsApiRequest { get; set; }
        public DateTime? ReceivedProviderRelationshipsApiResponse { get; set; }
        public DateTime PublishedProviderRelationshipsEvent { get; set; }
        public DateTime? ReceivedProviderRelationshipsEvent { get; set; }
    }
}