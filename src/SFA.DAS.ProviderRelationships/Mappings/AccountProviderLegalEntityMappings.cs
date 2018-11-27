using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountProviderLegalEntityMappings : Profile
    {
        public AccountProviderLegalEntityMappings()
        {
            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntityBasicDto>()
                .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.AccountProvider.Provider.Name));
                
            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntityDto>();

            CreateMap<AccountProviderLegalEntity, RelationshipDto>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.AccountProvider.Account.Id))
                .ForMember(d => d.AccountPublicHashedId, o => o.MapFrom(s => s.AccountProvider.Account.PublicHashedId))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.AccountProvider.Account.Name))
                .ForMember(d => d.AccountProviderId, o => o.MapFrom(s => s.AccountProvider.Id))
                .ForMember(d => d.AccountLegalEntityId, o => o.MapFrom(s => s.AccountLegalEntity.Id))
                .ForMember(d => d.AccountLegalEntityPublicHashedId, o => o.MapFrom(s => s.AccountLegalEntity.PublicHashedId))
                .ForMember(d => d.AccountLegalEntityName, o => o.MapFrom(s => s.AccountLegalEntity.Name));
        }
    }
}