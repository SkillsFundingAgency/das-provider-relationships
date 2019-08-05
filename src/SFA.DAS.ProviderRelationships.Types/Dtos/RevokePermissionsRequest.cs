using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class RevokePermissionsRequest
    {
        public long Ukprn { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public Operation[] OperationsToRevoke { get; set; }

        public RevokePermissionsRequest() { }

        public RevokePermissionsRequest(long ukprn, string accountLegalEntityPublicHashedId, Operation[] operationsToRevoke)
            : this()
        {
            Ukprn = ukprn;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            OperationsToRevoke = operationsToRevoke;
        }
    }
}
