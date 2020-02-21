using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountOverview;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class AccountMappings : Profile
    {
        public AccountMappings()
        {
            CreateMap<GetAccountOverviewQueryResult, AccountViewModel>();
        }
    }
}