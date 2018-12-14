using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck.Dtos;

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