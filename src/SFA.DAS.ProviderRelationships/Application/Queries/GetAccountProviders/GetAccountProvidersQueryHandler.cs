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
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders.Dtos;
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
            var accountProviders = await _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .OrderBy(ap => ap.Provider.Name)
                .ProjectTo<AccountProviderDto>(_configurationProvider)
                .ToListAsync(cancellationToken);
            
            var accountLegalEntitiesCount = await _db.Value.AccountLegalEntities.CountAsync(ale => ale.AccountId == request.AccountId, cancellationToken);
            var isOwner = await _authorizationService.IsAuthorizedAsync(EmployerUserRole.Owner);
            
            return new GetAccountProvidersQueryResult(accountProviders, accountLegalEntitiesCount, isOwner);
        }
    }
}

