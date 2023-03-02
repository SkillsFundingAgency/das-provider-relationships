using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;

namespace SFA.DAS.ProviderRelationships.Web.Mappings
{
    public class AccountProviderLegalEntityMappings : Profile
    {
        public AccountProviderLegalEntityMappings()
        {
            CreateMap<GetAccountProviderLegalEntityQueryResult, AccountProviderLegalEntityViewModel>()
                .ForMember(d => d.Confirmation, o => o.Ignore())
                .ForMember(d => d.AccountId, o => o.Ignore())
                .ForMember(d => d.UserRef, o => o.Ignore())
                .ForMember(d => d.AccountProviderId, o => o.MapFrom(s => s.AccountProvider.Id))
                .ForMember(d => d.AccountLegalEntityId, o => o.MapFrom(s => s.AccountLegalEntity.Id))
                .ForMember(d => d.Permissions,
                    o =>
                    {
                        o.PreCondition(s => s.AccountProviderLegalEntity != null);
                        o.MapFrom(s => s.AccountProviderLegalEntity.Operations.ToPermissions());
                    });
        }
    }
}