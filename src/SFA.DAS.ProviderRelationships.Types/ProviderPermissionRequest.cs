namespace SFA.DAS.ProviderRelationships.Types
{
    public class ProviderPermissionRequest
    {
        public long EmployerAccountLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public PermissionEnumDto Permission { get; set; }
    }
}