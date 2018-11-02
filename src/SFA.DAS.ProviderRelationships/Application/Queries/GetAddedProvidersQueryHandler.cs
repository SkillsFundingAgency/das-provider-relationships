using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;

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

        public Task<GetAddedProvidersQueryResponse> Handle(GetAddedProvidersQuery request, CancellationToken cancellationToken)
        {
            var accountProviders = _db.Value.AccountProviders
                .Where(ap => ap.Account.Id == request.AccountId)
                .ProjectTo<AccountProviderDto>(_configurationProvider)
                .ToList();

//                .ProjectTo<AccountProviderDto>(_configurationProvider)
                //.SingleOrDefaultAsync(cancellationToken);
            
//            if (accountProvider == null)
//            {
//                return null;
//            }
            
            return Task.FromResult(new GetAddedProvidersQueryResponse(accountProviders));
        }
    }
}

