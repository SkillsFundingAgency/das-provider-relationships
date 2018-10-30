namespace SFA.DAS.ProviderRelationships.Types
{
    public class ProviderRelationshipResponse
    {
        public long EmployerAccountId { get; set; }
        public string EmployerAccountPublicHashedId { get; set; }
        public string EmployerAccountName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string EmployerAccountLegalEntityName { get; set; }
        public long Ukprn { get; set; }
    }
}