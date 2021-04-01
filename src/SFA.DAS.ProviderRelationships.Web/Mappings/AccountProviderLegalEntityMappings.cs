using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Operations;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class AccountProviderLegalEntityMappings : Profile
    {
        public AccountProviderLegalEntityMappings()
        {
            CreateMap<GetAccountProviderLegalEntityQueryResult, GetAccountProviderLegalEntityViewModel>()
                .ForMember(d => d.AccountProviderId, o => o.Ignore())
                .ForMember(d => d.AccountLegalEntityId, o => o.Ignore())
                .ForMember(d => d.Operations, x => x.MapFrom(s => s.AccountProviderLegalEntity == null ?
                    null
                    :
                    s.AccountProviderLegalEntity.Operations
                    .Cast<Operation>()
                    .Select(o =>  new OperationViewModel
                    {
                        Value = o,
                        IsEnabled = true
                    })));            
            
            CreateMap<GetAccountProviderLegalEntityQueryResult, UpdateOperationViewModel>()
                .ForMember(d => d.AccountProviderId, o => o.MapFrom( m=> m.AccountProvider.Id))
                .ForMember(d => d.ProviderName, o => o.MapFrom(m => m.AccountProvider.ProviderName))
                .ForMember(d => d.AccountLegalEntityId, o => o.MapFrom(m => m.AccountLegalEntity.Id))
                .ForMember(d => d.Operation, o => o.Ignore())
                .ForMember(d => d.IsEnabled, o => o.Ignore())
                .ForMember(d => d.BackLink, o => o.Ignore())
                .ForMember(d => d.IsEditMode, o => o.MapFrom(s => s.AccountProviderLegalEntity != null && (s.AccountProviderLegalEntity.Operations.Count() > 0)))
                .ForMember(d => d.Operations, x => x.MapFrom(s => s.AccountProviderLegalEntity == null ?
                    null
                    :
                    s.AccountProviderLegalEntity.Operations
                    .Select(o => new OperationViewModel {
                        Value = o,
                        IsEnabled = true
                    }))
                );

            CreateMap<GetAccountProviderLegalEntityQueryResult, ConfirmOperationViewModel>()
                .ForMember(d => d.AccountProviderId, o => o.MapFrom(m => m.AccountProvider.Id))
                .ForMember(d => d.ProviderName, o => o.MapFrom(m => m.AccountProvider.ProviderName))
                .ForMember(d => d.AccountLegalEntityId, o => o.MapFrom(m => m.AccountLegalEntity.Id))
                .ForMember(d => d.BackLink, o => o.Ignore())
                .ForMember(d => d.Operations, x => x.MapFrom(s => s.AccountProviderLegalEntity == null ?
                    null
                    :
                    s.AccountProviderLegalEntity.Operations
                    .Select(o => new OperationViewModel {
                        Value = o,
                        IsEnabled = true
                    })));

            

            CreateMap<GetAccountProviderLegalEntityQueryResult, UpdateAccountProviderLegalEntityViewModel>()
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.AccountProviderId, o => o.Ignore())
                .ForMember(d => d.AccountLegalEntityId, o => o.Ignore())
                .ForMember(d => d.Operations, o => o.Ignore())
                .ForMember(d => d.UserRef, o => o.Ignore());

            CreateMap<GetUpdatedAccountProviderLegalEntityQueryResult, UpdatedAccountProviderLegalEntityViewModel>()
                .ForMember(d => d.AccountProviderId, o => o.Ignore())
                .ForMember(d => d.Choice, o => o.Ignore());
        }
    }
}