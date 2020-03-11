using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders
{
    public class GetAccountProvidersQueryHandler : IRequestHandler<GetAccountProvidersQuery, GetAccountProvidersQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IAuthorizationService _authorizationService;

        public GetAccountProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider, IAuthorizationService authorizationService)
        {
            _db = db;
            _configurationProvider = configurationProvider;
            _authorizationService = authorizationService;
        }

        public async Task<GetAccountProvidersQueryResult> Handle(GetAccountProvidersQuery request, CancellationToken cancellationToken)
        {
            var accountProviders = new List<AccountProviderDto>();

            var accountProviderIdsTask = _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .ToListAsync(cancellationToken);

            var accountLegalEntitiesCountTask =  _db.Value.AccountLegalEntities.CountAsync(ale => ale.AccountId == request.AccountId, cancellationToken);
            var isOwnerTask = _authorizationService.IsAuthorizedAsync(EmployerUserRole.Owner);

            await Task.WhenAll(accountProviderIdsTask, accountLegalEntitiesCountTask, isOwnerTask).ConfigureAwait(false);

            foreach (var accountProvider in accountProviderIdsTask.Result)
            {
                accountProviders.Add(await GetAccountProvider(request.AccountId, accountProvider.Id, cancellationToken));
            }

            return new GetAccountProvidersQueryResult(accountProviders, accountLegalEntitiesCountTask.Result, isOwnerTask.Result);
        }

        private async Task<AccountProviderDto> GetAccountProvider(long accountId, long accountProviderId, CancellationToken cancellationToken)
        {
            return await _db.Value.AccountProviders
                .Where(ap => ap.Id == accountProviderId && ap.Account.Id == accountId)
                .ProjectTo<AccountProviderDto>(_configurationProvider, new {accountProviderId})
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}

