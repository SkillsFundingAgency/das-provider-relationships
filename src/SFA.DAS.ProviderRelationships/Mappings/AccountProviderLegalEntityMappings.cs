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
            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntitySummaryDto>();

            CreateMap<AccountProviderLegalEntity, RelationshipDto>()
                .ForMember(d => d.Ukprn, o => o.MapFrom(s => s.AccountProvider.ProviderUkprn))
                .ForMember(d => d.EmployerAccountId, o => o.MapFrom(s => s.AccountProvider.Account.Id))
                .ForMember(d => d.EmployerAccountPublicHashedId, o => o.MapFrom(s => s.AccountProvider.Account.PublicHashedId))
                .ForMember(d => d.EmployerAccountName, o => o.MapFrom(s => s.AccountProvider.Account.Name))
                .ForMember(d => d.EmployerAccountProviderId, o => o.MapFrom(s => s.AccountProvider.Id))
                .ForMember(d => d.EmployerAccountLegalEntityId, o => o.MapFrom(s => s.AccountLegalEntity.Id))
                .ForMember(d => d.EmployerAccountLegalEntityPublicHashedId, o => o.MapFrom(s => s.AccountLegalEntity.PublicHashedId))
                .ForMember(d => d.EmployerAccountLegalEntityName, o => o.MapFrom(s => s.AccountLegalEntity.Name));
        }
    }
}