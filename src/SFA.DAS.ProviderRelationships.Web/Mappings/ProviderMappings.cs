using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Providers;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
            CreateMap<GetAddedAccountProviderQueryResult, AddedProviderViewModel>()
                .ForMember(d => d.Choice, o => o.Ignore());
            
            CreateMap<GetAddedAccountProviderQueryResult, AlreadyAddedProviderViewModel>()
                .ForMember(d => d.Choice, o => o.Ignore());
            
            CreateMap<GetProviderQueryResult, AddProviderViewModel>()
                .ForMember(d => d.Ukprn, o => o.MapFrom(s => s.Provider.Ukprn))
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.UserRef, o => o.Ignore())
                .ForMember(d => d.Choice, o => o.Ignore());
        }
    }
}