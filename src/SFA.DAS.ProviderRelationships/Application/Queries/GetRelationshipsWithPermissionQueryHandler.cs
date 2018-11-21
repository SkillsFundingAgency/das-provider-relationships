using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetRelationshipsWithPermissionQueryHandler : IRequestHandler<GetRelationshipsWithPermissionQuery, GetRelationshipsWithPermissionQueryResult>
    {
        public Task<GetRelationshipsWithPermissionQueryResult> Handle(GetRelationshipsWithPermissionQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}