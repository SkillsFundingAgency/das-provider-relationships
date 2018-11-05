using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class HasPermissionQueryHandler : IApiRequestHandler<HasPermissionQuery, bool>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public HasPermissionQueryHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task<bool> Handle(HasPermissionQuery request, CancellationToken cancellationToken)
        {
            var hasPermission = await _permissionsRepository.CreateQuery()
                .AnyAsync(p => p.Ukprn == request.Ukprn && p.EmployerAccountLegalEntityId == request.EmployerAccountLegalEntityId && p.Operations.Contains(request.Operation), cancellationToken)
                .ConfigureAwait(false);

            return hasPermission;
        }
    }
}