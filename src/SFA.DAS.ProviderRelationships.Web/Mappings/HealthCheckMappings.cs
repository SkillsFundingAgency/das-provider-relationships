using AutoMapper;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Web.ViewModels;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class HealthCheckMappings : Profile
    {
        public HealthCheckMappings()
        {
            CreateMap<GetHealthCheckQueryResponse, HealthCheckViewModel>();
        }
    }
}