using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountProviderMappings : Profile
    {
        public AccountProviderMappings()
        {
            CreateMap<AccountProvider, AccountProviderBasicDto>();
            CreateMap<AccountProvider, AccountProviderDto>();
            
            CreateMap<AccountProvider, AccountProviderSummaryDto>()
                .ForMember(d => d.AccountProviderLegalEntitiesWithPermissionsCount, o => o.MapFrom(s => s.AccountProviderLegalEntities.Count(aple => aple.Permissions.Any())));
        }
    }
}