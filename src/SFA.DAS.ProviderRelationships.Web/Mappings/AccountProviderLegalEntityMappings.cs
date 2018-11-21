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
                .ForMember(d => d.AccountId, x => x.Ignore())
                .ForMember(d => d.AccountProviderId, x => x.Ignore())
                .ForMember(d => d.AccountLegalEntityId, x => x.Ignore())
                .ForMember(d => d.Operations, x => x.MapFrom(s => Enum.GetValues(typeof(Operation))
                    .Cast<Operation>()
                    .Select(o => new OperationViewModel
                    {
                        Value = o,
                        IsEnabled = s.AccountProviderLegalEntity != null && s.AccountProviderLegalEntity.Permissions.Any(p => p.Operation == o)
                    })));
        }
    }
}