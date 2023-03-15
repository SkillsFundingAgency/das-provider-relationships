using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

namespace SFA.DAS.ProviderRelationships.Web.Mappings;

public class AccountProviderMappings : Profile
{
    public AccountProviderMappings()
    {
        CreateMap<GetAccountProvidersQueryResult, AccountProvidersViewModel>();

        CreateMap<GetAccountProviderQueryResult, GetAccountProviderViewModel>()
            .ForMember(d=> d.IsUpdatePermissionsOperationAuthorized, o => o.Ignore());

        CreateMap<GetAddedAccountProviderQueryResult, AddedAccountProviderViewModel>()
            .ForMember(d => d.Choice, o => o.Ignore());
            
        CreateMap<GetAddedAccountProviderQueryResult, AlreadyAddedAccountProviderViewModel>()
            .ForMember(d => d.Choice, o => o.Ignore());
            
        CreateMap<GetProviderToAddQueryResult, AddAccountProviderViewModel>()
            .ForMember(d => d.AccountHashedId, o => o.Ignore())
            .ForMember(d => d.Ukprn, o => o.Ignore())
            .ForMember(d => d.AccountId, o => o.Ignore())
            .ForMember(d => d.UserRef, o => o.Ignore())
            .ForMember(d => d.Choice, o => o.Ignore());
    }
}