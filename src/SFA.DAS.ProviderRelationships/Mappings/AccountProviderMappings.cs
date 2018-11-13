using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountProviderMappings : Profile
    {
        public AccountProviderMappings()
        {
            CreateMap<AccountProvider, AccountProviderDto>();
            CreateMap<AccountProvider, AccountProviderSummaryDto>();
            CreateMap<AccountProvider, AddedAccountProviderDto>();
        }
    }
}