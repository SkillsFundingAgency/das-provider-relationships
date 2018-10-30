namespace SFA.DAS.ProviderRelationships.Types
{
    public class ProviderRelationshipResponse
    {
        public long Ukprn { get; set; }
        public long EmployerAccountId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public string EmployerAccountLegalEntityName { get; set; }
    }
}