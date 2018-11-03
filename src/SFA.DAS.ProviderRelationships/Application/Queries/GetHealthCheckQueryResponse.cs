using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetHealthCheckQueryResponse
    {
        public HealthCheckDto HealthCheck { get; }

        public GetHealthCheckQueryResponse(HealthCheckDto healthCheck)
        {
            HealthCheck = healthCheck;
        }
    }
}