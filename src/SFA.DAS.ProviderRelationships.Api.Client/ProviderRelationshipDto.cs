namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipDto
    { 
        public long Ukprn { get; set; }
        public long EmployerAccountId { get; set; }
        public string LegalEntityHashedId { get; set; }
        public string LegalEntityName { get; set; }
    }
}