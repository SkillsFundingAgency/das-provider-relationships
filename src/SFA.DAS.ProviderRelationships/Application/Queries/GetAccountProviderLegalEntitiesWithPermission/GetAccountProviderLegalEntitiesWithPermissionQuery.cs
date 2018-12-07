using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission
{
    public class GetAccountProviderLegalEntitiesWithPermissionQuery : IRequest<GetAccountProviderLegalEntitiesWithPermissionQueryResult>
    {
        public long Ukprn { get; }
        public Operation Operation { get; }

        public GetAccountProviderLegalEntitiesWithPermissionQuery(long ukprn, Operation operation)
        {
            Ukprn = ukprn;
            Operation = operation;
        }
    }
}