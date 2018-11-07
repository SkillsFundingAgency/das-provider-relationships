using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProvidersQueryHandler : IRequestHandler<GetAddedProvidersQuery, GetAddedProvidersQueryResponse>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetAddedProvidersQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetAddedProvidersQueryResponse> Handle(GetAddedProvidersQuery request, CancellationToken cancellationToken)
        {
            var accountProviders = await _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .ProjectTo<GetAddedProvidersQueryResponse.AccountProvider>(_configurationProvider)
                .ToListAsync(cancellationToken);

            return new GetAddedProvidersQueryResponse(accountProviders);
        }
    }
}

