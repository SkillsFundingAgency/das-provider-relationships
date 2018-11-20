using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels.HealthCheck;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class HealthCheckMappings : Profile
    {
        public HealthCheckMappings()
        {
            CreateMap<GetHealthCheckQueryResult, HealthCheckViewModel>();
        }
    }
}