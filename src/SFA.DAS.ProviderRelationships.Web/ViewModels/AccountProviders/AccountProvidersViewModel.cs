using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;

public class AccountProvidersViewModel
{
    public List<AccountProviderDto> AccountProviders { get; set; }
    public int AccountLegalEntitiesCount { get; set; }
    public string AccountHashedId { get; set; }
}