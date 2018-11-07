using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetHealthCheckQueryReply
    {
        public HealthCheckDto HealthCheck { get; }

        public GetHealthCheckQueryReply(HealthCheckDto healthCheck)
        {
            HealthCheck = healthCheck;
        }
    }
}