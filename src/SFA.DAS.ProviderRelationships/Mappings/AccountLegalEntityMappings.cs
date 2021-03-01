using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountLegalEntityMappings : Profile
    {
        public AccountLegalEntityMappings()
        {
            long accountProviderId = 0;

            CreateMap<AccountLegalEntity, Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountLegalEntityDto>();

            CreateMap<AccountLegalEntity, AccountLegalEntityDto>()
                .ForMember(d => d.HadPermissions, o => o.MapFrom(s => s.AccountProviderLegalEntities
                    .Any(aple => aple.AccountProviderId == accountProviderId && aple.Updated.HasValue)))
                .ForMember(d => d.Operations, o => o.MapFrom(s => s.AccountProviderLegalEntities
                    .Where(aple => aple.AccountProviderId == accountProviderId)
                    .SelectMany(aple => aple.Permissions.Select(p => p.Operation))));
        }
    }
}