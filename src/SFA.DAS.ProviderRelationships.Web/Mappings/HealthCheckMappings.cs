using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck;
using SFA.DAS.ProviderRelationships.Web.ViewModels.HealthCheck;

namespace SFA.DAS.ProviderRelationships.Web.Mappings;

public class HealthCheckMappings : Profile
{
    public HealthCheckMappings()
    {
        CreateMap<GetHealthCheckQueryResult, HealthCheckViewModel>();
    }
}