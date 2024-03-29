using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission
{
    public class GetAccountProviderLegalEntitiesWithPermissionQuery : IRequest<GetAccountProviderLegalEntitiesWithPermissionQueryResult>
    {
        public string AccountHashedId { get; }
        public string AccountLegalEntityPublicHashedId { get; }
        public long? Ukprn { get; }
        public List<Operation> Operations { get; }

        public GetAccountProviderLegalEntitiesWithPermissionQuery(long? ukprn, string accountHashedId, string accountLegalEntityPublicHashedId, List<Operation> operations)
        {
            AccountHashedId = accountHashedId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            Ukprn = ukprn;
            Operations = operations;
        }
    }
}