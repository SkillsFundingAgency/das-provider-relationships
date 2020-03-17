using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountProviderMappings : Profile
    {
        public AccountProviderMappings()
        {
            CreateMap<AccountProvider, Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderDto>();
            CreateMap<AccountProvider, Application.Queries.GetAddedAccountProvider.Dtos.AccountProviderDto>();
            
            CreateMap<AccountProvider, AccountProviderDto>()
                .ForMember(d => d.AccountLegalEntities, o => o.MapFrom(s => s.Account.AccountLegalEntities.OrderBy(ale => ale.Name)))
                .ForMember(d=>d.FormattedProviderSuggestion,o=>o.Ignore());
        }
    }
}