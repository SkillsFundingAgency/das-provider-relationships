using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Queries.HasRelationshipWithPermission
{
    public class HasRelationshipWithPermissionQuery : IRequest<bool>
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