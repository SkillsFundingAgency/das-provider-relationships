using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider.Dtos;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountLegalEntityMappings : Profile
    {
        public AccountLegalEntityMappings()
        {
            long accountProviderId = 0;
            
            CreateMap<AccountLegalEntity, AccountLegalEntityBasicDto>();

            CreateMap<AccountLegalEntity, AccountLegalEntityDto>()
                .ForMember(d => d.Operations, o => o.MapFrom(s => s.AccountProviderLegalEntities
                    .Where(aple => aple.AccountProviderId == accountProviderId)
                    .SelectMany(aple => aple.Permissions.Select(p => p.Operation))));
        }
    }
}