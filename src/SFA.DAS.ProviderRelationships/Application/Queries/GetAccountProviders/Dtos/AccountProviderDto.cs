namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders.Dtos
{
    public class AccountProviderDto
    {
        public long Id { get; set; }
        public string ProviderName { get; set; }
        public int AccountProviderLegalEntitiesWithPermissionsCount { get; set; }
    }
}