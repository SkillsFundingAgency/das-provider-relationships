using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class AccountProviderMappings : Profile
    {
        public AccountProviderMappings()
        {
            CreateMap<GetAddedAccountProviderQueryResult, AddedAccountProviderViewModel>()
                .ForMember(d => d.AccountHashedId, o => o.Ignore())
                .ForMember(d => d.Choice, o => o.Ignore());
            
            CreateMap<GetAddedAccountProviderQueryResult, AlreadyAddedAccountProviderViewModel>()
                .ForMember(d => d.Choice, o => o.Ignore());
            
            CreateMap<GetProviderToAddQueryResult, AddAccountProviderViewModel>()
                .ForMember(d => d.Ukprn, o => o.MapFrom(s => s.Provider.Ukprn))
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.UserRef, o => o.Ignore())
                .ForMember(d => d.Choice, o => o.Ignore());
        }
    }
}