using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
           CreateMap<GetAllProvidersQueryResult, FindProviderViewModel>()
                .ForMember(d => d.Ukprn, s => s.Ignore())
                .ForMember(d => d.AccountId, s => s.Ignore());
        }
    }
}