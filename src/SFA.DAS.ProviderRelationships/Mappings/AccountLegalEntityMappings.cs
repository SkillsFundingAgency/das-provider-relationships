using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountLegalEntityMappings : Profile
    {
        public AccountLegalEntityMappings()
        {
            CreateMap<AccountLegalEntity, AccountLegalEntityBasicDto>();

            CreateMap<AccountLegalEntity, AccountLegalEntityDto>()
                .ForMember(d => d.Operations, o => o.MapFrom(s => s.AccountProviderLegalEntities.SelectMany(aple => aple.Permissions.Select(p => p.Operation))));
        }
    }
}