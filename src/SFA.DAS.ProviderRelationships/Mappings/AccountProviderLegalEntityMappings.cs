using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos;
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
            
            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntityDto>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.AccountProvider.Account.Id))
                .ForMember(d => d.AccountPublicHashedId, o => o.MapFrom(s => s.AccountProvider.Account.PublicHashedId))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.AccountProvider.Account.Name));
                
            CreateMap<AccountProviderLegalEntity, Application.Queries.GetAccountProviderLegalEntity.Dtos.AccountProviderLegalEntityDto>()
                .ForMember(d => d.Operations, o => o.MapFrom(s => s.Permissions.Select(p => p.Operation)));
        }
    }
}