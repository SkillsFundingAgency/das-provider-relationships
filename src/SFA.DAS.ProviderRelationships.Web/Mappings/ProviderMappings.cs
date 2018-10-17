using AutoMapper;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
            CreateMap<GetProviderQueryResponse, AddProviderViewModel>()
                .ForMember(d => d.Choice, o => o.Ignore())
                .ForMember(d => d.Ukprn, o => o.MapFrom(s => s.Provider.Ukprn.ToString()));
        }
    }
}