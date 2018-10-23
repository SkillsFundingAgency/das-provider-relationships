namespace SFA.DAS.ProviderRelationships.Types
{
    public class ProviderRelationshipRequest
    {
        public long Ukprn { get; set; }
        private PermissionEnumDto Permission { get; set; }
    }
}