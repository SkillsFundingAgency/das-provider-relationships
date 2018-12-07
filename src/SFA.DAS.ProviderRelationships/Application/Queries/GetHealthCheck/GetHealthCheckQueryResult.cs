using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck
{
    public class GetHealthCheckQueryResult
    {
        public HealthCheckDto HealthCheck { get; }

        public GetHealthCheckQueryResult(HealthCheckDto healthCheck)
        {
            HealthCheck = healthCheck;
        }
    }
}