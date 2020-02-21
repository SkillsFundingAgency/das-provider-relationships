using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountOverview
{
    public class GetAccountOverviewQueryHandler : IRequestHandler<GetAccountOverviewQuery, GetAccountOverviewQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IAuthorizationService _authorizationService;

        public GetAccountOverviewQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IAuthorizationService authorizationService)
        {
            _db = db;
            _authorizationService = authorizationService;
        }

        public async Task<GetAccountOverviewQueryResult> Handle(GetAccountOverviewQuery request, CancellationToken cancellationToken)
        {
            var accountLegalEntitiesCount = await _db.Value.AccountLegalEntities.CountAsync(ale => ale.AccountId == request.AccountId, cancellationToken);
            var accountProvidersCount = await _db.Value.AccountProviders.CountAsync(ap => ap.AccountId == request.AccountId, cancellationToken);
            var isOwner = await _authorizationService.IsAuthorizedAsync(EmployerUserRole.Owner);

            return new GetAccountOverviewQueryResult(accountProvidersCount, accountLegalEntitiesCount, isOwner);
        }
    }
}

