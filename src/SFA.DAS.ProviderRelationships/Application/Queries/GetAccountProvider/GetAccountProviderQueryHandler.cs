using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider.Dtos;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider
{
    public class GetAccountProviderQueryHandler : IRequestHandler<GetAccountProviderQuery, GetAccountProviderQueryResult>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IAuthorizationService _authorizationService;

        public GetAccountProviderQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider, IAuthorizationService authorizationService)
        {
            _db = db;
            _configurationProvider = configurationProvider;
            _authorizationService = authorizationService;
        }

        public async Task<GetAccountProviderQueryResult> Handle(GetAccountProviderQuery request, CancellationToken cancellationToken)
        {
            var accountProvider = await _db.Value.AccountProviders
                .Where(ap => ap.Id == request.AccountProviderId && ap.Account.Id == request.AccountId)
                .ProjectTo<AccountProviderDto>(_configurationProvider, new { accountProviderId = request.AccountProviderId })
                .SingleOrDefaultAsync(cancellationToken);
            
            if (accountProvider == null)
            {
                return null;
            }
            
            var isOwner = await _authorizationService.IsAuthorizedAsync(EmployerUserRole.Owner);

            return new GetAccountProviderQueryResult(accountProvider, isOwner);
        }
    }
}