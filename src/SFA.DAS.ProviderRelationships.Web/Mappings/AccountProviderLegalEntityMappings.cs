using System;
using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Operations;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class AccountProviderLegalEntityMappings : Profile
    {
        public AccountProviderLegalEntityMappings()
        {
            CreateMap<GetAccountProviderLegalEntityQueryResult, AccountProviderLegalEntityViewModel>()
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.UserRef, o => o.Ignore())
                .ForMember(d => d.AccountProviderId, o => o.MapFrom(s => s.AccountProvider.Id))
                .ForMember(d => d.AccountLegalEntityId, o => o.MapFrom(s => s.AccountLegalEntity.Id))
                .ForMember(d => d.NoOfProviderCreatedVacancies, o => o.Ignore())
                .ForMember(d => d.Operations, x => x.MapFrom(s => Enum.GetValues(typeof(Operation))
                    .Cast<Operation>()
                    .Select(o => new OperationViewModel {
                        Value = o,
                        IsEnabled = s.AccountProviderLegalEntity != null &&
                                    s.AccountProviderLegalEntity.Operations.Contains(o)
                    })));
        }
    }
}