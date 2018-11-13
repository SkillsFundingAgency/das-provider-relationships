using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasPermissionQuery : IApiRequest<bool>
    {
        public long Ukprn { get; }
        public long EmployerAccountLegalEntityId { get; }
        public Operation Operation { get; }

        public HasPermissionQuery(long ukprn, long employerAccountLegalEntityId, Operation operation)
        {
            Ukprn = ukprn;
            EmployerAccountLegalEntityId = employerAccountLegalEntityId;
            Operation = operation;
        }
    }
}