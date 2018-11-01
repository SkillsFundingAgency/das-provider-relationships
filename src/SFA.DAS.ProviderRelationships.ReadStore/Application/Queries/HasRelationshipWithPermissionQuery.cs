using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasRelationshipWithPermissionQuery : IRequest<bool>
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