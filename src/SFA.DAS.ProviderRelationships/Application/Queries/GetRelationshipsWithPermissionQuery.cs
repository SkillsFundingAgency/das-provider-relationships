using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetRelationshipsWithPermissionQuery : IRequest<GetRelationshipsWithPermissionQueryResult>
    {
        public long Ukprn { get; }
        public Operation Operation { get; }

        public GetRelationshipsWithPermissionQuery(long ukprn, Operation operation)
        {
            Ukprn = ukprn;
            Operation = operation;
        }
    }
}