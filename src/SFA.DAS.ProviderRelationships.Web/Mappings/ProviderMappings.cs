using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class ProviderMappings : Profile
    {
        public ProviderMappings()
        {
            CreateMap<FindAllProvidersQueryResult, FindProvidersViewModel>()
                .ForMember(d => d.Ukprn,s=>s.Ignore());
        }
    }
}