using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Queries.HasPermission
{
    public class HasPermissionQuery : IRequest<bool>
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
