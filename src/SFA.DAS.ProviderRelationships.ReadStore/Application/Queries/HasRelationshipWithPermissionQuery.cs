using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasRelationshipWithPermissionQuery : IApiRequest<bool>
    {
        public long Ukprn { get; }
        public Operation Operation { get; }

        public HasRelationshipWithPermissionQuery(long ukprn, Operation operation)
        {
            Ukprn = ukprn;
            Operation = operation;
        }
    }
}