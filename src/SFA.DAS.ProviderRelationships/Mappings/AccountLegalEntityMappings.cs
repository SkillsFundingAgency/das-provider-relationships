using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountLegalEntityMappings : Profile
    {
        public AccountLegalEntityMappings()
        {
            long accountProviderId = 0;
            
            CreateMap<AccountLegalEntity, Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountLegalEntityDto>();

            CreateMap<AccountLegalEntity, Application.Queries.GetAccountProvider.Dtos.AccountLegalEntityDto>()
                .ForMember(d => d.Operations, o => o.MapFrom(s => s.AccountProviderLegalEntities
                    .Where(aple => aple.AccountProviderId == accountProviderId)
                    .SelectMany(aple => aple.Permissions.Select(p => p.Operation))));
        }
    }
}