namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AccountViewModel
    {
        public int AccountProvidersCount { get; set; }
        public int AccountLegalEntitiesCount { get; set; }
        public bool IsAddProviderOperationAuthorized { get; set; }
        public bool IsUpdatePermissionsOperationAuthorized { get; set; }
    }
}