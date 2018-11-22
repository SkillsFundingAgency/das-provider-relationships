using System;
using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class AccountProviderLegalEntityMappings : Profile
    {
        public AccountProviderLegalEntityMappings()
        {
            CreateMap<GetAccountProviderLegalEntityQueryResult, GetAccountProviderLegalEntityViewModel>()
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.AccountProviderId, o => o.Ignore())
                .ForMember(d => d.AccountLegalEntityId, o => o.Ignore())
                .ForMember(d => d.Operations, x => x.MapFrom(s => Enum.GetValues(typeof(Operation))
                    .Cast<Operation>()
                    .Select(o => new OperationViewModel
                    {
                        Value = o,
                        IsEnabled = s.AccountProviderLegalEntity != null && s.AccountProviderLegalEntity.Permissions.Any(p => p.Operation == o)
                    })));
            
            CreateMap<GetAccountProviderLegalEntityQueryResult, UpdateAccountProviderLegalEntityViewModel>()
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.AccountProviderId, o => o.Ignore())
                .ForMember(d => d.AccountLegalEntityId, o => o.Ignore())
                .ForMember(d => d.Operations, o => o.Ignore());
        }
    }
}